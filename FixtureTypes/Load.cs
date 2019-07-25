using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using GelFrame.Telnet;

namespace GelFrame.FixtureTypes
{
    static class Load
    {
        // Set vars
        static private readonly List<Dictionary<string, string>> typeList = new List<Dictionary<string, string>>();

        /// <summary>
        /// Reload fixture types from onPC
        /// </summary>
        /// <returns>True on error</returns>
        static public bool Reload(MainForm mainForm)
        {
            // Create fixture type list. Returns true on error. 
            if (GenerateFixtureList(mainForm))
                return true;

            // Set fixture data list
            Data.SetTypeList(typeList);

            // Save data list to XML
            Save.XML();

            // Get current UTC time. Needed to determine which xml files to move to local library
            DateTime timeStamp = DateTime.UtcNow;

            // Export fixture types to from onPC to XML
            ExportFixtureTypes(mainForm);

            // Move fixture xml to local library for process
            MoveFixtureXML(timeStamp, mainForm);

            // Read profile XML
            Read.ProfileXML(mainForm);

            // Verify profile XML match onPC fixture list
            if (Data.VerifyProfileAndTypeListsMatch(mainForm))
                return true;

            // Completed successfully
            return false;
        }

        /// <summary>
        /// Generate the fixture list from onPC telnet connection
        /// </summary>
        static private bool GenerateFixtureList(MainForm mainForm)
        {
            // Set status
            mainForm.AddStatusNewLine(Config.StatusMessages.headerLoadFixtureTypes);
            mainForm.AddStatusNewLine(Config.StatusMessages.buffer + Config.StatusMessages.telNetStartingConnection);

            // Setup connection
            using (Connection TelnetConnection = new Connection())
            {
                // Set status
                mainForm.AddStatusNewLine(Config.StatusMessages.buffer + Config.StatusMessages.telNetCommands);

                // Run commands
                TelnetConnection.SendCommands(Config.Telnet.fixtureTypeCommands);

                // Check for sucessful telnet connection
                if (TelnetConnection.activeConnection)
                {
                    // Read back stream and build fixture types list
                    using (StreamReader streamReader = new StreamReader(TelnetConnection.Stream(), Encoding.UTF8))
                    {
                        // Define vars
                        string currentLine = "";
                        Dictionary<string, int> headerOffsetDictionary = new Dictionary<string, int>();

                        // Clear fixture type data
                        typeList.Clear();

                        // Process each line until the following line is found
                        while (!(currentLine.Contains(Config.Telnet.streamChaneDest) && currentLine.EndsWith(Config.Telnet.streamSlash)))
                        {
                            try
                            {
                                // Read line from network stream
                                currentLine = streamReader.ReadLine();

                                // Remove control characters and telnet formatting
                                string cleanLine = Functions.Telnet.RemoveFormatting(currentLine);

                                // Verify the script was able to enter setup
                                if (cleanLine == Config.Telnet.streamEnterSetupError)
                                {
                                    // Set status
                                    mainForm.AddStatusNewLine(Config.StatusMessages.buffer + Config.StatusMessages.telNetErrorEnterSetup);

                                    MessageBox.Show(Config.ErrorMessages.cannotEnterSetup);
                                    return true;
                                }

                                // Get starting position of each type in current line
                                if (cleanLine.Trim().EndsWith(Config.Fixture.typeList[Config.Fixture.typeList.Count - 1]))
                                    foreach (string header in Config.Fixture.typeList)
                                        headerOffsetDictionary.Add(header, cleanLine.IndexOf(header));

                                // Process fixture type lines
                                if (cleanLine.StartsWith(Config.Telnet.streamFixtureType))
                                {
                                    // Setup new value dictionary
                                    Dictionary<string, string> valueDictionary = new Dictionary<string, string>();

                                    // Loop through type list and exact each section from line
                                    foreach (string header in Config.Fixture.typeList)
                                    {
                                        // Get next header header
                                        string nextKey = GetNextFixtureKey(header);

                                        // Calcuate number of characters in current header by subtracting postion of next header (or end of string)
                                        int endingOffset = (nextKey == null) ? cleanLine.Length : (headerOffsetDictionary[nextKey] - 1);
                                        endingOffset -= headerOffsetDictionary[header];

                                        // Trim current header and add to value dictionary with header as the key
                                        string value = cleanLine.Substring(headerOffsetDictionary[header], endingOffset).Trim();
                                        valueDictionary.Add(header, value);
                                    }

                                    // Add value dictionary to object's returned list if number is not 1
                                    if ((valueDictionary[Config.Fixture.typeNumber] != "1") && (!(String.IsNullOrEmpty(valueDictionary[Config.Fixture.typeLongName]))))
                                    {
                                        // Set status
                                        mainForm.AddStatusNewLine(Config.StatusMessages.buffer + Config.StatusMessages.telNetFixtureFound + valueDictionary[Config.Fixture.typeLongName] + " " + valueDictionary[Config.Fixture.typeMode]);

                                        typeList.Add(valueDictionary);
                                    }
                                }
                            }
                            catch
                            {

                                MessageBox.Show(Config.ErrorMessages.telnetConnection);
                                return true;

                            }
                        }
                    }

                    // Set status
                    mainForm.AddStatusNewLine(Config.StatusMessages.done);
                }

                // Connection failed
                else
                {
                    // Set status
                    mainForm.AddStatusNewLine(Config.StatusMessages.buffer + Config.StatusMessages.telNetConnectionError);

                    // Set connection to false
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Exports all fixture types
        /// </summary>
        static private void ExportFixtureTypes(MainForm mainForm)
        {
            // Set status
            mainForm.AddStatusNewLine(Config.StatusMessages.headerExportFixtureTypes);
            mainForm.AddStatusNewLine(Config.StatusMessages.buffer + Config.StatusMessages.telNetStartingConnection);

            // Export fixture types
            using (Connection TelnetConnection = new Connection())
            {
                // Set status
                mainForm.AddStatusNewLine(Config.StatusMessages.buffer + Config.StatusMessages.telNetCommands);

                // Run start commands
                TelnetConnection.SendCommands(Config.Telnet.fixtureTypeExportStartCommands);

                // Export each fixture type and overwrite
                foreach (Dictionary<string, string> typeDictionary in Data.GetTypeList())
                {
                    // Create list of export commands
                    List<string> exportCommands = new List<string>()
                    {
                        Config.Telnet.streamExport + typeDictionary[Config.Fixture.typeNumber],
                        Config.Telnet.commandConfirmOverWrite,
                    };

                    // Set status
                    mainForm.AddStatusNewLine(Config.StatusMessages.buffer + Config.StatusMessages.exportFixtureTypes + typeDictionary[Config.Fixture.typeLongName] + " " + typeDictionary[Config.Fixture.typeMode]);

                    // Run export command
                    TelnetConnection.SendCommands(exportCommands);
                }

                // Run end commands (exits editsetup)
                TelnetConnection.SendCommands(Config.Telnet.rootDirectoryCommand);
            }
        }

        /// <summary>
        /// Find all export xml files from MA data directory and move them to local library directory
        /// </summary>
        /// <param name="timeStamp">time to compare file's modified time to and move is greater than or equal to</param>
        static private void MoveFixtureXML(DateTime timeStamp, MainForm mainForm)
        {
            // Vars
            string maLibaryDirectory = MaDataDirectory.GetDataDirectory(mainForm) + Config.Directories.maLibrary;

            // Ensure local library folder exists
            Directory.CreateDirectory(Config.Directories.localData);

            // Empty local directory
            DirectoryInfo localDataDirectoryInfo = new DirectoryInfo(Config.Directories.localData);
            foreach (FileInfo file in localDataDirectoryInfo.GetFiles())
                file.Delete();
            foreach (DirectoryInfo dir in localDataDirectoryInfo.GetDirectories())
                dir.Delete(true);

            // Make sure MA data directory exists
            if (Directory.Exists(maLibaryDirectory))
            {
                // Get list of files
                string[] files = Directory.GetFiles(maLibaryDirectory, Config.Fixture.matchFileType + Config.Fixture.fileType, SearchOption.TopDirectoryOnly);

                // Loop though all files
                foreach (string filePath in files)
                {
                    // Only process xml files
                    if (filePath.EndsWith(Config.Fixture.fileType))
                    {
                        // Get file info
                        FileInfo fileInfo = new FileInfo(filePath);

                        // Only process files modified after this script started
                        if (fileInfo.LastWriteTimeUtc >= timeStamp)

                            // Move file to local library directory
                            File.Move(filePath, Config.Directories.localData + fileInfo.Name);
                    }
                }
            }

            // Show error message if directory does not exist
            else
                MessageBox.Show(Config.ErrorMessages.missingMaDataDirectory);
        }

        /// <summary>
        /// Get the next key after the given key
        /// </summary>
        /// <param name="currentKey">Current Key</param>
        /// <returns>nextKey</returns>
        static private string GetNextFixtureKey(string currentKey)
        {
            string nextKey = null;
            bool foundCurrent = false;

            foreach (string header in Config.Fixture.typeList)
            {
                if (foundCurrent)
                    return header;

                if (header == currentKey)
                    foundCurrent = true;
            }

            return nextKey;
        }
    }
}

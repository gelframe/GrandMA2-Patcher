using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GelFrame.Telnet
{
    static class MaDataDirectory
    {
        static public string GetDataDirectory(MainForm mainForm)
        {
            // Set status
            mainForm.AddStatusNewLine(Config.StatusMessages.headerMaDataDirectory);
            mainForm.AddStatusNewLine(Config.StatusMessages.buffer + Config.StatusMessages.telNetStartingConnection);

            // Setup connection
            using (Connection TelnetConnection = new Connection())
            {
                // Set status
                mainForm.AddStatusNewLine(Config.StatusMessages.buffer + Config.StatusMessages.telNetCommands);

                // Run commands
                TelnetConnection.SendCommands(Config.Telnet.dataDirectoryCommands);

                // Check for sucessful telnet connection
                if (TelnetConnection.activeConnection)
                {
                    // Read back stream and build fixture types list
                    using (StreamReader streamReader = new StreamReader(TelnetConnection.Stream(), Encoding.UTF8))
                    {
                        // Define vars
                        string currentLine = "";

                        // Process each line until the following line is found
                        while (!(currentLine.Contains(Config.Telnet.streamChaneDest) && currentLine.EndsWith(Config.Telnet.streamSlash)))
                        {
                            try
                            {
                                // Read line from network stream
                                currentLine = streamReader.ReadLine();

                                // Remove control characters and telnet formatting
                                string cleanLine = Functions.Telnet.RemoveFormatting(currentLine).Trim(); ;

                                // Find onPC internal directory
                                if (cleanLine.StartsWith(Config.Telnet.streamInternalStart))
                                {
                                    // Reduce line to directory path
                                    cleanLine = cleanLine.Replace(Config.Telnet.streamInternalStart, "");
                                    cleanLine = cleanLine.Replace(Config.Telnet.streamInternalEnd, "");
                                    cleanLine = cleanLine.Replace("/", Config.Directories.seperator);

                                    // Set status
                                    mainForm.AddStatusNewLine(Config.StatusMessages.maDataDirectoryFound + cleanLine);

                                    // Return directory path
                                    return cleanLine;
                                }
                            }
                            catch
                            {
                                MessageBox.Show(Config.ErrorMessages.telnetConnection);
                            }
                        }
                    }
                }

                // Connection failed
                else
                {
                    mainForm.AddStatusNewLine(Config.StatusMessages.buffer + Config.StatusMessages.telNetConnectionError);
                }
            }

            // No directory found, return null
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GelFrame.Telnet;

namespace GelFrame.Process
{
    static class Process
    {
        public static void Execute(MainForm mainForm)
        {
            // Set status
            mainForm.AddStatusNewLine(Config.StatusMessages.headerProcessStart);

            // Get fixture data grid values
            DataGridViewRowCollection fixtureDataGrid = mainForm.GetGridData();

            // Check for empty fixture grid
            if (fixtureDataGrid.Count == 0)
            {
                mainForm.AddStatusNewLine(Config.StatusMessages.processErrorNoData);
                return;
            }

            // Save mapping XML
            Mapping.Save.XML(mainForm);

            // Check for all required columns. This should only come back false if the user enabled/disabled columns in the settings window after loading data.
            mainForm.AddStatusNewLine(Config.StatusMessages.headerProcessMissingColumns);
            if (CheckForRequiredColumns())
            {
                MessageBox.Show(Config.StatusMessages.processMissingColumns);
                mainForm.AddStatusInLine(Config.StatusMessages.processMissingColumns);
                return;
            }
            mainForm.AddStatusInLine(Config.StatusMessages.success);

            // Verify data and abort if errors found
            if (Patch.Verify.ErrorCheckData(mainForm))
            {
                // Clear all patch data
                Patch.Data.ClearAll();

                // Save empty patch data to XML
                Patch.Save.XML(mainForm.GetPatchFilePath());

                // Reload fixture grid data because of error
                Mapping.Grid.UpdateFromForm(mainForm);

                // Abort
                return;
            }

            // Check for unmapped profile selections in data grid and abort on error
            mainForm.AddStatusNewLine(Config.StatusMessages.headerProcessVerifyProfiles);
            if (FixtureTypes.Verify.FixtureTypeMapNullCheck(mainForm, fixtureDataGrid))
            {
                mainForm.AddStatusNewLine(Config.StatusMessages.processErrorVerify);
                return;
            }
            
            // Reload fixture types and verify onPC data still matches app data and abort on error
            if (FixtureTypes.Load.Reload(mainForm))
            {
                mainForm.AddStatusNewLine(Config.StatusMessages.processErrorReload);
                return;
            }

            // Reload fixture grid data because we have updated the onPC fixture type data
            Mapping.Grid.UpdateFromForm(mainForm);

            // Verify fixture data grid profile selection are still present after onPC data update and abort on error
            if (FixtureTypes.Verify.FixtureTypeMapData(mainForm, fixtureDataGrid))
            {
                mainForm.AddStatusNewLine(Config.StatusMessages.processErrorVerify);
                return;
            }
            
            // Set status
            mainForm.AddStatusNewLine(Config.StatusMessages.processVerifyProfilesSuccess);

            // Export XML
            Export exportObject = new Export(mainForm);

            // Import patch into MA onPC if export completed
            if (exportObject.CompletedWithOutError())
            {
                // Success
                bool success = false;

                // Set status
                mainForm.AddStatusNewLine(Config.StatusMessages.processImportHeader);

                // Store orginal command delay
                string orginalCommandDelay = Settings.Data.GetValue(Config.Settings.maCommandDelay);

                // Attempt to udpate the patch 4 times. Increase command delay each time
                for (int i = 0; i < 4; i++)
                {
                    // Multiple current command delay by 2
                    int commandDelay = Functions.Numbers.StringToPostiveInt(Settings.Data.GetValue(Config.Settings.maCommandDelay));
                    Settings.Data.Set(Config.Settings.maCommandDelay, (commandDelay * 2).ToString());

                    // Attempt to import patch
                    string message = ImportPatchIntoOnPC();

                    // Success!
                    if (message == Config.Telnet.statusSuccess)
                    {
                        // Set status
                        mainForm.AddStatusNewLine(Config.StatusMessages.processComplete);
                        MessageBox.Show(Config.StatusMessages.processComplete);
                        success = true;
                        break;
                    }

                    // Retry with increase command delay
                    else if (message == Config.Telnet.statusRetry)
                    {
                        mainForm.AddStatusNewLine(Config.StatusMessages.processImportAttemptFail);
                    }

                    // Cannot enter setup
                    else if (message == Config.Telnet.statusCannotEnterSetup)
                    {
                        // Set status
                        mainForm.AddStatusNewLine(Config.StatusMessages.buffer + Config.StatusMessages.telNetErrorEnterSetup);

                        MessageBox.Show(Config.ErrorMessages.cannotEnterSetup);
                        break;
                    }

                    // Default to connection error
                    else
                    {
                        // Set status
                        mainForm.AddStatusNewLine(Config.StatusMessages.error + Config.StatusMessages.telNetConnectionError);

                        // Supress default fail message 
                        success = true;
                        break;
                    }
                }

                // Import failed
                if (!(success))
                {
                    mainForm.AddStatusNewLine(Config.StatusMessages.processFail);
                    MessageBox.Show(Config.StatusMessages.processFail);
                }

                // Reset command delay
                Settings.Data.Set(Config.Settings.maCommandDelay, orginalCommandDelay);
            }
        }

        /// <summary>
        /// Import the patch xml file into onPC
        /// </summary>
        /// <returns>String with status of import attempt.</returns>
        static private string ImportPatchIntoOnPC()
        {
            // Setup connection
            using (Connection TelnetConnection = new Connection())
            {
                // Check for sucessful telnet connection
                if (TelnetConnection.activeConnection)
                {
                    // Read back stream and build fixture types list
                    using (StreamReader streamReader = new StreamReader(TelnetConnection.Stream(), Encoding.UTF8))
                    {
                        // Define vars
                        string currentLine = "";

                        while (!(currentLine.Contains(Config.Telnet.streamChaneDest) && currentLine.EndsWith(Config.Telnet.streamSlash)))
                        {
                            try
                            {
                                // Read line from network stream
                                currentLine = streamReader.ReadLine();

                                // Remove control characters and telnet formatting
                                string cleanLine = Functions.Telnet.RemoveFormatting(currentLine).Trim();

                                // Cannot enter setup, return error message
                                if (cleanLine == Config.Telnet.streamEnterSetupError)
                                    return Config.Telnet.statusCannotEnterSetup;

                                // Import successful
                                else if (cleanLine == Config.Telnet.streamXMLImported)
                                {
                                    TelnetConnection.SendCommands(Config.Telnet.rootDirectoryCommand);
                                }

                                // Send edit setup command
                                else if (cleanLine == Config.Telnet.streamLoggedInMessage)
                                    TelnetConnection.SendCommands(Config.Telnet.editSetupCommand);

                                // Send import generate patch command
                                else if (cleanLine.EndsWith(Config.Telnet.streamEditSetup))
                                {
                                    // Send import command
                                    TelnetConnection.SendCommands(Config.Telnet.importCommand);

                                    // Take a nap and let MA think
                                    Thread.Sleep(2000);
                                }

                                // Exited setup
                                else if (currentLine.Contains(Config.Telnet.streamChaneDest) && currentLine.EndsWith(Config.Telnet.streamSlash))
                                    return Config.Telnet.statusSuccess;

                            }

                            // Ran out of lines to process. Send retry message.
                            catch
                            {
                                return Config.Telnet.statusRetry;
                            }
                        }
                    }
                }
            }

            // Something terrible has happened if we got here...
            return Config.Telnet.statusConnectionError;
        }

        /// <summary>
        /// Check that patch data contains all the necessary columns as controled by the settings form
        /// </summary>
        /// <returns>False on error</returns>
        static private bool CheckForRequiredColumns()
        {
            // Set error bool default to false
            bool error = false;

            List<string> foundColumns = new List<string>();

            foreach (KeyValuePair<string, string> row in Patch.Data.GetPatchList()[0])
            {
                // Add column name to found columns list
                if (!(foundColumns.Contains(row.Key)))
                    foundColumns.Add(row.Key);
            }

            // Check for missing columns
            if (Settings.Data.GetEnabledColumnsList().Count() != foundColumns.Count())
                error = true;

            // Return error status bool
            return error;
        }
    }
}

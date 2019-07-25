using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GelFrame.Patch
{
    static class Verify
    {
        static public bool ErrorCheckData(MainForm mainForm)
        {
            // Set error bool default to false
            bool error = false;

            // Create lists from settings values
            List<string> excelStringsCannotBeEmpty = new List<string>(Config.Settings.settingsExcelStringsCannotBeEmpty);
            List<string> excelDoubles = new List<string>(Config.Settings.settingsExcelDoubles);
            List<string> excelPostiveInteger = new List<string>(Config.Settings.settingsExcelPostiveInteger);
            List<string> fixtureNumberList = new List<string>();
            List<string> channelNumberList = new List<string>();

            // Add address to approiate list based on combined universe/address setting
            if (Settings.Data.GetValue(Config.Settings.excelSingleAddressField) == Config.Settings.defaultTrue)
                excelStringsCannotBeEmpty.Add(Config.Settings.columnAddress);
            else
                excelPostiveInteger.Add(Config.Settings.columnAddress);

            // Only using fixture number
            if (Settings.Data.GetValue(Config.Settings.excelOnlyFixtureNumber) == Config.Settings.defaultTrue)
                excelPostiveInteger.Add(Config.Settings.columnFixtureNumber);

            // Set error handling vars
            int currentRow = 2;

            // Loop though all excel values
            foreach (Dictionary<string, string> patchList in Data.GetPatchList())
            {
                // Delcare vars for fixture and channel number. Used to verify either one exists if using both.
                bool validChannel = true;
                bool validFixture = true;

                // Loop through each column in given row
                foreach (KeyValuePair<string, string> row in patchList)
                {
                    // Set value
                    string value = row.Value;

                    // Set error message
                    string errorMessage = Config.StatusMessages.excelError + currentRow + ". " + Settings.Data.GetValue(row.Key);

                    // Strings that cannot be empty
                    if ((excelStringsCannotBeEmpty.Contains(row.Key)) && (String.IsNullOrEmpty(value)))
                    {
                        error = true;
                        mainForm.AddStatusNewLine(errorMessage + Config.StatusMessages.excelErrorEmpty);
                        continue;
                    }

                    // Columns that must be doubles 
                    if ((excelDoubles.Contains(row.Key)) && (!(Functions.Numbers.IsStringDouble(value))))
                    {
                        error = true;
                        mainForm.AddStatusNewLine(errorMessage + Config.StatusMessages.excelErrorMustBeNumber);
                        continue;
                    }

                    // Columns that must be postive integers
                    if ((excelPostiveInteger.Contains(row.Key)) && (!(Functions.Numbers.IsStringPostiveInteger(value))))
                    {
                        error = true;
                        mainForm.AddStatusNewLine(errorMessage + Config.StatusMessages.excelErrorMustBePostiveNumber);
                        continue;
                    }

                    // If duplicate unit and fixture numbers not allowed. Ignore empty values.
                    if ((Settings.Data.GetValue(Config.Settings.optionAllowMultiPatch) != Config.Settings.defaultTrue) && (!(string.IsNullOrEmpty(value))))
                    {
                        // Fixture number
                        if (row.Key == Config.Settings.columnFixtureNumber)
                        {
                            // Check for duplicates
                            if (fixtureNumberList.Contains(value))
                            {
                                error = true;
                                mainForm.AddStatusNewLine(errorMessage + " " + value + Config.StatusMessages.excelDuplicateNumber);
                                continue;
                            }

                            // Add value to fixture number list
                            fixtureNumberList.Add(value);
                        }

                        // Channel number
                        if (row.Key == Config.Settings.columnChannelNumber)
                        {
                            // Check for duplicates
                            if (channelNumberList.Contains(value))
                            {
                                error = true;
                                mainForm.AddStatusNewLine(errorMessage + " " + value + Config.StatusMessages.excelDuplicateNumber);
                                continue;
                            }

                            // Add value to fixture number list
                            channelNumberList.Add(value);
                        }
                    }

                    // Check combined universe/address options
                    if ((Settings.Data.GetValue(Config.Settings.excelSingleAddressField) == Config.Settings.defaultTrue) && (row.Key == Config.Settings.columnAddress))
                    {
                        // Set value as string
                        string valueString = value.ToString();

                        // Address column must contain separation character
                        if (!(valueString.Contains(Settings.Data.GetValue(Config.Settings.excelSeparatorCharacter))))
                        {
                            error = true;
                            mainForm.AddStatusNewLine(errorMessage + Config.StatusMessages.excelErrorMissingSeparationCharacter);
                            continue;
                        }

                        // Get split characeters
                        char[] splitChar = Settings.Data.GetValue(Config.Settings.excelSeparatorCharacter).ToCharArray();

                        // To many split characters
                        if (valueString.Split(splitChar).Length - 1 != 1)
                        {
                            error = true;
                            mainForm.AddStatusNewLine(errorMessage + Config.StatusMessages.excelErrorMissingSeparationCharacterOnce);
                            continue;
                        }

                        // Explode value by seprartion character
                        string[] explode = valueString.Split(splitChar, StringSplitOptions.None);

                        // Verify first explode is a number
                        if (!(Functions.Numbers.IsStringPostiveInteger(explode[0])))
                        {
                            error = true;
                            mainForm.AddStatusNewLine(errorMessage + Config.StatusMessages.excelErrorMissingSeparationCharacterUniverseNotNumber);
                            continue;
                        }

                        // Verify second explode is a number
                        if (!(Functions.Numbers.IsStringPostiveInteger(explode[1])))
                        {
                            error = true;
                            mainForm.AddStatusNewLine(errorMessage + Config.StatusMessages.excelErrorMissingSeparationCharacterAddressNotNumber);
                            continue;
                        }
                    }

                    // When using both fixture and channel number one is allowed to be empty.
                    if (Settings.Data.GetValue(Config.Settings.excelOnlyFixtureNumber) != Config.Settings.defaultTrue)
                    {
                        // Check for valid fixture number
                        if (row.Key == Config.Settings.columnFixtureNumber)
                        {
                            // Check for valid postive integer
                            if (!(Functions.Numbers.IsStringPostiveInteger(value)))
                            {
                                // Set fixture number to invalid
                                validFixture = false;

                                // If value is not null throw error. Only postive integer values allowed.
                                if ((!(string.IsNullOrEmpty(value))))
                                {
                                    error = true;
                                    mainForm.AddStatusNewLine(errorMessage + Config.StatusMessages.excelErrorMustBePostiveNumber);
                                    continue;
                                }
                            }
                        }

                        // Check for valid channel number
                        if (row.Key == Config.Settings.columnChannelNumber)
                        {
                            // Check for valid postive integer
                            if (!(Functions.Numbers.IsStringPostiveInteger(value)))
                            {
                                // Set fixture channel to invalid
                                validChannel = false;

                                // If value is not null throw error. Only postive integer values allowed.
                                if ((!(string.IsNullOrEmpty(value))))
                                {
                                    error = true;
                                    mainForm.AddStatusNewLine(errorMessage + Config.StatusMessages.excelErrorMustBePostiveNumber);
                                    continue;
                                }
                            }
                        }
                    }
                }

                // Error if using both fixture and channel number and both are invalid
                if ((Settings.Data.GetValue(Config.Settings.excelOnlyFixtureNumber) != Config.Settings.defaultTrue) && (!(validFixture)) && (!(validChannel)))
                {
                    error = true;
                    mainForm.AddStatusNewLine(Config.StatusMessages.excelError + currentRow + Config.StatusMessages.excelEmptyFixtureAndChannel);
                }

                // Increment current row count
                currentRow++;
            }

            // Show error message box
            if (error)
                MessageBox.Show(Config.ErrorMessages.excelInvalidData);

            // Return error status
            return error;
        }
    }
}

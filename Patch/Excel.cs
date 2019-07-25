using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using GelFrame.Excel;

namespace GelFrame.Patch
{
    class Excel
    {
        // Delcare data objects and lists
        private readonly CellsToArray excelObject;
        private readonly List<string> columnNames;
        private readonly List<string> enabledColumns;
        private readonly MainForm mainForm;

        public Excel(MainForm mainForm)
        {
            // Clear all patch data
            Data.ClearAll();

            // Set main form
            this.mainForm = mainForm;

            // Get excel file path 
            string filePath = mainForm.GetPatchFilePath();

            // Set status
            mainForm.AddStatusNewLine(Config.StatusMessages.headerReadExcel + filePath);

            // Verify patch file extists
            if (!(File.Exists(filePath)))
            {
                mainForm.AddStatusNewLine(Config.ErrorMessages.patchFileNotFound + " " + filePath);
                MessageBox.Show(Config.ErrorMessages.patchFileNotFound + "\n" + filePath);
                return;
            }

            // Only allow excel file extentsion
            if (!(Config.Patch.allowedPatchFileExtentsions.Contains(Path.GetExtension(filePath))))
            {
                mainForm.AddStatusNewLine(Config.ErrorMessages.invalidPatchFileType + " " + filePath);
                MessageBox.Show(Config.ErrorMessages.invalidPatchFileType + "\n" + filePath);
                return;
            }

            // Create Excel object
            excelObject = new CellsToArray(filePath);

            // Abort if any Excel error found
            if (excelObject.FoundError())
            {
                mainForm.AddStatusNewLine(Config.StatusMessages.excelFileErrors);
                return;
            }

            // Get column names
            columnNames = excelObject.GetColumnNameList();

            // Get list of enabled columns
            enabledColumns = Settings.Data.GetEnabledColumnsList();

            // Check for missing column headers and abort if errors are found
            if (ErrorCheckColumnHeaders())
                return;

            // Process excel row data and store in patch data object
            ProcessData();

            // Verify data and abort if errors found
            if (Verify.ErrorCheckData(mainForm))
            {
                // Clear all patch data
                Data.ClearAll();

                // Save empty patch data to XML
                Save.XML(filePath);

                // Abort
                return;
            }

            // Save patch data to XML
            Save.XML(filePath);

            // Print found fixture types to status box
            foreach (string fixture in Data.GetFixtureList())
            {
                mainForm.AddStatusNewLine(Config.StatusMessages.excelFoundFixtureType + fixture);
            }

            // Completed status
            mainForm.AddStatusNewLine(Config.StatusMessages.done);
        }

        /// <summary>
        /// Process excel row data and store in patch data object
        /// </summary>
        private void ProcessData()
        {
            // Loop though all excel values
            foreach (Dictionary<string, string> excelRowDictionary in excelObject.ReturnList())
            {
                // Vars
                string fixtureName = null;
                string mode = null;

                // Delcare row dictionary
                Dictionary<string, string> outputRowDictionary = new Dictionary<string, string>();

                // Loop through each excel row
                foreach (KeyValuePair<string, string> row in excelRowDictionary)
                {
                    // Get column name from user defined column name
                    string columnName = Settings.Data.GetKey(row.Key);

                    // Only add enabled columns
                    if (enabledColumns.Contains(columnName))
                    {
                        // Use settings key instead of user defined key from Excel column header
                        outputRowDictionary.Add(columnName, row.Value);

                        // Set fixture name for grid fixture type list
                        if (columnName == Config.Settings.columnFixtureName)
                            fixtureName = row.Value;

                        // Set mode for grid fixture type list
                        if (columnName == Config.Settings.columnMode)
                            mode = row.Value;
                    }
                }

                // Add row dictionary to excel data object
                Data.PatchListAdd(outputRowDictionary);

                // Add name to excel fixture data object
                Data.FixtureListAdd(Functions.Strings.GenerateGridFixtureName(fixtureName, mode));
            }
        }

        /// <summary>
        /// Check that all required headers are present in Excel file
        /// </summary>
        /// <returns></returns>
        private bool ErrorCheckColumnHeaders()
        {
            // Set error bool default to false
            bool error = false;

            // Setup colmun name error message string
            string colmunNameErrorMessage = "";

            // Check that required column names are present
            foreach (string columnName in enabledColumns)

                // Append column name to error message string if required column name is missing from Excel file
                if (!(columnNames.Contains(Settings.Data.GetValue(columnName).ToLower())))
                    colmunNameErrorMessage += "\n  - " + Settings.Data.GetValue(columnName);

            // Exit with error message if excel file is missing 
            if (colmunNameErrorMessage.Length > 0)
            {
                error = true;
                mainForm.AddStatusNewLine(Config.ErrorMessages.excelMissingColumns + " " + colmunNameErrorMessage);
                MessageBox.Show(Config.ErrorMessages.excelMissingColumns + colmunNameErrorMessage);
            }

            // Return error status
            return error;
        }
    }
}

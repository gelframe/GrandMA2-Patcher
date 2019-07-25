using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GelFrame.Mapping
{
    static class Grid
    {
        /// <summary>
        /// Update fixture profile mappings from current grid data
        /// </summary>
        /// <param name="mainForm"></param>
        public static void UpdateFromForm(MainForm mainForm)
        {
            // Set status
            mainForm.AddStatusNewLine(Config.StatusMessages.headerRedrawGrid);

            // Declare local dictionary
            Dictionary<string, string> currentSelections = new Dictionary<string, string>();

            // Record current grid data with no MA row numbers
            foreach (DataGridViewRow row in mainForm.GetGridData())
                currentSelections.Add(row.Cells[0].Value.ToString(), Functions.Strings.StripFixtureTypeRowNumber(row.Cells[1].Value?.ToString()));

            // Execute grid data update
            Update(mainForm, currentSelections);
            
            // Save to XML
            Save.XML(mainForm);
        }

        /// <summary>
        /// Update fixture profile mappings from XML file
        /// </summary>
        /// <param name="mainForm"></param>
        public static void UpdateFromXML(MainForm mainForm)
        {
            // Set status
            mainForm.AddStatusNewLine(Config.StatusMessages.headerGridFromXML);

            // Set current selections from XML
            Dictionary<string, string> currentSelections = Read.XML();
            
            // Execute grid data update
            Update(mainForm, currentSelections);
        }

        /// <summary>
        /// Execute grid data update with given current selections.
        /// Previous profile selections are stripped of their MA row number as that number can changed depending on user changes in onPC.
        /// </summary>
        /// <param name="mainForm"></param>
        /// <param name="currentSelections"></param>
        private static void Update(MainForm mainForm, Dictionary<string, string> currentSelections)
        {
            // Declare local dictionaries
            Dictionary<string, string> fixtureTypeMapNoNumbersDictionary = new Dictionary<string, string>();

            // Build map dictionary with stripped MA row numbers as the key and new MA row numbers as the value
            foreach (string fixtureType in FixtureTypes.Data.GetFixtureTypeMapList())
                if (!(fixtureTypeMapNoNumbersDictionary.ContainsKey(Functions.Strings.StripFixtureTypeRowNumber(fixtureType))))
                    fixtureTypeMapNoNumbersDictionary.Add(Functions.Strings.StripFixtureTypeRowNumber(fixtureType), fixtureType);

            // Clear grid
            mainForm.ClearGrid();

            // Reset column data source
            mainForm.SetGridColumnDataSource();

            // Update grid data using latest patch data fixture list
            foreach (string fixtureName in Patch.Data.GetFixtureList())
            {
                // Vars
                string profileValue = null;

                // Check if current selection contains fixture name and contains a selcted value
                if ((currentSelections.ContainsKey(fixtureName)) && (currentSelections[fixtureName] != null))

                    // Check that the fixture map with no numbers dictionary contains the current fixture name
                    if (fixtureTypeMapNoNumbersDictionary.ContainsKey(currentSelections[fixtureName]))

                        // Set profile value to previous selected but with udpated MA row number 
                        profileValue = fixtureTypeMapNoNumbersDictionary[currentSelections[fixtureName]];

                // Add values to new row
                mainForm.AddGridRow(fixtureName, profileValue);
            }

            // Set done status
            mainForm.AddStatusInLine(Config.StatusMessages.success);
        }

        /// <summary>
        /// Get MA row number from gridConstructs fixture name from given paramters and finds matching selected profile name from grid data. 
        /// </summary>
        /// <param name="gridData">Main form fixture data grid</param>
        /// <param name="fixtureName">Name to be used in grid name construction</param>
        /// <param name="mode">Mode to be used in grid name construction. Defaults to null</param>
        /// <returns>MA Row number</returns>
        static public string GetMaRowNumberByGridName(DataGridViewRowCollection gridData, string fixtureName, string mode = null)
        {
            // Loop though fixture type grid
            foreach (DataGridViewRow row in gridData)

                // Find given fixture in grid
                if (Functions.Strings.GenerateGridFixtureName(fixtureName, mode) == row.Cells[0].Value?.ToString())

                    // Return selected profile
                    return Functions.Strings.GetFixtureTypeRowNumber(row.Cells[1].Value?.ToString());

            // Return empty on no match
            return null;
        }

    }
}

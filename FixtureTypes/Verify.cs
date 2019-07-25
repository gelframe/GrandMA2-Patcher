using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GelFrame.FixtureTypes
{
    static class Verify
    {
        /// <summary>
        /// Verfiy that all selected profile values are in the current fixture type map list
        /// </summary>
        /// <param name="mainForm"></param>
        /// <param name="fixtureDataGrid"></param>
        /// <returns></returns>
        static public bool FixtureTypeMapData(MainForm mainForm, DataGridViewRowCollection fixtureDataGrid)
        {
            // Setup error bool
            bool error = false;

            // Get fixture mapping list
            List<string> fixtureTypeMapList = Data.GetFixtureTypeMapList();

            // Loop though data grid 
            foreach (DataGridViewRow row in fixtureDataGrid)
            {
                // Get selected profile value
                string selectedProfile = row.Cells[1].Value?.ToString();

                // Error if nothing is selected
                if (selectedProfile == null)
                {
                    mainForm.AddStatusNewLine(Config.StatusMessages.buffer + Config.StatusMessages.error + row.Cells[0].Value + Config.StatusMessages.noSelection);
                    error = true;
                    continue;
                }

                // Error if selected value is no longer in fixtre type map list
                if (!(fixtureTypeMapList.Contains(row.Cells[1].Value?.ToString())))
                {
                    mainForm.AddStatusNewLine(Config.StatusMessages.buffer + Config.StatusMessages.error + row.Cells[0].Value + Config.StatusMessages.colon + selectedProfile + Config.StatusMessages.notFound);
                    error = true;
                    continue;
                }

                // Print found message if no errors found
                mainForm.AddStatusNewLine(Config.StatusMessages.buffer + Config.StatusMessages.found + selectedProfile);
            }

            // Return success status
            return error;
        }

        /// <summary>
        /// Check for unselected profiles in fixture data grid
        /// </summary>
        /// <param name="mainForm"></param>
        /// <param name="fixtureDataGrid"></param>
        /// <returns></returns>
        static public bool FixtureTypeMapNullCheck(MainForm mainForm, DataGridViewRowCollection fixtureDataGrid)
        {
            // Setup return bool
            bool error = false;

            // Loop though data grid 
            foreach (DataGridViewRow row in fixtureDataGrid)
            {
                // Get selected profile value
                string selectedProfile = row.Cells[1].Value?.ToString();

                // Error if nothing is selected
                if (selectedProfile == null)
                {
                    mainForm.AddStatusNewLine(Config.StatusMessages.buffer + Config.StatusMessages.error + row.Cells[0].Value + Config.StatusMessages.noSelection);
                    error = true;
                    continue;
                }
            }

            // Return success status
            return error;
        }
    }
}

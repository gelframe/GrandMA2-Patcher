using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GelFrame.Patch
{
    ///
    class Read
    {
        /// <summary>
        /// Read fixture types xml and load values into fixture types data
        /// </summary>
        static public string XML()
        {
            // Default status message to sucess
            string status = Config.StatusMessages.success;

            // Only process if xml file exists
            if (File.Exists(Config.Directories.XML + Config.XML.patchFileName))
            {
                // Vars
                string fixtureName = null;
                string mode = null;

                // Load XML file
                XElement xmlElement = XElement.Load(Config.Directories.XML + Config.XML.patchFileName);

                // Set file path
                Data.ExcelFilePath = xmlElement.Element(Config.XML.settingsElementFile).Value.ToString();

                // Loop through each fixture xml node
                foreach (XElement fixtureElement in xmlElement.Descendants(Config.XML.settingsElementFixtureType))
                {
                    // Define fixture dictionary to load values into
                    Dictionary<string, string> dataDictionary = new Dictionary<string, string>();

                    // Loop through elements
                    foreach (XElement element in fixtureElement.Descendants())
                    {
                        // Load element into the fixture dictionary
                        dataDictionary.Add(element.Name.LocalName, element.Value);

                        // Set fixture name for grid fixture type list
                        if (element.Name.LocalName == Config.Settings.columnFixtureName)
                            fixtureName = element.Value;

                        // Set mode for grid fixture type list
                        if (element.Name.LocalName == Config.Settings.columnMode)
                            mode = element.Value;
                    }

                    // Add local dictionary to local list
                    Data.PatchListAdd(dataDictionary);

                    // Add name to fixture list
                    Data.FixtureListAdd(Functions.Strings.GenerateGridFixtureName(fixtureName, mode));
                }
            }

            // Set status to error if file not found
            else
            {
                status = Config.StatusMessages.error + Config.StatusMessages.noFileFound;
            }

            // Return status message
            return status;
        }
    }
}

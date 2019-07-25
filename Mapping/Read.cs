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
    static class Read
    {
        /// <summary>
        /// Read mapping xml file and return dictionary of mapping selections
        /// </summary>
        /// <returns></returns>
        static public Dictionary<string, string> XML()
        {
            // Declare local dictionary
            Dictionary<string, string> currentSelections = new Dictionary<string, string>();

            // Only process if xml file exists
            if (File.Exists(Config.Directories.XML + Config.XML.fixtureTypeFileName))
            {
                // Load XML file
                XElement xmlElement = XElement.Load(Config.Directories.XML + Config.XML.mappingFileName);

                // Loop through each row xml node
                foreach (XElement mappingElement in xmlElement.Descendants(Config.XML.settingsElementRow))

                    // Add fixture name and selected profile to current selection dictionary
                    currentSelections.Add(mappingElement.Element(Config.XML.settingsElementPatch).Value.ToString(), mappingElement.Element(Config.XML.settingsElementProfile).Value.ToString());
            }

            // Return current selections
            return currentSelections;
        }
    }
}

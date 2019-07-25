using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GelFrame.Patch
{
    static class Save
    {
        /// <summary>
        /// Saves patch type data to XML
        /// </summary>
        public static void XML(string filePath)
        {
            // Setup XML Document
            XDocument xmlDocument = new XDocument(
                new XDeclaration(Config.XML.version, Config.XML.encoding, Config.XML.standAlone)
                );

            // Define XML root
            XElement xmlRoot = new XElement(Config.XML.settingsRootPatch);

            // Save file location
            xmlRoot.Add(new XElement(Config.XML.settingsElementFile, filePath));

            // Loop through each section and append to the XML root
            foreach (Dictionary<string, string> patchTypeDictionary in Data.GetPatchList())
            {
                // Adds each item in the list to the xml tree and gets its value from settings dictionary
                xmlRoot.Add(new XElement(Config.XML.settingsElementFixtureType,
                    patchTypeDictionary.Select(keyValue => new XElement(keyValue.Key, keyValue.Value))));
            }

            // Add XML root to document 
            xmlDocument.Add(xmlRoot);

            // Save xml
            xmlDocument.Save(Config.Directories.XML + Config.XML.patchFileName);
        }
    }
}

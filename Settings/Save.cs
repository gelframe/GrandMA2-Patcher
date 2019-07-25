using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GelFrame.Settings
{
    static class Save
    {
        /// <summary>
        /// Read all fields from settings form and write them to XML settings file
        /// </summary>
        static public bool XML(Dictionary<string, string> settingsDictionary)
        {
            // Setup bool
            bool foundError = false;

            // Setup XML Document
            XDocument xmlDocument = new XDocument(
                new XDeclaration(Config.XML.version, Config.XML.encoding, Config.XML.standAlone)
                );

            // Define XML root
            XElement xmlRootElement = new XElement(Config.XML.settingsRootSettings);

            // Loop through each section and append to the XML root
            foreach (KeyValuePair<string, List<string>> settingsConfig in Config.Settings.settingsConfigDictionary)

                // Adds each item in the list to the xml tree and gets its value from settings dictionary
                xmlRootElement.Add(new XElement(settingsConfig.Key,
                        from value in settingsConfig.Value
                        select new XElement(value, settingsDictionary[value])));

            // Create rules element
            XElement xmlRulesElement = new XElement(Config.XML.settingsRulesElement);

            // Loop through rules in settings dictionary
            foreach (KeyValuePair<int, SortedDictionary<string, string>> rulesDictionary in Rules.GetRulesFromSettingsDictionary(settingsDictionary))

                // Add each rule to rules element
                xmlRulesElement.Add(new XElement(Config.XML.settingsRuleElement,
                        from keyValue in rulesDictionary.Value
                        select new XElement(keyValue.Key, keyValue.Value)));

            // Add rules element to root element
            xmlRootElement.Add(xmlRulesElement);

            // Add XML root to document 
            xmlDocument.Add(xmlRootElement);

            // Save to settings.xml
            xmlDocument.Save(Config.Directories.XML + Config.XML.settingsFileName);

            // Reload settings 
            Read.XML(xmlDocument);

            // Return error boolean
            return foundError;
        }
    }
}

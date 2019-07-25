using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GelFrame.Settings
{
    static class Read
    {
        /// <summary>
        /// Read all settings from settings XML and load them into settings data
        /// </summary>
        static public string XML(XDocument xmlDocument = null)
        {
            // Vars
            XElement xmlElement = null;

            // Default status message to sucess
            string status = Config.StatusMessages.success;

            // Use given xml to avoid hitting the disk
            if (xmlDocument != null)
                xmlElement = XElement.Parse(xmlDocument.ToString());

            // Read XML from disk
            else
            {
                // Check that xml file exists
                if (File.Exists(Config.Directories.XML + Config.XML.settingsFileName))
                    xmlElement = XElement.Load(Config.Directories.XML + Config.XML.settingsFileName);

                // Set status to error if file not found
                else
                    status = Config.StatusMessages.error + Config.StatusMessages.noFileFound;
            }

            // Only process if element is not null
            if (xmlElement != null)
            {
                // Vars
                int ruleCount = 1;

                // Set each descendant to the config array
                foreach (XElement element in xmlElement.Descendants())

                    // No nested elements and ignore rules
                    if ((!(element.HasElements)) && (!(Config.Settings.rulesSuffixList.Contains(Functions.Strings.CapFirstChar(element.Name.LocalName)))))
                        Data.Set(element.Name.LocalName, (string)element);

                // Reset rules dictionary
                Data.ClearRulesDictionary();

                // Process rules
                foreach (XElement rulesElement in xmlElement.Descendants(Config.XML.settingsRuleElement))
                {
                    // Load each rule value into the rules dictionary dictionary
                    foreach (XElement ruleElement in rulesElement.Descendants())
                        Data.SetRule(ruleCount, Functions.Strings.CapFirstChar(ruleElement.Name.LocalName), (string)ruleElement);

                    // Increase rule counter
                    ruleCount++;
                }
            }

            // Return status message
            return status;
        }
    }
}

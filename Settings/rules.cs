using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GelFrame.Settings
{
    static class Rules
    {
        /// <summary>
        /// Extract rules from settings dictionary
        /// </summary>
        /// <param name="settingsDictionary">Setting dictionary</param>
        /// <returns>Dictionary containing all rules serparted by rule</returns>
        static public Dictionary<int, SortedDictionary<string, string>> GetRulesFromSettingsDictionary(Dictionary<string, string> settingsDictionary)
        {
            // Create dictionary to return
            Dictionary<int, SortedDictionary<string, string>> rulesDictionary = new Dictionary<int, SortedDictionary<string, string>>();

            // Loop through settings dictionary
            foreach (KeyValuePair<string,string> record in settingsDictionary)
            {
                // Only process rules
                if (record.Key.StartsWith(Config.Settings.rulesPrefixName))
                {
                    // Get rule number
                    int ruleNumber = Functions.Numbers.StringToPostiveInt(record.Key);

                    // Get XML name
                    string key = XMLNameFromFormName(record.Key, ruleNumber);

                    // Create new rules dictionary entry based on rule number if it does not exist
                    if (!(rulesDictionary.ContainsKey(ruleNumber)))
                        rulesDictionary.Add(ruleNumber, new SortedDictionary<string, string>());

                    // Add record to rule number dictionary
                    rulesDictionary[ruleNumber].Add(key, record.Value);
                }
            }

            // Normalise rotations value
            NormaliseRotationValues(rulesDictionary);

            // Return dictionary containing all rules
            return rulesDictionary;
        }

        /// <summary>
        /// Normalise rotation values
        /// </summary>
        /// <param name="rulesDictionary"></param>
        static public void NormaliseRotationValues(Dictionary<int, SortedDictionary<string, string>> rulesDictionary)
        {
            // Get colmun names
            string changeColumn = Functions.Strings.LowerFirstChar(Config.Settings.rulesSuffixChangeColumn);
            string changeValue = Functions.Strings.LowerFirstChar(Config.Settings.rulesSuffixChangeValue);
            
            // Loop through rules dictionary 
            foreach (KeyValuePair<int, SortedDictionary<string, string>> ruleDictionary in rulesDictionary)
            {
                // Get the value of the change colmun
                string selectedColumnValue = Config.Settings.rulesChangeColumnDictionary[ruleDictionary.Value[changeColumn]];

                // If selected colmun is a rotation column then normalise the value
                if (Config.Settings.columnRotationList.Contains(selectedColumnValue))
                    ruleDictionary.Value[changeValue] = Functions.Numbers.StringNormaliseDegree(ruleDictionary.Value[changeValue]);
            }
        }

        /// <summary>
        /// Convert form name to XMl name
        /// </summary>
        /// <param name="name">Form field name</param>
        /// <param name="ruleNumber">Rule number</param>
        /// <returns>Name used in settings XML file</returns>
        static public string XMLNameFromFormName(string name, int ruleNumber)
        {
            // Remove rule prefix and number
            name = name.Replace(Config.Settings.rulesPrefixName + ruleNumber, "");

            // Convert to camel case
            name = Functions.Strings.LowerFirstChar(name);

            // Return name for use in XML file
            return name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GelFrame.Settings
{
    class Data
    {
        /// <summary>
        ///  Dictionary with all settings
        /// </summary>
        static private readonly Dictionary<string, string> settingsDictionary = new Dictionary<string, string>();

        /// <summary>
        ///  Dictionary with all rules
        /// </summary>
        static private readonly Dictionary<int, Dictionary<string, string>> settingsRulesDictionary = new Dictionary<int, Dictionary<string, string>>();


        /// <summary>
        /// Get settings value by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        static public string GetValue(string key)
        {
            // Return value from settings data
            if (settingsDictionary.ContainsKey(key))
                return settingsDictionary[key];

            // No match in settings data
            else
            {
                // Look for default value and return if exists
                if (Config.Settings.defaultValuesDictionary.ContainsKey(key))
                    return Config.Settings.defaultValuesDictionary[key]
;
                // Return empty
                return "";
            }
        }

        /// <summary>
        /// Get settings key from value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static public string GetKey(string value)
        {
            foreach (KeyValuePair<string, string> keyValue in settingsDictionary)
            {
                if (keyValue.Value.ToLower() == value.ToLower())
                {
                    return keyValue.Key;
                }
            }

            // No key found
            return null;
        }

        /// <summary>
        /// Set settings value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        static public void Set(string key, string value)
        {
            if (settingsDictionary.ContainsKey(key))
                settingsDictionary[key] = value;
            else
                settingsDictionary.Add(key, value);
        }

        /// <summary>
        /// Set rule value
        /// </summary>
        /// <param name="ruleCount"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        static public void SetRule(int ruleCount, string key, string value)
        {
            // Create rule number dictionary record if it doesnt exist
            if (!(settingsRulesDictionary.ContainsKey(ruleCount)))
                settingsRulesDictionary.Add(ruleCount, new Dictionary<string, string>());

            // Add value to dictionary
            if (settingsRulesDictionary[ruleCount].ContainsKey(key))
                settingsRulesDictionary[ruleCount][key] = value;
            else
                settingsRulesDictionary[ruleCount].Add(key, value);
        }

        /// <summary>
        /// Get the number of rules
        /// </summary>
        /// <returns>Number of rules</returns>
        static public int GetRulesCount()
        {
            return settingsRulesDictionary.Count();
        }

        /// <summary>
        /// Get complete rules dictionary
        /// </summary>
        /// <returns>complete rules dictionary</returns>
        static public Dictionary<int, Dictionary<string, string>> GetRulesDictionary()
        {
            // Make all the necesary keys are set
            foreach (KeyValuePair<int, Dictionary<string, string>> row in settingsRulesDictionary)
            {
                foreach (string ruleName in Config.Settings.rulesSuffixList)
                {
                    if (!(row.Value.ContainsKey(ruleName)))
                        row.Value.Add(ruleName, null);
                }

            }

            // Return dictionary with all keys
            return settingsRulesDictionary;
        }

        static public void ClearRulesDictionary()
        {
            settingsRulesDictionary.Clear();
        }

        /// <summary>
        /// Get list of the user defined column names
        /// </summary>
        /// <returns></returns>
        static public List<string> GetColumnNamesList()
        {
            List<string> settingsColumnNames = new List<string>();
            foreach (string columnKey in Config.Settings.columnNameList)
                settingsColumnNames.Add(Settings.Data.GetValue(columnKey).ToLower());

            return settingsColumnNames;
        }

        /// <summary>
        /// Get a list of all enabled columns based on settings check boxes
        /// </summary>
        /// <returns></returns>
        static public List<string> GetEnabledColumnsList()
        {
            List<string> enabledColumnsList = new List<string>();

            foreach (string columnName in Config.Settings.columnNameList)
            {
                // Determine if column is enabled from settings checkbox
                if (Config.Settings.disableCheckBoxDependancies.ContainsKey(Functions.Strings.GetLabelName(columnName)))

                    // Skip required check if check box setting value is true
                    if (Settings.Data.GetValue(Config.Settings.disableCheckBoxDependancies[Functions.Strings.GetLabelName(columnName)]) == Config.Settings.defaultTrue)
                        continue;

                // Determine if column is disabled from settings checkbox
                if (Config.Settings.enableCheckBoxDependancies.ContainsKey(Functions.Strings.GetLabelName(columnName)))

                    // Skip required check if check box setting value is false
                    if (Settings.Data.GetValue(Config.Settings.enableCheckBoxDependancies[Functions.Strings.GetLabelName(columnName)]) != Config.Settings.defaultTrue)
                        continue;

                enabledColumnsList.Add(columnName);
            }

            return enabledColumnsList;
        }
    }
}

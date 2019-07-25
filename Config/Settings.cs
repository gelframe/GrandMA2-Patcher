using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace GelFrame.Config
{
    class Settings
    {
        #region VARS
        // Options
        public static readonly string optionLayerBy = "optionLayerBy";
        public static readonly string optionCopyFixturetoChannel = "optionCopyFixturetoChannel";
        public static readonly string optionAllowMultiPatch = "optionAllowMultiPatch";

        // Rotation Offsets
        public static readonly string rotationOffsetX = "rotationOffsetX";
        public static readonly string rotationOffsetY = "rotationOffsetY";
        public static readonly string rotationOffsetZ = "rotationOffsetZ";

        // Rotation Invert
        public static readonly string rotationInvertX = "rotationInvertX";
        public static readonly string rotationInvertY = "rotationInvertY";
        public static readonly string rotationInvertZ = "rotationInvertZ";

        // Rotation Mapping
        public static readonly string rotationMappingX = "rotationMappingX";
        public static readonly string rotationMappingY = "rotationMappingY";
        public static readonly string rotationMappingZ = "rotationMappingZ";

        // MA opPC
        public static readonly string maConsoleIp = "maConsoleIP";
        public static readonly string maCommandDelay = "maCommandDelay";
        public static readonly string maTimeOut = "maTimeOut";

        // Prefixes
        public static readonly string prefixRotationOffset = "rotationOffset";
        public static readonly string prefixRotationInvert = "rotationInvert";
        public static readonly string prefixRotationMapping = "rotationMapping";
        public static readonly string prefixColumnRotation = "columnRotation";
        public static readonly string prefixColumnPosition = "columnPosition";
        public static readonly string prefixColumn = "column";
        public static readonly string prefixMA = "ma";
        public static readonly string prefixOption = "option";
        public static readonly string prefixExcel = "excel";
        public static readonly string prefixColumnLabel = "labelColumn";
        public static readonly string prefixRulesSuffix = "rulesSuffix";
        public static readonly string prefixRulesLabelPosition = "rulesLabelPosition";
        public static readonly string prefixRulesLabelRotation = "rulesLabelRotation";
        public static readonly string prefixRulesSwitch = "rulesSwitch";

        // Excel Columns
        public static readonly string columnFixtureName = "columnFixtureName";
        public static readonly string columnFixtureNumber = "columnFixtureNumber";
        public static readonly string columnChannelNumber = "columnChannelNumber";
        public static readonly string columnUniverse = "columnUniverse";
        public static readonly string columnAddress = "columnAddress";
        public static readonly string columnMode = "columnMode";
        public static readonly string columnLocation = "columnLocation";
        public static readonly string columnPositionX = "columnPositionX";
        public static readonly string columnPositionY = "columnPositionY";
        public static readonly string columnPositionZ = "columnPositionZ";
        public static readonly string columnRotationX = "columnRotationX";
        public static readonly string columnRotationY = "columnRotationY";
        public static readonly string columnRotationZ = "columnRotationZ";

        // Excel options
        public static readonly string excelOnlyFixtureNumber = "excelOnlyFixtureNumber";
        public static readonly string excelSeparatorCharacter = "excelSeparatorCharacter";
        public static readonly string excelSingleAddressField = "excelSingleAddressField";
        public static readonly string excelEnableModeField = "excelEnableModeField";
        public static readonly string excelEnableLocationField = "excelEnableLocationField";
        public static readonly string excelEnablePositionFields = "excelEnablePositionFields";
        public static readonly string excelEnableRotationFields = "excelEnableRotationFields";

        // Rules
        public static readonly string rulesPrefixName = "rule";
        public static readonly string rulesPrefixTitleName = "Rule #";
        public static readonly string rulesFormSuffixDelete = "Delete";
        public static readonly string rulesFormSuffixBox = "Box";
        public static readonly string rulesSuffixColumn = "Column";
        public static readonly string rulesSuffixSwitch = "Switch";
        public static readonly string rulesSuffixPattern = "Pattern";
        public static readonly string rulesSuffixChangeColumn = "ChangeColumn";
        public static readonly string rulesSuffixChangeValue = "ChangeValue";
        public static readonly string rulesSuffixCondition = "Condition";
        public static readonly string rulesSuffixEnabled = "Enabled";
        public static readonly string rulesSuffixChange = "Change";
        public static readonly string rulesLabelCondition = "Condition:";
        public static readonly string rulesLabelChange = "Change:";
        public static readonly string rulesEnabledText = "Enabled";
        public static readonly string rulesDeleteButtonText = "X";
        public static readonly string rulesLabelPositionX = "Position X";
        public static readonly string rulesLabelPositionY = "Position Y";
        public static readonly string rulesLabelPositionZ = "Position Z";
        public static readonly string rulesLabelRotationX = "Rotation X";
        public static readonly string rulesLabelRotationY = "Rotation Y";
        public static readonly string rulesLabelRotationZ = "Rotation Z";
        public static readonly string rulesLabelChannelNumber = "Channel #";
        public static readonly string rulesLabelFixtureNumber = "Fixture #";
        public static readonly string rulesSwitchContains = "Contains";
        public static readonly string rulesSwitchEndsWith = "Ends With";
        public static readonly string rulesSwitchEquals = "Equals";
        public static readonly string rulesSwitchStartsWith = "Starts With";
        public static readonly string rulesRangeSwitch = "Range";

        // Rule message boxes
        public static readonly string rulesMessageDeleteRuleHeader = "Delete Rule #{0}";
        public static readonly string rulesMessageDeleteRule = "Are you sure you want to delete rule #{0}?";
        public static readonly string rulesMessageDeleteAllRulesHeader = "Delete All Rules";
        public static readonly string rulesMessageDeleteAllRules = "Are you sure you want to delete all the rules?";

        // Default values
        public static readonly string defaultX = "X";
        public static readonly string defaultY = "Y";
        public static readonly string defaultZ = "Z";
        public static readonly string defaultZero = "0";
        public static readonly string defaultTrue = "True";
        public static readonly string defaultFalse = "False";
        #endregion

        #region LISTS
        /// <summary>
        /// List of all column names sorted alphabetically
        /// </summary>
        public static readonly List<string> columnNameList = BuildList(prefixColumn).OrderBy(x => x).ToList();

        /// <summary>
        /// List of all rotation offset names
        /// </summary>
        public static readonly List<string> rotationOffsetNameList = BuildList(prefixRotationOffset);

        /// <summary>
        /// List of all rotation mapping names
        /// </summary>
        public static readonly List<string> rotationMappingNameList = BuildList(prefixRotationMapping);

        /// <summary>
        /// List of all rotation invert names
        /// </summary>
        public static readonly List<string> rotationInvertNameList = BuildList(prefixRotationInvert);

        /// <summary>
        /// List of all rotation columns
        /// </summary>
        public static readonly List<string> columnRotationList = BuildList(prefixColumnRotation);

        /// <summary>
        /// List of all position columns
        /// </summary>
        public static readonly List<string> columnPositionList = BuildList(prefixColumnPosition);

        /// <summary>
        /// List of all rules sufix columns
        /// </summary>
        public static readonly List<string> rulesSuffixList = BuildList(prefixRulesSuffix);

        /// <summary>
        /// List of all position and rotation fields
        /// </summary>
        public static readonly List<string> columnXYZList = columnRotationList.Concat(columnPositionList).ToList();
        
        /// <summary>
        /// List of all rules change columns
        /// </summary>
        public static readonly List<string> rulesChangeColumnLabelList = BuildList(prefixRulesLabelPosition).Concat(BuildList(prefixRulesLabelRotation)).ToList();

        /// <summary>
        /// List of all rules change columns
        /// </summary>
        public static readonly List<string> rulesSwitchList = BuildList(prefixRulesSwitch);

        /// <summary>
        /// List of columns in Excel that are strings
        /// </summary>
        public static readonly List<string> settingsExcelStrings = new List<string>()
        {
            columnFixtureName,
            columnMode,
            columnLocation,
        };

        /// <summary>
        /// List of rotaion mapping values
        /// </summary>
        public static readonly List<string> settingsRotaionMappingValueList = new List<string>()
        {
            defaultX,
            defaultY,
            defaultZ,
        };

        /// <summary>
        /// List of columns in Excel that cannot be empty
        /// </summary>
        public static readonly List<string> settingsExcelStringsCannotBeEmpty = new List<string>()
        {
            columnFixtureName,
        };

        /// <summary>
        /// List of columns in Excel that must be doubles
        /// </summary>
        public static readonly List<string> settingsExcelDoubles = new List<string>()
        {
            columnPositionX,
            columnPositionY,
            columnPositionZ,
            columnRotationX,
            columnRotationY,
            columnRotationZ,
        };

        /// <summary>
        /// List of columns in Excel that must be postive integers
        /// </summary>
        public static readonly List<string> settingsExcelPostiveInteger = new List<string>()
        {
            columnUniverse,
        };

        /// <summary>
        /// List of columns that are a range for rules
        /// </summary>
        public static readonly List<string> rulesRangeList = new List<string>()
        {
            rulesLabelChannelNumber,
            rulesLabelFixtureNumber,
        };

        /// <summary>
        /// List of columns that are a range for rules
        /// </summary>
        public static readonly List<string> rulesRangeDataList = new List<string>()
        {
            rulesRangeSwitch,
        };
        #endregion

        #region DICTIONARIES
        /// <summary>
        /// Dictionary of default values
        /// </summary>
        public static readonly Dictionary<string, string> defaultValuesDictionary = new Dictionary<string, string>()
        {
            { rotationMappingX, defaultX },
            { rotationMappingY, defaultY },
            { rotationMappingZ, defaultZ },
            { rotationOffsetX, defaultZero },
            { rotationOffsetY, defaultZero },
            { rotationOffsetZ, defaultZero },
        };

        /// <summary>
        /// Dictionary that maps rotation combo box values to rotation columns
        /// </summary>
        public static readonly Dictionary<string, string> rotationMappingLookupDictionary = new Dictionary<string, string>()
        {
            { defaultX, columnRotationX },
            { defaultY, columnRotationY },
            { defaultZ, columnRotationZ },
        };

        /// <summary>
        /// Dictionary that maps rotation combo box values to rotation columns
        /// </summary>
        public static readonly Dictionary<string, string> rotationMappingToColumnDictionary = new Dictionary<string, string>()
        {
            { rotationMappingX, columnRotationX },
            { rotationMappingY, columnRotationY },
            { rotationMappingZ, columnRotationZ },
        };


        /// <summary>
        /// Dictionary that maps rotation column names to rotation offset names
        /// </summary>
        public static readonly Dictionary<string, string> rotationOffsetLookupDictionary = new Dictionary<string, string>()
        {
            { columnRotationX, rotationOffsetX },
            { columnRotationY, rotationOffsetY },
            { columnRotationZ, rotationOffsetZ },
        };

        /// <summary>
        /// Dictionary that maps rotation column names to rotation invert names
        /// </summary>
        public static readonly Dictionary<string, string> rotationInvertLookupDictionary = new Dictionary<string, string>()
        {
            { columnRotationX, rotationInvertX },
            { columnRotationY, rotationInvertY },
            { columnRotationZ, rotationInvertZ },
        };

        /// <summary>
        /// Dictionary that maps rules change columns
        /// </summary>
        public static readonly Dictionary<string, string> rulesChangeColumnDictionary = new Dictionary<string, string>()
        {
            { rulesLabelPositionX, columnPositionX },
            { rulesLabelPositionY, columnPositionY },
            { rulesLabelPositionZ, columnPositionZ },
            { rulesLabelRotationX, columnRotationX },
            { rulesLabelRotationY, columnRotationY },
            { rulesLabelRotationZ, columnRotationZ },
        };

        /// <summary>
        /// Dictionary of all setting options
        /// </summary>
        public static readonly Dictionary<string, List<string>> settingsConfigDictionary = new Dictionary<string, List<string>>()
        {
            {prefixOption, BuildList(prefixOption) },
            {prefixRotationMapping, BuildList(prefixRotationMapping) },
            {prefixRotationOffset, BuildList(prefixRotationOffset) },
            {prefixRotationInvert, BuildList(prefixRotationInvert) },
            {prefixMA, BuildList(prefixMA) },
            {prefixColumn, BuildList(prefixColumn) },
            {prefixExcel, BuildList(prefixExcel) },
        };

        /// <summary>
        /// Enable these fields when checkbox is true
        /// </summary>
        public static readonly Dictionary<string, string> enableCheckBoxDependancies = new Dictionary<string, string>()
        {
            { Functions.Strings.GetLabelName(excelSeparatorCharacter), excelSingleAddressField},
            { Functions.Strings.GetLabelName(columnMode), excelEnableModeField},
            { Functions.Strings.GetLabelName(columnLocation), excelEnableLocationField},
            { Functions.Strings.GetLabelName(columnPositionX), excelEnablePositionFields},
            { Functions.Strings.GetLabelName(columnPositionY), excelEnablePositionFields},
            { Functions.Strings.GetLabelName(columnPositionZ), excelEnablePositionFields},
            { Functions.Strings.GetLabelName(columnRotationX), excelEnableRotationFields},
            { Functions.Strings.GetLabelName(columnRotationY), excelEnableRotationFields},
            { Functions.Strings.GetLabelName(columnRotationZ), excelEnableRotationFields},
        };

        /// <summary>
        /// Disable these fields when checkbox is true
        /// </summary>
        public static readonly Dictionary<string, string> disableCheckBoxDependancies = new Dictionary<string, string>()
        {
            { Functions.Strings.GetLabelName(columnChannelNumber), excelOnlyFixtureNumber},
            { Functions.Strings.GetLabelName(columnUniverse), excelSingleAddressField},
        };
        #endregion

        #region Build Lists
        // Build data list from the static vars defined in this class and attributes
        private static List<string> BuildList(string patternToMatch)
        {
            // Initailize collection
            List<string> list = new List<string>();

            // Blocks
            FieldInfo[] fields = typeof(Settings).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
                if (field.FieldType == typeof(string))
                    if (field.Name.StartsWith(patternToMatch))
                        list.Add(field.GetValue(null).ToString());

            return list;
        }
        #endregion
    }
}

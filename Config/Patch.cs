using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace GelFrame.Config
{
    static class Patch
    {
        // Browse file selector filters
        public static readonly string selectDialogFilter = "Excel Files|*.xls;*.xlsx;*.xlsm";
        public static readonly string selectDialogTitle = "Select Excel file";

        // Layer organization options
        public static readonly string layerByFixtureType = "By Fixture Type";
        public static readonly string layerByLocation = "By Location";
        public static readonly string layerByUniverse = "By Universe";

        // Default anme
        public static readonly string defaultLayerName = "Layer";

        // Prefixes
        public static readonly string prefixUniverse = "Universe: ";

        /// <summary>
        /// List of all layer organization options
        /// </summary>
        public static List<string> layerOrganizationList = new List<string>(BuildList("layer"));

        /// <summary>
        /// File extentsions allowed for the patch file. Must be Excel compatible files. 
        /// </summary>
        public static List<string> allowedPatchFileExtentsions = new List<string>()
        {
            ".xls",
            ".xlsx",
            ".xlsm",
        };

        #region Build Lists
        // Build data list from the static vars defined in this class and attributes
        private static List<string> BuildList(string patternToMatch)
        {
            // Initailize collection
            List<string> list = new List<string>();

            // Blocks
            FieldInfo[] fields = typeof(Patch).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
                if (field.FieldType == typeof(string))
                    if (field.Name.StartsWith(patternToMatch))
                        list.Add(field.GetValue(null).ToString());

            return list;
        }
        #endregion
    }
}

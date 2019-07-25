using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace GelFrame.Config
{
    static class Fixture
    {
        // MA on PC telnet message grid 
        public static readonly string typeNumber = "No.";
        public static readonly string typeLongName = "LongName";
        public static readonly string typeShortName = "ShortName";
        public static readonly string typeMan = "Manufacturer";
        public static readonly string typeShortMan = "ShortManu";
        public static readonly string typeDMXFootprint = "DMXFootprint";
        public static readonly string typeInstances = "Instances";
        public static readonly string typeMode = "Mode";
        public static readonly string typeUsed = "Used";
        public static readonly string typeXYZ = "XYZ";
        public static readonly string typeRDMFixtureType = "RDMFixtureType";

        // MA xml profiles
        public static readonly string fileType = "xml";
        public static readonly string matchFileType = "*.";
        public static readonly string seperator = " - ";

        // Module Keys
        public static readonly string moduleBody = "moduleBody";
        public static readonly string moduleChannelCount = "moduleChannelCount";

        /// <summary>
        /// List of all fixture type columns
        /// </summary>
        public static readonly List<string> typeList = BuildList("type");

        #region Build Lists
        // Build data list from the static vars defined in this class and attributes
        private static List<string> BuildList(string patternToMatch)
        {
            // Initailize collection
            List<string> list = new List<string>();

            // Blocks
            FieldInfo[] fields = typeof(Fixture).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
                if (field.FieldType == typeof(string))
                    if (field.Name.StartsWith(patternToMatch))
                        list.Add(field.GetValue(null).ToString());

            return list;
        }
        #endregion
    }
}

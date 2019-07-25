using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GelFrame.Functions
{
    static class Strings
    {
        /// <summary>
        /// Captialize first character. Used to create proper camel case names
        /// </summary>
        /// <param name="text">String to convert</param>
        /// <returns>String with uppercase first character</returns>
        static public string CapFirstChar(string text)
        {
            return Char.ToUpperInvariant(text[0]) + text.Substring(1);
        }

        /// <summary>
        /// Lowercase first character. Used to create proper camel case names
        /// </summary>
        /// <param name="text">String to convert</param>
        /// <returns>String with uppercase first character</returns>
        static public string LowerFirstChar(string text)
        {
            return Char.ToLowerInvariant(text[0]) + text.Substring(1);
        }

        /// <summary>
        /// Convert rule name to settings column key
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static public string RuleNameToColmunKey(string name)
        {
            if (!(string.IsNullOrEmpty(name)))
            {
                name = name.Replace(" ", "");
                name = name.Replace("#", "Number");
                name = Config.Settings.prefixColumn + name;
            }

            return name;
        }

        /// <summary>
        /// Generates common label name from string
        /// </summary>
        /// <param name="text">String to add label to</param>
        /// <returns>Proper camel case string with label name in front</returns>
        public static string GetLabelName(string text)
        {
            return Config.App.lableText + Functions.Strings.CapFirstChar(text);
        }

        /// <summary>
        /// Remove MA fixure type row number from string
        /// </summary>
        /// <param name="fixtureType">String with MA row number at the end</param>
        /// <returns>String without MA row number</returns>
        public static string StripFixtureTypeRowNumber(string fixtureType)
        {
            // Return null if input is null
            if (String.IsNullOrEmpty(fixtureType))
                return null;

            // Explode incoming string by fixture seperator
            string[] explode = fixtureType.Split(new[] { Config.Fixture.seperator }, StringSplitOptions.None);

            // Remove last element which is the MA fixture row number
            Array.Resize(ref explode, explode.Length - 1);

            // Join string
            fixtureType = string.Join("", explode);

            // Return exploded array as a string
            return fixtureType;
        }

        public static string GetFixtureTypeRowNumber(string fixtureName)
        {
            // Return null if input is null
            if (String.IsNullOrEmpty(fixtureName))
                return null;

            // Explode incoming string by fixture seperator
            string[] explode = fixtureName.Split(new[] { Config.Fixture.seperator }, StringSplitOptions.None);

            // Return last element
            return explode[explode.Length - 1].Trim();
        }

        /// <summary>
        /// Create grid fixture name
        /// Appends mode to fixture name if mode is not null
        /// </summary>
        /// <param name="name">Fixture name</param>
        /// <param name="mode">Mode</param>
        /// <returns>Fixture name with mode in parenthesis</returns>
        public static string GenerateGridFixtureName(string name, string mode = null)
        {
            string gridFixtureName = name;

            if (!(String.IsNullOrEmpty(mode)))
                gridFixtureName += " (" + mode + ")";

            return gridFixtureName;
        }


    }
}


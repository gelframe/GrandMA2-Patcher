using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GelFrame.Functions
{
    static class Numbers
    {
        /// <summary>
        /// Converts string to int. 
        /// Strips out any non numberic character 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int StringToPostiveInt(string input)
        {
            var numericChars = "0123456789".ToCharArray();
            string textString = new String(input.Where(c => numericChars.Any(n => n == c)).ToArray());
            return System.Convert.ToInt32(textString);
        }

        /// <summary>
        /// Normalise given value between 0 and 360 degrees.
        /// </summary>
        /// <param name="stringDegree">Value to process as string</param>
        /// <returns>Calucaled value as string</returns>
        public static string StringNormaliseDegree(string stringDegree)
        {
            // Convert string to double and normalize 0 to 360
            double doubleDegree = StringToDouble(stringDegree) % 360;

            // Convert negative degrees to postive
            if (doubleDegree < 0) doubleDegree += 360;

            // Subtract 360 if greater than 180
            if (doubleDegree > 180) doubleDegree -= 360;

            // Return calucaled value as string
            return doubleDegree.ToString();
        }

        public static bool IsStringPostiveInteger(string number)
        {
            // Check if string is a valid double
            if (Double.TryParse(number, out double doubleValue))

                // Check if number is a valid int that is not negative
                if ((doubleValue > 0) && unchecked(doubleValue == (int)doubleValue))
                    return true;

            return false;
        }

        public static double StringToDouble(string number)
        {
            // Check if string is a valid double
            if (Double.TryParse(number, out double doubleValue))
                return doubleValue;

            return 0;
        }

        public static bool IsStringDouble(string number)
        {
            // Check if string is a valid double
            if (Double.TryParse(number, out _))
                return true;

            return false;
        }

        /// <summary>
        /// Validate rules range. Returns true on error
        /// </summary>
        /// <param name="value">Range as a string</param>
        /// <returns>True on error</returns>
        static public bool ValidateRange(string value)
        {
            // Allow empty value
            if (!(string.IsNullOrEmpty(value)))
            {
                // Explode value
                string[] explode = value.Split(',');

                foreach (string range in explode)
                {
                    // Process range
                    if (range.Contains("-"))
                    {
                        // Explode range
                        string[] rangeExplode = range.Split('-');

                        // Range should only have one dash
                        if (rangeExplode.Length > 2)
                            return true;

                        // Check for whole postive interger
                        foreach (string number in rangeExplode)
                        {
                            if (!(IsStringPostiveInteger(number)))
                                return true;
                        }

                        // First value must be smaller than second value
                        if (StringToDouble(rangeExplode[0]) > StringToDouble(rangeExplode[1]))
                            return true;
                    }

                    // Process single value
                    else
                    {
                        // Check for whole postive interger
                        if (!(IsStringPostiveInteger(range)))
                            return true;
                    }
                }
            }

            // No errors found, range is valid
            return false;
        }

        /// <summary>
        /// Create range list from string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static public List<string> CreateListFromRange(string value)
        {
            // Create empty range list
            List<string> rangeList = new List<string>();

            // Validate range
            if ((ValidateRange(value)) || (string.IsNullOrEmpty(value)))
                return rangeList;

            // Explode range
            string[] explode = value.Split(',');

            // Loop through each range
            foreach (string range in explode)
            {
                // Process range
                if (range.Contains("-"))
                {
                    // Explode range
                    string[] rangeExplode = range.Split('-');

                    // Loop through range and add each value to list
                    for (double i = StringToDouble(rangeExplode[0]); i <= StringToDouble(rangeExplode[1]); i++)
                    {
                        if (!(rangeList.Contains(i.ToString())))
                            rangeList.Add(i.ToString());
                    }
                }

                // Process single value
                else
                {
                    if (!(rangeList.Contains(range)))
                        rangeList.Add(range);
                }
            }

            // Return range list
            return rangeList;
        }

        /// <summary>
        /// Check if value is in given range
        /// </summary>
        /// <param name="value"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        static public bool IsValueInRange(string value, string range)
        {
            // Check value against range
            if (CreateListFromRange(range).Contains(value))
                return true;

            // No matches, return false
            return false;
        }
    }
}

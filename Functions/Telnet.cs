using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GelFrame.Functions
{
    static class Telnet
    {
        /// <summary>
        /// Strips all telnet formatting from given string
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns>String without formatting</returns>
        public static string RemoveFormatting(string inputString)
        {
            string cleanLine = new string(inputString.Where(c => !char.IsControl(c)).ToArray());
            Regex telnetFormatting = new Regex(@"\[([0-9][0-9])m");
            cleanLine = telnetFormatting.Replace(cleanLine, "");

            return cleanLine;
        }
    }
}

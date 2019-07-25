using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GelFrame.Config
{
    static class ErrorMessages
    {
        // Telnet
        public static readonly string telnetConnection = "Connection Error.\n - Is MA onPC Running?\n - Is TelNet Enabled?\n - Is the IP address correct?";

        // Settings
        public static readonly string saveSettings = "Error saving settings. Please try again.";
        public static readonly string locationNotEnabled = "Location column must be enabled in settings in order to organize layers by location";
        public static readonly string noSettingsFiles = "No setting file was found. Please complete all the settings options prior to use.";
        public static readonly string emptySettings = "The following settings cannot be empty:";
        public static readonly string invalidRange = "Invalid Range \"{0}\"\n" +
            " - Single whole numbers or a range with a dash\n" +
            " - Seperate ranges with a comma\n" +
            " - First number must be less than the second number in a range.\n" +
            " - Example: 101-121,450,231-240";

        // MA
        public static readonly string cannotEnterSetup = "Cannot enter setup. Is MA onPC in the setup menu?";
        public static readonly string missingMaDataDirectory = "Cannot locate MA Program Data directory.\n - Is the data directory setting correct?";
        public static readonly string wrongMaDataDirectory = "Error verifying imported onPC profiles\n - Does the data directory setting match the running version of onPC?";

        // Excel
        public static readonly string excelMissingColumns = "Excel file is missing required columns:";
        public static readonly string excelInvalidData = "There is invalid data in the Excel file. Please see the status box for detailed infomation.";
        public static readonly string excelDuplicateColumnNames = "Excel file cannot contain duplicate column headers.\nFound more than one column named: ";
        public static readonly string excelEmptyHeader= "Excel file cannot contain empty headers. Column: ";
        public static readonly string invalidPatchFileType = "Only Excel files are supported";
        public static readonly string patchFileNotFound = "Patch file not found.";
    }
}

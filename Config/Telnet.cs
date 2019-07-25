using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using GelFrame.Settings;

namespace GelFrame.Config
{
    static class Telnet
    {
        // onPC options
        public static readonly int consolePort = 30000;

        // Commands
        public static readonly string commandLogin = "Login administrator admin";
        public static readonly string commandConfirmOverWrite = "1";
        public static readonly string commandChangeDirectory = "cd ";
        public static readonly string commandEnterSetup = "editsetup";
        public static readonly string commandFixtureTypes = "fixturetypes";
        public static readonly string commandList = "list";
        public static readonly string commandSelectDrive = "sd";
        public static readonly string commandSlash = "/";
        public static readonly string commandImportPatch = "import " + Config.XML.exportXMLtoMAFileName + " at layers";

        // Messages returned in stream reader
        public static readonly string streamEnterSetupError = "Error #22: CANNOT ENTER DESTINATION";
        public static readonly string streamLoggedInMessage = "Logged in as User 'administrator'";
        public static readonly string streamChaneDest = "ChangeDest";
        public static readonly string streamSlash = commandSlash;
        public static readonly string streamEditSetup = "ChangeDest editsetup";
        public static readonly string streamFixtureType = "FixtureType";
        public static readonly string streamExport = "export ";
        public static readonly string streamXMLImported = "1 object(s) from \"" + Config.XML.exportXMLtoMAFileName + "\" imported.";
        public static readonly string streamInternalStart = "1 : name='Internal' path='";
        public static readonly string streamInternalEnd = "/shows'";

        // Status switches
        public static readonly string statusSuccess = "statusSuccess";
        public static readonly string statusCannotEnterSetup = "statusCannotEnterSetup";
        public static readonly string statusRetry = "statusRetry";
        public static readonly string statusConnectionError = "statusConnectionError";

        /// <summary>
        /// List of commands used to get onPC data directory
        /// </summary>
        public static readonly List<string> dataDirectoryCommands = new List<string>()
        {
            commandSelectDrive,
            commandChangeDirectory + commandSlash,
        };

        /// <summary>
        /// List of commands used to export fixture types table
        /// </summary>
        public static readonly List<string> fixtureTypeCommands = new List<string>()
        {
            commandChangeDirectory + commandEnterSetup,
            commandChangeDirectory + commandFixtureTypes,
            commandList,
            commandChangeDirectory + commandSlash,
        };

        /// <summary>
        /// List of commands to enter setup
        /// </summary>
        public static readonly List<string> editSetupCommand = new List<string>()
        {
            commandChangeDirectory + commandEnterSetup
        };

        /// <summary>
        /// List of commands to import xml patch
        /// </summary>
        public static readonly List<string> importCommand = new List<string>()
        {
            commandImportPatch
        };

        /// <summary>
        /// List of commands to change to the root directory
        /// </summary>
        public static readonly List<string> rootDirectoryCommand = new List<string>()
        {
            commandChangeDirectory + commandSlash
        };

        /// <summary>
        /// List of commands to start fixture type export
        /// </summary>
        public static readonly List<string> fixtureTypeExportStartCommands = new List<string>()
        {
            commandChangeDirectory + commandEnterSetup,
            commandChangeDirectory + commandFixtureTypes,
        };
    }
}

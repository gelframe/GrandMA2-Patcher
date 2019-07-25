using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GelFrame.Config
{
    static class StatusMessages
    {
        // Appendable
        public static readonly string dots = "...";
        public static readonly string colon = ": ";
        public static readonly string buffer = "   + ";

        // Prefixes
        public static readonly string error = "Error: ";
        public static readonly string skipping = "Skipping: ";
        public static readonly string loading = "Loading ";
        public static readonly string found = "Found: ";

        // Success and completion
        public static readonly string success = "Success!";
        public static readonly string done = buffer + "Done!";

        // Not Found
        public static readonly string noFileFound = "No file found.";
        public static readonly string notFound = " not found :(";
        public static readonly string noSelection = " has no profile selected";

        // Loading settings
        public static readonly string loadingXMlSettings = loading + "settings" + dots;
        public static readonly string loadingXMlFixtureData = loading + "previous GrandMA fixture data" + dots;
        public static readonly string loadingXMlFixtureProfileData = loading + "previous fixture profiles" + dots;
        public static readonly string loadingXMlPatchData = loading + "previous patch" + dots;
        public static readonly string loadingXMlGridData = loading + "previous fixture mapping grid" + dots;

        // Settings 
        public static readonly string settingsUpdated = "Settings updated!";

        // Telnet
        public static readonly string telNetStartingConnection = "Starting telnet connection to MA onPC";
        public static readonly string telNetCommands = "Sendng telnet commands";
        public static readonly string telNetFixtureFound = "Found fixture type: ";
        public static readonly string telNetConnectionError = "Telnet connection error";
        public static readonly string telNetErrorEnterSetup = "Telnet error: Cannot enter setup menu";
        public static readonly string telNetStreamError = "Telnet stream error. Please try again.";

        // Excel patch file
        public static readonly string headerReadExcel = "Processing Excel patch file: ";
        public static readonly string excelFoundFixtureType = buffer + "Found fixture type: ";

        // Excel patch data verification errors
        public static readonly string excelError = buffer + "Excel Error. Row: ";
        public static readonly string excelErrorEmpty = " cannot be empty.";
        public static readonly string excelErrorMustBeNumber = " must be a number.";
        public static readonly string excelErrorMustBePostiveNumber = " must be a postive number with no decimal.";
        public static readonly string excelErrorMissingSeparationCharacter = " must contain the separation character.";
        public static readonly string excelErrorMissingSeparationCharacterOnce = " must only contain the separation character once.";
        public static readonly string excelErrorMissingSeparationCharacterUniverseNotNumber = " values before the separation character must be postive numbers.";
        public static readonly string excelErrorMissingSeparationCharacterAddressNotNumber = " values after the separation character must be postive numbers.";
        public static readonly string excelFileErrors = buffer + error + "Excel file errors.";
        public static readonly string excelDuplicateNumber = " is a duplicate. Duplicate numbers/multipatch disabled in settings.";
        public static readonly string excelEmptyFixtureAndChannel = ". Either fixture or channel number must be a postive number with no decimal.";

        // onPC profile XML
        public static readonly string headerProfileXMLStart = "Processing GrandMA2 fixture profile XML files";
        public static readonly string profileXMLprocessing = "Loading: ";

        // MA data directory
        public static readonly string headerMaDataDirectory = "Loading internal data directory from MA onPC";
        public static readonly string maDataDirectoryFound = buffer + "Found: ";

        // Loading fixture types
        public static readonly string headerLoadFixtureTypes = "Loading fixture types from MA onPC";
        public static readonly string headerExportFixtureTypes = "Exporting fixture types from MA onPC";
        public static readonly string exportFixtureTypes = "Exporting: ";

        // Fixture type verify
        public static readonly string headerFixtureTypeVerify = "Verifying found fixture types match exported onPC profiles" + dots;
        public static readonly string fixtureTypeVerifyMissingProfile = buffer + "Missing profile for row number: {0} {1} {2}";

        // Fixture data grid
        public static readonly string headerRedrawGrid = "Reloading data grid" + dots;
        public static readonly string headerGridFromXML = "Loading data grid from XML" + dots;

        // Process XML to import into onPC
        public static readonly string headerProcessStart = "Starting MA patchfile generation and import process";
        public static readonly string headerProcessVerifyProfiles = "Verifying fixture types in onPC match the data in fixture mapping grid" + dots;
        public static readonly string processVerifyProfilesSuccess = "Verifying fixture types complete. " + success;
        public static readonly string headerProcessMissingColumns = "Checking for all required columns" + dots;
        public static readonly string processImportHeader = "Importing patch XML into onPC";
        public static readonly string processComplete = "MA onPC updated! Import complete!";

        // Process errors
        public static readonly string processImportAttemptFail = buffer + "Attempt failed. Increasing command delay and trying again.";
        public static readonly string processErrorReload = "Error reloading onPC fixture types. Please try again.";
        public static readonly string processErrorVerify = "Error verifiying onPC fixture types with the mapped data in the grid. Please update grid selection and try again.";
        public static readonly string processErrorNoData = "No data to process. Please load Excel data using Load Excel Data button.";
        public static readonly string processErrorOrganizeLayers = "Error organizing patch data into layers.";
        public static readonly string processFail = "Failed to import patch into onPC. Increase command delay in settings and try again.";
        public static readonly string processMissingColumns = "Invalid columns found. Reload Excel data to fix.";

        // Export XML
        public static readonly string headerExport = "Starting MA2 patch XML file creation";
        public static readonly string exportCovertDMX = buffer + "Converting DMX to MA format" + dots;
        public static readonly string exportSortPatchList = buffer + "Sorting patch data" + dots;
        public static readonly string exportCopyFixtureNumbertoChannel = buffer + "Copying fixture numbers to channel numbers" + dots;
        public static readonly string exportOrganizeLayers = buffer + "Organizing patch data into MA layers" + dots;
        public static readonly string exportCreatingXML = buffer + "Creating the XML file that will be imported into MA onPC" + dots;


    }
}

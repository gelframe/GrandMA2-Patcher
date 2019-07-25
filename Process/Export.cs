using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;

namespace GelFrame.Process
{
    class Export
    {
        // Vars
        private readonly XNamespace xNameSpace = Config.XML.nameSpace;
        private readonly MainForm mainForm;
        private bool success = false;

        /// <summary>
        /// Patch data with DMX values converted
        /// </summary>
        private readonly List<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();

        /// <summary>
        /// Dictionary with layers as the key and patch data 
        /// </summary>
        private readonly SortedDictionary<string, List<Dictionary<string, string>>> layerDictionary = new SortedDictionary<string, List<Dictionary<string, string>>>();

        /// <summary>
        /// Dictionary to store fixture names and the current count of each. Used to generate fixture names
        /// </summary>
        private readonly Dictionary<string, int> fixtureNameCountsDictionary = new Dictionary<string, int>();

        /// <summary>
        /// Check for any errors during export
        /// </summary>
        /// <returns></returns>
        public bool CompletedWithOutError()
        {
            return success;
        }

        /// <summary>
        /// Export initialize function
        /// </summary>
        /// <param name="mainForm"></param>
        public Export(MainForm mainForm)
        {
            // Set main form
            this.mainForm = mainForm;

            // Set status
            mainForm.AddStatusNewLine(Config.StatusMessages.headerExport);

            // Get patch list and sort it
            mainForm.AddStatusNewLine(Config.StatusMessages.exportSortPatchList);
            SortPatchList();
            mainForm.AddStatusInLine(Config.StatusMessages.success);

            // Convert DMX to MA format
            mainForm.AddStatusNewLine(Config.StatusMessages.exportCovertDMX);
            ConvertDMX();
            mainForm.AddStatusInLine(Config.StatusMessages.success);

            // Copy fixture number to channel number 
            CopyFixtureNumber(mainForm);

            // Organize patch data by layer
            mainForm.AddStatusNewLine(Config.StatusMessages.exportOrganizeLayers);
            if (OrganizeByLayer())
            {
                mainForm.AddStatusNewLine(Config.StatusMessages.processErrorOrganizeLayers);
                return;
            }
            mainForm.AddStatusInLine(Config.StatusMessages.success);

            // Create XML file
            mainForm.AddStatusNewLine(Config.StatusMessages.exportCreatingXML);
            CreateXML();
        }

        /// <summary>
        /// Create XML file with patch
        /// </summary>
        private void CreateXML()
        {
            // Setup XML Document
            XDocument xmlDocument = new XDocument(
                new XDeclaration(Config.XML.version, Config.XML.encoding, Config.XML.standAlone),
                new XProcessingInstruction(Config.XML.styleSheet, Config.XML.styleSheet1),
                new XProcessingInstruction(Config.XML.styleSheet, Config.XML.styleSheet2)
                );

            // Setup root element with namespaces. Too lazy to write a proper loop
            XElement xmlRoot = new XElement(xNameSpace + Config.XML.maElementMA);

            // Add info to root element
            xmlRoot.Add(CreateInfo());

            // Add layers to root element
            xmlRoot.Add(CreateLayers());

            // Add XML root to document 
            xmlDocument.Add(xmlRoot);

            // Save file
            //xmlDocument.Save(Config.Directories.app + Config.XML.exportXMLtoMAFileName);
            xmlDocument.Save(Telnet.MaDataDirectory.GetDataDirectory(mainForm) + Config.Directories.maImportExport + Config.XML.exportXMLtoMAFileName);

            // Set success to true
            success = true;
        }

        /// <summary>
        /// Create info element
        /// </summary>
        /// <returns></returns>
        private XElement CreateInfo()
        {
            // Create info element
            XElement xInfoElement = new XElement(xNameSpace + Config.XML.maElementInfo,
                new XAttribute(Config.XML.maAttributeDateTime, DateTime.Now.ToString(Config.XML.dateFormat))
                );

            // Return element
            return xInfoElement;
        }

        /// <summary>
        /// Create layers element
        /// </summary>
        /// <returns></returns>
        private XElement CreateLayers()
        {
            // Create layers element
            XElement xLayersElement = new XElement(xNameSpace + Config.XML.maElementLayers,
                new XAttribute(Config.XML.maAttributeIndex, Config.XML.maIndexLayers)
                );

            // Layer vars
            int layerIndex = 1;

            // Loop through layers
            foreach (KeyValuePair<string, List<Dictionary<string, string>>> layerData in layerDictionary)
            {
                // Vars
                int fixtureIndex = 0;

                // Create layers element
                XElement xLayerElement = new XElement(xNameSpace + Config.XML.maElementLayer,
                    new XAttribute(Config.XML.maAttributeIndex, layerIndex),
                    new XAttribute(Config.XML.maAttributeName, RemoveUniversePad(layerData.Key))
                    );

                // Loop through each fixture
                foreach (Dictionary<string, string> fixturePatchList in layerData.Value)
                {
                    // Add fixture element
                    xLayerElement.Add(CreateFixture(fixtureIndex, fixturePatchList));

                    // Increment fixture index
                    fixtureIndex++;
                }

                // Increment layer index 
                layerIndex++;

                // Add layer to layers element
                xLayersElement.Add(xLayerElement);
            }

            // Return element
            return xLayersElement;
        }

        /// <summary>
        /// Create fixture element
        /// </summary>
        /// <param name="indexNumber">Fixture index number</param>
        /// <param name="fixturePatchDictionary">Patch list row</param>
        /// <returns>Complete fixure element</returns>
        private XElement CreateFixture(int indexNumber, Dictionary<string, string> fixturePatchDictionary)
        {
            // Set vars
            string address = fixturePatchDictionary[Config.Settings.columnAddress];
            string mode = null;
            Dictionary<string, string> rotationDictionary = new Dictionary<string, string>();

            // Make sure fixture patch dictionary contains keys for all postion and rotation xyz
            foreach (string key in Config.Settings.columnXYZList)
                if (!(fixturePatchDictionary.ContainsKey(key)))
                    fixturePatchDictionary[key] = null;

            // Process rotations if enabled
            if (Settings.Data.GetValue(Config.Settings.excelEnableRotationFields) == Config.Settings.defaultTrue)
            {
                // Create rotation dictionary from settings mapping
                foreach (string rotationMap in Config.Settings.rotationMappingNameList)
                {
                    string rotationKey = Config.Settings.rotationMappingToColumnDictionary[rotationMap];
                    string mapValue = Config.Settings.rotationMappingLookupDictionary[Settings.Data.GetValue(rotationMap)];
                    string rotationValue = fixturePatchDictionary[mapValue];

                    rotationDictionary.Add(rotationKey, rotationValue);
                }

                // Normalise rotation values and apply offsets
                foreach (string rotationColumn in Config.Settings.columnRotationList)
                {
                    // Convert rotation to double
                    double rotationDouble = Functions.Numbers.StringToDouble(rotationDictionary[rotationColumn]);

                    // Apply offsets
                    rotationDouble += Functions.Numbers.StringToDouble(Settings.Data.GetValue(Config.Settings.rotationOffsetLookupDictionary[rotationColumn]));

                    // Apply invert
                    if (Settings.Data.GetValue(Config.Settings.rotationInvertLookupDictionary[rotationColumn]) == Config.Settings.defaultTrue)
                        rotationDouble *= -1;

                    // Normalise
                    rotationDictionary[rotationColumn] = Functions.Numbers.StringNormaliseDegree(rotationDouble.ToString());
                }
            }

            // Create empty rotation dictionary if rotations are disabled
            else
            {
                // Create zero value record for each rotation column
                foreach (string key in Config.Settings.columnRotationList)
                    rotationDictionary.Add(key, Config.Settings.defaultZero);
            }

            // Process rules if location or postion are in use
            if ((Settings.Data.GetValue(Config.Settings.excelEnableRotationFields) == Config.Settings.defaultTrue) ||
                (Settings.Data.GetValue(Config.Settings.excelEnablePositionFields) == Config.Settings.defaultTrue))
            {
                // Loop through rules
                foreach (KeyValuePair<int, Dictionary<string, string>> rulesDictionary in Settings.Data.GetRulesDictionary())
                {
                    // Get rule record
                    Dictionary<string, string> rule = rulesDictionary.Value;

                    // Only process enabled rules
                    if (rule[Config.Settings.rulesSuffixEnabled] == Config.Settings.defaultTrue)
                    {
                        // Get values
                        string columnName = Functions.Strings.RuleNameToColmunKey(rule[Config.Settings.rulesSuffixColumn]);
                        string changeColumnName = Functions.Strings.RuleNameToColmunKey(rule[Config.Settings.rulesSuffixChangeColumn]);
                        string switchValue = rule[Config.Settings.rulesSuffixSwitch];
                        string changeValue = rule[Config.Settings.rulesSuffixChangeValue];
                        double changeDouble = Functions.Numbers.StringToDouble(changeValue);
                        string pattern = rule[Config.Settings.rulesSuffixPattern].ToLower();

                        // Validate column to match
                        if ((string.IsNullOrEmpty(columnName)) || (!(fixturePatchDictionary.ContainsKey(columnName))))
                            continue;

                        // Validate column to change
                        if ((string.IsNullOrEmpty(changeColumnName)) || (!(fixturePatchDictionary.ContainsKey(changeColumnName))) || (!(Config.Settings.columnXYZList.Contains(changeColumnName))))
                            continue;

                        // Abort if column to change is position and position is disabled
                        if ((Settings.Data.GetValue(Config.Settings.excelEnablePositionFields) != Config.Settings.defaultTrue) && (Config.Settings.columnPositionList.Contains(changeColumnName)))
                            continue;

                        // Abort if column to change is rotation and rotation is disabled
                        if ((Settings.Data.GetValue(Config.Settings.excelEnableRotationFields) != Config.Settings.defaultTrue) && (Config.Settings.columnRotationList.Contains(changeColumnName)))
                            continue;

                        // Abort if column to match is empty
                        if (string.IsNullOrEmpty(fixturePatchDictionary[columnName]))
                            continue;

                        // Wildcard
                        if (pattern == "*")
                        {
                            // bypass switch
                        }

                        // Contains
                        else if (switchValue == Config.Settings.rulesSwitchContains)
                        {
                            if (!(fixturePatchDictionary[columnName].ToLower().Contains(pattern)))
                                continue;
                        }

                        // Ends with
                        else if (switchValue == Config.Settings.rulesSwitchEndsWith)
                        {
                            if (!(fixturePatchDictionary[columnName].ToLower().EndsWith(pattern)))
                                continue;
                        }

                        // Equals
                        else if (switchValue == Config.Settings.rulesSwitchEquals)
                        {
                            if (!(fixturePatchDictionary[columnName].ToLower() == pattern))
                                continue;
                        }

                        // Starts with
                        else if (switchValue == Config.Settings.rulesSwitchStartsWith)
                        {
                            if (!(fixturePatchDictionary[columnName].ToLower().StartsWith(pattern)))
                                continue;
                        }

                        // Range
                        else if(switchValue == Config.Settings.rulesRangeSwitch)
                        {
                            if (!(Functions.Numbers.IsValueInRange(fixturePatchDictionary[columnName], pattern)))
                                continue;
                        }

                        // No valid switch option
                        else
                            continue;

                        // Change postion column
                        if (Config.Settings.columnPositionList.Contains(changeColumnName))
                        {
                            double valueDouble = Functions.Numbers.StringToDouble(fixturePatchDictionary[changeColumnName]);
                            fixturePatchDictionary[changeColumnName] = (valueDouble + changeDouble).ToString();
                        }

                        // Change rotation column
                        else if (Config.Settings.columnRotationList.Contains(changeColumnName))
                        {
                            double valueDouble = Functions.Numbers.StringToDouble(rotationDictionary[changeColumnName]);
                            rotationDictionary[changeColumnName] = Functions.Numbers.StringNormaliseDegree((valueDouble + changeDouble).ToString());
                        }
                    }
                }
            }

            // Get mode if set
            if (fixturePatchDictionary.ContainsKey(Config.Settings.columnMode))
                mode = fixturePatchDictionary[Config.Settings.columnMode];

            // Get MA row number
            string maRowNumber = Mapping.Grid.GetMaRowNumberByGridName(mainForm.GetGridData(), fixturePatchDictionary[Config.Settings.columnFixtureName], mode);

            // Get fixture type data
            Dictionary<string, object> fixtureTypeData = FixtureTypes.Data.GetProfileByRowNumber(maRowNumber);

            // Setup lists from fixture type data
            List<Dictionary<string, string>> instancesList = (List<Dictionary<string, string>>)fixtureTypeData[Config.XML.maElementInstances];
            Dictionary<string, Dictionary<string, string>> moduleDictionary = (Dictionary<string, Dictionary<string, string>>)fixtureTypeData[Config.XML.maElementModules];

            // Create fixture element
            XElement xFixtureElement = new XElement(xNameSpace + Config.XML.maElementFixture,
            new XAttribute(Config.XML.maAttributeIndex, indexNumber)
            );

            // Get short fixture name
            string fixtureShortName = fixtureTypeData[Config.XML.maElementShortName].ToString();

            // Update fixture count value if it exists
            if (fixtureNameCountsDictionary.ContainsKey(fixtureShortName))
                fixtureNameCountsDictionary[fixtureShortName] += 1;

            // Create new fixture count record and set value to 1
            else
                fixtureNameCountsDictionary.Add(fixtureShortName, 1);

            // Fixture name
            xFixtureElement.Add(new XAttribute(Config.XML.maAttributeName, fixtureShortName + " " + fixtureNameCountsDictionary[fixtureShortName]));

            // Fixture number
            if ((fixturePatchDictionary.ContainsKey(Config.Settings.columnFixtureNumber)) && (!(String.IsNullOrEmpty(fixturePatchDictionary[Config.Settings.columnFixtureNumber]))))
                xFixtureElement.Add(new XAttribute(Config.XML.maAttributeFixtureId, fixturePatchDictionary[Config.Settings.columnFixtureNumber]));

            // Channel number
            if ((fixturePatchDictionary.ContainsKey(Config.Settings.columnChannelNumber)) && (!(String.IsNullOrEmpty(fixturePatchDictionary[Config.Settings.columnChannelNumber]))))
                xFixtureElement.Add(new XAttribute(Config.XML.maAttributeChannelId, fixturePatchDictionary[Config.Settings.columnChannelNumber]));

            // Create fixture type element
            XElement xFixtureTypeElement = new XElement(xNameSpace + Config.XML.maElementFixtureType,
            new XAttribute(Config.XML.maAttributeName, maRowNumber + " " + fixtureTypeData[Config.XML.maAttributeName] + " " + fixtureTypeData[Config.XML.maAttributeMode])
            );

            // Create fixture type number element
            XElement xNoElement = new XElement(xNameSpace + Config.XML.maElementNo, maRowNumber);
            // Return element

            // Add number to fixture type
            xFixtureTypeElement.Add(xNoElement);

            // Add fixture type to fixture
            xFixtureElement.Add(xFixtureTypeElement);

            // Multipart fixtures
            if (instancesList.Count > 1)
            {
                // Add absolute position to fixture element
                xFixtureElement.Add(CreateAbsolutePosition(
                    fixturePatchDictionary[Config.Settings.columnPositionX],
                    fixturePatchDictionary[Config.Settings.columnPositionY],
                    fixturePatchDictionary[Config.Settings.columnPositionZ],
                    rotationDictionary[Config.Settings.columnRotationX],
                    rotationDictionary[Config.Settings.columnRotationY],
                    rotationDictionary[Config.Settings.columnRotationZ]
                ));

                // Loop through each instance
                foreach (Dictionary<string, string> instanceDictionary in instancesList)
                {
                    // Vars
                    string instanceAddress;

                    // Use excel data patch if no patch is found
                    if (!(instanceDictionary.ContainsKey(Config.XML.maAttributePatch)))
                        instanceAddress = address;

                    // Add found instance patch to excel data patch and subtract 1
                    else
                        instanceAddress = (Functions.Numbers.StringToPostiveInt(address) + Functions.Numbers.StringToPostiveInt(instanceDictionary[Config.XML.maAttributePatch]) - 1).ToString();

                    // Add sub fixture to fixture element
                    xFixtureElement.Add(CreateSubFixture(
                        instanceDictionary[Config.XML.maAttributeIndex],                                                            // Sub fixture index
                        instanceAddress,                                                                                            // Address
                        moduleDictionary[instanceDictionary[Config.XML.maAttributeModuleIndex]][Config.Fixture.moduleChannelCount], // Channel count
                        Config.Settings.defaultFalse                                                                                // Include position
                    ));
                }
            }

            // Single instance fixtures
            else
            {
                // Add subfixture to fixture element
                xFixtureElement.Add(CreateSubFixture(
                    Config.XML.defaultValueZero,                                                        // Sub fixture index
                    address,                                                                            // Address
                    moduleDictionary[Config.XML.defaultValueZero][Config.Fixture.moduleChannelCount],   // Channel count
                    Config.Settings.defaultTrue,                                                        // Include position
                    fixturePatchDictionary[Config.Settings.columnPositionX],                            // X position
                    fixturePatchDictionary[Config.Settings.columnPositionY],                            // Y position
                    fixturePatchDictionary[Config.Settings.columnPositionZ],                            // Z position
                    rotationDictionary[Config.Settings.columnRotationX],                                // X rotation
                    rotationDictionary[Config.Settings.columnRotationY],                                // Y rotation
                    rotationDictionary[Config.Settings.columnRotationZ]                                 // Z rotation
                ));
            }

            // Return fixture element
            return xFixtureElement;
        }

        /// <summary>
        /// Create sub fixture element
        /// </summary>
        /// <param name="indexNumber">Sub fixture index number</param>
        /// <param name="address">Patch address</param>
        /// <param name="channelString">Channel count</param>
        /// <param name="includePosition">Fake bool based on string matching "True" to include absolute position element</param>
        /// <param name="pX">Position X</param>
        /// <param name="pY">Position Y</param>
        /// <param name="pZ">Position Z</param>
        /// <param name="rX">Rotation X</param>
        /// <param name="rY">Rotation Y</param>
        /// <param name="rZ">Rotation Z</param>
        /// <returns>Complete sub fixture element</returns>
        private XElement CreateSubFixture(string indexNumber, string address, string channelString, string includePosition, string pX = null, string pY = null, string pZ = null, string rX = null, string rY = null, string rZ = null)
        {
            // Create sub fixture element
            XElement xSubFixtureElement = new XElement(xNameSpace + Config.XML.maElementSubFixture,
            new XAttribute(Config.XML.maAttributeIndex, indexNumber),
            new XAttribute(Config.XML.maAttributeReact, Config.XML.defaultValueReact),
            new XAttribute(Config.XML.maAttributeColor, Config.XML.defaultValueColor)
            );

            // Create patch element
            XElement xPatchElement = new XElement(xNameSpace + Config.XML.maElementPatch);

            // Create address element
            XElement xAddressElement = new XElement(xNameSpace + Config.XML.maElementAddress, address);

            // Add address to patch
            xPatchElement.Add(xAddressElement);

            // Add patch to sub fixture
            xSubFixtureElement.Add(xPatchElement);

            // Add absolute position to sub fixture
            if (includePosition == Config.Settings.defaultTrue)
                xSubFixtureElement.Add(CreateAbsolutePosition(pX, pY, pZ, rX, rY, rZ));

            // Convert channel string to integeter
            int channelCount = Functions.Numbers.StringToPostiveInt(channelString);

            // Add channel elements
            for (int i = 0; i < channelCount; i++)
            {
                // Create channel element
                XElement xChannelElement = new XElement(xNameSpace + Config.XML.maElementChannel,
                new XAttribute(Config.XML.maAttributeIndex, i)
                );

                // Add channel element to sub fixture
                xSubFixtureElement.Add(xChannelElement);
            }

            // Return sub fixture element
            return xSubFixtureElement;
        }

        /// <summary>
        /// Create position element
        /// </summary>
        /// <param name="pX">Position X</param>
        /// <param name="pY">Position Y</param>
        /// <param name="pZ">Position Z</param>
        /// <param name="rX">Rotation X</param>
        /// <param name="rY">Rotation Y</param>
        /// <param name="rZ">Rotation Z</param>
        /// <returns>Complete absolute position element</returns>
        private XElement CreateAbsolutePosition(string pX = null, string pY = null, string pZ = null, string rX = null, string rY = null, string rZ = null)
        {
            // Set default values
            pX = (String.IsNullOrEmpty(pX)) ? Config.Settings.defaultZero : pX;
            pY = (String.IsNullOrEmpty(pY)) ? Config.Settings.defaultZero : pY;
            pZ = (String.IsNullOrEmpty(pZ)) ? Config.Settings.defaultZero : pZ;
            rX = (String.IsNullOrEmpty(rX)) ? Config.Settings.defaultZero : rX;
            rY = (String.IsNullOrEmpty(rY)) ? Config.Settings.defaultZero : rY;
            rZ = (String.IsNullOrEmpty(rZ)) ? Config.Settings.defaultZero : rZ;

            // Create poistion element
            XElement xAbsolutePositionElement = new XElement(xNameSpace + Config.XML.maElementAbsolutePosition);

            // Create location element
            XElement xLocationElement = new XElement(xNameSpace + Config.XML.maElementLocation,
            new XAttribute(Config.XML.maAttributeX, pX),
            new XAttribute(Config.XML.maAttributeY, pY),
            new XAttribute(Config.XML.maAttributeZ, pZ)
            );

            // Create rotation element
            XElement xRotationElement = new XElement(xNameSpace + Config.XML.maElementRotation,
            new XAttribute(Config.XML.maAttributeX, rX),
            new XAttribute(Config.XML.maAttributeY, rY),
            new XAttribute(Config.XML.maAttributeZ, rZ)
            );

            // Create scale element
            XElement xScaleElement = new XElement(xNameSpace + Config.XML.maElementScaling,
            new XAttribute(Config.XML.maAttributeX, "1"),
            new XAttribute(Config.XML.maAttributeY, "1"),
            new XAttribute(Config.XML.maAttributeZ, "1")
            );

            // Add elements to postion element
            xAbsolutePositionElement.Add(xLocationElement);
            xAbsolutePositionElement.Add(xRotationElement);
            xAbsolutePositionElement.Add(xScaleElement);

            // Return position element
            return xAbsolutePositionElement;
        }

        /// <summary>
        /// Get data patch list and sort by unit number and then channel number
        /// </summary>
        public void SortPatchList()
        {
            // Setup unsorted list
            List<Dictionary<string, string>> unsortedPatchList;

            // Sort by only fixture number if setting checkbox is true. 
            if (Settings.Data.GetValue(Config.Settings.excelOnlyFixtureNumber) == Config.Settings.defaultTrue)
                unsortedPatchList = new List<Dictionary<string, string>>(Patch.Data.GetPatchList().OrderBy(dict => dict[Config.Settings.columnFixtureNumber]));

            // Sort by channel number, then fixture number. This puts channel number on the bottom of fixture numbers with no channel number.
            else
                unsortedPatchList = new List<Dictionary<string, string>>(Patch.Data.GetPatchList().OrderBy(dict => dict[Config.Settings.columnChannelNumber]).ThenBy(dict => dict[Config.Settings.columnFixtureNumber]));

            // Loop through unsorted list and add all items to patch dictionary in new sorted order
            foreach (Dictionary<string, string> patchDictionary in unsortedPatchList)
                patchList.Add(new Dictionary<string, string>(patchDictionary));
        }

        /// <summary>
        /// Convert universe and address to MA format. 
        /// Explodes address into both unviverse and address if using seperation character
        /// </summary>
        private void ConvertDMX()
        {
            // Loop through patchlist 
            foreach (Dictionary<string, string> patchDictionary in patchList)
            {
                // Split universe/address 
                if (Settings.Data.GetValue(Config.Settings.excelSingleAddressField) == Config.Settings.defaultTrue)
                {
                    // Get split character
                    char[] splitChar = Settings.Data.GetValue(Config.Settings.excelSeparatorCharacter).ToCharArray();

                    // Explode value by seprartion character
                    string[] explode = patchDictionary[Config.Settings.columnAddress].Split(splitChar, StringSplitOptions.None);

                    // Add values to row dictionary
                    patchDictionary[Config.Settings.columnUniverse] = explode[0];
                    patchDictionary[Config.Settings.columnAddress] = explode[1];
                }

                // Convert strings to ints
                int universe = Convert.ToInt32(patchDictionary[Config.Settings.columnUniverse]);
                int address = Convert.ToInt32(patchDictionary[Config.Settings.columnAddress]);

                // Save address as complete
                patchDictionary[Config.Settings.columnAddress] = (universe * 512 + address - 512).ToString();
            }
        }

        /// <summary>
        /// Copy fixture numbers to channel numbers but only if:
        ///     1. setting checkbox is enabled
        ///     2. no channel numbers are present or channel number column is disabled
        /// </summary>
        /// <param name="mainForm"></param>
        public void CopyFixtureNumber(MainForm mainForm)
        {
            // Check that settings checkbox is enabled
            if (Settings.Data.GetValue(Config.Settings.optionCopyFixturetoChannel) == Config.Settings.defaultTrue)
            {
                // Vars
                int channelNumberCount = 0;

                // Skip channel number count if channel number column is disable in Excel settings
                if (Settings.Data.GetValue(Config.Settings.excelOnlyFixtureNumber) != Config.Settings.defaultTrue)
                {
                    // Get the number of channel number records that contain a valid number
                    foreach (Dictionary<string, string> patchRow in patchList)
                        if ((patchRow.ContainsKey(Config.Settings.columnChannelNumber)) && (!(String.IsNullOrEmpty(patchRow[Config.Settings.columnChannelNumber]))))
                            channelNumberCount++;
                }

                // Copy fixture number to channel number if no channel numbers were found
                if (channelNumberCount == 0)
                {
                    // Set status
                    mainForm.AddStatusNewLine(Config.StatusMessages.exportCopyFixtureNumbertoChannel);

                    // Copy fixture number to channel number
                    foreach (Dictionary<string, string> patchRow in patchList)
                        patchRow[Config.Settings.columnChannelNumber] = patchRow[Config.Settings.columnFixtureNumber];

                    // Set success status
                    mainForm.AddStatusInLine(Config.StatusMessages.success);
                }
            }
        }

        /// <summary>
        /// Create layer dictionary
        /// </summary>
        /// <returns>False on error</returns>
        public bool OrganizeByLayer()
        {
            // Set error bool default to false
            bool error = false;

            // Vars
            string selectedOption = Settings.Data.GetValue(Config.Settings.optionLayerBy);
            string layerColumn;

            // By Location
            if (selectedOption == Config.Patch.layerByLocation)
            {
                // Make sure the location column exists
                if (!(Settings.Data.GetEnabledColumnsList().Contains(Config.Settings.columnLocation)))
                {
                    MessageBox.Show(Config.ErrorMessages.locationNotEnabled);

                    // Return true error
                    return true;
                }

                // Set vars
                layerColumn = Config.Settings.columnLocation;
            }

            // By universe
            else if (selectedOption == Config.Patch.layerByUniverse)
                layerColumn = Config.Settings.columnUniverse;

            // Default to fixture type
            else
                layerColumn = Config.Settings.columnFixtureName;

            // Loop through patch list 
            foreach (Dictionary<string, string> patchDictionary in patchList)
            {
                // Get layer name
                string layerName = PadUniverse(patchDictionary[layerColumn]);

                // Default layer if empty
                if (String.IsNullOrEmpty(layerName))
                    layerName = Config.Patch.defaultLayerName;

                // Layer dictionary key
                if (!(layerDictionary.ContainsKey(layerName)))
                    layerDictionary.Add(layerName, new List<Dictionary<string, string>>());

                // Add patch data
                layerDictionary[layerName].Add(patchDictionary);
            }

            // Return error status
            return error;
        }

        /// <summary>
        /// Pad the universe layer name so it sorts correctly
        /// </summary>
        /// <param name="layerName"></param>
        /// <returns></returns>
        private string PadUniverse(string layerName)
        {
            if (Settings.Data.GetValue(Config.Settings.optionLayerBy) == Config.Patch.layerByUniverse)
            {
                if (layerName.Length == 1)
                    return Config.Patch.prefixUniverse + "00" + layerName;

                else if (layerName.Length == 2)
                    return Config.Patch.prefixUniverse + "0" + layerName;
            }

            return layerName;
        }

        private string RemoveUniversePad(string layerName)
        {
            if (Settings.Data.GetValue(Config.Settings.optionLayerBy) == Config.Patch.layerByUniverse)
            {
                layerName = layerName.Replace(": 00", ": ");
                layerName = layerName.Replace(": 0", ": ");
            }

            return layerName;
        }
    }
}

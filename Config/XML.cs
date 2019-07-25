using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GelFrame.Config
{
    class XML
    {
        // File names
        public static readonly string settingsFileName = "settings.xml";
        public static readonly string fixtureTypeFileName = "fixture_types.xml";
        public static readonly string patchFileName = "patch.xml";
        public static readonly string mappingFileName = "mapping.xml";
        public static readonly string exportXMLtoMAFileName = "GelFrameGeneratedPatchImport.xml";
        public static readonly string exportBlankXMLFileName = "GelFrameBlankPatchImport.xml";

        // XDocument vars
        public static readonly string version = "1.0";
        public static readonly string encoding = "utf-8";
        public static readonly string standAlone = "yes";
        public static readonly string styleSheet = "xml-stylesheet";
        public static readonly string styleSheetType = "type = \"text/xsl\"";
        public static readonly string styleSheet1 = styleSheetType+ " href=\"styles/fixture+layer+layers @html@default.xsl\"";
        public static readonly string styleSheet2 = styleSheetType+ " href=\"styles/fixture+layer+layers @csv.xsl\" alternate=\"yes\"";
        public static readonly string dateFormat = "yyyy-MM-ddTHH:mm:ss";

        // Namespace vars
        public static readonly string nameSpace = "http://schemas.malighting.de/grandma2/xml/MA";

        // Root vars for app created xml data files
        public static readonly string settingsRootSettings = "settings";
        public static readonly string settingsRootFixtureType = "fixtureTypes";
        public static readonly string settingsRootPatch = "patch";
        public static readonly string settingsRootMapping = "mapping";

        // Rules
        public static readonly string settingsRulesElement = "rules";
        public static readonly string settingsRuleElement = "rule";

        // Element names for app created xml data files
        public static readonly string settingsElementFixtureType = "type";
        public static readonly string settingsElementPatch = "fixture";
        public static readonly string settingsElementFile = "file";
        public static readonly string settingsElementRow = "row";
        public static readonly string settingsElementProfile = "profile";

        // MA XML attribute names
        // Should be all lowercase reguardless of native XML. 
        // Native XML is converted to lowercase in app when matching these vars
        // Example: instace XYZ attributes are uppercase in exported fixture profile xml while other XYZ attribute names are lowercase 
        public static readonly string maAttributeName = "name";
        public static readonly string maAttributeDateTime= "datetime";
        public static readonly string maAttributeIndex = "index";
        public static readonly string maAttributeMode = "mode";
        public static readonly string maAttributeModuleIndex = "module_index";
        public static readonly string maAttributePatch = "patch";
        public static readonly string maAttributeX = "x";
        public static readonly string maAttributeY = "y";
        public static readonly string maAttributeZ = "z";
        public static readonly string maAttributeFixtureId = "fixture_id";
        public static readonly string maAttributeChannelId = "channel_id";
        public static readonly string maAttributeIsMultipatch = "is_multipatch";
        public static readonly string maAttributeReact = "react_to_grandmaster";
        public static readonly string maAttributeColor = "color";

        // MA element names
        public static readonly string maElementMA= "MA";
        public static readonly string maElementInfo = "Info";
        public static readonly string maElementFixtureType= "FixtureType";
        public static readonly string maElementShortName = "short_name";
        public static readonly string maElementManufacturer = "manufacturer";
        public static readonly string maElementInstances = "Instances";
        public static readonly string maElementInstance = "Instance";
        public static readonly string maElementModules = "Modules";
        public static readonly string maElementModule = "Module";
        public static readonly string maElementNo = "No";
        public static readonly string maElementAbsolutePosition = "AbsolutePosition";
        public static readonly string maElementLocation = "Location";
        public static readonly string maElementRotation = "Rotation";
        public static readonly string maElementScaling = "Scaling";
        public static readonly string maElementFixture = "Fixture";
        public static readonly string maElementSubFixture = "SubFixture";
        public static readonly string maElementLayers = "Layers";
        public static readonly string maElementLayer = "Layer";
        public static readonly string maElementChannelType = "ChannelType";
        public static readonly string maElementPatch = "Patch";
        public static readonly string maElementAddress = "Address";
        public static readonly string maElementChannel = "Channel";
        public static readonly string maElementBody = "Body";

        // Index values
        public static readonly string maIndexLayers = "3";
        
        // Default values
        public static readonly string defaultValueColor = "ffffff";
        public static readonly string defaultValueReact = "true";
        public static readonly string defaultValueZero = "0";

        /// <summary>
        /// List of attributes used in the fixture profile instance node
        /// </summary>
        public static List<string> maInstanceAttributeList = new List<string>()
        {
            maAttributeIndex,
            maAttributeModuleIndex,
            maAttributePatch,
            maAttributeX,
            maAttributeY,
            maAttributeZ,
        };

    }
}

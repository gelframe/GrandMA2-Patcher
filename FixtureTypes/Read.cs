using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GelFrame.FixtureTypes
{
    static class Read
    {
        /// <summary>
        /// Read fixture types xml and load values into fixture types data
        /// </summary>
        static public string XML()
        {
            // Default status message to sucess
            string status = Config.StatusMessages.success;

            // Only process if xml file exists
            if (File.Exists(Config.Directories.XML + Config.XML.fixtureTypeFileName))
            {
                // Define local list to load xml values into
                List<Dictionary<string, string>> typeList = new List<Dictionary<string, string>>();

                // Load XML file
                XElement xmlElement = XElement.Load(Config.Directories.XML + Config.XML.fixtureTypeFileName);

                // Loop through each fixture xml node
                foreach (XElement fixtureElement in xmlElement.Descendants(Config.XML.settingsElementFixtureType))
                {
                    // Define fixture dictionary to load values into
                    Dictionary<string, string> dataDictionary = new Dictionary<string, string>();

                    // Load each element into the fixture dictionary
                    foreach (XElement element in fixtureElement.Descendants())
                        dataDictionary.Add(element.Name.LocalName, element.Value);

                    // Add local dictionary to local list
                    typeList.Add(dataDictionary);
                }

                // Load local list into fixture types data
                Data.SetTypeList(typeList);
            }

            // Set status to skip if file not found
            else
                status = Config.StatusMessages.skipping + Config.StatusMessages.noFileFound;

            // Return status message
            return status;
        }

        /// <summary>
        /// Process exported fixture profile xml files.
        /// The only data extracted is the necessary pieces to build the import patch XML
        /// </summary>
        static public void ProfileXML(MainForm mainForm)
        {
            // Set start status message
            mainForm.AddStatusNewLine(Config.StatusMessages.headerProfileXMLStart);

            // Clear data profile XML list
            Data.ClearProfileXMLList();

            // Get list of all XML files in local library
            string[] files = Directory.GetFiles(Config.Directories.localData, Config.Fixture.matchFileType + Config.Fixture.fileType, SearchOption.TopDirectoryOnly);

            // Loop though all files
            foreach (string filePath in files)
            {
                // Only process xml files
                if (filePath.EndsWith(Config.Fixture.fileType))
                {
                    // Send status message
                    mainForm.AddStatusNewLine(Config.StatusMessages.buffer + Config.StatusMessages.profileXMLprocessing + Path.GetFileName(filePath));

                    // Define fixture dictionaries/lists to load values into
                    Dictionary<string, object> dataDictionary = new Dictionary<string, object>();
                    Dictionary<string, Dictionary<string, string>> moduleDictionary = new Dictionary<string, Dictionary<string, string>>();
                    List<Dictionary<string, string>> instancesList = new List<Dictionary<string, string>>();

                    // Load XML file
                    XElement xmlElement = XElement.Load(filePath);

                    // Loop through all xml nodes
                    foreach (XElement fixtureElement in xmlElement.Descendants())
                    {
                        // Fixture Type
                        if (fixtureElement.Name.LocalName.ToLower() == Config.XML.maElementFixtureType.ToLower())
                        {
                            dataDictionary.Add(Config.XML.maAttributeName, fixtureElement.Attribute(Config.XML.maAttributeName).Value);
                            dataDictionary.Add(Config.XML.maAttributeMode, fixtureElement.Attribute(Config.XML.maAttributeMode)?.Value);
                        }

                        // Short Name
                        if (fixtureElement.Name.LocalName.ToLower() == Config.XML.maElementShortName.ToLower())
                            dataDictionary.Add(Config.XML.maElementShortName, fixtureElement.Value);

                        // Manufacturer
                        if (fixtureElement.Name.LocalName.ToLower() == Config.XML.maElementManufacturer.ToLower())
                            dataDictionary.Add(Config.XML.maElementManufacturer, fixtureElement.Value);

                        // Fixture Type Node
                        if (fixtureElement.Name.LocalName.ToLower() == Config.XML.maElementModule.ToLower())
                        { 
                            // Delcare channel count var
                            int channelCount = 0;
                            bool foundBody = false;

                            // Count number of channel types in module
                            foreach (XElement moduleChildElement in fixtureElement.Descendants())
                            {
                                // Find body node
                                if (moduleChildElement.Name.LocalName.ToLower() == Config.XML.maElementBody.ToLower())
                                    foundBody = true;

                                // Increase channel counter
                                if (moduleChildElement.Name.LocalName.ToLower() == Config.XML.maElementChannelType.ToLower())
                                    channelCount++;
                            }

                            // Create module dictionary row. Index is the key, dictionary contains found body and channel count as strings.
                            moduleDictionary.Add(fixtureElement.Attribute(Config.XML.maAttributeIndex).Value, 
                                new Dictionary<string, string>()
                                {
                                    {Config.Fixture.moduleBody, foundBody.ToString() },
                                    {Config.Fixture.moduleChannelCount, channelCount.ToString() }
                                }
                                );
                        }

                        // Instances
                        if (fixtureElement.Name.LocalName.ToLower() == Config.XML.maElementInstance.ToLower())
                        {
                            // Delcare instance dictionary
                            Dictionary<string, string> instanceDictionary = new Dictionary<string, string>();

                            // Loop though all attributes and add present attributes to the instance dictionary
                            foreach (XAttribute instanceAttribute in fixtureElement.Attributes())
                                if (Config.XML.maInstanceAttributeList.Contains(instanceAttribute.Name.ToString().ToLower()))
                                    instanceDictionary.Add(instanceAttribute.Name.ToString().ToLower(), instanceAttribute.Value);

                            // Add instance dictionary to fixture's instance list
                            instancesList.Add(instanceDictionary);
                        }
                    }

                    // Add sub lists to data dictionary
                    dataDictionary.Add(Config.XML.maElementInstances, instancesList);
                    dataDictionary.Add(Config.XML.maElementModules, moduleDictionary);

                    // Add fixture profile data to data object
                    Data.AddProfileXMLList(dataDictionary);
                }
            }

            // Set end status message
            mainForm.AddStatusNewLine(Config.StatusMessages.done);
        }
    }
}

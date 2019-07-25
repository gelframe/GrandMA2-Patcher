using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GelFrame.FixtureTypes
{
    static class Data
    {
        /// <summary>
        /// List of unique fixture types
        /// </summary>
        static private readonly List<Dictionary<string, string>> typeList = new List<Dictionary<string, string>>();

        /// <summary>
        /// List of all data extract from the fixture type XML files
        /// </summary>
        static private readonly List<Dictionary<string, object>> profileXMLList = new List<Dictionary<string, object>>();

        /// <summary>
        /// Compare profile and type list to ensure they match
        /// </summary>
        /// <returns>True on error</returns>
        static public bool VerifyProfileAndTypeListsMatch(MainForm mainForm)
        {
            // Vars
            bool foundError = false;

            // Set status
            mainForm.AddStatusNewLine(Config.StatusMessages.headerFixtureTypeVerify);

            // Loop through fixture types
            foreach (Dictionary<string, string> fixtureType in typeList)
            {
                // Vars
                bool missingProfile = true;

                // Loop through profiles
                foreach (Dictionary<string, object> profile in profileXMLList)
                {
                    // Break out of loop on match of profile to fixture type
                    if ((profile[Config.XML.maElementShortName].ToString() == fixtureType[Config.Fixture.typeShortName]) &&
                        (profile[Config.XML.maAttributeMode].ToString() == fixtureType[Config.Fixture.typeMode]) &&
                        (profile[Config.XML.maElementManufacturer].ToString() == fixtureType[Config.Fixture.typeMan]))
                    {
                        missingProfile = false;
                        break;
                    }
                }

                // Set error status if profile is not found
                if (missingProfile)
                {
                    foundError = true;
                    mainForm.AddStatusNewLine(string.Format(
                        Config.StatusMessages.fixtureTypeVerifyMissingProfile,
                        fixtureType[Config.Fixture.typeNumber],
                        fixtureType[Config.Fixture.typeMan],
                        fixtureType[Config.Fixture.typeLongName]
                    ));
                }
            }

            // Set success message
            if (!(foundError))
                mainForm.AddStatusInLine(Config.StatusMessages.success);

            // Display error message
            else
                MessageBox.Show(Config.ErrorMessages.wrongMaDataDirectory);

            // Return error status
            return foundError;
        }

        /// <summary>
        /// Creates list of fixture profile names for use in fixture data grid
        /// </summary>
        /// <returns></returns>
        static public List<string> GetFixtureTypeMapList()
        {
            // Create list to be returned
            List<string> fixtureTypeMapList = new List<string>();

            // Loop through all profiles
            foreach (Dictionary<string, string> typeDictionary in typeList)
            {
                // Create text string
                string textString = typeDictionary[Config.Fixture.typeMan];
                textString += " " + typeDictionary[Config.Fixture.typeLongName];
                textString += " (" + typeDictionary[Config.Fixture.typeMode] + ")";
                textString += Config.Fixture.seperator + typeDictionary[Config.Fixture.typeNumber];

                // Add text string to fixture map list
                fixtureTypeMapList.Add(textString);
            }

            // Sort list
            fixtureTypeMapList.Sort();

            // Return sorted list
            return fixtureTypeMapList;
        }

        /// <summary>
        /// Returns profile dictionary from given fixture grid profile selection
        /// </summary>
        /// <param name="selection">Selected profile value from fixture data grid</param>
        /// <returns>Profile XML data</returns>
        static public Dictionary<string, object> GetProfileByRowNumber(string maRowNumber)
        {
            // Loop through profiles
            foreach (Dictionary<string, string> fixtureType in typeList)
            {
                // Find row of given ma row number
                if (fixtureType[Config.Fixture.typeNumber] == maRowNumber)
                {

                    // Loop through profile 
                    foreach (Dictionary<string, object> profile in profileXMLList)
                    {
                        if ((profile[Config.XML.maElementShortName].ToString() == fixtureType[Config.Fixture.typeShortName]) &&
                            (profile[Config.XML.maAttributeMode].ToString() == fixtureType[Config.Fixture.typeMode]) &&
                            (profile[Config.XML.maElementManufacturer].ToString() == fixtureType[Config.Fixture.typeMan]))
                        {
                            return profile;
                        }
                    }
                }
            }

            // Return empty on no match
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// Add new row to profile XML list
        /// </summary>
        /// <param name="dataDictionary">Dictionary to add to XML list</param>
        static public void AddProfileXMLList(Dictionary<string, object> dataDictionary)
        {
            profileXMLList.Add(dataDictionary);
        }

        /// <summary>
        /// Return profile XML list
        /// </summary>
        /// <returns>profile XML list</returns>
        static public List<Dictionary<string, object>> GetProfileXMLList()
        {
            return profileXMLList;
        }

        /// <summary>
        /// Clear profile XML list
        /// </summary>
        static public void ClearProfileXMLList()
        {
            profileXMLList.Clear();
        }

        /// <summary>
        /// Get the type list
        /// </summary>
        /// <returns>Type list</returns>
        static public List<Dictionary<string, string>> GetTypeList()
        {
            return typeList;
        }

        /// <summary>
        /// Reset the type list
        /// </summary>
        /// <param name="value">Complete type list to replace currently stored list</param>
        static public void SetTypeList(List<Dictionary<string, string>> value)
        {
            typeList.Clear();
            foreach (Dictionary<string, string> dictionary in value)
                typeList.Add(dictionary);
        }
    }
}

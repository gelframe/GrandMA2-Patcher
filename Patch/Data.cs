using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GelFrame.Patch
{
    static class Data
    {
        // Delcare object data lists
        static private readonly List<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
        static private readonly List<string> fixtureList = new List<string>();

        /// <summary>
        /// Excel file path
        /// </summary>
        static public string ExcelFilePath { set; get; }

        /// <summary>
        /// Add row data to patch list
        /// </summary>
        /// <param name="rowDictionary">Dictionary of row data</param>
        static public void PatchListAdd(Dictionary<string, string> rowDictionary)
        {
            patchList.Add(rowDictionary);
        }

        /// <summary>
        /// Add unique values to the fixture list
        /// </summary>
        /// <param name="fixtureName">Name of fixture to add to list if unique</param>
        static public void FixtureListAdd(string fixtureName)
        {
            if (!(String.IsNullOrEmpty(fixtureName)) && (!(fixtureList.Contains(fixtureName))))
                fixtureList.Add(fixtureName);
        }

        /// <summary>
        /// Empty all lists
        /// </summary>
        static public void ClearAll()
        {
            patchList.Clear();
            fixtureList.Clear();
        }

        /// <summary>
        /// Return patch list
        /// </summary>
        /// <returns></returns>
        static public List<Dictionary<string, string>> GetPatchList()
        {
            return patchList;
        }

        /// <summary>
        /// Return list of fixture names sorted alphabetically
        /// </summary>
        /// <returns>fixture names list sorted alphabetically</returns>
        static public List<string> GetFixtureList()
        {
            return fixtureList.OrderBy(x => x).ToList();
        }

    }
}

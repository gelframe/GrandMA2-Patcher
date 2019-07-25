using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;

namespace GelFrame.Mapping
{
    static class Save
    {
        /// <summary>
        /// Saves mapping data to XML
        /// </summary>
        public static void XML(MainForm mainForm)
        {
            // Setup XML Document
            XDocument xmlDocument = new XDocument(new XDeclaration(Config.XML.version, Config.XML.encoding, Config.XML.standAlone)                );

            // Define XML root
            XElement xmlRoot = new XElement(Config.XML.settingsRootMapping);

            // Loop through each section and append to the XML root
            foreach (DataGridViewRow row in mainForm.GetGridData())
            {
                XElement xmlRow = new XElement(Config.XML.settingsElementRow);
                xmlRow.Add(new XElement(Config.XML.settingsElementPatch, row.Cells[0].Value.ToString()));
                xmlRow.Add(new XElement(Config.XML.settingsElementProfile, Functions.Strings.StripFixtureTypeRowNumber(row.Cells[1].Value?.ToString())));
                xmlRoot.Add(xmlRow);
            }

            // Add XML root to document 
            xmlDocument.Add(xmlRoot);

            // Save xml
            xmlDocument.Save(Config.Directories.XML + Config.XML.mappingFileName);
        }
    }
}

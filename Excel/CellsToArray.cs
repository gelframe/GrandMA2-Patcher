using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

using Office = Microsoft.Office.Interop;

namespace GelFrame.Excel
{
    class CellsToArray
    {
        // Delcare vars
        private readonly List<string> columnNameList = new List<string>();
        private readonly List<Dictionary<string, string>> cellValueList = new List<Dictionary<string, string>>();
        private readonly string fileName;
        private object[,] cellValueObject;
        private bool error;

        public CellsToArray(string fileName)
        {
            error = false;

            // Only run if file exists
            if (File.Exists(fileName))
            {
                // Set vars
                this.fileName = fileName;

                // Turn Excel data into object
                ExcelToObject();
                ObjectToList();
            }
            else
            {
                error = true;
            }
        }

        public bool FoundError()
        {
            return error;
        }

        public object[,] ReturnObject()
        {
            return cellValueObject;
        }

        public List<string> GetColumnNameList()
        {
            return columnNameList;
        }

        /// <summary>
        /// Convert excel worksheet into an object. Only work on signle worksheet workbooks.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Object of all Excel worksheet data.</returns>
        private void ExcelToObject()
        {
            try
            {
                // Create new excel instance
                Office.Excel.Application excelApplication = new Office.Excel.Application
                {
                    Visible = false
                };

                // Open file
                Office.Excel._Workbook workbook = excelApplication.Workbooks.Open(fileName, Type.Missing, true);
                Office.Excel._Worksheet worksheet = workbook.ActiveSheet;

                // Get range
                Office.Excel.Range firstCell = worksheet.get_Range("A1", Type.Missing);
                Office.Excel.Range lastCell = worksheet.Cells.SpecialCells(Office.Excel.XlCellType.xlCellTypeLastCell, Type.Missing);

                Office.Excel.Range worksheetCells = worksheet.get_Range(firstCell, lastCell);
                cellValueObject = worksheetCells.Value2 as object[,];

                workbook.Close();
                excelApplication.Quit();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// Creates a list of each row. 
        /// List elements are dictionary values where the key is the column column
        /// </summary>
        /// <param name="fileName">file to convert</param>
        /// <returns>list of dictionarys with column as key and row values</returns>
        public List<Dictionary<string, string>> ReturnList()
        {
            return cellValueList;
        }

        private void ObjectToList()
        {
            // Clear columnlist
            columnNameList.Clear();

            // Set number of rows and columns
            int NumberOfcolumns = cellValueObject.GetLength(1);
            int NumberOfRows = cellValueObject.GetLength(0);

            // Loop through cells
            for (int i = 1; i <= NumberOfRows; i++)
            {
                // Get columns from first row
                if (i == 1)

                    // Loop through columns 
                    for (int i2 = 1; i2 <= NumberOfcolumns; i2++)
                    {
                        // Ignore columns with empty header
                        if (cellValueObject[i, i2] != null)
                        {
                            // Check for duplicate column names
                            if (columnNameList.Contains(cellValueObject[i, i2]?.ToString().Trim().ToLower()))
                            {
                                error = true;
                                MessageBox.Show(Config.ErrorMessages.excelDuplicateColumnNames + cellValueObject[i, i2].ToString().Trim());
                                return;
                            }

                            // Write to column list
                            columnNameList.Add(cellValueObject[i, i2].ToString().Trim().ToLower());
                        }
                    }

                // Process normal rows
                else
                {
                    // Delcare dictionary for row list
                    Dictionary<string, string> rowData = new Dictionary<string, string>();

                    // Loop through row data and use column dictionary as 
                    for (int i2 = 1; i2 <= NumberOfcolumns; i2++)

                        // Ignore columns with empty header
                        if (cellValueObject[1, i2] != null)

                            // Write to row data dictionary
                            rowData.Add(cellValueObject[1, i2].ToString().Trim().ToLower(), cellValueObject[i, i2]?.ToString().Trim());

                    // Add row data to data list
                    cellValueList.Add(rowData);
                }
            }
        }
    }
}

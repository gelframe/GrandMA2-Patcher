using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GelFrame
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // Set form name
            Text = Config.App.name + Config.App.verison;

            // Load settings 
            AddStatusNewLine(Config.StatusMessages.loadingXMlSettings);
            var settingsStatus = Settings.Read.XML();
            AddStatusInLine(settingsStatus);

            // Force settings form on error
            if (settingsStatus.StartsWith(Config.StatusMessages.error))
            {
                MessageBox.Show(Config.ErrorMessages.noSettingsFiles);
                Settings.SettingsForm settingsForm = new Settings.SettingsForm(this);
                settingsForm.ShowDialog();
                return;
            }

            // Load fixture data XML
            AddStatusNewLine(Config.StatusMessages.loadingXMlFixtureData);
            AddStatusInLine(FixtureTypes.Read.XML());

            // Load fixture profile XML
            FixtureTypes.Read.ProfileXML(this);

            // Load patch data XML
            AddStatusNewLine(Config.StatusMessages.loadingXMlPatchData);
            AddStatusInLine(Patch.Read.XML());
            
            // Set patch file value
            patchFileTextBox.Text = Patch.Data.ExcelFilePath;

            // Verify patch data and clear all if errors are found
            if (Patch.Verify.ErrorCheckData(this))
            {
                // Clear all patch data
                Patch.Data.ClearAll();

                // Save cleared data to XML
                Patch.Save.XML(patchFileTextBox.Text);
            }
            
            // Load data grid
            Mapping.Grid.UpdateFromXML(this);

            // Set data grid data source
            SetGridColumnDataSource();

        }

        /// <summary>
        /// Settings button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            Settings.SettingsForm settingsForm = new Settings.SettingsForm(this);
            settingsForm.ShowDialog();
        }

        /// <summary>
        /// Process button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessButton_Click(object sender, EventArgs e)
        {
            Process.Process.Execute(this);
        }

        /// <summary>
        /// Load fixture types
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadFixtureTypesButton_Click(object sender, EventArgs e)
        {
            // Reload fixture types
            bool reloadError = FixtureTypes.Load.Reload(this);

            // Reload column data
            if (!(reloadError))
                Mapping.Grid.UpdateFromForm(this);
        }

        /// <summary>
        /// Read Excel path data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadExcelButton_Click(object sender, EventArgs e)
        {
            // Process excel file
            new Patch.Excel(this);

            // Reload column data
            Mapping.Grid.UpdateFromForm(this);
        }

        /// <summary>
        /// Get excel patch file path from input text box
        /// </summary>
        /// <returns></returns>
        public string GetPatchFilePath()
        {
            return patchFileTextBox.Text.Trim();
        }

        /// <summary>
        /// File selection dialog box for Excel patch file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowseButton_Click(object sender, EventArgs e)
        {
            // Display file select dialog box
            using (OpenFileDialog selectFilesOpenFileDialog = new OpenFileDialog())
            {
                // Set the file dialog to filter for Excel files.
                selectFilesOpenFileDialog.Filter = Config.Patch.selectDialogFilter;
                selectFilesOpenFileDialog.Title = Config.Patch.selectDialogTitle;
                selectFilesOpenFileDialog.Multiselect = false;

                // Only set value if user clicked ok. Do nothing if user cancels
                if (selectFilesOpenFileDialog.ShowDialog() == DialogResult.OK)
                    patchFileTextBox.Text = selectFilesOpenFileDialog.FileName;
            }
        }

        /// <summary>
        /// Add new line to the status box
        /// </summary>
        /// <param name="value">Text to add on new line</param>
        public void AddStatusNewLine(string value)
        {
            if (statusBox.Text.Length == 0)
                statusBox.Text = value;
            else
                statusBox.AppendText("\r\n" + value);
        }

        /// <summary>
        /// Add text to current line in status box
        /// </summary>
        /// <param name="value">Text to append to current line</param>
        public void AddStatusInLine(string value)
        {
            if (statusBox.Text.Length == 0)
                statusBox.Text = value;
            else
                statusBox.AppendText(" " + value);
        }

        /// <summary>
        /// Function that runs on status box text changed. Used to force the textbox to scroll to the bottom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusBox_TextChanged(object sender, EventArgs e)
        {
            ScrollToEnd();
        }

        /// <summary>
        /// Scroll to bottom of status box
        /// </summary>
        private void ScrollToEnd()
        {
            statusBox.SelectionStart = statusBox.Text.Length;
            statusBox.ScrollToCaret();
            statusBox.Refresh();
        }

        /// <summary>
        /// Function that runs after the form is shown for the first time. Necessary to run scroll to end function after form is drawn.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Shown(object sender, EventArgs e)
        {
            ScrollToEnd();
        }

        /// <summary>
        /// Clear fixture data grid
        /// </summary>
        public void ClearGrid()
        {
            fixtureDataGrid.Rows.Clear();
        }

        /// <summary>
        /// Add new row to fixture data grid
        /// </summary>
        /// <param name="name">Fixture name</param>
        /// <param name="profile">Selected </param>
        public void AddGridRow(string name, string profile)
        {
            if (String.IsNullOrEmpty(profile))
                fixtureDataGrid.Rows.Add(name);
            else
                fixtureDataGrid.Rows.Add(name, profile);
        }

        /// <summary>
        /// Sets fixture data grid combo box data source to fixture type map list
        /// </summary>
        public void SetGridColumnDataSource()
        {
            dataColumnProfile.DataSource = FixtureTypes.Data.GetFixtureTypeMapList();
        }

        /// <summary>
        /// Returns all fixture data grid data
        /// </summary>
        /// <returns></returns>
        public DataGridViewRowCollection GetGridData()
        {
            // Force cell change to ensure all combo boxes are saved correctly
            if (fixtureDataGrid.Rows.Count > 0)
                fixtureDataGrid.CurrentCell = fixtureDataGrid.Rows[0].Cells[0];

            // Return grid rows
            return fixtureDataGrid.Rows;
        }

        /// <summary>
        /// Force dropdown in column databoxes to open on one click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FixtureDataGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            // Make sure the clicked row/column is valid and not the header row
            bool validClick = ((e.RowIndex != -1) && (e.ColumnIndex != -1) && (e.RowIndex != 1));

            // Set sender as data grid view
            DataGridView datagridview = sender as DataGridView;

            // Check to make sure the cell clicked is the cell containing the combobox 
            if (datagridview.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn && validClick)
            {
                // Open dropdown on first click
                datagridview.BeginEdit(true);
                ((ComboBox)datagridview.EditingControl).DroppedDown = true;
            }
        }

        /// <summary>
        /// Exit button in menu and alt + f4 button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        /// <summary>
        /// Run on program closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Prompt user to save his data
            if (e.CloseReason == CloseReason.UserClosing)
            {

            }

            // Autosave and clear up ressources
            if (e.CloseReason == CloseReason.WindowsShutDown)
            {

            }

            // Save mapping XML
            Mapping.Save.XML(this);

        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.About aboutForm = new Help.About();
            aboutForm.ShowDialog();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GelFrame.Settings
{
    public partial class SettingsForm : Form
    {
        // Main form
        private readonly MainForm mainForm;

        public SettingsForm(MainForm mainForm)
        {
            // Setup form
            InitializeComponent();

            // Set main form
            this.mainForm = mainForm;

            // Populate combo box data
            optionLayerBy.DataSource = Config.Patch.layerOrganizationList;

            // Set all text box values from currently loaded settings
            IList<TextBox> textBoxes = Functions.Forms.GetAllControls<TextBox>(this);
            foreach (TextBox textBox in textBoxes)
            {
                // Set textbox values from settings data
                textBox.Text = Settings.Data.GetValue(textBox.Name);

                // Normailze rotation offsets
                if (Config.Settings.rotationOffsetNameList.Contains(textBox.Name))
                    textBox.Text = Functions.Numbers.StringNormaliseDegree(textBox.Text);
            }

            // Set all combo box values from currently loaded settings
            IList<ComboBox> comboBoxes = Functions.Forms.GetAllControls<ComboBox>(this);
            foreach (ComboBox comboBox in comboBoxes)
            {
                // Apply datasource to rotation mappings
                if (Config.Settings.rotationMappingNameList.Contains(comboBox.Name))
                    comboBox.DataSource = new List<string>(Config.Settings.settingsRotaionMappingValueList);

                // Apply settings value
                comboBox.SelectedItem = Settings.Data.GetValue(comboBox.Name);
            }

            // Set all check box values from currently loaded settings
            IList<CheckBox> checkBoxes = Functions.Forms.GetAllControls<CheckBox>(this);
            foreach (CheckBox checkBox in checkBoxes)
                if (Settings.Data.GetValue(checkBox.Name) == Config.Settings.defaultTrue)
                    checkBox.Checked = true;
                else
                    checkBox.Checked = false;

            // Apply checkbox rules
            ApplyCheckBoxRules();

            // Add one empty rules if no rules found
            if (Settings.Data.GetRulesCount() == 0)
                AddRule();

            // Process rules
            else
            {
                // Process each rule
                foreach (KeyValuePair<int, Dictionary<string, string>> rulesDictionary in Settings.Data.GetRulesDictionary())
                {
                    // Create new rule box
                    AddRule();

                    // Rule number
                    int ruleNumber = rulesDictionary.Key;

                    // Rule Vars
                    string rulePrefix = Config.Settings.rulesPrefixName + ruleNumber;
                    GroupBox ruleBox = Functions.Forms.GetGroupBoxByName(rulePrefix + Config.Settings.rulesFormSuffixBox, rulesBox);

                    // Text boxes
                    IList<TextBox> ruleTextBoxes = Functions.Forms.GetAllControls<TextBox>(ruleBox);
                    foreach (TextBox textBox in ruleTextBoxes)
                    {
                        // Get rule name
                        string name = textBox.Name.Replace(rulePrefix, "");

                        // Set textbox values from rules data
                        if (rulesDictionary.Value.ContainsKey(name))
                            textBox.Text = rulesDictionary.Value[name];
                    }

                    // Set all check box values from currently loaded settings
                    IList<CheckBox> ruleCheckBoxes = Functions.Forms.GetAllControls<CheckBox>(ruleBox);
                    foreach (CheckBox checkBox in ruleCheckBoxes)
                    {
                        // Get rule name
                        string name = checkBox.Name.Replace(rulePrefix, "");


                        if ((rulesDictionary.Value.ContainsKey(name)) && (rulesDictionary.Value[name] == Config.Settings.defaultTrue))
                            checkBox.Checked = true;
                        else
                            checkBox.Checked = false;
                    }
                }
            }

            // Renumber rules
            ReNumberAllRules();
        }

        /// <summary>
        /// Set combo box selected item after form is shown. Combo box data sources not working correctly with this code in the 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsForm_Shown(object sender, EventArgs e)
        {
            // Process each rule
            foreach (KeyValuePair<int, Dictionary<string, string>> rulesDictionary in Settings.Data.GetRulesDictionary())
            {
                // Rule number
                int ruleNumber = rulesDictionary.Key;

                // Rule Vars
                string rulePrefix = Config.Settings.rulesPrefixName + ruleNumber;
                GroupBox ruleBox = Functions.Forms.GetGroupBoxByName(rulePrefix + Config.Settings.rulesFormSuffixBox, rulesBox);

                // Combo boxes
                IList<ComboBox> ruleComboBoxes = Functions.Forms.GetAllControls<ComboBox>(ruleBox);
                foreach (ComboBox comboBox in ruleComboBoxes)
                {
                    // Get rule name
                    string name = comboBox.Name.Replace(rulePrefix, "");

                    // Set selected item value from rules data
                    if (rulesDictionary.Value.ContainsKey(name))
                        comboBox.SelectedItem = rulesDictionary.Value[name].Trim();
                }

                // Normalise rule values
                Functions.Forms.NormaliseRuleValuesFromRule(rulePrefix, rulesBox);
            }
        }

        public List<string> GetColumnNameList()
        {
            List<string> columnNameList = new List<string>();

            IList<Label> labels = Functions.Forms.GetAllControls<Label>(this);
            foreach (Label label in labels)
            {
                if (label.Name.StartsWith(Config.Settings.prefixColumnLabel))
                {
                    columnNameList.Add(label.Text.Replace(":", "").Trim());
                }
            }
            columnNameList.Sort();
            return columnNameList;
        }

        public void SetIPText(string ipAddress)
        {
            maConsoleIP.Text = ipAddress;
        }

        public string ConsoleIP
        {
            get => this.maConsoleIP.Text;
            set => this.maConsoleIP.Text = value;
        }

        private void SelectIP_Click(object sender, EventArgs e)
        {
            IpForm ipForm = new IpForm(this);
            ipForm.ShowDialog();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Save settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Delcare settings dictionary
            Dictionary<string, string> settingsDictionary = new Dictionary<string, string>();

            // Empty box string
            string emptyTextBoxes = "";
            IList<Label> labels = Functions.Forms.GetAllControls<Label>(this);

            // Process all list boxes
            IList<TextBox> textBoxes = Functions.Forms.GetAllControls<TextBox>(this);
            foreach (TextBox textBox in textBoxes)
            {
                // Normailze rotation offsets
                if (Config.Settings.rotationOffsetNameList.Contains(textBox.Name))
                    textBox.Text = Functions.Numbers.StringNormaliseDegree(textBox.Text);

                // Only enabled values cannot be empty and ignore rules boxes
                if ((!(textBox.Name.StartsWith(Config.Settings.rulesPrefixName)) && (((Label)this.Controls.Find(Functions.Strings.GetLabelName(textBox.Name), true)[0]).Enabled) && (String.IsNullOrEmpty(textBox.Text.Trim()))))
                {
                    // Loop through all form labels
                    foreach (Label label in labels)

                        // Find label that matches textbox name
                        if (label.Name == Functions.Strings.GetLabelName(textBox.Name))

                            // Append label name to empty text box string
                            emptyTextBoxes += "\n  - " + label.Text.Remove(label.Text.Length - 1);
                }
                
                // Validate rules range
                else if (textBox.Name.EndsWith(Config.Settings.rulesSuffixPattern))
                {
                    // Validate range
                    if ((Functions.Forms.GetSwitchSelectedItem(textBox, rulesBox) == Config.Settings.rulesRangeSwitch) && (Functions.Numbers.ValidateRange(textBox.Text)))
                    {
                        MessageBox.Show(string.Format(Config.ErrorMessages.invalidRange, textBox.Text));
                        textBox.SelectAll();
                        textBox.Focus();
                        return;
                    }
                }
                
                // Save value reguardless of enabled status
                settingsDictionary.Add(textBox.Name, textBox.Text.Trim());
            }

            // Abort if any boxes are empty
            if (emptyTextBoxes.Length != 0)
            {
                MessageBox.Show(Config.ErrorMessages.emptySettings + emptyTextBoxes);
                return;
            }

            // Process all combo boxes
            IList<ComboBox> comboBoxes = Functions.Forms.GetAllControls<ComboBox>(this);
            foreach (ComboBox comboBox in comboBoxes)
                settingsDictionary.Add(comboBox.Name, comboBox.SelectedItem.ToString().Trim());

            // Process all check boxes
            IList<CheckBox> checkBoxes = Functions.Forms.GetAllControls<CheckBox>(this);
            foreach (CheckBox checkBox in checkBoxes)
                settingsDictionary.Add(checkBox.Name, checkBox.Checked.ToString().Trim());

            // Save Settings
            bool foundError = Save.XML(settingsDictionary);

            // Close settings window if no error
            if (foundError)
                MessageBox.Show(Config.ErrorMessages.saveSettings);
            else
            {
                // Save Settings
                mainForm.AddStatusNewLine(Config.StatusMessages.settingsUpdated);

                this.Close();
            }
        }

        private void TextBoxOnlyPostiveInteger_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Functions.Forms.TextBoxOnlyPostiveInteger(e);
        }

        /// <summary>
        /// Make degree textboxes only allow doubles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxOnlyDouble_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Functions.Forms.TextBoxOnlyDouble((sender as TextBox), e);
        }

        /// <summary>
        /// Increase or decrease value with arrow keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxDoubleAddSubtractOne_KeyDown(object sender, KeyEventArgs e)
        {
            // Process up and down presses
            bool handled = Functions.Forms.TextBoxDoubleArrowButtons((sender as TextBox), e);

            // Handle key input
            e.Handled = handled;

            // Normailze string if handle returned true
            if (handled)
                (sender as TextBox).Text = Functions.Numbers.StringNormaliseDegree((sender as TextBox).Text);
        }

        /// <summary>
        /// Normalises value between 0 and 360
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxOnlyDegree_Leave(object sender, EventArgs e)
        {
            // Normalise textbox string
            (sender as TextBox).Text = Functions.Numbers.StringNormaliseDegree((sender as TextBox).Text);
        }

        /// <summary>
        /// Enable/disable labels based on checkbox values
        /// </summary>
        private void ApplyCheckBoxRules()
        {
            // Get checkbox status
            Dictionary<string, bool> checkBoxStatus = CheckBoxStatus();

            // Process enable fields when checkbox is true
            foreach (KeyValuePair<string, string> enable in Config.Settings.enableCheckBoxDependancies)
            {
                // Get control by name
                var control = this.Controls.Find(enable.Key, true);

                // Enable control if checkbox is true
                if (checkBoxStatus[enable.Value])
                    control[0].Enabled = true;
                else
                    control[0].Enabled = false;
            }

            // Process disable fields when checkbox is true
            foreach (KeyValuePair<string, string> enable in Config.Settings.disableCheckBoxDependancies)
            {
                // Get control by name
                var control = this.Controls.Find(enable.Key, true);

                // Enable control if checkbox is true
                if (checkBoxStatus[enable.Value])
                    control[0].Enabled = false;
                else
                    control[0].Enabled = true;
            }
        }

        /// <summary>
        /// Get the checked/unchecked status of all checkboxes
        /// </summary>
        /// <returns>Dictionary with checkbox status</returns>
        private Dictionary<string, bool> CheckBoxStatus()
        {
            Dictionary<string, bool> checkBoxStatusDictionary = new Dictionary<string, bool>();

            // Get all checkboxes
            IList<CheckBox> checkBoxes = Functions.Forms.GetAllControls<CheckBox>(this);

            // Loop though checkboxes
            foreach (CheckBox checkBox in checkBoxes)

                // Write checkbox status to dictionary
                checkBoxStatusDictionary.Add(checkBox.Name, checkBox.Checked);

            // Return dictionary
            return checkBoxStatusDictionary;
        }

        private void CheckedChangedRules(object sender, EventArgs e)
        {
            ApplyCheckBoxRules();
        }

        /// <summary>
        /// Get the greatest rule box number
        /// </summary>
        /// <returns></returns>
        private int HighestRuleValue()
        {
            // Vars
            int highestRuleValue = 0;
            IList<GroupBox> groupBoxes = Functions.Forms.GetAllControls<GroupBox>(rulesBox);

            // Loop through all groupboxes
            foreach (GroupBox groupBox in groupBoxes)
            {
                if (groupBox.Name.StartsWith(Config.Settings.rulesPrefixName))
                {
                    // Convert name to int. This will remove all non numberic characters
                    int currentRuleNumber = Functions.Numbers.StringToPostiveInt(groupBox.Name);

                    // Set highest rule value if this value is greatest
                    if (currentRuleNumber > highestRuleValue)
                        highestRuleValue = currentRuleNumber;
                }
            }

            // Return highest number
            return highestRuleValue;
        }

        /// <summary>
        /// Number of rules
        /// </summary>
        /// <returns></returns>
        private int NumberofRules()
        {
            // Vars
            int ruleCount = 0;
            IList<GroupBox> groupBoxes = Functions.Forms.GetAllControls<GroupBox>(rulesBox);

            // Loop through all groupboxes and count rules
            foreach (GroupBox groupBox in groupBoxes)
                if (groupBox.Name.StartsWith(Config.Settings.rulesPrefixName))
                    ruleCount++;

            // Return highest number
            return ruleCount;
        }

        /// <summary>
        /// Add new rule button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddRule_Click(object sender, EventArgs e)
        {
            AddRule();
        }


        /// <summary>
        /// Add new empty rule
        /// </summary>
        private void AddRule()
        {

            // Suspend layout so new elements get scaled correctly
            SuspendLayout();
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;

            // Vars
            int ruleValue = HighestRuleValue() + 1;
            int ruleMultiplier = NumberofRules() - 1;
            string rulePrefix = Config.Settings.rulesPrefixName + ruleValue.ToString();

            // Create Condition Label
            Label ruleConditionLabel = new Label
            {
                AutoSize = true,
                Location = new Point(4, 24),
                Margin = new Padding(2, 0, 2, 0),
                Name = Functions.Strings.GetLabelName(rulePrefix + Config.Settings.rulesSuffixCondition),
                Size = new Size(54, 13),
                TabIndex = 8,
                Text = Config.Settings.rulesLabelCondition,
                TextAlign = ContentAlignment.MiddleRight,
            };

            // Create column combo box
            ComboBox ruleColumn = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                FormattingEnabled = true,
                Location = new Point(62, 20),
                Margin = new Padding(2),
                Name = rulePrefix + Config.Settings.rulesSuffixColumn,
                Size = new Size(141, 21),
                TabIndex = 9,
                DataSource = new List<string>(GetColumnNameList())
            };

            // Add events to change column
            ruleColumn.SelectedValueChanged += new EventHandler(this.RuleColumnChangeValue_Leave);

            // Create switch combo box
            ComboBox ruleSwitch = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                FormattingEnabled = true,
                Location = new Point(207, 20),
                Margin = new Padding(2),
                Name = rulePrefix + Config.Settings.rulesSuffixSwitch,
                Size = new Size(87, 21),
                TabIndex = 10,
                DataSource = new List<string>(Config.Settings.rulesSwitchList)
            };

            // Create pattern text box
            TextBox rulePattern = new TextBox
            {
                Location = new Point(298, 20),
                Margin = new Padding(2),
                MaxLength = 255,
                Name = rulePrefix + Config.Settings.rulesSuffixPattern,
                Size = new Size(145, 20),
                TabIndex = 8,
            };

            // Add events to pattern
            rulePattern.KeyPress += new KeyPressEventHandler(this.RulePatternValue_KeyPress);
            rulePattern.Leave += new EventHandler(this.RulePatternValue_Leave);

            // Create Condition Label
            Label ruleChangeLabel = new Label
            {
                AutoSize = true,
                Location = new Point(11, 49),
                Margin = new Padding(2, 0, 2, 0),
                Name = Functions.Strings.GetLabelName(rulePrefix + Config.Settings.rulesSuffixChange),
                Size = new Size(47, 13),
                TabIndex = 8,
                Text = Config.Settings.rulesLabelChange,
                TextAlign = ContentAlignment.MiddleRight,
            };

            // Create change column combo box
            ComboBox ruleChangeColumn = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                FormattingEnabled = true,
                Location = new Point(62, 46),
                Margin = new Padding(2),
                Name = rulePrefix + Config.Settings.rulesSuffixChangeColumn,
                Size = new Size(141, 21),
                TabIndex = 9,
                DataSource = new List<string>(Config.Settings.rulesChangeColumnLabelList),
            };

            // Add events to change column
            ruleChangeColumn.SelectedValueChanged += new EventHandler(this.RuleChangeValue_Leave);

            // Create change value text box
            TextBox ruleChangeValue = new TextBox
            {
                Location = new Point(207, 46),
                Margin = new Padding(2),
                MaxLength = 25,
                Name = rulePrefix + Config.Settings.rulesSuffixChangeValue,
                Size = new Size(87, 20),
                TabIndex = 8,
                TextAlign = HorizontalAlignment.Center
            };

            // Add events to change value
            ruleChangeValue.KeyDown += new KeyEventHandler(this.RuleChangeValue_KeyDown);
            ruleChangeValue.KeyPress += new KeyPressEventHandler(this.TextBoxOnlyDouble_KeyPress);
            ruleChangeValue.Leave += new EventHandler(this.RuleChangeValue_Leave);

            // Create enabled checkbox
            CheckBox ruleEnabled = new CheckBox
            {
                AutoSize = true,
                Location = new Point(335, 48),
                Margin = new Padding(2),
                Name = rulePrefix + Config.Settings.rulesSuffixEnabled,
                Size = new Size(65, 17),
                TabIndex = 15,
                Text = Config.Settings.rulesEnabledText,
                UseVisualStyleBackColor = true,
            };

            // Create delete button
            Button ruleDeleteButton = new Button
            {
                Location = new Point(425, 46),
                Name = rulePrefix + Config.Settings.rulesFormSuffixDelete,
                Size = new Size(18, 21),
                Text = Config.Settings.rulesDeleteButtonText,
                UseVisualStyleBackColor = true,
            };

            // Add events tot delete button
            ruleDeleteButton.Click += new EventHandler(this.RuleDelete_Click);

            // Create new rule box
            GroupBox newRuleBox = new GroupBox
            {
                Margin = new Padding(2),
                Name = rulePrefix + Config.Settings.rulesFormSuffixBox,
                Text = Config.Settings.rulesPrefixTitleName + ruleValue,
                Padding = new Padding(2),
                Size = new Size(450, 74),
                TabStop = false,
                Dock = DockStyle.None
            };

            // Add controls to new rule box
            newRuleBox.Controls.Add(ruleConditionLabel);
            newRuleBox.Controls.Add(ruleColumn);
            newRuleBox.Controls.Add(ruleSwitch);
            newRuleBox.Controls.Add(rulePattern);
            newRuleBox.Controls.Add(ruleChangeLabel);
            newRuleBox.Controls.Add(ruleChangeColumn);
            newRuleBox.Controls.Add(ruleChangeValue);
            newRuleBox.Controls.Add(ruleEnabled);
            newRuleBox.Controls.Add(ruleDeleteButton);

            // Add rule box to rules panel
            rulesPanel.Controls.Add(newRuleBox);

            // Resume layout
            ResumeLayout();

            // Renumber all rules
            ReNumberAllRules();
        }

        /// <summary>
        /// Delete all rules and empty the values in #1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAllRules_Click(object sender, EventArgs e)
        {
            // Confirm delete all
            if ((MessageBox.Show(Config.Settings.rulesMessageDeleteAllRules, Config.Settings.rulesMessageDeleteAllRulesHeader, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes))
                return;

            // Delete all rules
            IList<GroupBox> groupBoxes = Functions.Forms.GetAllControls<GroupBox>(rulesBox);
            foreach (GroupBox groupBox in groupBoxes)
                rulesPanel.Controls.Remove(groupBox);

            // Add new empty rule
            AddRule_Click(sender, e);
        }

        /// <summary>
        /// Delete single rule 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RuleDelete_Click(object sender, EventArgs e)
        {
            // Get the rule number of the clicked button
            int clickedRuleNumber = Functions.Numbers.StringToPostiveInt((sender as Button).Name);

            // Confirm delete
            if ((MessageBox.Show(string.Format(Config.Settings.rulesMessageDeleteRule, clickedRuleNumber.ToString()), string.Format(Config.Settings.rulesMessageDeleteRuleHeader, clickedRuleNumber.ToString()), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes))
                return;

            // Loop through all rules
            IList<GroupBox> groupBoxes = new List<GroupBox>(Functions.Forms.GetAllControls<GroupBox>(rulesBox));
            foreach (GroupBox groupBox in groupBoxes)
            {
                // Get the number of this rule box
                int ruleNumber = Functions.Numbers.StringToPostiveInt(groupBox.Name);

                // Delete clicked rule
                if (ruleNumber == clickedRuleNumber)

                    // Delete rule box
                    rulesPanel.Controls.Remove(groupBox);
            }

            // Renumber rules
            ReNumberAllRules();
        }

        /// <summary>
        /// Renumbers rules
        /// </summary>
        private void ReNumberAllRules()
        {
            // Vars
            int newRuleNumber = 1;

            // Loop through all rule boxes
            List<GroupBox> groupBoxes = new List<GroupBox>(Functions.Forms.GetAllControls<GroupBox>(rulesBox));
            foreach (GroupBox groupBox in groupBoxes)
            {
                // Reset text to new rule number
                groupBox.Text = Config.Settings.rulesPrefixTitleName + newRuleNumber;

                // Increase rule counter
                newRuleNumber++;
            }
        }

        /// <summary>
        /// Rule change value leave 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RuleChangeValue_Leave(object sender, EventArgs e)
        {
            Functions.Forms.NormaliseRuleValuesFromForm(sender, rulesBox);
        }

        /// <summary>
        /// Rule change value leave 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RulePatternValue_Leave(object sender, EventArgs e)
        {
            // Process range switch
            if (Functions.Forms.GetSwitchSelectedItem(sender, rulesBox) == Config.Settings.rulesRangeSwitch)
            {
                // Validate range 
                if (Functions.Numbers.ValidateRange((sender as TextBox).Text))
                {
                    MessageBox.Show(string.Format(Config.ErrorMessages.invalidRange, (sender as TextBox).Text));
                    (sender as TextBox).SelectAll();
                    (sender as TextBox).Focus();
                }
            }
        }

        /// <summary>
        /// Rule colmun value leave 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RuleColumnChangeValue_Leave(object sender, EventArgs e)
        {
            // Combobox
            ComboBox column = (sender as ComboBox);

            // Get the rule number of the clicked button
            int clickedRuleNumber = Functions.Numbers.StringToPostiveInt(column.Name);
            string rulePrefix = Config.Settings.rulesPrefixName + clickedRuleNumber.ToString();

            // Get switch combo box
            ComboBox switchComboBox = Functions.Forms.GetComboBoxByName(rulePrefix + Config.Settings.rulesSuffixSwitch, rulesBox);

            // Process range selection
            if (Config.Settings.rulesRangeList.Contains(column.SelectedItem))
            {
                // Set switch data source to range
                switchComboBox.DataSource = new List<string>(Config.Settings.rulesRangeDataList);

                // Validate current pattern
                TextBox patternTextBox = Functions.Forms.GetTextBoxByName(rulePrefix + Config.Settings.rulesSuffixPattern, rulesBox);

                // Ignore empty box
                if (!(string.IsNullOrEmpty(patternTextBox.Text)))
                    RulePatternValue_Leave((patternTextBox as object), null);
            }

            // Remove range selection
            else
            {
                // Only update data source if current selection is range
                if ((string)switchComboBox.SelectedItem == Config.Settings.rulesRangeSwitch)
                    switchComboBox.DataSource = new List<string>(Config.Settings.rulesSwitchList);
            }
        }

        /// <summary>
        /// Rule change value key press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RuleChangeValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = Functions.Forms.TextBoxOnlyDouble((sender as TextBox), e);
        }

        /// <summary>
        /// Rule pattern change value key press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RulePatternValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Process range switch
            if (Functions.Forms.GetSwitchSelectedItem(sender, rulesBox) == Config.Settings.rulesRangeSwitch)
                e.Handled = Functions.Forms.TextBoxRange(e);
        }

        /// <summary>
        /// Rule change value key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RuleChangeValue_KeyDown(object sender, KeyEventArgs e)
        {
            // Process up and down presses
            bool handled = Functions.Forms.TextBoxDoubleArrowButtons((sender as TextBox), e);

            // Handle key input
            e.Handled = handled;

            // Normailze string if handle returned true
            if (handled)
                Functions.Forms.NormaliseRuleValuesFromForm(sender, rulesBox);
        }
    }
}

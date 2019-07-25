using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GelFrame.Functions
{
    static class Forms
    {
        #region CONTROLS
        /// <summary>
        /// Loop through all controls and find given control
        /// </summary>
        /// <typeparam name="T">All controls</typeparam>
        /// <param name="control">Specific control to find</param>
        /// <returns></returns>
        public static IList<T> GetAllControls<T>(Control control) where T : Control
        {
            var lst = new List<T>();
            foreach (Control item in control.Controls)
            {
                if (item is T ctr)
                    lst.Add(ctr);
                else
                    lst.AddRange(GetAllControls<T>(item));

            }
            return lst;
        }

        /// <summary>
        /// Find all nested controls
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static IList<T> FindControlsRecursive<T>(Control parent) where T : Control
        {
            var foundControls = new List<T>();
            FindControlsRecursive(parent, foundControls);
            return foundControls;
        }

        private static void FindControlsRecursive<T>(Control parent, List<T> foundControls) where T : Control
        {
            foreach (Control control in parent.Controls)
            {
                if (control is T)
                    foundControls.Add((T)control);
                else
                    FindControlsRecursive(control, foundControls);
            }
        }

        /// <summary>
        /// Get textbox by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static TextBox GetTextBoxByName(string name, Control parent)
        {
            // Get all textboxes
            IList<TextBox> textBoxes = FindControlsRecursive<TextBox>(parent);

            // Find textbox that matches name
            foreach (TextBox textBox in textBoxes)
                if (textBox.Name == name)
                    return textBox;

            // No textbox found
            return null;
        }

        /// <summary>
        /// Get combo box by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static ComboBox GetComboBoxByName(string name, Control parent)
        {
            // Get all comboboxes
            IList<ComboBox> comboBoxes = FindControlsRecursive<ComboBox>(parent);

            // Find combobox that matches name
            foreach (ComboBox comboBox in comboBoxes)
                if (comboBox.Name == name)
                    return comboBox;

            // No combobox found
            return null;
        }

        /// <summary>
        /// Get checkbox by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static CheckBox GetCheckBoxByName(string name, Control parent)
        {
            // Get all checkboxes
            IList<CheckBox> checkBoxes = FindControlsRecursive<CheckBox>(parent);

            // Find checkbox that matches name
            foreach (CheckBox checkBox in checkBoxes)
                if (checkBox.Name == name)
                    return checkBox;

            // No checkbox found
            return null;
        }

        /// <summary>
        /// Get group box by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static GroupBox GetGroupBoxByName(string name, Control parent)
        {
            // Get all groupboxes
            IList<GroupBox> groupBoxes = FindControlsRecursive<GroupBox>(parent);

            // Find groupbox that matches name
            foreach (GroupBox groupBox in groupBoxes)
                if (groupBox.Name == name)
                    return groupBox;

            // No groupbox found
            return null;
        }
        #endregion

        #region TEXTBOXES
        /// <summary>
        /// Only allow double in textbox
        /// Usage: e.Handled = Functions.Forms.TextBoxOnlyDouble((sender as TextBox), e);
        /// </summary>
        /// <param name="textBox">Textbox to process</param>
        /// <param name="e">Handled bool</param>
        /// <returns></returns>
        static public bool TextBoxOnlyDouble(TextBox textBox, KeyPressEventArgs e)
        {
            // Only allow numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
                return true;

            // Only allow one decimal point
            if ((e.KeyChar == '.') && (textBox.Text.IndexOf('.') > -1))
                return true;

            // Only allow negative at beginning
            if ((e.KeyChar == '-') && ((textBox.SelectionStart != 0) || (textBox.Text.Contains("-"))))
                return true;

            return false;
        }

        /// <summary>
        /// Increase/decrease double value by one whole number based on arrow up/down key press
        /// Usage: e.Handled = Functions.Forms.TextBoxDoubleArrowButtons((sender as TextBox), e);
        /// </summary>
        /// <param name="textBox">Textbox to process</param>
        /// <param name="e">Handled bool</param>
        /// <returns></returns>
        static public bool TextBoxDoubleArrowButtons(TextBox textBox, KeyEventArgs e)
        {
            // Increase
            if (e.KeyCode == Keys.Down)
            {
                double degree = Functions.Numbers.StringToDouble(textBox.Text);
                textBox.Text = (degree - 1).ToString();
                textBox.SelectionStart = textBox.Text.Length;
                textBox.SelectionLength = 0;
                return true;
            }

            // Decrease
            if (e.KeyCode == Keys.Up)
            {
                double degree = Functions.Numbers.StringToDouble(textBox.Text);
                textBox.Text = (degree + 1).ToString();
                textBox.SelectionStart = textBox.Text.Length;
                textBox.SelectionLength = 0;
                return true;
            }

            return false;
        }

        /// Only allow double in textbox
        /// Usage: e.Handled = Functions.Forms.TextBoxOnlyPostiveInteger(e);
        /// </summary>
        /// <param name="textBox">Textbox to process</param>
        /// <param name="e">Handled bool</param>
        /// <returns></returns>
        static public bool TextBoxOnlyPostiveInteger(KeyPressEventArgs e)
        {
            // Only allow postive whole numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                return true;

            return false;
        }

        /// <summary>
        /// Normalise rule change value if change column is a rotation column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="rulesBox"></param>
        static public void NormaliseRuleValuesFromForm(object sender, GroupBox rulesBox)
        {
            // Vars 
            string ruleName;

            // Get row rule number
            if (sender is ComboBox)
                ruleName = Config.Settings.rulesPrefixName + Numbers.StringToPostiveInt((sender as ComboBox).Name);
            else
                ruleName = Config.Settings.rulesPrefixName + Numbers.StringToPostiveInt((sender as TextBox).Name);

            NormaliseRuleValuesFromRule(ruleName, rulesBox);
        }

        /// <summary>
        /// Normalise rule change value if change column is a rotation column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="rulesBox"></param>
        static public void NormaliseRuleValuesFromRule(string ruleName, GroupBox rulesBox)
        {
            // Build box names
            string changeColumnComboBoxName = ruleName + Config.Settings.rulesSuffixChangeColumn;
            string changeChangeValueTextBoxName = ruleName + Config.Settings.rulesSuffixChangeValue;

            // Get textbox to modify
            TextBox valueTextBox = GetTextBoxByName(changeChangeValueTextBoxName, rulesBox);

            // Get change column selected value
            ComboBox changeComboBox = GetComboBoxByName(changeColumnComboBoxName, rulesBox);
            string changeColumnComboBoxValue = Config.Settings.rulesChangeColumnDictionary[changeComboBox.SelectedItem.ToString()];

            // Normalise value if change column is a rotation column
            if (Config.Settings.columnRotationList.Contains(changeColumnComboBoxValue))
            {
                valueTextBox.Text = Functions.Numbers.StringNormaliseDegree(valueTextBox.Text);
                valueTextBox.SelectionStart = valueTextBox.Text.Length;
                valueTextBox.SelectionLength = 0;
            }
        }

        /// <summary>
        /// Normalise rule change value if change column is a rotation column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="rulesBox"></param>
        static public bool TextBoxRange(KeyPressEventArgs e)
        {
            // Only allow postive whole numbers, dash, and comma
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ',') && (e.KeyChar != '-'))
                return true;

            return false;
        }

        /// <summary>
        /// Get the seleteced switch value for a given row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="groupBox"></param>
        /// <returns></returns>
        static public string GetSwitchSelectedItem(object sender, GroupBox groupBox)
        {
            // Vars 
            string rulePrefix;

            // Get row rule number
            if (sender is ComboBox)
                rulePrefix = Config.Settings.rulesPrefixName + Numbers.StringToPostiveInt((sender as ComboBox).Name);
            else
                rulePrefix = Config.Settings.rulesPrefixName + Numbers.StringToPostiveInt((sender as TextBox).Name);

            // Get switch combo box
            ComboBox switchComboBox = Functions.Forms.GetComboBoxByName(rulePrefix + Config.Settings.rulesSuffixSwitch, groupBox);

            // Return selected item
            return (string)switchComboBox.SelectedItem;
        }


        #endregion
    }
}

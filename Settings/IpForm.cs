using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace GelFrame.Settings
{
    public partial class IpForm : Form
    {
        // Define object vars
        private readonly List<string> ipAddressList = new List<string>();
        private readonly SettingsForm settingsForm;

        public IpForm(SettingsForm settingsForm)
        {
            // Initial form
            InitializeComponent();
            GetAllLocalIPv4();

            // Set vars
            this.settingsForm = settingsForm;
            ipListBox.DataSource = ipAddressList;

            // Set selected item to IP address
            ipListBox.SelectedItem = settingsForm.ConsoleIP;
        }

        /// <summary>
        /// Build list of all ipv4 addresses
        /// </summary>
        public void GetAllLocalIPv4()
        {
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        ipAddressList.Add(ip.Address.ToString());
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            settingsForm.ConsoleIP = ipListBox.SelectedItem.ToString();
            this.Close();
        }

        /// <summary>
        /// Override esc key press and close without saving
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}

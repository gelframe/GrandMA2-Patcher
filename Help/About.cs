using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GelFrame.Help
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();

            labelVersion.Text = Config.Help.version + Config.App.verison;
        }

        private void Website_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Config.Help.url);
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

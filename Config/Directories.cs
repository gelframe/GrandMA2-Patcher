using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GelFrame.Config
{
    static class Directories
    {
        // Directory config
        public static readonly string seperator = "\\";

        // MA onPC directoriesb
        public static readonly string maImportExport = "\\importexport\\";
        public static readonly string maLibrary = "\\library\\";

        // App directories
        public static readonly string app = Path.GetDirectoryName(Application.ExecutablePath) + seperator;
        public static readonly string XML = app + "data" + seperator;
        public static readonly string localData = Path.GetDirectoryName(Application.ExecutablePath) + maLibrary;

    }
}

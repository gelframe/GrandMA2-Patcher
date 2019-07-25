using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GelFrame.Functions
{
    class File
    {


        // https://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use
        public static bool IsFileLocked(string fileName)
        {
            FileInfo file = new FileInfo(fileName);
            FileStream stream = null;
            if (file.Exists)
            {
                try
                {
                    stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                }
                catch (IOException)
                {
                    //the file is unavailable because it is:
                    //still being written to
                    //or being processed by another thread
                    //or does not exist (has already been processed)
                    return true;
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }
            //file is not locked
            return false;
        }

        public static string SaveFileDialog(string title, string filter, string initialDirectory = null)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = filter,
                Title = title
            };

            if (initialDirectory != null)
                saveFileDialog.InitialDirectory = initialDirectory;

            bool fileUnlocked;
            do
            {
                saveFileDialog.ShowDialog();

                // If the file name is not an empty string open it for saving.  
                if (saveFileDialog.FileName == "")
                    return null;

                if (IsFileLocked(saveFileDialog.FileName))
                {
                    fileUnlocked = false;
                    saveFileDialog.InitialDirectory = Path.GetDirectoryName(saveFileDialog.FileName);
                    saveFileDialog.FileName = "";
                    MessageBox.Show($"Error: {saveFileDialog.FileName} is open in another program.\nPlease close this file or select another file name to save to.");
                }
                else
                    fileUnlocked = true;

            } while (fileUnlocked == false);

            return saveFileDialog.FileName;
        }
    }
}

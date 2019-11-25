using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace FTPSplitUtility
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = ".pdf";
            openFileDialog1.Filter = "Pdf documents (.pdf)|*.pdf";
            openFileDialog1.InitialDirectory = "S:\\RNDB\\CFD\\";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }

        }

        private void btnSplitPath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = "S:\\RNDB\\CFD\\Split\\";   // "S:\\RNDB\\CFD\\Split";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textSplitPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnSplitFiles_Click(object sender, EventArgs e)
        {
            string strCommand;

            Cursor.Current = Cursors.WaitCursor;

            strCommand = textBox1.Text + " burst output " + "\"" + textSplitPath.Text + "\\" + txtSplitName.Text + "_%04d.pdf\"";

            var processInfo = new ProcessStartInfo("pdftk.exe", strCommand);

            processInfo.CreateNoWindow = true;

            processInfo.UseShellExecute = false;

            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            var process = Process.Start(processInfo);

            process.Start();

            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            //MOVE FILE TO PROCESSED
            string path = textBox1.Text;
            string path2 = textSplitPath.Text + "\\processed\\" + txtSplitName.Text + ".pdf";

            if (!File.Exists(path))
            {
                // This statement ensures that the file is created,
                // but the handle is not kept.
                using (FileStream fs = File.Create(path)) { }
            }

            // Ensure that the target does not exist.
            if (File.Exists(path2))
                File.Delete(path2);

            // Move the file.
            File.Move(path, path2);
            Console.WriteLine("{0} was moved to {1}.", path, path2);

            Cursor.Current = Cursors.Default;

            MessageBox.Show("Files were split to " + textSplitPath.Text, "SplitFiles Completion");


        }
    }
}

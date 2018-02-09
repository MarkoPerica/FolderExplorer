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

namespace FolderExplorer
{
    public partial class Form1 : Form
    {
        List<string> listFiles = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listFiles.Clear();
            listView1.Items.Clear();
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Odaberite putanju." })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string item in Directory.GetFiles(fbd.SelectedPath))
                    {
                        
                    }
                }
            }    
        }        
    }
}

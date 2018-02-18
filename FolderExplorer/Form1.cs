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
        List<ListViewItem> listFiles = new List<ListViewItem>();
        
        public Form1()
        {
            InitializeComponent();
        }
                                 
        private void button1_Click(object sender, EventArgs e)
        {
            listFiles.Clear();
            listView1.Items.Clear();
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;
                        
            using(FolderBrowserDialog fbd = new FolderBrowserDialog() { Description="Select your path." })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = fbd.SelectedPath;
                    foreach(string file in Directory.GetFiles(fbd.SelectedPath))
                    {
                        FileInfo fi = new FileInfo(file);
                        item = new ListViewItem(fi.Name, 1);
                        subItems = new ListViewItem.ListViewSubItem[]
                        {
                            new ListViewItem.ListViewSubItem(item, file.Length.ToString()),
                            new ListViewItem.ListViewSubItem(item, fi.LastAccessTime.ToShortDateString() + "/" + fi.LastAccessTime.ToShortTimeString())
                        };
                        item.SubItems.AddRange(subItems);
                        listView1.Items.Add(item);
                    }

                    foreach(string directory in Directory.GetDirectories(fbd.SelectedPath))
                    {
                        DirectoryInfo di = new DirectoryInfo(directory);
                        item = new ListViewItem(di.Name, 0);
                        subItems = new ListViewItem.ListViewSubItem[]
                        {
                            new ListViewItem.ListViewSubItem(item, ""),
                            new ListViewItem.ListViewSubItem(item, di.LastAccessTime.ToShortDateString() + "/" + di.LastAccessTime.ToShortTimeString())
                        };
                        item.SubItems.AddRange(subItems);
                        listView1.Items.Add(item);
                    }
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
       
}

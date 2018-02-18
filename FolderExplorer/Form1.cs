using System;
using System.Collections;
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
        private int sortColumn = -1;
                
        public Form1()
        {
            InitializeComponent();
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            button1.Click += new EventHandler(this.button1_Click);
            checkBox1.Click += new EventHandler(this.checkBox1_CheckedChanged);
        }
                                 
        private void button1_Click(object sender, EventArgs e)
        {
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

        private void listView1_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            if (e.Column != sortColumn)
            {
                sortColumn = e.Column;
                listView1.Sorting = SortOrder.Ascending;
            }
            else
            {
                if (listView1.Sorting == SortOrder.Ascending)
                    listView1.Sorting = SortOrder.Descending;
                else
                    listView1.Sorting = SortOrder.Ascending;
            }
            listView1.Sort();
            this.listView1.ListViewItemSorter = new ListViewItemComparer(e.Column, listView1.Sorting);

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            FileAttributes attributes = File.GetAttributes(txtPath.Text);

            if (checkBox1.Checked == true)
            {
                if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    attributes = RemoveAttribute(attributes, FileAttributes.Hidden);
                    File.SetAttributes(txtPath.Text, attributes);
                }
                else
                {
                    File.SetAttributes(txtPath.Text, File.GetAttributes(txtPath.Text) | FileAttributes.Hidden);
                }
            }
        }

        private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
        {
            return attributes & ~attributesToRemove;
        }
    }

    class ListViewItemComparer : IComparer
    {
        private int column;
        private SortOrder order;
        
        public ListViewItemComparer()
        {
            column = 0;
            order = SortOrder.Ascending;
        }

        public ListViewItemComparer(int column, SortOrder order)
        {
            this.column = column;
            this.order = order;
        }

        public int Compare(object x, object y)
        {
            int returnVal;
            try
            {
                System.DateTime firstDate = DateTime.Parse(((ListViewItem)x).SubItems[column].Text);
                System.DateTime secondDate = DateTime.Parse(((ListViewItem)y).SubItems[column].Text);
                returnVal = DateTime.Compare(firstDate, secondDate);
            }
            catch
            {
                returnVal = String.Compare(((ListViewItem)x).SubItems[column].Text,
                                          ((ListViewItem)y).SubItems[column].Text);
            }
            if (order == SortOrder.Descending)
                returnVal *= -1;
            return returnVal;
        }
    }
}

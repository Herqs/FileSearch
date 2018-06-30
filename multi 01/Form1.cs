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
using System.Threading;
using System.Threading.Tasks;

namespace multi_01
{
    public partial class Form1 : Form
    {
        FolderBrowserDialog Fbd = new FolderBrowserDialog();
        int i = 0,ii=0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Fbd.ShowDialog() == DialogResult.OK)
            {

                richTextBox1.AppendText("\r\nLocation locked successfuly");
                richTextBox1.ScrollToCaret();

                button2.Enabled = true;
            }
        }

        private void ListFiles(FolderBrowserDialog Fbd)
        {
            String[] Files = Directory.GetFiles(Fbd.SelectedPath);

            /*foreach (string file in Files)
            {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        listBox1.Items.Add(file);
                    });

            }*/
            if (checkBox1.Checked==true)
            {
                foreach (string file in Files)
                {
                    if (Path.GetFileName(file).Contains(textBox1.Text))
                    {
                        object obj = (file);
                        Thread t3 = new Thread(addfile);
                        t3.Start(obj);
                    }


                          //  this.BeginInvoke((MethodInvoker)delegate
                         //   {
                         //       listBox3.Items.Add(Path.GetFileName(file));
                         //   });
                }
            }
        }

        private void addfile(object file)
        {
            string filestring = file.ToString();

            this.BeginInvoke((MethodInvoker)delegate
            {
                ListViewItem item1 = new ListViewItem(Path.GetFileName(filestring));
                item1.SubItems.Add(Path.GetDirectoryName(filestring));
                listView1.Items.Add(item1);
              //  listView1.Items.Add(filestring);1111111
                listBox3.Items.Add(filestring);
            });

        }

        private void Godeep(Object obj)
        {
            ii++;
            FolderBrowserDialog Fbd = new FolderBrowserDialog();
            Fbd.SelectedPath = obj.ToString();
            ListFiles(Fbd);
            String[] Dirs = Directory.GetDirectories(Fbd.SelectedPath);
            FolderBrowserDialog Fbdd= new FolderBrowserDialog();
            foreach (string dir in Dirs)
            {
                Fbdd.SelectedPath = dir;
                this.BeginInvoke((MethodInvoker)delegate
                {
                 //   listBox2.Items.Add(dir);
                });
                Godeep(Fbdd.SelectedPath);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox1.Enabled = true;
            else
                textBox1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listView1.Items.Clear();
            i = 0;
            ii = 0;
            object obj = Fbd.SelectedPath;

            richTextBox1.AppendText("\r\nStarting folder calculation");
            richTextBox1.ScrollToCaret();

            CalcFol();
            label2.Text = i.ToString();

            Thread t1 = new Thread(Godeep);
            Thread t2 = new Thread(SearchProgress);

            t2.IsBackground = true;
            t1.IsBackground = true;

            t2.Start();
            t1.Start(obj);

            richTextBox1.AppendText("\r\nStarting file analyzing process");
            richTextBox1.ScrollToCaret();
            if (checkBox1.Checked==true)
                richTextBox1.AppendText("\r\nLooking for keyword: "+textBox1.Text);

            richTextBox1.AppendText("\r\nStarting progress bar");
            richTextBox1.ScrollToCaret();
        }   

      /*  private void CountFolders(string path)
        {
            FolderBrowserDialog Fbd = new FolderBrowserDialog();
            Fbd.SelectedPath = path;

            String[] Dirs = Directory.GetDirectories(Fbd.SelectedPath);
            FolderBrowserDialog Fbdd = new FolderBrowserDialog();

            foreach (string dir in Dirs)
            {
                Fbdd.SelectedPath = dir;
                i++;
                
                CountFolders(Fbdd.SelectedPath);
            }
        }*/

        private void CalcFol()
        {
            string[] folders = System.IO.Directory.GetDirectories($@"{Fbd.SelectedPath}\", "*", System.IO.SearchOption.AllDirectories);
            foreach(string asd in folders)
            {
                i++;
            }
            richTextBox1.AppendText("\r\nFolders calculated successfully, total folders: "+i);
            richTextBox1.ScrollToCaret();
            i++;
        }


        private void SearchProgress()
        {
            bool go = true;
            while (go)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    progressBar1.Value = ii / i * 100;
                    if (ii==i)
                    {
                        richTextBox1.AppendText("\r\nAnalyzing complete");
                        go = false;
                    }
                    label3.Text = ii.ToString();
                });
                Thread.Sleep(50);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using KLib2_CSharp;

namespace klib2_test
{
    public partial class Form1 : Form
    {
        KLib2 klib = new KLib2();
        bool connet = false;
        Thread thread;
        int nRow;
        int nCol;
        Label[][] labels;

        public Form1()
        {
            InitializeComponent();
            button1.Text = "Connect";
            this.DoubleBuffered = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!connet)
            {
                if (klib.Start())
                {
                    connet = true;
                    thread = new Thread(new ThreadStart(workingthread));
                    thread.Start();
                    Invoke(new MethodInvoker(() => button1.Text = "Disconnect"));
                }
                return;
            }
            else
            {
                if (klib.Stop())
                {
                    connet = false;
                    if(sender != null)
                        thread.Abort();
                    Invoke(new MethodInvoker(() => button1.Text = "Connect"));
                }
            }
        }

        private void FormResize(int _row, int _col)
        {
            this.Width = _col * 20 + 100;
            this.Height = _row * 21;
            button1.Location = new Point(this.Width - 100,button1.Location.Y);
        }

        private void workingthread()
        {
            while(connet)
            {
                if (nRow == 0)
                {
                    if (!klib.BIsFirstConnect)
                    {
                        nRow = klib.nRow;
                        nCol = klib.nCol;
                        //Form Windows Label View
                        labels = new Label[nCol][];

                        Invoke(new MethodInvoker(() => { FormResize( nCol, nRow); }));
                        this.SuspendLayout();
                        for (int i = 0; i < nCol; ++i)
                        {
                            labels[i] = new Label[nRow];
                            for (int j = 0; j <  nRow; ++j)
                            {
                                labels[i][j] = new Label();
                                labels[i][j].AutoSize = true;
                                labels[i][j].Name = string.Format("Label{0}_{1}", i, j);
                                labels[i][j].Text = "0";
                                labels[i][j].MaximumSize = new Size(100, 100);
                                labels[i][j].Location = new Point(i * 20, j * 20);
                                Invoke(new MethodInvoker(() => { contorolsAdd(labels[i][j]); }));
                            }
                        }
                        this.ResumeLayout(false);
                        this.PerformLayout();
                        this.Invalidate();

                    }
                }
                //get API data
                byte[] data = klib.Read();

                //Form Windows Label View
                if (data == null)
                {
                    button1_Click(null, null);
                    MessageBox.Show("Disconnect ForceLAB2!", "Connect error!");
                    return;
                }
                for (int i = 0; i <  nCol; ++i)
                {
                    for (int j = 0; j < nRow; ++j)
                    {
                        if (labels[i][j].Text != data[j * nCol + i].ToString())
                            //labels[i][j].Text = data[j * nCol + i].ToString();
                            Invoke(new MethodInvoker(()=>labelsdatachange(data[j * nCol + i].ToString(), j, i)));
                    }
                }

                //Console Windows Write Code
                //for (int i = 0; i < nCol; ++i)
                //{
                //    for (int j = 0; j < nRow; ++j)
                //    {
                //        Console.Write(data[i * nRow + j].ToString("000")+" ");
                //    }
                //    Console.WriteLine();
                //}

                //Console.WriteLine();
                //Console.WriteLine();
            }
        }

        private void labelsdatachange(string _data,int _x,int _y)
        {
            labels[_y][_x].Text = _data;
        }

        private void contorolsAdd(Label _label)
        {
            this.Controls.Add(_label);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (thread != null)
            {
                try
                {
                    thread.Abort();
                }
                catch(Exception err)
                {
                    Console.WriteLine(err);
                }
            }
        }
    }
}

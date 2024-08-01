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
    public partial class MainForm : Form
    {
        KLib2 klib = new KLib2();
        bool connet = false;
        Thread thread;
        int nRow;
        int nCol;
        Label[][] labels;

        public MainForm()
        {
            InitializeComponent();
            button1.Text = "Connect";
            this.DoubleBuffered = true;
        }

        private void buttonConnet_Click(object sender, EventArgs e)
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
            DateTime startTime = DateTime.Now;
            uint count = 0;
            while(connet)
            {
                if (nRow == 0)
                {
                    if (!klib.BIsFirstConnect)
                    {
                        nRow = klib.nRow;
                        nCol = klib.nCol;

                        Invoke(new MethodInvoker(() => { FormResize( nCol, nRow); }));
                        this.ResumeLayout(false);
                        this.PerformLayout();
                        this.Invalidate();

                    }
                }
                //get API data
                byte[] data = klib.Read();
                StringBuilder dataString = new StringBuilder();

                //Form Windows Label View
                if (data == null)
                {
                    buttonConnet_Click(null, null);
                    MessageBox.Show("Disconnect ForceLAB2!", "Connect error!");
                    return;
                }
                for (int j = 0; j < nRow; ++j)
                {
                    for (int i = 0; i < nCol; ++i)
                    {
                        dataString.Append( $"{data[j * nCol + i].ToString("000")},");
                    }
                    dataString.Append("\n");
                }
                dataString.Append("\n");

                this.Invoke(new MethodInvoker(
                        () => { richTextBoxData.Text = dataString.ToString(); }
                    )
                );
                
                count++;
                if(DateTime.Now - startTime > TimeSpan.FromSeconds(1))
                {
                    Console.WriteLine(count);
                    count = 0;
                    startTime = DateTime.Now;
                }

            }
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

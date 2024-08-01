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
using System.Collections;

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

        private void FormResize(int _col, int _row)
        {
            this.Width = _col * 35 + 100;
            this.Height = _row * 25;
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

                if (data == null)
                {
                    buttonConnet_Click(null, null);
                    MessageBox.Show("Disconnect ForceLAB2!", "Connect error!");
                    return;
                }

                StringBuilder dataString = new StringBuilder();
                if(klib.GetDataType == KLib2.DATATYPE.Force)
                {
                    // Force 데이터의 경우 double로 형변환 후 표현
                    double[] doubleArray = ByteArrayToDoubleArray(data);

                    for (int j = 0; j < nRow; ++j)
                    {
                        for (int i = 0; i < nCol; ++i)
                        {
                            dataString.Append($"{doubleArray[j * nCol + i].ToString("0.000")},");
                        }
                        dataString.Append("\n");
                    }
                    dataString.Append("\n");
                }
                else
                {
                    for (int j = 0; j < nRow; ++j)
                    {
                        for (int i = 0; i < nCol; ++i)
                        {
                            dataString.Append($"{data[j * nCol + i].ToString("000")},");
                        }
                        dataString.Append("\n");
                    }
                    dataString.Append("\n");
                }
              
             

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

        public static double[] ByteArrayToDoubleArray(byte[] byteArray)
        {
            if (byteArray == null)
                throw new ArgumentNullException(nameof(byteArray));

            // Ensure the byte array length is a multiple of 8
            if (byteArray.Length % 8 != 0)
                throw new ArgumentException("The length of the byte array must be a multiple of 8.", nameof(byteArray));

            int doubleCount = byteArray.Length / 8;
            double[] doubleArray = new double[doubleCount];

            for (int i = 0; i < doubleCount; i++)
            {
                doubleArray[i] = BitConverter.ToDouble(byteArray, i * 8);
            }

            return doubleArray;
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

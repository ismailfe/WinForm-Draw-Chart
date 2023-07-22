using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using IDSQL;

namespace GrafikCizOlustur
{
    public partial class Form1 : Form
    {
        SQL SQL = new SQL();
        string SQLServerName = "SQLEXPRESS";
        string DBName = "DBChart";
        string TableName = "tb1";
        string[] TableColumn = new string[4];
        
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            SQL.ConnectionPath = "Data Source=.\\" + SQLServerName + ";Initial Catalog=" + DBName + ";Integrated Security=True";
            TableColumn[0] = "C0";
            TableColumn[1] = "C1";
            TableColumn[2] = "C2";
            TableColumn[3] = "C3";

        }

        private void button1_Click(object sender, EventArgs e)
        {
           // SQL.WRITE_ToSQL_WithWriteUniqueNo(TableName, TableColumn, )

              SQL.READ_FromSQL_FillDGV(TableName, dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
          //string A =   SQL.Open_CMD("SELECT STDEV(C01) FROM " + TableName); 
          //string B = SQL.Open_CMD("SELECT STDEV(DISTINCT " + " C01 " +") AS Distinct_Values, STDEV("+ " C01 " + ") AS All_Values FROM " + TableName);

            string C = SQL.Open_CMD_ResultinDGV(TB_Sorgu.Text, dataGridView1);
            TB_Sorgu_Durum.Text = C;

        }




        private void btnDraw_Click(object sender, EventArgs e)
        {
            float mean          = float.Parse(txtMean.Text);
            float stddev        = float.Parse(txtStdDev.Text);
            float var           = stddev * stddev;

            // Make a bitmap.
            Bitmap bm = new Bitmap(picGraph.ClientSize.Width, picGraph.ClientSize.Height);
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;

                // Define the mapping from world
                // coordinates onto the PictureBox.
                const float wxmin = -10.1f;
                const float wymin = -0.2f;
                const float wxmax = -wxmin;
                const float wymax = 1.1f;
                const float wwid = wxmax - wxmin;
                const float whgt = wymax - wymin;
                RectangleF world = new RectangleF(wxmin, wymin, wwid, whgt);
                PointF[] device_points =
                {
                    new PointF(0, picGraph.ClientSize.Height),
                    new PointF(picGraph.ClientSize.Width, picGraph.ClientSize.Height),
                    new PointF(0, 0),
                };
                Matrix transform = new Matrix(world, device_points);

                // Make a thin Pen to use.
                using (Pen pen = new Pen(Color.Red, 0))
                {
                    using (Font font = new Font("Arial", 8))
                    {
                        // Draw the X axis.
                        //gr.Transform = transform;
                        //pen.Color = Color.Black;
                        //gr.DrawLine(pen, wxmin, 0, wxmax, 0);
                        //for (int x = (int)wxmin; x <= wxmax; x++)
                        //{
                        //    gr.DrawLine(pen, x, -0.05f, x, 0.05f);
                        //    gr.DrawLine(pen, x + 0.25f, -0.025f, x + 0.25f, 0.025f);
                        //    gr.DrawLine(pen, x + 0.50f, -0.025f, x + 0.50f, 0.025f);
                        //    gr.DrawLine(pen, x + 0.75f, -0.025f, x + 0.75f, 0.025f);
                        //}

                        // Label the X axis.
                        //gr.Transform = new Matrix();
                        //gr.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                        //List<PointF> ints = new List<PointF>();
                        //for (int x = (int)wxmin; x <= wxmax; x++)
                        //    ints.Add(new PointF(x, -0.07f));
                        //PointF[] ints_array = ints.ToArray();
                        //transform.TransformPoints(ints_array);

                        //using (StringFormat sf = new StringFormat())
                        //{
                        //    sf.Alignment = StringAlignment.Center;
                        //    sf.LineAlignment = StringAlignment.Near;
                        //    int index = 0;
                        //    for (int x = (int)wxmin; x <= wxmax; x++)
                        //    {
                        //        gr.DrawString(x.ToString(), font, Brushes.Black,
                        //            ints_array[index++], sf);
                        //    }
                        //}

                        // Draw the Y axis.
                        //gr.Transform = transform;
                        //pen.Color = Color.Black;
                        //gr.DrawLine(pen, 0, wymin, 0, wymax);
                        //for (int y = (int)wymin; y <= wymax; y++)
                        //{
                        //    gr.DrawLine(pen, -0.2f, y, 0.2f, y);
                        //    gr.DrawLine(pen, -0.1f, y + 0.25f, 0.1f, y + 0.25f);
                        //    gr.DrawLine(pen, -0.1f, y + 0.50f, 0.1f, y + 0.50f);
                        //    gr.DrawLine(pen, -0.1f, y + 0.75f, 0.1f, y + 0.75f);
                        //}

                        // Label the Y axis.
                        //gr.Transform = new Matrix();
                        //gr.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                        //ints = new List<PointF>();
                        //for (float y = 0.25f; y < 1.01; y += 0.25f)
                        //    ints.Add(new PointF(0.2f, y));
                        //ints_array = ints.ToArray();
                        //transform.TransformPoints(ints_array);

                        //using (StringFormat sf = new StringFormat())
                        //{
                        //    sf.Alignment = StringAlignment.Near;
                        //    sf.LineAlignment = StringAlignment.Center;
                        //    int index = 0;
                        //    foreach (float y in new float[] { 0.25f, 0.5f, 0.75f, 1.0f })
                        //    {
                        //        gr.DrawString(y.ToString("0.00"), font, Brushes.Black,
                        //            ints_array[index++], sf);
                        //    }
                        //}

                        // Draw the curve.
                        gr.Transform = transform;
                        List<PointF> points = new List<PointF>();
                        float one_over_2pi = (float)(1.0 / (stddev * Math.Sqrt(2 * Math.PI)));
                        ch.Series[0].Points.Clear();
                        float dx = (wxmax - wxmin) / picGraph.ClientSize.Width;


                        float[] NDis = new float[2500];
                        float[] Yex = new float[2500];
                        for (int i = 0; i < 2499; i++)
                        {
                            if (i == 0)
                            {
                                NDis[i] = mean - (stddev * 10);
                            }
                            else
                            {
                                NDis[i] = (stddev / 10) + NDis[i-1];
                            }

                            Yex[i] = F(NDis[i], one_over_2pi, mean, stddev, var);

                            ch.Series[0].Points.AddXY(NDis[i], Yex[i]);

                        }






                        for (float x = wxmin; x <= wxmax; x += dx)
                        {
                            float y = F(x, one_over_2pi, mean, stddev, var);
                   
                            points.Add(new PointF(x, y));
                            //ch.Series[0].Points.AddXY(A, y);
                            if (y != 0)
                            {


                            }
                        }




                        //for (int x = 0; x <= dataGridView1.RowCount - 2; x ++)
                        //{
                        //    float A = mean - ( stddev * 2 ) ;
                        //    float y = F(A, one_over_2pi, mean, stddev, var);
                        //    points.Add(new PointF(A, y));
                        //    if (y != 0)
                        //    {

                        //    }
                        //}
                        pen.Color = Color.Red;
                        gr.DrawLines(pen, points.ToArray());
                    } // Font
                } // Pen

                picGraph.Image = bm;
            }
        }


        public void GrafikCiz(float std, float avg, int cnt)
        {
            float mean = avg; // float.Parse(txtMean.Text);
            float stddev = std; // float.Parse(txtStdDev.Text);
            float stddevKaresi      = stddev * stddev;
            float one_over_2pi      = (float)(1.0 / (stddev * Math.Sqrt(2 * Math.PI)));

            float[] NDis            = new float[cnt];
            float[] Yex             = new float[cnt];

            ch.Series[0].Points.Clear();
            ch.ChartAreas[0].RecalculateAxesScale();
            ch.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            ch.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            ch.ChartAreas[0].AxisX.Maximum = avg * 3; // 100;
            ch.ChartAreas[0].AxisX.Minimum = avg * -1; // - 20;
            

            int Carpan = 2;

            if ((cnt / 20) > 15 )
            {
                Carpan = 15;
            }

            else if ((cnt / 20) < 2 )
            {
                Carpan = 5;
            }
            else if((cnt / 20) <= 15 && (cnt / 20) >= 2)
            {
                Carpan = (cnt / 20);
            }
            else
            {
                Carpan = 3;
            }

            for (int i = 0; i < cnt - 1; i++)
            {
                if (i == 0)
                {
                    NDis[i] = mean - (stddev * (Carpan) ); //15
                }
                else
                {
                    NDis[i] = (stddev / (Carpan)) + NDis[i - 1];
                }

                Yex[i] = F(NDis[i], one_over_2pi, mean, stddev, stddevKaresi);

                ch.Series[0].Points.AddXY(NDis[i], Yex[i]);

            }
        }


        // The normal distribution function.
        private float F(float x, float one_over_2pi, float mean, float stddev, float stddevKaresi)
        {
            return (float)(one_over_2pi * Math.Exp(-(x - mean) * (x - mean) / (2 * stddevKaresi)));
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void B_RasgeleSayiOlustur_Click(object sender, EventArgs e)
        {
            //Random a = new Random();
            //int rnd = a.Next(1 - 100);
            string cmd      = "SELECT RAND()*(10-5)+5";
            string cmd1     = "Delete tb2";
            string cmd2     = "DELETE tb2 DECLARE @sayac1 INT = 1 WHILE @sayac1< " + Adt.Text + "BEGIN insert INTO tb2(x) VALUES(RAND() * ("+ lmtU.Text +"-"+ lmtA.Text +") +" + lmtA.Text + " );  SET @sayac1 += 1 END";
            string cmd3 = "DECLARE @std float DECLARE @avg float DECLARE @cnt float SELECT @std = STDEV(x) from tb2 SELECT @avg = AVG(x) From tb2 SELECT @cnt = COUNT(x) From tb2 UPDATE tb3 SET std = @std, avg = @avg, cnt = @cnt SELECT* FROM tb3";


            string a = SQL.Open_CMD_ResultinDGV(cmd2, dataGridView1);
            string B = SQL.Open_CMD_ResultinDGV(cmd3, dataGridView2);

            float std = float.Parse(dataGridView2.Rows[0].Cells[1].Value.ToString() );
            float avg = float.Parse(dataGridView2.Rows[0].Cells[2].Value.ToString());
            int cnt = int.Parse(dataGridView2.Rows[0].Cells[3].Value.ToString());

            GrafikCiz(std, avg, cnt);
        }


                 

        private void chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var xAxis = chart.ChartAreas[0].AxisX;
            var yAxis = chart.ChartAreas[0].AxisY;

            try
            {
                if (e.Delta < 0) // Scrolled down.
                {
                    xAxis.ScaleView.ZoomReset();
                    yAxis.ScaleView.ZoomReset();
                }
                else if (e.Delta > 0) // Scrolled up.
                {
                    var xMin = xAxis.ScaleView.ViewMinimum;
                    var xMax = xAxis.ScaleView.ViewMaximum;
                    var yMin = yAxis.ScaleView.ViewMinimum;
                    var yMax = yAxis.ScaleView.ViewMaximum;

                    var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
                    var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;
                    var posYStart = yAxis.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / 4;
                    var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / 4;

                    xAxis.ScaleView.Zoom(posXStart, posXFinish);
                    yAxis.ScaleView.Zoom(posYStart, posYFinish);
                }
            }
            catch { }
        }

        private void ch_MouseDown(object sender, MouseEventArgs e)
        {

        }
    }
}

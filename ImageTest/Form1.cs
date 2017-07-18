using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Filters;

namespace ImageTest
{
    public partial class Form1 : Form
    {

        Color selectedColor;
        bool chromaSet;
        Bitmap StartBitmap;


        public Form1()
        {
            InitializeComponent();
            selectedColor = Color.White;
            StartBitmap = new Bitmap(1, 1);
        }

        private Bitmap overlayBitmaps(Bitmap topBitmap, Bitmap bottomBitmap)
        {
            Bitmap combined = new Bitmap(bottomBitmap.Size.Width, bottomBitmap.Size.Height);
            for (int x = 0; x < combined.Size.Width; x++)
            {
                for (int y = 0; y < combined.Size.Height; y++)
                {
                    if (topBitmap.GetPixel(x, y).A == 255)
                    {
                        combined.SetPixel(x, y, topBitmap.GetPixel(x, y));
                    }
                    else
                    {
                        combined.SetPixel(x, y, bottomBitmap.GetPixel(x, y));
                    }

                }
            }

            return combined;
        }

        private double colorDistance(Color C1, Color C2)
        {
            double r = (C1.R + C2.R) / 2.0;
            int deltaR = C1.R - C2.R;
            int deltaG = C1.G - C2.G;
            int deltaB = C1.B - C2.B;

            double deltaColor = Math.Sqrt((2 + r / 256) * deltaR * deltaR + 4 * deltaG * deltaG + (2 + (255 - r) / 256) * deltaB * deltaB);
            return deltaColor;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            


            Image overlay = Image.FromFile(@"C:\users\feltytj\desktop\TransparentTest.png");
            Image source = Image.FromFile(@"C:\users\feltytj\desktop\cameraGreen.jpg");
            Bitmap ov = new Bitmap(overlay);
            Bitmap src = new Bitmap(source);
            Bitmap Overlaid = new Bitmap(1,1);

            

            if (ov.Size == src.Size)
            {
                Overlaid = overlayBitmaps(ov, src);
                Overlaid.ImageBlurFilter(ExtBitmap.BlurType.GaussianBlur5x5);
                StartBitmap = Overlaid;
            }
            else
            {
                return;
            }

            picBox1.Image = Overlaid;
            picBox1.Refresh();


        }

        private void picBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(e.X.ToString()+","+e.Y.ToString());

            Bitmap lcl = new Bitmap(picBox1.Image);
           

            selectedColor = lcl.GetPixel(e.X, e.Y);
            chromaSet = true;
            Bitmap clrB = new Bitmap(50, 50);
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    clrB.SetPixel(x, y, selectedColor);
                }
            }

            clrBox.Image = clrB;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Image ChromaBack = Image.FromFile(@"C:\users\feltytj\desktop\Sat_resize.jpg");
            Bitmap ChromaBackBitMap = new Bitmap(ChromaBack);

            Bitmap currImage = new Bitmap(StartBitmap);
            

            if (chromaSet)
            {
                
                for (int x = 0; x < currImage.Size.Width; x++)
                {
                    for (int y = 0; y < currImage.Size.Height; y++)
                    {
                        Color currColor = currImage.GetPixel(x,y);
                        double colorDist = Math.Abs(colorDistance(currColor,selectedColor));
                        double threshold = Convert.ToDouble(textBox1.Text);

                        if (currColor.R < 55 && currColor.G < 255 && currColor.G > 100 && currColor.B > 50 && currColor.B < 80)
                            currImage.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                        
                        //if (colorDist < threshold || (currColor.G > 1.75*currColor.R) || (currColor.G > 1.75*currColor.B) )
                        //{
                        //    currImage.SetPixel(x, y, Color.FromArgb(0, 0, 0, 0));
                        //}
                    }
                }

                //picBox1.Image = currImage;
                //picBox1.Refresh();

                Bitmap Combined = overlayBitmaps(currImage, ChromaBackBitMap);
                picBox1.Image = Combined;
                picBox1.Refresh();
            }

        }

        
        

        private void button3_Click(object sender, EventArgs e)
        {
           Image ChromaBack = Image.FromFile(@"C:\users\feltytj\desktop\Sat_resize.jpg");
           Bitmap bg = new Bitmap(ChromaBack);

           Image source = Image.FromFile(@"C:\users\feltytj\desktop\cameraGreen.jpg");
           Bitmap fg = new Bitmap(source);

           Bitmap outB = new Bitmap(bg);

           outB = greenScreen.computeGreenScreen(fg, bg, selectedColor);
           picBox1.Image = outB;
           picBox1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TCamDevice[] allDevice = DeviceManager.GetAllDevices();
            

            allDevice[0].ShowWindow(picBox1);

            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TCamDevice[] allDevice = DeviceManager.GetAllDevices();
            allDevice[0].Stop();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            TCamDevice[] allDevice = DeviceManager.GetAllDevices();
            Bitmap capture = allDevice[0].GetFrame();
            allDevice[0].Stop();

            Image ChromaBack = Image.FromFile(@"C:\users\feltytj\desktop\Sat_resize.jpg");
            Bitmap bg = new Bitmap(ChromaBack);

            picBox2.Image = capture;

            //Image source = Image.FromFile(@"C:\users\feltytj\desktop\cameraGreen.jpg");
            //Bitmap fg = new Bitmap(source);

            Bitmap outB = new Bitmap(bg);

            outB = greenScreen.computeGreenScreen(capture, bg, selectedColor);
            picBox1.Image = outB;
            picBox1.Refresh();
        }

        private void picBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Bitmap lcl = new Bitmap(picBox2.Image);


            selectedColor = lcl.GetPixel(e.X, e.Y);
            chromaSet = true;
            Bitmap clrB = new Bitmap(50, 50);
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    clrB.SetPixel(x, y, selectedColor);
                }
            }

            clrBox.Image = clrB;
        }

        


        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Filters;
using Touchless.Vision.Camera;
using WebCamLib;

namespace ImageTest
{
    public partial class Form1 : Form
    {

        Color selectedColor;
        bool chromaSet;
        Bitmap StartBitmap;
        private CameraFrameSource _frameSource;
        private static Bitmap _latestFrame;

        public Form1()
        {
            InitializeComponent();
            selectedColor = Color.White;
            StartBitmap = new Bitmap(1, 1);
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
                Overlaid = greenScreen.overlayBitmaps(ov, src);
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

        
        

        private void button3_Click(object sender, EventArgs e)
        {
           Image ChromaBack = Image.FromFile(@"C:\users\feltytj\desktop\Sat_resize.jpg");
           Bitmap bg = new Bitmap(ChromaBack);
            
           Image source = Image.FromFile(@"C:\users\feltytj\desktop\cameraGreen.jpg");
           Bitmap fg = new Bitmap(picBox1.Image);//source);

           Bitmap outB = new Bitmap(bg);

           //have to make sure the pictures are the same size
           int fg_H = fg.Height;
           int fg_W = fg.Width;
           int bg_H = bg.Height;
           int bg_W = bg.Width;

           int smallestH = Math.Min(fg_H, bg_H);
           int smallestW = Math.Min(fg_W, bg_W);

           Bitmap fgB = new Bitmap(fg.GetThumbnailImage(smallestW, smallestH, null, System.IntPtr.Zero));
           Bitmap bgB = new Bitmap(bg.GetThumbnailImage(smallestW, smallestH, null, System.IntPtr.Zero));

           outB = greenScreen.computeGreenScreen(fgB, bgB, selectedColor, trackBar1.Value, trackBar2.Value);
           picBox1.Image = outB;
           picBox1.Refresh();
        }

               

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                // Refresh the list of available cameras
                comboBoxCameras.Items.Clear();
                foreach (Camera cam in CameraService.AvailableCameras)
                    comboBoxCameras.Items.Add(cam);

                if (comboBoxCameras.Items.Count > 0)
                    comboBoxCameras.SelectedIndex = 0;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            thrashOldCamera();
        }

        private void thrashOldCamera()
        {
            // Trash the old camera
            if (_frameSource != null)
            {
                _frameSource.NewFrame -= OnImageCaptured;
                _frameSource.Camera.Dispose();
                setFrameSource(null);
                picBox1.Paint -= new PaintEventHandler(drawLatestImage);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // Early return if we've selected the current camera
            if (_frameSource != null && _frameSource.Camera == comboBoxCameras.SelectedItem)
                return;

            thrashOldCamera();
            startCapturing();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            thrashOldCamera();
            picBox1.Image = _latestFrame;
            picBox1.Refresh();
        }

        private void setFrameSource(CameraFrameSource cameraFrameSource)
        {
            if (_frameSource == cameraFrameSource)
                return;

            _frameSource = cameraFrameSource;
        }

        private void startCapturing()
        {
            try
            {
                Camera c = (Camera)comboBoxCameras.SelectedItem;
                setFrameSource(new CameraFrameSource(c));
                _frameSource.Camera.CaptureWidth = 1600;
                _frameSource.Camera.CaptureHeight = 1200;
                _frameSource.Camera.Fps = 20;
                _frameSource.NewFrame += OnImageCaptured;

                picBox1.Paint += new PaintEventHandler(drawLatestImage);
                _frameSource.StartFrameCapture();
            }
            catch (Exception ex)
            {
                comboBoxCameras.Text = "Select A Camera";
                MessageBox.Show(ex.Message);
            }
        }

        private void drawLatestImage(object sender, PaintEventArgs e)
        {
            if (_latestFrame != null)
            {
                // Draw the latest image from the active camera
                e.Graphics.DrawImage(_latestFrame, 0, 0, _latestFrame.Width, _latestFrame.Height);
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            // snap camera
            if (_frameSource != null)
                _frameSource.Camera.ShowPropertiesDialog();
        }

        public void OnImageCaptured(Touchless.Vision.Contracts.IFrameSource frameSource, Touchless.Vision.Contracts.Frame frame, double fps)
        {
            _latestFrame = frame.Image;
            picBox1.Invalidate();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = trackBar1.Value.ToString();
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            textBox3.Text = trackBar2.Value.ToString();
        }
        
    }
}

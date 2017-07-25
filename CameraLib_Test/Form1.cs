using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices.ComTypes;
using Camera_NET;
using DirectShowLib;
using System.Drawing.Imaging;

namespace CameraLib_Test
{
    public partial class FormCameraControlTool : Form
    {
        // Camera choice
        private CameraChoice _CameraChoice = new CameraChoice();
        private Bitmap currSnapShot;
        private Bitmap chromaBack = null;
        private Bitmap combinedChroma = null;
        private Color chromaC = Color.White;

        public FormCameraControlTool()
        {
            InitializeComponent();
            chromaBack = new Bitmap(@"C:\users\feltytj\desktop\MarsTest_RightSize.jpg");
        }

        private void comboBoxCameraList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCameraList.SelectedIndex < 0)
            {
                cameraControl.CloseCamera();
            }
            else
            {
                // Set camera
                SetCamera(_CameraChoice.Devices[comboBoxCameraList.SelectedIndex].Mon, null);
            }

            FillResolutionList();
        }

        private void comboBoxResolutionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cameraControl.CameraCreated)
                return;

            int comboBoxResolutionIndex = comboBoxResolutionList.SelectedIndex;
            if (comboBoxResolutionIndex < 0)
            {
                return;
            }
            ResolutionList resolutions = Camera.GetResolutionList(cameraControl.Moniker);

            if (resolutions == null)
                return;

            if (comboBoxResolutionIndex >= resolutions.Count)
                return; // throw

            if (0 == resolutions[comboBoxResolutionIndex].CompareTo(cameraControl.Resolution))
            {
                // this resolution is already selected
                return;
            }

            // Recreate camera
            SetCamera(cameraControl.Moniker, resolutions[comboBoxResolutionIndex]);
        }

        private void FormCameraControlTool_Load(object sender, EventArgs e)
        {
            // Fill camera list combobox with available cameras
            FillCameraList();

            // Select the first one
            if (comboBoxCameraList.Items.Count > 0)
            {
                comboBoxCameraList.SelectedIndex = 0;
            }

            // Fill camera list combobox with available resolutions
            FillResolutionList();
        }

        private void FormCameraControlTool_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Close camera
            cameraControl.CloseCamera();
        }

        private void SetCamera(IMoniker camera_moniker, Resolution resolution)
        {
            try
            {
                // NOTE: You can debug with DirectShow logging:
                //cameraControl.DirectShowLogFilepath = @"C:\YOUR\LOG\PATH.txt";

                // Makes all magic with camera and DirectShow graph
                cameraControl.SetCamera(camera_moniker, resolution);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, @"Error while running camera");
            }

            if (!cameraControl.CameraCreated)
                return;

            // If you are using Direct3D surface overlay
            // (see documentation about rebuild of library for it)
            //cameraControl.UseGDI = false;

            cameraControl.MixerEnabled = true;

            cameraControl.OutputVideoSizeChanged += Camera_OutputVideoSizeChanged;

            UpdateCameraBitmap();
                       
        }
        
        // Event handler for OutputVideoSizeChanged event
        private void Camera_OutputVideoSizeChanged(object sender, EventArgs e)
        {
            // Update camera's bitmap (new size needed)
            UpdateCameraBitmap();
                        
        }

        private void UpdateCameraBitmap()
        {
            if (!cameraControl.MixerEnabled)
                return;
        }

        private void FillResolutionList()
        {
            comboBoxResolutionList.Items.Clear();

            if (!cameraControl.CameraCreated)
                return;

            ResolutionList resolutions = Camera.GetResolutionList(cameraControl.Moniker);

            if (resolutions == null)
                return;


            int index_to_select = -1;

            for (int index = 0; index < resolutions.Count; index++)
            {
                comboBoxResolutionList.Items.Add(resolutions[index].ToString());

                if (resolutions[index].CompareTo(cameraControl.Resolution) == 0)
                {
                    index_to_select = index;
                }
            }

            // select current resolution
            if (index_to_select >= 0)
            {
                comboBoxResolutionList.SelectedIndex = index_to_select;
            }
        }

        private void FillCameraList()
        {
            comboBoxCameraList.Items.Clear();

            _CameraChoice.UpdateDeviceList();

            foreach (var camera_device in _CameraChoice.Devices)
            {
                comboBoxCameraList.Items.Add(camera_device.Name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!cameraControl.CameraCreated)
                return;

            Bitmap bitmap = null;
            try
            {
                bitmap = cameraControl.SnapshotSourceImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Error while getting a snapshot");
            }

            if (bitmap == null)
                return;

            currSnapShot = bitmap;
            pictureBoxScreenshot.Image = bitmap.GetThumbnailImage(pictureBoxScreenshot.Width, pictureBoxScreenshot.Height, null, (IntPtr)0);
            pictureBoxScreenshot.Update();
        }

        private void pictureBoxScreenshot_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Bitmap snapShotImage = new Bitmap(pictureBoxScreenshot.Image);
            chromaC = snapShotImage.GetPixel(e.X, e.Y);
            Bitmap chromaBoxBMP = new Bitmap(chromaBox.Width, chromaBox.Height);
            for (int x = 0; x < chromaBox.Width; x++)
                for (int y = 0; y < chromaBox.Height; y++)
                    chromaBoxBMP.SetPixel(x, y,chromaC);
            chromaBox.Image = chromaBoxBMP;
            chromaBox.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int tolA = Convert.ToInt32(tolABox.Text);
            int tolB = Convert.ToInt32(tolBBox.Text);

            //images need to be the same size
            //so the chroma image either needs to shrink or be upconverted.

            double aspectRatioSnapShot = currSnapShot.Width / currSnapShot.Height;
            double aspectRatioChroma = chromaBack.Width / currSnapShot.Height;

            int imageSizeSnapShot = currSnapShot.Width * currSnapShot.Height;
            int imageSizeChroma = chromaBack.Width * chromaBack.Height;

            if (imageSizeSnapShot != imageSizeChroma)
            {
                //resize chroma to be same size
                chromaBack = greenScreen.ResizeImage(chromaBack, currSnapShot.Width, currSnapShot.Height);
            }
            

            combinedChroma = greenScreen.computeGreenScreen(currSnapShot, chromaBack, chromaC, tolA, tolB);
            pictureBoxScreenshot.Image = combinedChroma.GetThumbnailImage(pictureBoxScreenshot.Width, pictureBoxScreenshot.Height, null, (IntPtr)0);
            pictureBoxScreenshot.Update();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    chromaBack = new Bitmap(ofd.FileName);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "BMP|*.bmp|JPG|*.jpg|PNG|*.png";
                ImageFormat format = ImageFormat.Png;
                System.Drawing.Imaging.Encoder myEncoderColor;
                System.Drawing.Imaging.Encoder myEncoderQuality;
                EncoderParameter myEncoderParameter;
                EncoderParameter myEncoderParameterQ;
                EncoderParameters myEncoderParameters;

                myEncoderColor = System.Drawing.Imaging.Encoder.ColorDepth;
                myEncoderQuality = System.Drawing.Imaging.Encoder.Quality;

                myEncoderParameters = new EncoderParameters(2);

                myEncoderParameter = new EncoderParameter(myEncoderColor, 24);
                myEncoderParameterQ = new EncoderParameter(myEncoderQuality, 95);
                myEncoderParameters.Param[0] = myEncoderParameter;
                myEncoderParameters.Param[0] = myEncoderParameterQ;

                if(sfd.ShowDialog()==DialogResult.OK)
                {
                    string ext = System.IO.Path.GetExtension(sfd.FileName);
                    switch (ext)
                    {
                        case ".jpg":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;
                    }
                    combinedChroma.Save(sfd.FileName, format);
                }
            }

        }
    }
}

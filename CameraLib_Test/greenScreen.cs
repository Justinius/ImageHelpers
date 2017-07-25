using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace CameraLib_Test
{
    public static class greenScreen
    {
        private static int rgb2y(int r, int g, int b)
        {
            /*a utility function to convert colors in RGB into YCbCr*/
            int y;
            y = (int)Math.Round(0.299 * r + 0.587 * g + 0.114 * b);
            return (y);
        }

        private static int rgb2cb(int r, int g, int b)
        {
            /*a utility function to convert colors in RGB into YCbCr*/
            int cb;
            cb = (int)Math.Round(128 + -0.168736 * r - 0.331264 * g + 0.5 * b);
            return (cb);
        }

        private static int rgb2cr(int r, int g, int b)
        {
            /*a utility function to convert colors in RGB into YCbCr*/
            int cr;
            cr = (int)Math.Round(128 + 0.5 * r - 0.418688 * g - 0.081312 * b);
            return (cr);
        }

        private static double colorclose(int Cb_p, int Cr_p, int Cb_key, int Cr_key, int tola, int tolb)
        {
            /*decides if a color is close to the specified hue*/
            double temp = Math.Sqrt((Cb_key - Cb_p) * (Cb_key - Cb_p) + (Cr_key - Cr_p) * (Cr_key - Cr_p));
            if (temp < tola) { return (0.0); }
            if (temp < tolb) { return ((temp - tola) / (tolb - tola)); }
            return (1.0);
        }

        public static Bitmap computeGreenScreen(Bitmap foreGround, Bitmap backGround, Color chromakey, int tola = 40, int tolb = 100)
        {
            Bitmap outB = new Bitmap(backGround);

            int b, g, r, cb, cr; //y
            int b_key, g_key, r_key, cb_key, cr_key; //, tola, tolb;
            double mask;


            r_key = chromakey.R;
            g_key = chromakey.G;
            b_key = chromakey.B;
            cb_key = rgb2cb(r_key, g_key, b_key);
            cr_key = rgb2cr(r_key, g_key, b_key);

            //tola = 40;
            //tolb = 100;

            for (int x = 0; x < backGround.Size.Width; x++)
            {
                for (int y = 0; y < backGround.Size.Height; y++)
                {
                    Color fgC = foreGround.GetPixel(x, y);
                    Color bgC = backGround.GetPixel(x, y);
                    cb = rgb2cb(fgC.R, fgC.G, fgC.B);
                    cr = rgb2cr(fgC.R, fgC.G, fgC.B);
                    mask = colorclose(cb, cr, cb_key, cr_key, tola, tolb);
                    mask = 1 - mask;
                    r = (byte)(Math.Max(fgC.R - mask * r_key, 0) + mask * bgC.R);
                    g = (byte)(Math.Max(fgC.G - mask * g_key, 0) + mask * bgC.G);
                    b = (byte)(Math.Max(fgC.B - mask * b_key, 0) + mask * bgC.B);
                    outB.SetPixel(x, y, Color.FromArgb(255, r, g, b));
                }
            }

            return outB;
        }
    
        public static Bitmap overlayBitmaps(Bitmap topBitmap, Bitmap bottomBitmap)
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

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}

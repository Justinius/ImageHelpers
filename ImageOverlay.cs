private void button1_Click(object sender, EventArgs e)
{

    Image overlay = Image.FromFile(@"C:\users\feltytj\desktop\TransparentTest.png");
    Image source = Image.FromFile(@"C:\users\feltytj\desktop\MarsTest_RightSize.jpg");
    Bitmap ov = new Bitmap(overlay);
    Bitmap src = new Bitmap(source);
    //picBox1.Image = Image.FromFile(@"C:\users\feltytj\desktop\TransparentTest.png");
    //picBox1.Image = Image.FromFile(@"C:\users\feltytj\desktop\MarsTest_RightSize.jpg");
    Bitmap Overlaid = new Bitmap(ov.Size.Width, ov.Size.Height);


    if (ov.Size == src.Size)
    {
        for (int x = 0; x < ov.Size.Width; x++)
        {
            for (int y = 0; y < ov.Size.Height; y++)
            {
                if(ov.GetPixel(x,y).A == 255)
                {
                    Overlaid.SetPixel(x,y,ov.GetPixel(x,y));
                }
                else
                {
                    Overlaid.SetPixel(x,y,src.GetPixel(x,y));
                }

            }
        }
    }

    picBox1.Image = Overlaid;
    picBox1.Refresh();
}

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace PhotoEditing
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Image image = Image.FromFile(@"C:\car.jpg");
            int width = image.Width;
            int height = image.Height;
            var bmp = new Bitmap(image, width, height);
            double m = 0.97;
            Rectangle rect = new Rectangle(0, (int)(height * (1-m)), width, (int)(height));
            //First we define a rectangle with the help of already calculated points  
            Bitmap OriginalImage = new Bitmap(image, width, height);
            //Original image  
            Bitmap _img = new Bitmap(width, (int)(height * m));
            // for cropinf image  
            Graphics g = Graphics.FromImage(_img);
            // create graphics  
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //set image attributes  
            g.DrawImage(OriginalImage, 0, 0, rect, GraphicsUnit.Pixel);
            _img.Save("tst.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            RotateImage(_img, (float)0.5).Save("tstr.jpeg");

        }
        public static Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }
    }
}


using ImageMagick;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using static System.Net.WebRequestMethods;

namespace PhotoEditing
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //Image image = Image.FromFile(@"C:\car.jpg");


            //RotateImage(CropImage(image, (float)0.97), (float)0.5).Save("tstr.jpeg");
            CompressImage(new FileInfo("tstr.jpeg"));//.Save("compressed.jpeg");
        }

        public static Image CropImage(Image image, float percent)
        {
            int width = image.Width;
            int height = image.Height;
            var bmp = new Bitmap(image, width, height);
            double m = percent;
            Rectangle rect = new Rectangle(0, (int)(height * (1 - m)), width, (int)(height));
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
            return _img;
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
        public static void CompressImage(FileInfo sourceImage)
        {
            Console.WriteLine("Bytes before: " + sourceImage.Length);
            var optimizer = new ImageOptimizer();
            optimizer.Compress(sourceImage);

            sourceImage.Refresh();
            Console.WriteLine("Bytes after:  " + sourceImage.Length);
            //Save compressed image
            //return sourceImage;
        }
    }
}


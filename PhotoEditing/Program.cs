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
            Image image = Image.FromFile(@"C:\car.jpeg");
            Image cropped = CropImage(image, (float)0.97);
            Image rotated = RotateImage(cropped, (float)1);
            double proportion = (double)rotated.Width / (double)rotated.Height;
            int maxWidth = 800;
            Size newSizxe = new Size(maxWidth, (int)(maxWidth / proportion));
            Image resized = ResizeImage((Bitmap)rotated, newSizxe);
            //Image compressed = ResizeImage((Bitmap)rotated, new Size { Height = rotated.Height / 2, Width = rotated.Width / 2 });
            resized.Save("all.png");
            CompressImage(new FileInfo("all.png"));
            //RotateImage(CropImage(image, (float)0.97), (float)0.5).Save("tstr.jpeg");
            
            //ResizeImage((Bitmap)image, new Size { Height = image.Height/3, Width = image.Width/3 }).Save("resize.png");
            //CompressImage(new FileInfo("resize.png"));//.Save("compressed.jpeg");
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
        }
        private static Bitmap ResizeImage(Bitmap mg, Size newSize)
        {
            double ratio = 0d;
            double myThumbWidth = 0d;
            double myThumbHeight = 0d;
            int x = 0;
            int y = 0;

            Bitmap bp;

            if ((mg.Width / Convert.ToDouble(newSize.Width)) > (mg.Height /
            Convert.ToDouble(newSize.Height)))
                ratio = Convert.ToDouble(mg.Width) / Convert.ToDouble(newSize.Width);
            else
                ratio = Convert.ToDouble(mg.Height) / Convert.ToDouble(newSize.Height);
            myThumbHeight = Math.Ceiling(mg.Height / ratio);
            myThumbWidth = Math.Ceiling(mg.Width / ratio);

            //Size thumbSize = new Size((int)myThumbWidth, (int)myThumbHeight);
            Size thumbSize = new Size((int)newSize.Width, (int)newSize.Height);
            bp = new Bitmap(newSize.Width, newSize.Height);
            x = (newSize.Width - thumbSize.Width) / 2;
            y = (newSize.Height - thumbSize.Height);
            // Had to add System.Drawing class in front of Graphics ---
            System.Drawing.Graphics g = Graphics.FromImage(bp);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Rectangle rect = new Rectangle(x, y, thumbSize.Width, thumbSize.Height);
            g.DrawImage(mg, rect, 0, 0, mg.Width, mg.Height, GraphicsUnit.Pixel);

            return bp;

        }
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
    }
}


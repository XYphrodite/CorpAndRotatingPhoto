using ImageMagick;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace PhotoEditing
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string sDir = @"C:\Users\mielta\source\repos\CarDbHtmlParser\CarDbHtmlParser\bin\Debug\netcoreapp3.1\ph";
            var list = DirSearch(sDir);
            foreach (string s in list)
            {
                string imageName;
                Regex regex = new Regex(@"\\");
                var l = regex.Split(s);
                imageName = l[l.Length - 1];
                Image image = Image.FromFile(s);
                Image cropped = CropImage(image, (float)0.92);
                Image rotated = RotateImage(cropped, (float)0.5);
                rotated.Save(imageName,ImageFormat.Jpeg);
                //CompressImage(new FileInfo(imageName));
            }
        }
        static List<string> DirSearch(string sDir)
        {
            List<string> list = new List<string>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    list.Add(f);
                }

                foreach (string d in Directory.GetDirectories(sDir))
                {
                    list.AddRange(DirSearch(d));
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
            return list;
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
            //Console.WriteLine("Bytes before: " + sourceImage.Length);
            var optimizer = new ImageOptimizer();
            optimizer.Compress(sourceImage);
            sourceImage.Refresh();
            //Console.WriteLine("Bytes after:  " + sourceImage.Length);
        }
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
    }
}


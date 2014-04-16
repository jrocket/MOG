using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoG.Domain
{
    public class BitmapHelper
    {
        public static byte[] ImageToByte2(Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        public static Stream ResizeImage(Stream originalImage, int canvasWidth, int canvasHeight)
        {
            using (Image img = Image.FromStream(originalImage))
            {
                Image resizedImage = ResizeImage(img, canvasWidth, canvasHeight);


                System.Drawing.Imaging.ImageCodecInfo info = ImageCodecInfo.GetImageEncoders()
                .Where(codec => String.Compare(codec.FilenameExtension, "*.png", true) == 0)
                    .FirstOrDefault();

                //MemoryStream stream = new MemoryStream();

                MemoryStream stream = new MemoryStream();
                if (info != null)
                {
                    EncoderParameters encoderParameters;
                    encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality,
                                     100L);


                 resizedImage.Save(stream, info, encoderParameters);
                   
                }
                
                return stream;
            }
        }


        public static Image ResizeImage(Image originalImage, int canvasWidth, int canvasHeight)
        {
            int originalWidth = originalImage.Width;
            int originalHeight = originalImage.Height;
            System.Drawing.Image thumbnail =
                new Bitmap(canvasWidth, canvasHeight); // changed parm names
            System.Drawing.Graphics graphic =
                         System.Drawing.Graphics.FromImage(thumbnail);

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;



            // Figure out the ratio
            double ratioX = (double)canvasWidth / (double)originalWidth;
            double ratioY = (double)canvasHeight / (double)originalHeight;
            // use whichever multiplier is smaller
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            // now we can get the new height and width
            int newHeight = Convert.ToInt32(originalHeight * ratio);
            int newWidth = Convert.ToInt32(originalWidth * ratio);

            // Now calculate the X,Y position of the upper-left corner 
            // (one of these will always be zero)
            int posX = Convert.ToInt32((canvasWidth - (originalWidth * ratio)) / 2);
            int posY = Convert.ToInt32((canvasHeight - (originalHeight * ratio)) / 2);

            graphic.Clear(Color.White); // white padding
            graphic.DrawImage(originalImage, posX, posY, newWidth, newHeight);

            return thumbnail;
        }

        //public static bool ResizeImage(string filepath,
        //             int canvasWidth, int canvasHeight)
        //{
        //    bool result = false;
        //    string extension = System.IO.Path.GetExtension(filepath);
        //    Image image = Image.FromFile(filepath);


        //    var thumbnail = ResizeImage(image, canvasWidth, canvasHeight);


        //    System.Drawing.Imaging.ImageCodecInfo info = ImageCodecInfo.GetImageEncoders()
        //        .Where(codec => String.Compare(codec.FilenameExtension, "*" + extension, true) == 0)
        //            .FirstOrDefault();

        //    if (info != null)
        //    {
        //        EncoderParameters encoderParameters;
        //        encoderParameters = new EncoderParameters(1);
        //        encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality,
        //                         100L);

        //        string destinationFileName = String.Format("{0}_t{1}",
        //            System.IO.Path.GetFileNameWithoutExtension(filepath)
        //            , extension);
        //        string destinationPath = System.IO.Path.GetDirectoryName(filepath);

        //        string path = System.IO.Path.Combine(destinationPath, destinationFileName);

        //        thumbnail.Save(path, info, encoderParameters);
        //        result = true;
        //    }
        //    return result;

        //}
    }
}

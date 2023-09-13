using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class Helper
    {
        public static byte[] Compressimage(byte[] srcImgArray, int width = 0, int height = 0)
        {
            byte[] byteData = null;
            try
            {

               
                    // Convert stream to image
                    System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
                    using var image = (Image)converter.ConvertFrom(srcImgArray);
                   // using var image = Image.FromStream(memory);

                    float maxHeight = 900.0f;
                    float maxWidth = 900.0f;
                    int newWidth = width;
                    int newHeight = height;

                    var originalBMP = new Bitmap(image);
                    //int originalWidth = originalBMP.Width;
                    // int originalHeight = originalBMP.Height;

                    //if (originalWidth > maxWidth || originalHeight > maxHeight)
                    //{
                    //    // To preserve the aspect ratio  
                    //    float ratioX = (float)maxWidth / (float)originalWidth;
                    //    float ratioY = (float)maxHeight / (float)originalHeight;
                    //    float ratio = Math.Min(ratioX, ratioY);
                    //    newWidth = (int)(originalWidth * ratio);
                    //    newHeight = (int)(originalHeight * ratio);
                    //}

                    //else
                    //{
                    //    newWidth = (int)originalWidth;
                    //    newHeight = (int)originalHeight;
                    //}

                    var bitmap = new Bitmap(originalBMP, newWidth, newHeight);
                    var imgGraph = Graphics.FromImage(bitmap);

                    imgGraph.SmoothingMode = SmoothingMode.Default;
                    imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);

                    //  var extension = Path.GetExtension(targetPath).ToLower();
                    // for file extension having png and gif
                    //if (extension == ".png" || extension == ".gif")
                    //{
                    //    bitmap.Save(targetPath, image.RawFormat);
                    //}

                    // for file extension having .jpg or .jpeg
                    // else if (extension == ".jpg" || extension == ".jpeg")
                    // {
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    var encoderParameters = new EncoderParameters(1);
                    var parameter = new EncoderParameter(myEncoder, 50L);
                    encoderParameters.Param[0] = parameter;

                    // Save image to targetPath
                    // bitmap.Save(targetPath, jpgEncoder, encoderParameters);
                    // Bitmap bImage = newImage;  // Your Bitmap Image
                    System.IO.MemoryStream ms = new MemoryStream();
                    bitmap.Save(ms, jpgEncoder, encoderParameters);
                    byteData = ms.ToArray();

                    //}
                    bitmap.Dispose();
                    imgGraph.Dispose();
                    originalBMP.Dispose();
                    return byteData;
                



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}

using System;
using System.IO;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.Misc.Interface;
using PhotoSauce.MagicScaler;

namespace Blue_Eyes_White_Dragon.DataAccess.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly ILogger _logger;

        public ImageRepository(ILogger logger)
        {
            _logger = logger;
        }

        public void ConvertImage(DirectoryInfo destinationPath, FileInfo imageFile, string orgName, ProcessImageSettings settings)
        {
            var newImagePath = Path.Combine(destinationPath.FullName, orgName);
            var imageLocation = imageFile.FullName;
            try
            {
                if (!destinationPath.Exists)
                {
                    destinationPath.Create();
                }
                using (var outStream = new FileStream(newImagePath, FileMode.Create))
                {
                    MagicImageProcessor.ProcessImage(imageLocation, outStream, settings);
                }
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }





        //public Bitmap ConvertImageJpg()
        //{
        //    Bitmap myBitmap;
        //    ImageCodecInfo myImageCodecInfo;
        //    Encoder myEncoder;
        //    EncoderParameter myEncoderParameter;
        //    EncoderParameters myEncoderParameters;

        //    // Create a Bitmap object based on a BMP file.
        //    myBitmap = new Bitmap("Shapes.bmp");

        //    // Get an ImageCodecInfo object that represents the JPEG codec.
        //    myImageCodecInfo = GetEncoderInfo(Constants.JpgMimeType);

        //    // Create an Encoder object based on the GUID

        //    // for the Quality parameter category.
        //    myEncoder = Encoder.Quality;

        //    // Create an EncoderParameters object.

        //    // An EncoderParameters object has an array of EncoderParameter

        //    // objects. In this case, there is only one

        //    // EncoderParameter object in the array.
        //    myEncoderParameters = new EncoderParameters(1);

        //    // Save the bitmap as a JPEG file with quality level 25.
        //    myEncoderParameter = new EncoderParameter(myEncoder, 25L);
        //    myEncoderParameters.Param[0] = myEncoderParameter;
        //    myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters);

        //    // Save the bitmap as a JPEG file with quality level 50.
        //    myEncoderParameter = new EncoderParameter(myEncoder, 50L);
        //    myEncoderParameters.Param[0] = myEncoderParameter;
        //    myBitmap.Save("Shapes050.jpg", myImageCodecInfo, myEncoderParameters);

        //    // Save the bitmap as a JPEG file with quality level 75.
 
        //    myBitmap.Save("Shapes075.jpg", myImageCodecInfo, myEncoderParameters);

        //    return myBitmap;
        //}

        //private EncoderParameters GetQuality92(Encoder quality)
        //{
        //    return new EncoderParameters(1)
        //    {
        //        Param =
        //        {
        //            [0] = new EncoderParameter(quality, 92L) 
        //        }
        //    };
        //}

        //private static ImageCodecInfo GetEncoderInfo(string mimeType)
        //{
        //    int j;
        //    var encoders = ImageCodecInfo.GetImageEncoders();
        //    for (j = 0; j < encoders.Length; ++j)
        //    {
        //        if (encoders[j].MimeType == mimeType)
        //            return encoders[j];
        //    }
        //    return null;
        //}

        //public void SaveBitmap(Bitmap bitmap, string path, ImageCodecInfo imageCodec, EncoderParameters encoding)
        //{
        //    bitmap.Save(path, imageCodec, encoding);
        //}
    }
}

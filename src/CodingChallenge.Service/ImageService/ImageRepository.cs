using System.IO;

namespace CodingChallenge.Service.ImageService
{
    public class ImageRepository
    {
        private string BaseImagePath {get; set;}
        private string CacheImagePath { get; set; }
        private bool PreserveAspect { get; set; }
        private int ImageQuality { get; set; }

        public ImageRepository(string baseImagePath, string cacheImagePath, bool preserveAspect = true, int imageQuality = 100)
        {
            BaseImagePath = baseImagePath;
            CacheImagePath = cacheImagePath;
            PreserveAspect = preserveAspect;
            ImageQuality = imageQuality;
        }

        /// <summary>
        /// Retrieve an image, either from the cache or creating it from a base library image
        /// The image characteristics essentially form a composite key
        /// </summary>
        /// <param name="sourceImageName"></param>
        /// <param name="destinationHeightPx"></param>
        /// <param name="destinationWidthPx"></param>
        /// <param name="destinationFormat"></param>
        /// <param name="includeImageData">if true then we read the image file data into a byte array</param>
        /// <param name="destinationBackgroundColour"></param>
        /// <param name="watermark"></param>
        /// <returns>an ImageData object, if there is a failure then ImageData.DestinationFileExists should be set to false</returns>
        public ImageData RetrieveImage(string sourceImageName, int destinationHeightPx, int destinationWidthPx,
                                    ImageFormat.Format destinationFormat, bool includeImageData,
                                    string destinationBackgroundColour = null, string watermark = null)
        {
            ImageData image = new ImageData();


            string sourceImageFilepath = Path.Combine(BaseImagePath, sourceImageName);

            string cacheImageFilepath = ImageCache.MakeCacheFileName(CacheImagePath, sourceImageName,
                                                                    destinationHeightPx, destinationWidthPx, destinationFormat,
                                                                    PreserveAspect, ImageQuality, destinationBackgroundColour, watermark);

            if (!File.Exists(cacheImageFilepath))
            {
                // this is effectively a repository Create()
                // it is not in the cache so create it according to the supplied details
                ImageConverter ic = new ImageConverter();

                image = ic.Convert(sourceImageFilepath, cacheImageFilepath, destinationHeightPx, destinationWidthPx,
                                    destinationFormat, PreserveAspect, ImageQuality, destinationBackgroundColour, watermark);

                // if this was successful we should have a file created in the image cache and the details in 
                // the returned ImageData object.

                // if converstion was not successful then DestinationFileExists should be false;
            }
            else
            {
                // the file is in the cache!! read the destination details fron the file
                ImageDetails cacheImageDetails = ImageFile.GetImageFileDetails(cacheImageFilepath);
                ImageDetails baseImageDetails = ImageFile.GetImageFileDetails(sourceImageFilepath);

                // conversion was successful
                image.SourceFileExists = true;
                image.SourceImageFileName = sourceImageFilepath;
                image.Source = baseImageDetails;
                image.Destination = cacheImageDetails;
                image.DestinationFileExists = true;
                image.DestinationImageFileName = cacheImageFilepath;
                image.CacheHit = true;
            }

            if(includeImageData && File.Exists(cacheImageFilepath))
            {
                byte[] b = File.ReadAllBytes(cacheImageFilepath);
                image.Data = b;
            }

            return image;
        }
    }
}

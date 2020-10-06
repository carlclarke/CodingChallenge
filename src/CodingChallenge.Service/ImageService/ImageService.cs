using System.Collections.Generic;

namespace CodingChallenge.Service.ImageService
{
    public class ImageService : IImageService
    {
        public ImageData RetrieveImage(string sourceImageName, int destinationHeightPx, int destinationWidthPx,
                                    ImageFormat.Format destinationFormat, bool includeImageData, 
                                    string baseImagePath, string cacheImagePath,
                                    string destinationBackgroundColour = null, string watermark = null)
        {
            ImageRepository repo = new ImageRepository(baseImagePath, cacheImagePath);

            return repo.RetrieveImage(sourceImageName, destinationHeightPx, destinationWidthPx,
                                    destinationFormat, includeImageData,
                                    destinationBackgroundColour, watermark);
        }

        public ImageFormat.Format ParseImageType(string s)
        {
            return ImageFormat.Parse(s);
        }

        public List<ImageDetails> GetAllFiles(string path, int skip, int take, out int totalFiles)
        {
            return ImageFile.GetAllFiles(path, skip, take, out totalFiles);
        }

        public ImageDetails GetImageFileDetails(string pathAndName)
        {
            return ImageFile.GetImageFileDetails(pathAndName);
        }
    }
}

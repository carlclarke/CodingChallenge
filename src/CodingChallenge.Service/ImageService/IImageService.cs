using System.Collections.Generic;

namespace CodingChallenge.Service.ImageService
{
    public interface IImageService
    {
        ImageData RetrieveImage(string sourceImageName, int destinationHeightPx, int destinationWidthPx,
                                   ImageFormat.Format destinationFormat, bool includeImageData,
                                   string baseImagePath, string cacheImagePath,
                                   string destinationBackgroundColour = null, string watermark = null);
        ImageFormat.Format ParseImageType(string s);
        public List<ImageDetails> GetAllFiles(string path, int start, int take, out int totalFiles);
        public ImageDetails GetImageFileDetails(string pathAndName);
    }
}
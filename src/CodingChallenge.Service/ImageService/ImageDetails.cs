
namespace CodingChallenge.Service.ImageService
{
    /// <summary>
    /// The is a 'format agnostic' image file wrapper which should contain enough information for a consumer to transport the data.
    /// It does not provide in depth information about the images although sub classes could do this if required.
    /// </summary>
    public class ImageDetails
    {
        public string ImageName { get; set; }

        public int ImageHeightPx { get; set; }

        public int ImageWidthPx { get; set; }

        public string ImageFormat { get; set; } = "Undefined";
    }
}

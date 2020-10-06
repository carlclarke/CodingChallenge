
namespace CodingChallenge.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageDetailsModel
    {
        /// <summary>
        /// Full path to image
        /// </summary>
        public string ImageFilePath { get; set; }
        /// <summary>
        /// Image (file) name
        /// </summary>
        public string ImageName { get; set; }
        /// <summary>
        /// Image height in px
        /// </summary>
        public int ImageHeightPx { get; set; }
        /// <summary>
        /// Image width in px
        /// </summary>
        public int ImageWidthPx { get; set; }
        /// <summary>
        /// Image format
        /// </summary>
        public string ImageFormat { get; set; }
    }
}

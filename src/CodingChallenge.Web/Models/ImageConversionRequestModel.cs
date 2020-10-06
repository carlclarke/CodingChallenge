using System;
using System.ComponentModel.DataAnnotations;

namespace CodingChallenge.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageConversionRequestModel
    {
        /// <summary>
        /// Suported formats
        /// </summary>
        public enum Format
        { 
            /// <summary>
            /// Portable Network Graphics
            /// </summary>
            PNG,
            /// <summary>
            /// Joint Photographic Experts Group
            /// </summary>
            JPG
        }
        /// <summary>
        /// Source details
        /// </summary>
        public ImageDetailsModel SourceImage { get; set; } = new ImageDetailsModel();

        /// <summary>
        /// Requested image height in px
        /// </summary>
        [Required]
        [Range(1, 10000)]
        public int ImageHeightPx { get; set; }

        /// <summary>
        /// Requested image width in px
        /// </summary>
        [Required]
        [Range(1, 10000)]
        public int ImageWidthPx { get; set; }

        /// <summary>
        /// Requested image format
        /// </summary>
        [EnumDataType(typeof(Format))]
        public Format ImageFormat { get; set; }

        /// <summary>
        /// Requested watermark (optional)
        /// </summary>
        [StringLength(20)]
        public string Watermark { get; set; }
        
        /// <summary>
        /// Requested background colour (optional)
        /// </summary>
        [StringLength(8)]
        public string BackgroundColour { get; set; }

        /// <summary>
        /// Flag to show that request has been posted and is reponse data
        /// </summary>
        public bool ShowGETLink { get; set; }

    }
}

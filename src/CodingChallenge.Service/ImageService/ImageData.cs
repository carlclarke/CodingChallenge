using System;
using System.Collections.Generic;
using System.Text;

namespace CodingChallenge.Service.ImageService
{
    /// <summary>
    /// The is a format agnostic image file wrapper which should contain enough information for a consumer to transport the data.
    /// It does not provide in depth information about the images although sub classes could if required.
    /// </summary>
    public class ImageData
    {
        public string SourceImageFileName { get; set; }

        public bool SourceFileExists { get; set; }

        public ImageDetails Source { get; set; } = new ImageDetails();

        public string DestinationImageFileName { get; set; }

        public bool DestinationFileExists { get; set; }

        public ImageDetails Destination { get; set; } = new ImageDetails();

        public bool CacheHit { get; set; }

        /// <summary>
        /// This is the raw image byte data
        /// </summary>
        public byte[] Data { get; set; }
    }
}

using CodingChallenge.Service.ImageService;
using System.Collections.Generic;

namespace CodingChallenge.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageListModel
    {
        /// <summary>
        /// Start offset
        /// </summary>
        public int Skip { get; set; }
        /// <summary>
        /// Records to return
        /// </summary>
        public int Take { get; set; }
        /// <summary>
        /// UI/User friendly start record number
        /// </summary>
        public int Start { get => Skip + 1; }
        /// <summary>
        /// UI/User friendly end record number
        /// </summary>
        public int End { get => Skip + Take; }
        /// <summary>
        /// Total records available
        /// </summary>
        public int TotalAvailable { get; set; }
        /// <summary>
        /// List of ImageDetails objects for each image
        /// </summary>
        public List<ImageDetails> Items { get; set; } = new List<ImageDetails>();
    }
}

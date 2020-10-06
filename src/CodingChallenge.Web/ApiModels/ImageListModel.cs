using CodingChallenge.Service.ImageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingChallenge.Web.ApiModels
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
        /// Total records available
        /// </summary>
        public int TotalAvailable { get; set; }
        /// <summary>
        /// List of ImageDetails objects for each image
        /// </summary>
        public List<ImageDetails> Items { get; set; } = new List<ImageDetails>();
    }
}

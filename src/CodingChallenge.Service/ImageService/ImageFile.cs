using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace CodingChallenge.Service.ImageService
{
    class ImageFile
    {
        /// <summary>
        /// Get a sorted, paged list of files
        /// </summary>
        /// <remarks>
        /// this could get slow for may files, I am getting times of approx 4 s for 500 files on a magnetic (non-SSD) disk. can be improve using a database or a memory cached file list
        /// </remarks>
        /// <param name="path">the path to list (sun directories are not listed)</param>
        /// <param name="skip">the starting file index</param>
        /// <param name="take">how many to return</param>
        /// <returns></returns>
        public static List<ImageDetails> GetAllFiles(string path, int skip, int take, out int totalFiles)
        {
            List<ImageDetails> result = new List<ImageDetails>();

            totalFiles = 0;

            try
            {
                totalFiles = Directory.EnumerateFiles(path).Count();

                var files = Directory.EnumerateFiles(path).OrderBy(e => e).Skip(skip).Take(take).ToList();

                foreach (var item in files)
                {
                    string fullpath = Path.Combine(path, item);

                    ImageDetails img = GetImageFileDetails(fullpath);

                    result.Add(img);
                }
            }
            catch(Exception ex)
            {
                // log this exception (log4net etc)
                Debug.WriteLine(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Gets the basic image details for a single file
        /// </summary>
        /// <param name="pathAndName">the full path to the image file</param>
        /// <returns></returns>
        public static ImageDetails GetImageFileDetails(string pathAndName)
        {
            ImageDetails img = new ImageDetails();

            try
            {
                using var fs = File.OpenRead(pathAndName);
                using var skms = new SKManagedStream(fs);
                // using codec should be quicker as it can examine the image file header details
                // skiasharpe will select the appropriate codec for the file
                using var codec = SKCodec.Create(skms);
                img.ImageName = Path.GetFileName(pathAndName);
                img.ImageHeightPx = codec.Info.Height;
                img.ImageWidthPx = codec.Info.Width;

#pragma warning disable IDE0066 // Convert switch statement to expression
                switch (codec.EncodedFormat)
#pragma warning restore IDE0066 // Convert switch statement to expression
                {
                    case SKEncodedImageFormat.Png:
                        img.ImageFormat = ImageFormat.Format.PNG.ToString();
                        break;
                    case SKEncodedImageFormat.Jpeg:
                        img.ImageFormat = ImageFormat.Format.JPG.ToString();
                        break;
                    default:
                        img.ImageFormat = ImageFormat.Format.Undefined.ToString();
                        break;
                }
            }
            catch (Exception ex)
            {
                // log this exception (log4net etc)
                Debug.WriteLine(ex.Message);
            }

            return img;
        }
    }
}

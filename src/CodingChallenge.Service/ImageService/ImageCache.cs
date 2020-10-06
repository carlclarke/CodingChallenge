using System.IO;

namespace CodingChallenge.Service.ImageService
{
    public class ImageCache
    {

        /// <summary>
        /// Make a detination file name, this is effectively the primary key in our 'filing system database'
        /// </summary>
        /// <remarks>
        /// If a 'filing system database' was sufficient then we could also introduce 'buckets' to store cached files within
        /// for example we could simply split all files by format with a folder for 'PNG' and a folder for 'JPG', we could then subdivide
        /// those folders by some other characteristics such as background colour etc. This could provide improved access speed as the filing
        /// system would have smaller indexes to search and maintain. Additionally much slower and cheaper storage could be used for certain 
        /// characteristics such as very large dimension files, files with watermarks etc where the benefit of caching migh be less frequently used
        /// (although the cache vs. CPU should always be a plus benefit, the longer term storage cost may outweigh it).
        /// 
        /// Note
        /// ====
        /// Filenames may be limited in length see:
        /// https://docs.microsoft.com/en-us/windows/win32/fileio/naming-a-file?redirectedfrom=MSDN#maximum-path-length-limitation
        /// 
        /// 
        /// 
        /// </remarks>
        /// <param name="path"></param>
        /// <param name="sourceFilename"></param>
        /// <param name="destinationHeightPx"></param>
        /// <param name="destinationWidthPx"></param>
        /// <param name="destinationFormat"></param>
        /// <param name="preserveAspect"></param>
        /// <param name="destinationBackgroundColour"></param>
        /// <param name="watermark"></param>
        /// <returns></returns>
        public static string MakeCacheFileName(string path, string sourceFilename, int destinationHeightPx, int destinationWidthPx,
                                        ImageFormat.Format destinationFormat, bool preserveAspect, int imageQuality, 
                                        string destinationBackgroundColour = null, string watermark = null)
        {
            string result;

            string filenameWithoutExt = Path.GetFileNameWithoutExtension(sourceFilename);
            string fileExt = Path.GetExtension(sourceFilename);

            // we need to choose something to separate the original baseFilename from the text that we will add for the 
            // conversion parameters and that choice must not be present in the original baseFilename.
            // we can use unicode chars for NTFS but not easily for older FAT16/FAT32 file systems where 
            // there may or may not be OEM CP <-> Unicode translation
            string suffixSeparator = "🥃";
            string fs = "_";
            string safeWatermark = Encode(watermark);

            string cacheSuffix = 
                $"{destinationHeightPx}{fs}{destinationWidthPx}{fs}{destinationFormat}{fs}{preserveAspect}" +
                $"{fs}{imageQuality}{fs}{destinationBackgroundColour}{fs}{safeWatermark}";

            string filename = $"{filenameWithoutExt}{suffixSeparator}{cacheSuffix}{fileExt}";

            result = Path.Combine(path, filename);

            return result;
        }

        /// <summary>
        /// We can't have certain characters in filenames and the watermark may pose a problem,
        /// we could just remove them in a lossy fashion but Url encoding or Base32 encoding
        /// should solve the problem
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string Encode(string s)
        {
            return System.Net.WebUtility.UrlEncode(s);
        }


    }
}

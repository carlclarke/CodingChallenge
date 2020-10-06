using System;

namespace CodingChallenge.Service.ImageService
{
    public static class ImageFormat
    {
        /// <summary>
        /// The available image format type(s)
        /// </summary>
        public enum Format
        {
            Undefined,
            PNG,
            JPG
        }

        /// <summary>
        /// return the IANA media type / mime type for a particular image format type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToMimeType(this Format type)
        {
#pragma warning disable IDE0066 // Convert switch statement to expression
            switch (type)
#pragma warning restore IDE0066 // Convert switch statement to expression
            {
                case Format.PNG:
                    return "image/png";
                case Format.JPG:
                    return "image/jpeg";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Parse a string back into an enum. We could manually convert some 
        /// variations such as JPG & JPEG bot not for this challenge
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Format Parse(string s)
        {
            if(Enum.TryParse(typeof(Format), s, true, out object result))
            {
                return (Format) result;
            }

            // return default
            return Format.Undefined;
        }
    }
}

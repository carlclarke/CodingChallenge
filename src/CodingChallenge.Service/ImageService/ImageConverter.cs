using SkiaSharp;
using System;
using System.Diagnostics;
using System.IO;

namespace CodingChallenge.Service.ImageService
{
    public class ImageConverter
    {
        /// <summary>
        /// Read the source image, resize, add optional background colour, add optional watermark, save to cache 
        /// and return converted image data to caller
        /// </summary>
        /// <remarks>
        /// It is complex and could do with breaking apart for readability
        /// </remarks>
        /// <param name="sourceImageName"></param>
        /// <param name="destinationFilename"></param>
        /// <param name="destinationHeightPx"></param>
        /// <param name="destinationWidthPx"></param>
        /// <param name="destinationFormat"></param>
        /// <param name="preserveAspect"></param>
        /// <param name="imageQuality"></param>
        /// <param name="destinationBackgroundColour"></param>
        /// <param name="watermark"></param>
        /// <returns></returns>
        public ImageData Convert(string sourceImageName, string destinationFilename, int destinationHeightPx, int destinationWidthPx, 
                                    ImageFormat.Format destinationFormat, bool preserveAspect, int imageQuality, string destinationBackgroundColour = null, string watermark = null)
        {
            ImageData image = new ImageData();

            ImageDetails sourceDetails = ImageFile.GetImageFileDetails(sourceImageName);

            // source file not found
            if(sourceDetails.ImageFormat == ImageFormat.Format.Undefined.ToString())
            {
                image.SourceImageFileName = sourceImageName;
                image.SourceFileExists = false;

                return image;
            }

            int destHeight = destinationHeightPx;
            int destWidth = destinationWidthPx;

            SKEncodedImageFormat skiaOutputFormat = ImageFormatToSkiaFormat(destinationFormat);

            using (var fs = File.OpenRead(sourceImageName))
            {
                using var skms = new SKManagedStream(fs);
                using var source = SKBitmap.Decode(skms);
                if (preserveAspect)
                {
                    // override the request dimensions and establish the new size maintaining aspect
                    double scaleWidth = (double)destinationWidthPx / (double)source.Width;
                    double scaleHeight = (double)destinationHeightPx / (double)source.Height;
                    double scaleAspect = Math.Min(scaleWidth, scaleHeight);

                    destHeight = (int)((double)source.Height * scaleAspect);
                    destWidth = (int)((double)source.Width * scaleAspect);
                }

                var resized = source.Resize(new SKImageInfo(destWidth, destHeight), SKFilterQuality.High);

                if (resized != null)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(destinationBackgroundColour))
                        {
                            resized = AddBackgroundColour(resized, destinationBackgroundColour);
                        }

                        if (!string.IsNullOrEmpty(watermark))
                        {
                            DrawWatermark(resized, watermark);
                        }

                        using var img = SKImage.FromBitmap(resized);
                        using var dest = File.OpenWrite(destinationFilename);
                        using var encoded = img.Encode(skiaOutputFormat, imageQuality);
                        if (encoded != null)
                        {
                            try
                            {
                                encoded.SaveTo(dest);
                            }
                            catch (Exception ex)
                            {
                                // log
                                Debug.WriteLine(ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // log
                        Debug.WriteLine(ex.Message);
                    }
                    finally
                    {
                        resized.Dispose();
                    }
                }
            }

            if (File.Exists(destinationFilename))
            {
                // conversion was successful
                image.SourceFileExists = true;
                image.SourceImageFileName = sourceImageName;
                image.Source = sourceDetails;
                image.Destination.ImageFormat = destinationFormat.ToString();
                image.Destination.ImageHeightPx = destHeight;
                image.Destination.ImageWidthPx = destWidth;
                image.DestinationFileExists = true;
                image.DestinationImageFileName = destinationFilename;
            }
            else
            {
                // conversion failed
                image.SourceFileExists = true;
                image.SourceImageFileName = sourceImageName;
                image.Source = sourceDetails;
                image.DestinationFileExists = false;
            }

            return image;
        }


        /// <summary>
        /// Add the backgound colour
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="hexColour"></param>
        /// <returns></returns>
        private static SKBitmap AddBackgroundColour(SKBitmap bitmap, string hexColour)
        {
            var result = new SKBitmap(bitmap.Info);

            var canvas = new SKCanvas(result);

            canvas.Clear();

            if (hexColour.Contains("x"))
            {
                hexColour = hexColour.Substring(hexColour.IndexOf('x') + 1);
            }

            var fill = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColor.Parse(hexColour)
            };

            canvas.DrawRect(0, 0, bitmap.Width, bitmap.Height, fill);
            canvas.DrawBitmap(bitmap, 0, 0);

            canvas.Flush();

            return result;
        }

        /// <summary>
        /// Add the watermark
        /// </summary>
        /// <remarks>
        /// Used image height / 12 for text height but we could look at how the text scales in height 
        /// and textwidth to allow any text to fit.
        /// 
        /// Used a hard coded 45 degrees for watermark text rotation but we could work out the angle 
        /// from bottom left to top right easily if required.
        /// 
        /// Watermark colour and transparency could be parameters or obtained from configuration if 
        /// global values are acceptable
        /// </remarks>
        /// <param name="bitmap"></param>
        /// <param name="watermark"></param>
        private static void DrawWatermark(SKBitmap bitmap, string watermark)
        {
            float textHeight = bitmap.Height / 12.0f;

            var canvas = new SKCanvas(bitmap);
            var font = SKTypeface.FromFamilyName("Arial");
            var brush = new SKPaint
            {
                Typeface = font,
                TextSize = textHeight,
                IsAntialias = true,
                Color = SKColors.Gray.WithAlpha(128)
            };

            int textWidth = (int)brush.MeasureText(watermark);

            canvas.Save();
            canvas.RotateDegrees(-45, bitmap.Width / 2, bitmap.Height / 2);
            canvas.DrawText(watermark, (bitmap.Width - textWidth) / 2, bitmap.Height / 2, brush);
            canvas.Restore();
            canvas.Flush();
        }

        /// <summary>
        /// Translate from the ImageFormat.Format enum to the Skia encoded image format enum
        /// </summary>
        /// <param name="fmt"></param>
        /// <returns></returns>
        private static SKEncodedImageFormat ImageFormatToSkiaFormat(ImageFormat.Format fmt)
        {
            switch (fmt)
            {
                case ImageFormat.Format.JPG:
                    return SKEncodedImageFormat.Jpeg;
                case ImageFormat.Format.PNG:
                    return SKEncodedImageFormat.Png;
                case ImageFormat.Format.Undefined:
                    break;
            }
            return SKEncodedImageFormat.Jpeg;
        }
    }
}

using CodingChallenge.Service.ImageService;
using NUnit.Framework;
using System.IO;

namespace CodingChallenge.Tests
{
    [TestFixture]
    public class ImageService_RetrieveImage_Tests
    {
        private IImageService _service;
        private readonly string badBaseImagePath = "";
        private readonly string goodBaseImagePath = @"E:\product_images";
        private readonly string goodCacheImagePath = @"E:\cached_product_images";
        private readonly string aGoodImageFilename = "01_04_2019_001103.png";
        private readonly string aBadImageFilename = "";

        [SetUp]
        public void Setup()
        {
            _service = new ImageService();
        }

        [Test]
        public void RetrieveImage_ShouldNotThrowException_WithBadPath()
        {
            Assert.DoesNotThrow(() =>
            {
               var result = _service.RetrieveImage(aBadImageFilename, 200, 200, ImageFormat.Format.PNG, true, badBaseImagePath, goodCacheImagePath);
            });
        }

        [Test]
        public void RetrieveImage_ShouldReturnFileNotFound_WithBadPath()
        {
            var result = _service.RetrieveImage(aBadImageFilename, 200, 200, ImageFormat.Format.PNG, true, badBaseImagePath, goodCacheImagePath);

            Assert.IsTrue(result.SourceFileExists == false);
        }

        [Test]
        public void RetrieveImage_ShouldReturnImageDetails_WithGoodPath()
        {
            var result = _service.RetrieveImage(aGoodImageFilename, 200, 200, ImageFormat.Format.PNG, true, goodBaseImagePath, goodCacheImagePath);

            Assert.IsTrue(result.DestinationFileExists == true);
            Assert.IsTrue(File.Exists(result.DestinationImageFileName));
            Assert.IsTrue(result.Destination.ImageFormat == ImageFormat.Format.PNG.ToString());
        }

        [Test]
        public void RetrieveImage_ShouldReturnCorrectSizeImage_WithGoodPath()
        {
            var result = _service.RetrieveImage(aGoodImageFilename, 200, 200, ImageFormat.Format.PNG, true, goodBaseImagePath, goodCacheImagePath);

            // one dimension must be correct because of maintaining the aspect ratio
            Assert.IsTrue(result.Destination.ImageHeightPx == 200 || result.Destination.ImageWidthPx == 200);
        }

        [Test]
        public void RetrieveImage_ShouldReturnCachedImage_WithGoodPath()
        {
            var temp = _service.RetrieveImage(aGoodImageFilename, 200, 200, ImageFormat.Format.PNG, true, goodBaseImagePath, goodCacheImagePath);

            // clear the cache
            File.Delete(temp.DestinationImageFileName);

            var nonCachedResult = _service.RetrieveImage(aGoodImageFilename, 200, 200, ImageFormat.Format.PNG, true, goodBaseImagePath, goodCacheImagePath);
            
            Assert.IsTrue(nonCachedResult.CacheHit == false);

            var cachedResult = _service.RetrieveImage(aGoodImageFilename, 200, 200, ImageFormat.Format.PNG, true, goodBaseImagePath, goodCacheImagePath);

            // at least the second file should be a cache hit
            Assert.IsTrue(cachedResult.CacheHit == true);
        }
    }
}
using CodingChallenge.Service.ImageService;
using NUnit.Framework;
using System.IO;

namespace CodingChallenge.Tests
{
    [TestFixture]
    public class ImageService_GetImageFileDetails_Tests
    {
        private IImageService _service;
        private readonly string badBaseImagePath = "";
        private readonly string goodBaseImagePath = @"E:\product_images";
        private readonly string anImageFilename = "01_04_2019_001103.png";

        [SetUp]
        public void Setup()
        {
            _service = new ImageService();
        }

        [Test]
        public void GetImageFileDetails_ShouldNotThrowException_WithBadPath()
        {
            Assert.DoesNotThrow(() =>
            {
               var result = _service.GetImageFileDetails(badBaseImagePath);
            });
        }

        [Test]
        public void GetImageFileDetails_ShouldReturnFormatUndefined_WithBadPath()
        {
            var result = _service.GetImageFileDetails(badBaseImagePath);

            Assert.IsTrue(result.ImageFormat == ImageFormat.Format.Undefined.ToString());
        }

        [Test]
        public void GetImageFileDetails_ShouldReturnNullOrEmptyName_WithBadPath()
        {
            var result = _service.GetImageFileDetails(badBaseImagePath);

            Assert.IsTrue(string.IsNullOrEmpty(result.ImageName));
        }

        [Test]
        public void GetImageFileDetails_ShouldCorrectDetails_WithGoodPathAndName()
        {
            string path = Path.Combine(goodBaseImagePath, anImageFilename);

            var result = _service.GetImageFileDetails(path);

            Assert.IsTrue(result.ImageName == anImageFilename);
            Assert.IsTrue(result.ImageFormat == ImageFormat.Format.PNG.ToString());
            Assert.IsTrue(result.ImageHeightPx == 1024);
            Assert.IsTrue(result.ImageWidthPx == 788);
        }

    }
}
using CodingChallenge.Service.ImageService;
using NUnit.Framework;

namespace CodingChallenge.Tests
{
    [TestFixture]
    public class ImageService_ParseImageType_Tests
    {
        private IImageService _service;

        [SetUp]
        public void Setup()
        {
            _service = new ImageService();
        }

        [Test]
        public void ParseImageType_JPG_ShouldParse()
        {
            var result = _service.ParseImageType("JPG");

            Assert.IsTrue(result == ImageFormat.Format.JPG);
        }

        [Test]
        public void ParseImageType_Jpg_ShouldParse()
        {
            var result = _service.ParseImageType("Jpg");

            Assert.IsTrue(result == ImageFormat.Format.JPG);
        }

        [Test]
        public void ParseImageType_JPEG_ShouldNotParse()
        {
            var result = _service.ParseImageType("JPEG");

            Assert.IsTrue(result == ImageFormat.Format.Undefined);
        }

        [Test]
        public void ParseImageType_PNG_ShouldParse()
        {
            var result = _service.ParseImageType("PNG");

            Assert.IsTrue(result == ImageFormat.Format.PNG);
        }

        [Test]
        public void ParseImageType_Png_ShouldParse()
        {
            var result = _service.ParseImageType("Png");

            Assert.IsTrue(result == ImageFormat.Format.PNG);
        }



    }
}
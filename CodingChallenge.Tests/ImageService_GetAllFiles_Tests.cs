using CodingChallenge.Service.ImageService;
using NUnit.Framework;

namespace CodingChallenge.Tests
{
    [TestFixture]
    public class ImageService_GetAllFiles_Tests
    {
        private IImageService _service;
        private readonly string badBaseImagePath = "";
        private readonly string goodBaseImagePath = @"E:\product_images";

        [SetUp]
        public void Setup()
        {
            _service = new ImageService();
        }

        [Test]
        public void GetAllFiles_ShouldNotThrowException_WithBadPath()
        {
            Assert.DoesNotThrow(() =>
            {
               var result = _service.GetAllFiles(badBaseImagePath, 0, 10, out int totalFiles);
            });
        }

        [Test]
        public void GetAllFiles_ReturnsEmptyList_WithBadPath()
        {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var result = _service.GetAllFiles(badBaseImagePath, 0, 10, out int totalFiles);
#pragma warning restore IDE0059 // Unnecessary assignment of a value

            Assert.IsTrue(result.Count == 0);
        }

        [Test]
        public void GetAllFiles_ReturnsZeroTotalFiles_WithBadPath()
        {
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            var result = _service.GetAllFiles(badBaseImagePath, 0, 10, out int totalFiles);
#pragma warning restore IDE0059 // Unnecessary assignment of a value

            Assert.IsTrue(totalFiles == 0);
        }

        [Test]
        public void GetAllFiles_ShouldNotThrowException_WithGoodPath()
        {
            Assert.DoesNotThrow(() =>
            {
                var result = _service.GetAllFiles(goodBaseImagePath, 0, 10, out int totalFiles);
            });
        }

        /// <summary>
        /// This does assume that the folder has files!!
        /// </summary>
        [Test]
        public void GetAllFiles_ShouldReturnFileDetails_WithGoodPath()
        {
            var result = _service.GetAllFiles(goodBaseImagePath, 0, 10, out int totalFiles);

            Assert.IsTrue(result.Count > 0);
            Assert.IsTrue(result.Count <= 10);
            Assert.IsTrue(totalFiles > 0);
        }

        /// <summary>
        /// This does assume that the folder has 500 files!!
        /// </summary>
        [Test]
        public void GetAllFiles_ShouldReturnCorrectTotalFiles_WithGoodPath()
        {
            var result = _service.GetAllFiles(goodBaseImagePath, 0, 10, out int totalFiles);

            Assert.IsTrue(result.Count == 10);
            Assert.IsTrue(totalFiles == 500);
        }

    }
}
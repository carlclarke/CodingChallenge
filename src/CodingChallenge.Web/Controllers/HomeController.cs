using System.IO;
using CodingChallenge.Service.ImageService;
using CodingChallenge.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CodingChallenge.Web.Controllers
{
    /// <summary>
    /// Controller for home resource
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// ctor with DI
        /// </summary>
        /// <param name="imageService"></param>
        /// <param name="configuration"></param>
        public HomeController(IImageService imageService, IConfiguration configuration)
        {
            _imageService = imageService;
            _configuration = configuration;
        }

        /// <summary>
        /// GET / or /home or /home/index.htm etc
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [Route("home")]
        [Route("home/index")]
        [Route("home/index/{id?}")]
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET /list
        /// </summary>
        /// <returns></returns>
        [Route("list")]
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult List([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            string basePath = _configuration.GetValue<string>("BaseImagePath");

            ImageListModel model = new ImageListModel
            {
                Skip = skip,
                Take = take,
                Items = _imageService.GetAllFiles(basePath, skip, take, out int totalFiles),
                TotalAvailable = totalFiles
            };

            return View(model);
        }

        /// <summary>
        /// GET /imagedetails
        /// </summary>
        /// <returns></returns>
        [Route("image-details/{baseImageName}")]
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult Details([FromRoute] string baseImageName)
        {
            string basePath = _configuration.GetValue<string>("BaseImagePath");
            string pathAndName = Path.Combine(basePath, baseImageName);

            var details = _imageService.GetImageFileDetails(pathAndName);

            ImageDetailsModel model = new ImageDetailsModel
            {
                ImageFilePath = pathAndName,
                ImageName = details.ImageName,
                ImageHeightPx = details.ImageHeightPx,
                ImageWidthPx = details.ImageWidthPx,
                ImageFormat = details.ImageFormat
            };

            return View(model);
        }

        /// <summary>
        /// GET /image-conversion
        /// </summary>
        /// <returns></returns>
        [Route("image-conversion/{baseImageName}")]
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult ImageConversion([FromRoute] string baseImageName)
        {
            string basePath = _configuration.GetValue<string>("BaseImagePath");
            string pathAndName = Path.Combine(basePath, baseImageName);

            var details = _imageService.GetImageFileDetails(pathAndName);

            ImageConversionRequestModel model = new ImageConversionRequestModel();
            model.SourceImage.ImageFilePath = pathAndName;
            model.SourceImage.ImageName = details.ImageName;
            model.SourceImage.ImageHeightPx = details.ImageHeightPx;
            model.SourceImage.ImageWidthPx = details.ImageWidthPx;
            model.SourceImage.ImageFormat = details.ImageFormat;

            // add some defaults for the UI
            model.ImageFormat = ImageConversionRequestModel.Format.JPG;
            model.ImageHeightPx = details.ImageHeightPx;
            model.ImageWidthPx = details.ImageWidthPx;

            return View(model);
        }

        /// <summary>
        /// POST /image-conversion
        /// </summary>
        /// <returns></returns>
        [Route("image-conversion/{inputModel}")]
        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult ImageConversion(ImageConversionRequestModel model)
        {
            model.ShowGETLink = true;

            return View(model);
        }

        /// <summary>
        /// GET /get-image 
        /// </summary>
        /// <returns>converted image data</returns>
        [Route("get-image/{baseImageName}/{heightPx}/{widthPx}/{format}")]
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult GetImage([FromRoute] string baseImageName, [FromRoute] int heightPx, [FromRoute] int widthPx, [FromRoute] string format, [FromQuery] string backgroundColour = null, [FromQuery] string watermark = null)
        {
            ImageFormat.Format fmt = _imageService.ParseImageType(format);

            // simple validation, would use model validation for more robust scenarios
            if (fmt == ImageFormat.Format.Undefined)
            {
                return BadRequest("Bad image format");
            }

            string basePath = _configuration.GetValue<string>("BaseImagePath");
            string cachePath = _configuration.GetValue<string>("CacheImagePath");

            ImageData image = _imageService.RetrieveImage(baseImageName, heightPx, widthPx, fmt, true,
                                                          basePath, cachePath, backgroundColour, watermark);

            if (!image.SourceFileExists)
            {
                return NotFound("Image resource not found");
            }

            return File(image.Data, _imageService.ParseImageType(image.Destination.ImageFormat).ToMimeType());
        }
    }
}

using CodingChallenge.Service.ImageService;
using CodingChallenge.Web.ApiModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CodingChallenge.ApiControllers
{
    /// <summary>
    /// Images reource controller
    /// </summary>
    [ApiController]
    [Route("api/v1/images")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// ctor with dependency injection
        /// </summary>
        /// 
        /// <param name="imageService"></param>
        /// <param name="configuration"></param>
        public ImagesController(IImageService imageService, IConfiguration configuration)
        {
            _imageService = imageService;
            _configuration = configuration;
        }
                
        /// <summary>
        /// Get a single image as specified by the request parameters
        /// </summary>
        /// <param name="baseImageName">the high resolution source image</param>
        /// <param name="heightPx">the desired height in pixels</param>
        /// <param name="widthPx">the desired width in pixels</param>
        /// <param name="format">the returned image format e.g. jpg/png/</param>
        /// <param name="backgroundColour">optional background colour for image specified as a hex RGB colour with 16 bits 
        /// for each RGB component e.g black is 0xFFFFFF</param>
        /// <param name="watermark">optional watermark string, the text will be centered in the image at 18pt size for simplicity</param>
        /// <returns>an image with details as requested</returns>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{baseImageName}/{heightPx}/{widthPx}/{format}")]
        public IActionResult Get([FromRoute]string baseImageName, [FromRoute] int heightPx, [FromRoute] int widthPx, [FromRoute] string format, [FromQuery]string backgroundColour, [FromQuery] string watermark)
        {
            ImageFormat.Format fmt = _imageService.ParseImageType(format);

            // simple validation, would use model validation for more robust scenarios
            if(fmt == ImageFormat.Format.Undefined)
            {
                return BadRequest("Bad image format");
            }

            string basePath = _configuration.GetValue<string>("BaseImagePath");
            string cachePath = _configuration.GetValue<string>("CacheImagePath");

            ImageData image = _imageService.RetrieveImage(baseImageName, heightPx, widthPx, fmt, true, 
                                                          basePath, cachePath, backgroundColour, watermark);

            if(!image.SourceFileExists)
            {
                return NotFound("Image resource not found");
            }

            return File(image.Data, _imageService.ParseImageType(image.Destination.ImageFormat).ToMimeType());
        }

        /// <summary>
        /// Return a list of images available from the system
        /// </summary>
        /// <param name="skip">the start offset (for paging)</param>
        /// <param name="take">the number of records to return</param>
        /// <returns></returns>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("list")]
        public IActionResult List([FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            string basePath = _configuration.GetValue<string>("BaseImagePath");

            ImageListModel model = new ImageListModel
            {
                Skip = skip,
                Take = take,
                Items = _imageService.GetAllFiles(basePath, skip, take, out int totalFiles),
                TotalAvailable = totalFiles
            };

            return new OkObjectResult(model);
        }
    }
}

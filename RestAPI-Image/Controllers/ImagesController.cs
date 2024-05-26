using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ImageManipulationAPI.Models;
using ImageManipulationAPI.Exceptions;
using RestAPI_Image.Services.Interfaces;

namespace ImageManipulationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ILogger<ImagesController> _logger;
        private readonly IImagesServices _imagesServices;
        public ImagesController(IImagesServices imagesServices, ILogger<ImagesController> logger)
        {
            _imagesServices = imagesServices;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<string> Post([FromBody] ImageRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.ImageBase64))
            {
                return BadRequest("Invalid request data.");
            }

            string? result = _imagesServices.ApplyWatermarkImageOrText(request);
            return Ok(result);
        }

       
    }
}

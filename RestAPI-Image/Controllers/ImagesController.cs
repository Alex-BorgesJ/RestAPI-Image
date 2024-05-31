using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ImageManipulationAPI.Models;
using ImageManipulationAPI.Exceptions;
using RestAPI_Image.Services.Interfaces;
using RestAPI_Image.DTOs;

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
        [Route("upload")]
        public async Task<IActionResult> Post(ImageDTO dto)
        {
            try
            {
                if (dto.fileImage == null || dto.fileImage.Length == 0)
                {
                    return BadRequest("Arquivo não enviado.");
                }

                byte[]? result = await _imagesServices.ApplyWatermarkImageOrText(dto);
                return File(result, "image/png");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro ao processar o arquivo: {ex.Message}");
            }

        }


    }
}

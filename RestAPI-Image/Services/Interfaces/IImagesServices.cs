using ImageManipulationAPI.Models;
using Microsoft.AspNetCore.Mvc;
using RestAPI_Image.DTOs;
using System.Drawing;

namespace RestAPI_Image.Services.Interfaces
{
    public interface IImagesServices
    {
        Task<byte[]?> ApplyWatermarkImageOrText(ImageDTO dto);
    }
}
using ImageManipulationAPI.Models;

namespace RestAPI_Image.DTOs
{
    public class ImageDTO
    {
        public IFormFile fileImage { get; set; }
        public IFormFile? fileWatermark { get; set; }
        public ImageRequest request { get; set; }
    }
}

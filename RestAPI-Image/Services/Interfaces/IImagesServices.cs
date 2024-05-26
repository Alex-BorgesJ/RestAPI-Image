using ImageManipulationAPI.Models;

namespace RestAPI_Image.Services.Interfaces
{
    public interface IImagesServices
    {
        string? ApplyWatermarkImageOrText(ImageRequest request);
    }
}

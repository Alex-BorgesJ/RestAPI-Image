using RestAPI_Image.Services.Interfaces;
using System.Drawing.Imaging;
using System.Drawing;
using ImageManipulationAPI.Models;

namespace RestAPI_Image.Services.Images
{
    public class ImagesServices : IImagesServices
    {
        public ImagesServices()
        {

        }

        public string? ApplyWatermarkImageOrText(ImageRequest request)
        {
            try
            {
                // Decodificar a imagem principal
                byte[] imageBytes = Convert.FromBase64String(request.ImageBase64);
                using var ms = new MemoryStream(imageBytes);
                var image = Image.FromStream(ms);

                // Aplicar a marca d'água
                using var graphics = Graphics.FromImage(image);

                if (!string.IsNullOrWhiteSpace(request.WatermarkText))
                {
                    ApplyTextWatermark(graphics, image, request.WatermarkText, request.XOffset, request.YOffset, request.WatermarkRotation);
                }
                else if (!string.IsNullOrWhiteSpace(request.WatermarkImageBase64))
                {
                    //decodificando WaterMarkImage 
                    byte[] wmBytes = Convert.FromBase64String(request.WatermarkImageBase64);
                    using var wmMs = new MemoryStream(wmBytes);
                    var wmImage = Image.FromStream(wmMs);
                    if (wmImage.Width <= image.Width && wmImage.Height <= image.Height)
                    {
                        ApplyImageWatermark(graphics, image, request.WatermarkImageBase64, request.XOffset, request.YOffset, request.WatermarkRotation);
                    }
                    else
                    {
                        return "A watermark image has dimensions incompatible with the main image.";
                    }
                }
                else
                {
                   return "Watermark text or watermark image must be provided.";
                }

                // Converter a imagem de volta para base64
                using var msWatermarked = new MemoryStream();
                image.Save(msWatermarked, ImageFormat.Png);
                var imageBytesWatermarked = msWatermarked.ToArray();
                var imageBase64Watermarked = Convert.ToBase64String(imageBytesWatermarked);

                return imageBase64Watermarked;
            }
            catch (ArgumentException ex)
            {
                return ex.Message;
            }
        }


        public void ApplyTextWatermark(Graphics graphics, Image image, string watermarkText, int xOffset, int yOffset, int rotation)
        {
            var font = new Font("Arial", 20);
            var color = Color.White;
            var brush = new SolidBrush(color);

            // Salvar o estado do gráfico
            var state = graphics.Save();

            // Aplicar rotação
            graphics.TranslateTransform(image.Width - xOffset, image.Height - yOffset);
            graphics.RotateTransform(rotation);
            graphics.DrawString(watermarkText, font, brush, 0, 0);

            // Restaurar o estado do gráfico
            graphics.Restore(state);
        }

        public void ApplyImageWatermark(Graphics graphics, Image image, string watermarkImageBase64, int xOffset, int yOffset, int rotation)
        {
            byte[] watermarkImageBytes = Convert.FromBase64String(watermarkImageBase64);
            using var ms = new MemoryStream(watermarkImageBytes);
            var watermarkImage = Image.FromStream(ms);

            // Salvar o estado do gráfico
            var state = graphics.Save();

            // Aplicar rotação
            graphics.TranslateTransform(image.Width - xOffset, image.Height - yOffset);
            graphics.RotateTransform(rotation);
            graphics.DrawImage(watermarkImage, new Point(0, 0));

            // Restaurar o estado do gráfico
            graphics.Restore(state);

        }
    }
}

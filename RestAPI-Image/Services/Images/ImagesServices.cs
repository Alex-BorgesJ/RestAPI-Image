using RestAPI_Image.Services.Interfaces;
using System.Drawing.Imaging;
using System.Drawing;
using ImageManipulationAPI.Models;
using RestAPI_Image.DTOs;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace RestAPI_Image.Services.Images
{
    public class ImagesServices : IImagesServices
    {
        public ImagesServices()
        {

        }

        public async Task<byte[]?> ApplyWatermarkImageOrText(ImageDTO dto)
        {
            try
            {
                string base64String;
                using (var memoryStream = new MemoryStream())
                {
                    await dto.fileImage.CopyToAsync(memoryStream);
                    byte[] bytes = memoryStream.ToArray();
                    base64String = Convert.ToBase64String(bytes);
                }

                byte[] imageBytes = Convert.FromBase64String(base64String);
                using var ms = new MemoryStream(imageBytes);
                var image = Image.FromStream(ms);


                //Aplicar a marca d'água
                using var graphics = Graphics.FromImage(image);

                if (!string.IsNullOrWhiteSpace(dto.request.WatermarkText))
                {
                    ApplyTextWatermark(graphics, image, dto.request.WatermarkText, dto.request.XOffset, dto.request.YOffset, dto.request.WatermarkRotation);
                }
                else if (dto.fileWatermark != null || dto.fileWatermark.Length != 0)
                {
                    string base64Wmark;
                    using (var memoryStream = new MemoryStream())
                    {
                        await dto.fileWatermark.CopyToAsync(memoryStream);
                        byte[] bytes = memoryStream.ToArray();
                        base64Wmark = Convert.ToBase64String(bytes);
                    }

                    //decodificando WaterMarkImage 
                    byte[] wmBytes = Convert.FromBase64String(base64Wmark);
                    using var wmMs = new MemoryStream(wmBytes);
                    var wmImage = Image.FromStream(wmMs);
                    if (wmImage.Width <= image.Width && wmImage.Height <= image.Height)
                    {
                        ApplyImageWatermark(graphics, image, base64Wmark, dto.request.XOffset, dto.request.YOffset, dto.request.WatermarkRotation);
                    }
                    else
                    {
                        throw new ArgumentException("A watermark image has dimensions incompatible with the main image.");
                    }
                }
                else
                {
                    throw new ArgumentException("Watermark text or watermark image must be provided.");
                }

                // Seus códigos para obter os bytes da imagem com marca d'água
                using var msWatermarked = new MemoryStream();
                image.Save(msWatermarked, ImageFormat.Png);
                var imageBytesWatermarked = msWatermarked.ToArray();
                return imageBytesWatermarked;

            }
            catch (ArgumentException ex)
            {
                throw;
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
using System;
using ImageManipulationAPI.Exceptions;

namespace ImageManipulationAPI.Models
{
    public class ImageRequest
    {
        private string _imageBase64;
        private string _watermarkText = string.Empty;
        private string _watermarkImageBase64 = string.Empty;
        private int _xOffset;
        private int _yOffset;
        private int _watermarkRotation;

        public string ImageBase64
        {
            get => _imageBase64;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ImageRequestValidationException("ImageBase64 cannot be null or empty.");
                }
                _imageBase64 = value;
            }
        }

        public string WatermarkText
        {
            get => _watermarkText;
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && value.Length > 100)
                {
                    throw new ImageRequestValidationException("WatermarkText cannot exceed 100 characters.");
                }
                _watermarkText = value;
            }
        }

        public string WatermarkImageBase64
        {
            get => _watermarkImageBase64;
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && !IsBase64String(value))
                {
                    throw new ImageRequestValidationException("Invalid base64 string for WatermarkImageBase64.");
                }
                _watermarkImageBase64 = value;
            }
        }

        public int XOffset
        {
            get => _xOffset;
            set
            {
                if (value < 0)
                {
                    throw new ImageRequestValidationException("XOffset cannot be negative.");
                }
                _xOffset = value;
            }
        }

        public int YOffset
        {
            get => _yOffset;
            set
            {
                if (value < 0)
                {
                    throw new ImageRequestValidationException("YOffset cannot be negative.");
                }
                _yOffset = value;
            }
        }

        public int WatermarkRotation
        {
            get => _watermarkRotation;
            set
            {
                if (value < 0 || value > 360)
                {
                    throw new ImageRequestValidationException("WatermarkRotation must be between 0 and 360 degrees.");
                }
                _watermarkRotation = value;
            }
        }

        private bool IsBase64String(string value)
        {
            Span<byte> buffer = new Span<byte>(new byte[value.Length]);
            return Convert.TryFromBase64String(value, buffer, out _);
        }
    }
}

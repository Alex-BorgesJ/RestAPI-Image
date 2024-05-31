using System;
using ImageManipulationAPI.Exceptions;

namespace ImageManipulationAPI.Models
{
    public class ImageRequest
    {
        private string _watermarkText = string.Empty;
        private int _xOffset;
        private int _yOffset;
        private int _watermarkRotation;

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
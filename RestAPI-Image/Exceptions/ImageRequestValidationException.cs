using System;

namespace ImageManipulationAPI.Exceptions
{
    public class ImageRequestValidationException : Exception
    {
        public ImageRequestValidationException(string message) : base(message)
        {
        }
    }
}

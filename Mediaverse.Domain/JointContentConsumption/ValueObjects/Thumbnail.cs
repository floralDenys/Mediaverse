using System;

namespace Mediaverse.Domain.JointContentConsumption.ValueObjects
{
    public class Thumbnail
    {
        public int Width { get; }
        public int Height { get; }
        public string Url { get; }

        public Thumbnail(int width, int height, string url)
        {
            try
            {
                if (width <= 0)
                {
                    throw new ArgumentException("Width must be more than 0");
                }

                if (height <= 0)
                {
                    throw new ArgumentException("Height must be more than 0");
                }

                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentException("Thumbnail requires an url to an image");
                }

                Width = width;
                Height = height;
                Url = url;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create thumbnail", exception);
            }
        }
    }
}
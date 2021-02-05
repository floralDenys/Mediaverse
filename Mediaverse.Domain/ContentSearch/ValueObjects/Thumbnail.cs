using System;

namespace Mediaverse.Domain.ContentSearch.ValueObjects
{
    public class Thumbnail
    {
        public long Width { get; }
        public long Height { get; }
        public string Url { get; }

        public Thumbnail(long width, long height, string url)
        {
            try
            {
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
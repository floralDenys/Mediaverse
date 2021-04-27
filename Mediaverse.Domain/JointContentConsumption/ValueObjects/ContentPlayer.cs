using System;

namespace Mediaverse.Domain.JointContentConsumption.ValueObjects
{
    public class ContentPlayer : IEquatable<ContentPlayer>
    {
        public int Width { get; }
        public int Height { get; }
        public string Html { get; }

        public ContentPlayer(int width, int height, string html)
        {
            try
            {
                if (width < 0)
                {
                    throw new ArgumentException("Given width is invalid");
                }

                if (height < 0)
                {
                    throw new ArgumentException("Given height is invalid");
                }

                if (string.IsNullOrEmpty(html))
                {
                    throw new ArgumentException("Given html is invalid");
                }

                Width = width;
                Height = height;
                Html = html;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Could not create content player", exception);
            }
        }

        public bool Equals(ContentPlayer other)
        {
            if (other == null)
            {
                return false;
            }

            return Width == other.Width && Height == other.Height && Html == other.Html;
        }

        public override bool Equals(object obj)
        {
            if (obj is ContentPlayer other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(Width, Height, Html);
    }
}
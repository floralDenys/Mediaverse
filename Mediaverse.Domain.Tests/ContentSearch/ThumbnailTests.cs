using Mediaverse.Domain.ContentSearch.Enums;
using Mediaverse.Domain.ContentSearch.ValueObjects;
using Xunit;

namespace Mediaverse.Domain.Tests.ContentSearch
{
    public class ThumbnailTests
    {
        [Fact]
        public void Create_preview()
        {
            long actualWidth = 100;
            long actualHeight = 100;
            string actualUrl = "https://someurl.com";
            
            Thumbnail thumbnail = new Thumbnail(
                actualWidth,
                actualHeight,
                actualUrl);
            
            Assert.Equal(actualWidth, thumbnail.Width);
            Assert.Equal(actualHeight, thumbnail.Height);
            Assert.Equal(actualUrl, thumbnail.Url);
        }
    }
}
using Mediaverse.Domain.ContentSearch.Enums;
using Mediaverse.Domain.ContentSearch.ValueObjects;
using Xunit;

namespace Mediaverse.Domain.Tests.ContentSearch
{
    public class PreviewTests
    {
        [Fact]
        public void Create_preview()
        {
            string actualExternalId = "someId";
            MediaContentSource actualSource = MediaContentSource.YouTube;
            MediaContentType actualType = MediaContentType.Video;
            string actualTitle = "sometitle";
            string actualDescription = "somedescription";
            Thumbnail actualThumbnail = new Thumbnail(100, 100, "https://someurl.com");
            
            var preview = new Preview(
                actualExternalId,
                actualSource,
                actualType,
                actualTitle,
                actualDescription,
                actualThumbnail);

            Assert.Equal(actualExternalId, preview.ContentId.ExternalId);
            Assert.Equal(actualSource, preview.ContentId.ContentSource);
            Assert.Equal(actualType, preview.ContentId.ContentType);
            Assert.Equal(actualTitle, preview.Title);
            Assert.Equal(actualDescription, preview.Description);
            Assert.Equal(actualThumbnail.Url, preview.Thumbnail.Url);
        }
    }
}
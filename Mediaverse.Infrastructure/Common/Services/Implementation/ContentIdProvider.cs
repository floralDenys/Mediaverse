using System;
using Mediaverse.Domain.ContentSearch.Enums;
using Mediaverse.Infrastructure.Common.Repositories;

namespace Mediaverse.Infrastructure.Common.Services.Implementation
{
    public class ContentIdProvider : IContentIdProvider
    {
        public class ContentId
        {
            public Guid InternalId { get; set; }
            public string ExternalId { get; set; }
            public MediaContentSource Source { get; set; }
        }

        private readonly IContentIdRepository _contentIdRepository;
        private readonly IGuidProvider _guidProvider;

        public ContentIdProvider(
            IContentIdRepository contentIdRepository,
            IGuidProvider guidProvider)
        {
            _contentIdRepository = contentIdRepository;
            _guidProvider = guidProvider;
        }
        
        public Guid GetOrCreateInternalId(string externalId, MediaContentSource source)
        {
            var contentId = _contentIdRepository.GetContentId(externalId, source) 
                            ?? new ContentId
                            {
                                ExternalId = externalId,
                                Source = source,
                                InternalId = _guidProvider.GetNewGuid()
                            };

            return contentId.InternalId;
        }
    }
}
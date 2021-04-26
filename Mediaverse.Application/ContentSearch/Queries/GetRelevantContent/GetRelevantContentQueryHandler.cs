using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.ContentSearch.Queries.GetRelevantContent.Dtos;
using Mediaverse.Domain.Common;
using Mediaverse.Domain.ContentSearch.Enums;
using Mediaverse.Domain.ContentSearch.Repositories;
using Mediaverse.Domain.ContentSearch.Services;
using Microsoft.Extensions.Logging;

namespace Mediaverse.Application.ContentSearch.Queries.GetRelevantContent
{
    public class GetRelevantContentQueryHandler : IRequestHandler<GetRelevantContentQuery, SearchResultDto>
    {
        private readonly IContentRepository _contentRepository;
        private readonly IQueryStringProcessor _queryStringProcessor;
        private readonly ILogger<GetRelevantContentQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetRelevantContentQueryHandler(
            IContentRepository contentRepository,
            IQueryStringProcessor queryStringProcessor,
            ILogger<GetRelevantContentQueryHandler> logger,
            IMapper mapper)
        {
            _contentRepository = contentRepository;
            _queryStringProcessor = queryStringProcessor;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<SearchResultDto> Handle(GetRelevantContentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var contentQueryType = _queryStringProcessor.DefineQueryStringType(request.SelectedSource, request.QueryString);

                string queryString = contentQueryType == ContentQueryType.ContentId
                    ? _queryStringProcessor.ExtractExternalContentIdFromUrl(request.SelectedSource, request.QueryString)
                    : request.QueryString;
                
                var searchResult = await _contentRepository.SearchForContentAsync(
                    request.SelectedSource,
                    contentQueryType,
                    queryString,
                    cancellationToken);

                return _mapper.Map<SearchResultDto>(searchResult);
            }
            catch (InformativeException exception)
            {
                _logger.LogError(exception, $"Could not get relevant content for {request}");
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not get relevant content for {request}", exception);
                throw new InformativeException("Could not get relevant content for your request. Please retry");
            }
        }
    }
}
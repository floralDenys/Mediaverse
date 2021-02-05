using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mediaverse.Application.ContentSearch.Queries.GetRelevantContent.Dtos;
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
                var searchResult =  await _contentRepository.SearchForContentAsync(
                    request.SelectedSource,
                    contentQueryType,
                    request.QueryString,
                    cancellationToken);

                return _mapper.Map<SearchResultDto>(searchResult);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Could not get relevant content for {request}", exception);
                throw new InvalidOperationException("Could not get relevant content. Please retry");
            }
        }
    }
}
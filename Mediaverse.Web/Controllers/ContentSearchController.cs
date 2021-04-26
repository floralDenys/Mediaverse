using System.Threading.Tasks;
using MediatR;
using Mediaverse.Application.ContentSearch.Queries.GetRelevantContent;
using Mediaverse.Domain.ContentSearch.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Mediaverse.Web.Controllers
{
    public class ContentSearchController : Controller
    {
        private readonly IMediator _mediator;

        public ContentSearchController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<ActionResult> SearchForContent(MediaContentSource source, string queryString)
        {
            var query = new GetRelevantContentQuery {SelectedSource = source, QueryString = queryString};
            var searchResult = await _mediator.Send(query);
            return PartialView("SearchResults", searchResult);
        }
    }
}
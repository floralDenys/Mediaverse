using System.Threading.Tasks;
using MediatR;
using Mediaverse.Application.ContentSearch.Queries.GetRelevantContent;
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
        public async Task<ActionResult> SearchForContent(GetRelevantContentQuery query)
        {
            var searchResult = await _mediator.Send(query);
            return PartialView("SearchResults", searchResult);
        }
    }
}
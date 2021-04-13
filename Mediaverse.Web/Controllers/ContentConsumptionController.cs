using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Commands.CreateRoom;
using Microsoft.AspNetCore.Mvc;

namespace Mediaverse.Web.Controllers
{
    public class ContentConsumptionController : Controller
    {
        private readonly IMediator _mediator;

        public ContentConsumptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult Index(Guid viewerId)
        {
            return View(viewerId);
        }
        
        [HttpGet]
        public IActionResult CreateRoom(Guid viewerId)
        {
            var command = new CreateRoomCommand {HostId = viewerId};
            return View(command);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRoom(CreateRoomCommand command, CancellationToken cancellationToken)
        {
            var room = await _mediator.Send(command, cancellationToken);
            return RedirectToAction("Room", "ContentConsumption", new {roomId = room.Id});
        }
    }
}
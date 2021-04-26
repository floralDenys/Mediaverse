using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Commands.AddContentToPlaylist;
using Mediaverse.Application.JointContentConsumption.Commands.CloseRoom;
using Mediaverse.Application.JointContentConsumption.Commands.CreateRoom;
using Mediaverse.Application.JointContentConsumption.Commands.JoinRoom;
using Mediaverse.Application.JointContentConsumption.Commands.LeaveRoom;
using Mediaverse.Application.JointContentConsumption.Commands.RemoveContentFromPlaylist;
using Mediaverse.Application.JointContentConsumption.Commands.SwitchContent;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;
using Mediaverse.Application.JointContentConsumption.Queries.GetRoom;
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
            
            return Json(new
            {
                redirectToUrl = @Url.Action(
                    "Room",
                    "ContentConsumption",
                    new { roomId = room.Id})
            });
        }

        [HttpPost]
        public async Task CloseRoom(CloseRoomCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult> JoinRoom(Guid guestId, string roomToken, CancellationToken cancellationToken)
        {
            var command = new JoinRoomCommand {ViewerId = guestId, RoomToken = roomToken};
            var room = await _mediator.Send(command, cancellationToken);
            
            return Json(new
            {
                redirectToUrl = @Url.Action(
                    "Room",
                    "ContentConsumption",
                    new {roomId = room.Id})
            });
        }

        [HttpPost]
        public async Task LeaveRoom(LeaveRoomCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult> AddContentToPlaylist(
            AddContentToPlaylistCommand command,
            CancellationToken cancellationToken)
        {
            var playlist = await _mediator.Send(command, cancellationToken);
            return View("Playlist", playlist);
        }

        [HttpGet]
        public async Task<ActionResult> Room(Guid roomId, CancellationToken cancellationToken)
        {
            var query = new GetRoomQuery {RoomId = roomId};
            var room = await _mediator.Send(query, cancellationToken);
            return View(room);
        }
        
        [HttpPost]
        public async Task<PlaylistDto> RemoveContentFromPlaylist(
            RemoveContentFromPlaylistCommand command,
            CancellationToken cancellationToken)
        {
            var playlist = await _mediator.Send(command, cancellationToken);
            return playlist;
        }

        [HttpPost]
        public async Task<ActionResult> SwitchContent(SwitchContentCommand command, CancellationToken cancellationToken)
        {
            var content = await _mediator.Send(command, cancellationToken);
            return View("ContentPlayer", content);
        }
    }
}
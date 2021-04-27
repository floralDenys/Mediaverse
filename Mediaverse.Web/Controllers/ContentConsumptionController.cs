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
using Mediaverse.Application.JointContentConsumption.Queries.GetAvailablePlaylists;
using Mediaverse.Application.JointContentConsumption.Queries.GetPlaylist;
using Mediaverse.Application.JointContentConsumption.Queries.GetPlaylist.Dtos;
using Mediaverse.Application.JointContentConsumption.Queries.GetRoom;
using Mediaverse.Infrastructure.Authentication.Extensions;
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
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> CreateRoom()
        {
            var viewerId = User.GetCurrentUserId();
            
            var query = new GetAvailablePlaylistsQuery {HostId = viewerId};
            var availablePlaylists = await _mediator.Send(query);
            
            // adding default option
            availablePlaylists.Add(new SelectablePlaylistDto
            {
                Id = default,
                Name = "Create temporary"
            });
            
            var command = new CreateRoomCommand {HostId = viewerId, AvailablePlaylists = availablePlaylists};
            return View(command);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRoom(CreateRoomCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var room = await _mediator.Send(command, cancellationToken);

                return Json(new
                {
                    redirectToUrl = @Url.Action(
                        "Room",
                        "ContentConsumption",
                        new {roomId = room.Id})
                });
            }
            catch (Exception exception)
            {
                return BadRequest(new {message = exception.Message});
            }
        }

        [HttpPost]
        public async Task CloseRoom(CloseRoomCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult> JoinRoom(string roomToken, CancellationToken cancellationToken)
        {
            try
            {
                var command = new JoinRoomCommand
                {
                    ViewerId = User.GetCurrentUserId(),
                    RoomToken = roomToken
                };
                var room = await _mediator.Send(command, cancellationToken);

                return Json(new
                {
                    redirectToUrl = @Url.Action(
                        "Room",
                        "ContentConsumption",
                        new {roomId = room.Id})
                });
            }
            catch (Exception exception)
            {
                return BadRequest(new {message = exception.Message});
            }
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
            return RedirectToAction("Playlist", new {playlistId = playlist.Id});
        }

        [HttpGet]
        public async Task<ActionResult> Room(Guid roomId, CancellationToken cancellationToken)
        {
            var query = new GetRoomQuery {RoomId = roomId};
            var room = await _mediator.Send(query, cancellationToken);
            return View(room);
        }

        [HttpGet]
        public async Task<ActionResult> Playlist(Guid playlistId, CancellationToken cancellationToken)
        {
            var query = new GetPlaylistQuery {PlaylistId = playlistId};
            var playlist = await _mediator.Send(query, cancellationToken);
            return PartialView(playlist);
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
            return PartialView("ContentPlayer", content);
        }
    }
}
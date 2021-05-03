using System;
using System.Threading;
using System.Threading.Tasks;
using Lib.AspNetCore.ServerSentEvents;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Commands.AddContentToPlaylist;
using Mediaverse.Application.JointContentConsumption.Commands.ChangeContentPlayerState;
using Mediaverse.Application.JointContentConsumption.Commands.CloseRoom;
using Mediaverse.Application.JointContentConsumption.Commands.CreateRoom;
using Mediaverse.Application.JointContentConsumption.Commands.JoinRoom;
using Mediaverse.Application.JointContentConsumption.Commands.LeaveRoom;
using Mediaverse.Application.JointContentConsumption.Commands.RemoveContentFromPlaylist;
using Mediaverse.Application.JointContentConsumption.Commands.SwitchContent;
using Mediaverse.Application.JointContentConsumption.Queries.GetAvailablePlaylists;
using Mediaverse.Application.JointContentConsumption.Queries.GetCurrentlyPlayingContent;
using Mediaverse.Application.JointContentConsumption.Queries.GetPlaylist;
using Mediaverse.Application.JointContentConsumption.Queries.GetPlaylist.Dtos;
using Mediaverse.Application.JointContentConsumption.Queries.GetPublicRooms;
using Mediaverse.Application.JointContentConsumption.Queries.GetRoom;
using Mediaverse.Infrastructure.Authentication.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mediaverse.Web.Controllers
{
    [Authorize]
    public class ContentConsumptionController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IServerSentEventsService _service;
        
        public ContentConsumptionController(
            IMediator mediator,
            IServerSentEventsService sentEventsService)
        {
            _mediator = mediator;
            _service = sentEventsService;
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

        [AllowAnonymous]
        [HttpGet]
        public ActionResult JoinRoomByLink(string roomToken)
        {
            return View("JoinRoomByLink", roomToken);
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
        public async Task LeaveRoom(Guid roomId)
        {
            var command = new LeaveRoomCommand {RoomId = roomId, ViewerId = User.GetCurrentUserId()};
            await _mediator.Send(command, CancellationToken.None);
        }

        [HttpPost]
        public async Task<ActionResult> AddContentToPlaylist(
            AddContentToPlaylistCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(command, cancellationToken);
                
                await _service.SendEventAsync("playlist_updated", cancellationToken);

                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(new {message = exception.Message});
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> RemoveContentFromPlaylist(
            RemoveContentFromPlaylistCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(command, cancellationToken);

                await _service.SendEventAsync("playlist_updated", cancellationToken);
                
                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(new {message = exception.Message});
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> SwitchContent(SwitchContentCommand command, CancellationToken cancellationToken)
        {
            try
            {
                await _mediator.Send(command, cancellationToken);
                
                await _service.SendEventAsync("switched_content", cancellationToken);

                return Ok();
            }
            catch (Exception exception)
            {
                return BadRequest(new {message = exception.Message});
            }
        }

        [HttpPost]
        public async Task<ActionResult> ChangeContentPlayerState(ChangeContentPlayerStateCommand command)
        {
            try
            {
                await _mediator.Send(command);
                
                await _service.SendEventAsync(command.State);

                return Ok();
            }
            catch (Exception exception)
            { 
                return BadRequest(new {message = exception.Message});
            }
        }

        [HttpGet]
        public async Task<ActionResult> Room(Guid roomId, CancellationToken cancellationToken)
        {
            var query = new GetRoomQuery {RoomId = roomId};
            var room = await _mediator.Send(query, cancellationToken);
            return View(room);
        }

        [HttpGet]
        public async Task<ActionResult> Playlist(Guid roomId, CancellationToken cancellationToken)
        {
            var query = new GetPlaylistQuery {RoomId = roomId};
            var playlist = await _mediator.Send(query, cancellationToken);
            return PartialView(playlist);
        }
        
        [HttpGet]
        public async Task<ActionResult> CurrentlyPlayingContent(Guid roomId, CancellationToken cancellationToken)
        {
            var query = new GetCurrentlyPlayingContentQuery {RoomId = roomId}; 
            var roomDto = await _mediator.Send(query, cancellationToken);
            return PartialView("ContentPlayer", roomDto);
        }

        [HttpGet]
        public async Task<ActionResult> GetPublicRooms(CancellationToken cancellationToken)
        {
            var query = new GetPublicRoomsQuery();
            var rooms = await _mediator.Send(query, cancellationToken);
            return PartialView("PublicRooms", rooms);
        }
    }
}
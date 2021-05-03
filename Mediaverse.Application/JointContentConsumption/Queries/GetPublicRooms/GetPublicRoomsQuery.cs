using System.Collections.Generic;
using MediatR;
using Mediaverse.Application.JointContentConsumption.Common.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Queries.GetPublicRooms
{
    public class GetPublicRoomsQuery : IRequest<IEnumerable<RoomDto>>
    { }
}
using System;
using System.Collections.Generic;

namespace Mediaverse.Application.JointContentConsumption.Common.Dtos
{
    public class AffectedViewersDto
    {
        public IEnumerable<Guid> ViewerIds { get; set; }
    }
}
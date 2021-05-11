using System;
using System.Collections.Generic;

namespace Mediaverse.Application.JointContentConsumption.Common.Dtos
{
    public class AffectedViewers
    {
        public IEnumerable<Guid> ViewerIds { get; set; }
    }
}
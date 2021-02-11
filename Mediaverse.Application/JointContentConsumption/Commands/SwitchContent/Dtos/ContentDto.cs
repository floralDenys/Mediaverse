using Mediaverse.Application.JointContentConsumption.Common.Dtos;

namespace Mediaverse.Application.JointContentConsumption.Commands.SwitchContent.Dtos
{
    public class ContentDto
    {
        public ContentIdDto ContentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
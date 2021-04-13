using AutoMapper;
using Mediaverse.Domain.JointContentConsumption.ValueObjects;
using JointContentConsumptionContext = Mediaverse.Domain.JointContentConsumption;
using ContentSearchContext = Mediaverse.Domain.ContentSearch;

namespace Mediaverse.Infrastructure.JointContentConsumption.Mapping
{
    public class InfrastructureMapping : Profile
    {
        public InfrastructureMapping() => ConfigureMappings();
        
        private void ConfigureMappings()
        {
            CreateMap<ContentSearchContext.ValueObjects.Content, JointContentConsumptionContext.Entities.Content>()
                .ConstructUsing(c => new JointContentConsumptionContext.Entities.Content(
                    new ContentId(
                        c.Id.ExternalId,
                        (JointContentConsumptionContext.Enums.MediaContentSource) c.Id.ContentSource,
                        (JointContentConsumptionContext.Enums.MediaContentType) c.Id.ContentType),
                    c.Title,
                    new ContentPlayer(c.PlayerWidth, c.PlayerHeight, c.PlayerHtml),
                    c.Description));
        }
    }
}
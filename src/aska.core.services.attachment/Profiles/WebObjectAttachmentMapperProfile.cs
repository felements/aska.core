using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using kd.domainmodel.Attachment;
using kd.services.attachment.Handlers.Paths;
using kd.services.attachment.Models;

namespace kd.services.attachment.Profiles
{
    public class WebObjectAttachmentMapperProfile : Profile
    {
        public WebObjectAttachmentMapperProfile(IConstructAttachmentPathHandler constructAttachmentPathHandler)
        {
            CreateMap<AttachmentEntity, WebObjectAttachment>()
                .ForMember(m => m.Type, e => e.MapFrom(a => a.Type))
                .ForMember(m => m.Id, e => e.MapFrom(a => a.Id))
                .ForMember(m => m.LastModifiedDateUtc, e => e.MapFrom(a => a.LastModifiedDateUtc))
                .ForMember(m => m.Title, e => e.MapFrom(a => a.Title))
                .ForMember(m => m.OriginalFileName, e => e.MapFrom(a => a.OriginalFileName))
                .ForMember(m => m.Urls, e => e.ResolveUsing((attachment, result) =>
                     attachment.Variants.Select(variant => new KeyValuePair<ObjectAttachmentVariant, string>(variant, constructAttachmentPathHandler.Handle(attachment, variant)))))
                .ForSourceMember("RawVariants", o => o.Ignore())
                .ForSourceMember("Variants", o => o.Ignore());
        }
        
    }
}
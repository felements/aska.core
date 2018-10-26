using System.Collections.Generic;
using kd.domainmodel.Attachment;
using Nancy;

namespace kd.services.attachment.Handlers
{
    public interface IObjectAttachmentHandler
    {
        IEnumerable<AttachmentEntity> Create(IEnumerable<HttpFile> files);
    }
}
using System.Collections.Generic;
using Autofac;
using kd.domainmodel.Attachment;
using kd.services.attachment.Factory;
using kd.services.attachment.Handlers.Variants;
using kd.services.attachment.Store;
using Nancy;
using NLog;

namespace kd.services.attachment.Handlers
{
    internal class ObjectAttachmentHandler : IObjectAttachmentHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IAttachmentFactory _attachmentFactory;
        private readonly IAttachmentStore _attachmentStore;
        private readonly ILifetimeScope _scope;

        public ObjectAttachmentHandler(
            IAttachmentFactory attachmentFactory, 
            IAttachmentStore attachmentStore,
            ILifetimeScope scope)
        {
            _attachmentFactory = attachmentFactory;
            _attachmentStore = attachmentStore;
            _scope = scope;
        }

        public IEnumerable<AttachmentEntity> Create(IEnumerable<HttpFile> files)//todo: remove reference to Nancy
        {
            if (files == null) yield break;
            foreach (var httpFile in files)
            {
                var attachment = _attachmentFactory.Create(httpFile.Name);
                if (attachment == null) continue;

                // save original to the store (disk by default)
                if (!_attachmentStore.Store(attachment, ObjectAttachmentVariant.Original, httpFile.Value))
                {
                    Logger.Error("Cannot save attachment " + attachment.OriginalFileName);
                    continue;
                }

                // handle variants (ex.: create resized variants for image attachment)
                var variantsHandler = _scope.ResolveKeyed<IAttachmentVariantsHandler>(attachment.Type);
                variantsHandler.Handle(attachment);

                yield return attachment;
            }
        }
    }
}
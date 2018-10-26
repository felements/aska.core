using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using kd.domainmodel.Attachment;
using kd.infrastructure.CommandQuery.Interfaces;
using kd.infrastructure.CommandQuery.Specification;
using kd.misc.Constants;
using kd.services.attachment.Handlers;
using kd.services.attachment.Models;
using Nancy;
using Nancy.Security;
using NLog;

namespace kd.web.api.Core
{
    public sealed class AttachmentsModule : NancyModule
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AttachmentsModule(IQueryFactory queryFactory,
            ICommandFactory commandFactory,
            IObjectAttachmentHandler attachmentHandler,
            IMapper mapper) : base(Routing.ApiV1.Base + "/attachments")
        {
            this.RequiresAuthentication();
           
            //
            //      Get attachment by id
            Get("/{attachment_id:guid}", args =>
            {
                var id = (Guid)args.attachment_id;
                var attachment = queryFactory.GetQuery<AttachmentEntity, ByIdExpressionSpecification<AttachmentEntity>>()
                    .Where(new ByIdExpressionSpecification<AttachmentEntity>(id))
                    .FirstOrDefault();

                if (attachment == null) return Negotiate.WithStatusCode(HttpStatusCode.NotFound);
                var webAttachment = mapper.Map<AttachmentEntity, WebObjectAttachment>(attachment);
                return Negotiate.WithModel(webAttachment).WithStatusCode(HttpStatusCode.OK);
            });

            //
            //      Remove attachment by id
            // TODO: 1. find values with key 'attachment' and value 'attachment_id'
            // 2. remove attachment
            Delete("/{attachment_id:guid}", args =>
            {
                var id = (Guid)args.attachment_id;
                var attachment = queryFactory.GetQuery<AttachmentEntity, ByIdExpressionSpecification<AttachmentEntity>>()
                    .Where(new ByIdExpressionSpecification<AttachmentEntity>(id))
                    .SingleOrDefault();

                if (attachment == null) return Negotiate.WithStatusCode(HttpStatusCode.NotFound);
                commandFactory.GetDeleteCommand<AttachmentEntity>().Execute(attachment);

                return Negotiate.WithStatusCode(HttpStatusCode.NoContent);
            });

            //
            //      Create new attachment
            Post("/", args =>
            {
                // TODO: verify attachments

                // upload files
                var attachments = attachmentHandler.Create(Request.Files)?.ToList() ?? new List<AttachmentEntity>();

                // create entities in DB
                foreach (var attachment in attachments)
                {
                    commandFactory.GetCreateCommand<AttachmentEntity>().Execute(attachment);
                    Logger.Debug("Uploaded new attachment #{0:D} '{1}'", attachment.Id, attachment.OriginalFileName);
                }

                var webObjectAttachments = attachments.Select(mapper.Map<AttachmentEntity, WebObjectAttachment>).ToList();
                return Negotiate.WithModel(webObjectAttachments).WithStatusCode(HttpStatusCode.OK);
            });

            //
            //      Update attachment data
            Put("/{attachment_id}", args => Negotiate.WithStatusCode(HttpStatusCode.NotImplemented));

        }
    }
}
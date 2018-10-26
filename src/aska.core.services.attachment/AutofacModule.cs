using Autofac;
using Autofac.Core;
using AutoMapper;
using kd.domainmodel.Attachment;
using kd.services.attachment.Converter.AttachmentConverter;
using kd.services.attachment.Converter.ConversionTaskFactory;
using kd.services.attachment.Factory;
using kd.services.attachment.Handlers;
using kd.services.attachment.Handlers.Paths;
using kd.services.attachment.Handlers.Variants;
using kd.services.attachment.Profiles;
using kd.services.attachment.Store;
using NLog;

namespace kd.services.attachment
{
    public class AutofacModule : Module
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected override void Load(ContainerBuilder builder)
        {
            Logger.Debug("Register " + typeof(AttachmentFactory).Name);
            builder.RegisterType<AttachmentFactory>()
                .As<IAttachmentFactory>()
                .InstancePerLifetimeScope();

            Logger.Debug("Register " + typeof(FileAttachmentStore).Name);
            builder.RegisterType<FileAttachmentStore>()
                .As<IAttachmentStore>()
                .WithParameter(new ResolvedParameter(
                        (info, context) => info.ParameterType == typeof(IConstructAttachmentPathHandler),
                        (info, context) => context.ResolveKeyed<IConstructAttachmentPathHandler>(AttachmentPathContext.Filesystem)))
                .InstancePerLifetimeScope();

            Logger.Debug("Register " + typeof(ImageAttachmentConverter).Name);
            builder.RegisterType<ImageAttachmentConverter>()
                .As<IAttachmentConverter>()
                .WithParameter(new ResolvedParameter(
                        (info, context) => info.ParameterType == typeof(IConstructAttachmentPathHandler),
                        (info, context) => context.ResolveKeyed<IConstructAttachmentPathHandler>(AttachmentPathContext.Filesystem)))
                .InstancePerLifetimeScope();

            Logger.Debug("Register " + typeof(ObjectAttachmentHandler).Name);
            builder.RegisterType<ObjectAttachmentHandler>()
                .As<IObjectAttachmentHandler>()
                .InstancePerLifetimeScope();

            Logger.Debug("Register " + typeof(AttachmentConversionTaskFactory).Name);
            builder.RegisterType<AttachmentConversionTaskFactory>()
                .As<IAttachmentConversionTaskFactory>()
                .InstancePerLifetimeScope();

            #region attachments conversion

            Logger.Debug("Register " + typeof(AttachmentConversionTaskFactory).Name);
            builder.RegisterType<AttachmentConversionTaskFactory>()
                .As<IAttachmentConversionTaskFactory>()
                .InstancePerLifetimeScope();

            Logger.Debug("Register implementations of " + typeof(IAttachmentConverter).Name);

            Logger.Debug(" - " + typeof(ImageAttachmentConverter).Name);
            builder.RegisterType<ImageAttachmentConverter>()
                .Keyed<IAttachmentConverter>(ObjectAttachmentVariant.SquareSmallImage)
                .Keyed<IAttachmentConverter>(ObjectAttachmentVariant.LargeImage)
                .Keyed<IAttachmentConverter>(ObjectAttachmentVariant.MediumImage)
                .Keyed<IAttachmentConverter>(ObjectAttachmentVariant.SmallImage)
                .Keyed<IAttachmentConverter>(ObjectAttachmentVariant.SquareLargeImage)
                .InstancePerLifetimeScope();

            Logger.Debug(" - " + typeof(DummyAttachmentConverter).Name);
            builder.RegisterType<ImageAttachmentConverter>()
                .Keyed<IAttachmentConverter>(ObjectAttachmentVariant.Original)
                .InstancePerLifetimeScope();

            #endregion


            #region IAttachmentVariantsHandler
            Logger.Debug("Register keyed implementations of " + typeof(IAttachmentVariantsHandler).Name);

            Logger.Debug(" - " + FileAttachmentType.Common.ToString("G"));
            builder.RegisterType<CommonAttachmentVariantsHandler>()
                .Keyed<IAttachmentVariantsHandler>(FileAttachmentType.Common)
                .InstancePerLifetimeScope();

            Logger.Debug(" - " + FileAttachmentType.Document.ToString("G"));
            builder.RegisterType<DocumentAttachmentVariantsHandler>()
                .Keyed<IAttachmentVariantsHandler>(FileAttachmentType.Document)
                .InstancePerLifetimeScope();

            Logger.Debug(" - " + FileAttachmentType.Image.ToString("G"));
            builder.RegisterType<ImageAttachmentVariantsHandler>()
                .Keyed<IAttachmentVariantsHandler>(FileAttachmentType.Image)
                .WithParameter(new ResolvedParameter(
                        (info, context) => info.ParameterType == typeof(IConstructAttachmentPathHandler),
                        (info, context) => context.ResolveKeyed<IConstructAttachmentPathHandler>(AttachmentPathContext.Filesystem)))
                .InstancePerLifetimeScope();
            #endregion

            #region Automapper profiles
            Logger.Debug("Register AutoMapper profile " + typeof(FilesystemObjectAttachmentMapperProfile).Name);
            builder.RegisterType<FilesystemObjectAttachmentMapperProfile>()
                .As<Profile>()
                .WithParameter(new ResolvedParameter(
                        (info, context) => info.ParameterType == typeof(IConstructAttachmentPathHandler),
                        (info, context) => context.ResolveKeyed<IConstructAttachmentPathHandler>(AttachmentPathContext.Filesystem)))
                .InstancePerLifetimeScope();

            Logger.Debug("Register AutoMapper profile " + typeof(WebObjectAttachmentMapperProfile).Name);
            builder.RegisterType<WebObjectAttachmentMapperProfile>()
                .As<Profile>()
                .WithParameter(new ResolvedParameter(
                        (info, context) => info.ParameterType == typeof(IConstructAttachmentPathHandler),
                        (info, context) => context.ResolveKeyed<IConstructAttachmentPathHandler>(AttachmentPathContext.Web)))
                .InstancePerLifetimeScope(); 
            #endregion

            #region Path construction
            Logger.Debug("Register attachment path construction handlers - " + typeof(IConstructAttachmentPathHandler).Name);

            Logger.Debug(" - " + AttachmentPathContext.Filesystem.ToString("G"));
            builder.RegisterType<ConstructAttachmentFilesystemRelatedPathHandler>()
                .Keyed<IConstructAttachmentPathHandler>(AttachmentPathContext.Filesystem)
                .InstancePerLifetimeScope();

            Logger.Debug(" - " + AttachmentPathContext.Web.ToString("G"));
#if STUB_IMAGES
            builder.RegisterType<ConstructAttachmentStubWebRelatedPathHandler>()
                .Keyed<IConstructAttachmentPathHandler>(AttachmentPathContext.Web)
                .InstancePerLifetimeScope();
#else
            builder.RegisterType<ConstructAttachmentWebRelatedPathHandler>()
                .Keyed<IConstructAttachmentPathHandler>(AttachmentPathContext.Web)
                .InstancePerLifetimeScope();
#endif

            #endregion

        }
    }
}
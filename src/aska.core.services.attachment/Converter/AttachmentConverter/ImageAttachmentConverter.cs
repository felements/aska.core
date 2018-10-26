using System;
using System.Configuration;
using kd.domainmodel.Attachment;
using kd.infrastructure.ShellCommandExecutionProvider;
using kd.misc;
using kd.services.attachment.Converter.ConversionTaskFactory;

namespace kd.services.attachment.Converter.AttachmentConverter
{
    internal class ImageAttachmentConverter : IAttachmentConverter
    {
        //TODO: move to configuration
        private const string ResizeCommandArgs = "-verbose {{SOURCE}}  -filter Triangle -define filter:support=2 -thumbnail x{{SIZE}} -unsharp 0.25x0.25+8+0.065 -dither None -posterize 136 -quality 82 -define jpeg:fancy-upsampling=off -define png:compression-filter=5 -define png:compression-level=9 -define png:compression-strategy=1 -define png:exclude-chunk=all -interlace none -colorspace sRGB -strip  {{DESTINATION}}";
        private const string SquareCommandArgs = "-verbose -define jpeg:size={{SIZE_PRELOAD}}x{{SIZE_PRELOAD}} -define png:size={{SIZE_PRELOAD}}x{{SIZE_PRELOAD}} {{SOURCE}} -filter Triangle -define filter:support=2 -thumbnail \"{{SIZE}}x{{SIZE}}^\" -gravity center -crop {{SIZE}}x{{SIZE}}+0+0 -unsharp 0.25x0.25+8+0.065 -dither None -posterize 136 -quality 82 -define jpeg:fancy-upsampling=off -define png:compression-filter=5 -define png:compression-level=9 -define png:compression-strategy=1 -define png:exclude-chunk=all -interlace none -colorspace sRGB -strip {{DESTINATION}}";

        private const string SizeToken = "{{SIZE}}";
        private const string SizePreloadToken = "{{SIZE_PRELOAD}}";
        private const string SourceToken = "{{SOURCE}}";
        private const string DestinationToken = "{{DESTINATION}}";

        private readonly IShellCommandExecutionProvider _shellExecute;

        public ImageAttachmentConverter(IShellCommandExecutionProvider shellExecute)
        {
            _shellExecute = shellExecute;

            if (!ResizeCommandArgs.Contains(SizeToken)
                || !ResizeCommandArgs.Contains(SourceToken)
                || !ResizeCommandArgs.Contains(DestinationToken))
                throw new ConfigurationErrorsException(
                    "Check the image resizer command line. One of the tokens is missing");
        }

        public AttachmentConversionTaskResult Proceed(ObjectAttachmentVariant variant, string sourceFile, string targetFile)
        {
            var cmd = PrepareCommand(variant, sourceFile, targetFile);
            var executionResult = _shellExecute.Execute(cmd, TimeSpan.FromMinutes(2));
           
            return new AttachmentConversionTaskResult(executionResult.Succeed, executionResult.Reason);
        }

        private static ShelCommand PrepareCommand(ObjectAttachmentVariant variant, string sourceFile, string targetFile)
        {
            string args = "";
            switch (variant)
            {
                case ObjectAttachmentVariant.SmallImage:
                    args = ResizeCommandArgs.Replace(SizeToken, "350").Replace(SourceToken, sourceFile).Replace(DestinationToken, targetFile);
                    break;
                case ObjectAttachmentVariant.MediumImage:
                    args = ResizeCommandArgs.Replace(SizeToken, "800").Replace(SourceToken, sourceFile).Replace(DestinationToken, targetFile);
                    break;
                case ObjectAttachmentVariant.LargeImage:
                    args = ResizeCommandArgs.Replace(SizeToken, "1280").Replace(SourceToken, sourceFile).Replace(DestinationToken, targetFile);
                    break;
                case ObjectAttachmentVariant.SquareSmallImage:
                    args = SquareCommandArgs.Replace(SizeToken, "300").Replace(SizePreloadToken, "500").Replace(SourceToken, sourceFile).Replace(DestinationToken, targetFile);
                    break;
                case ObjectAttachmentVariant.SquareLargeImage:
                    args = SquareCommandArgs.Replace(SizeToken, "700").Replace(SizePreloadToken, "1000").Replace(SourceToken, sourceFile).Replace(DestinationToken, targetFile);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(variant), variant, null);
            }
            
            var cmd = new ShelCommand
            {
                Command = "convert",
                WorkingDirectory = "../../../../../thirdparty/imagemagick-win-x64/",
                //WorkingDirectory = "../../../../thirdparty/imagemagick-win-x64/",
                Arguments = args
            };
            if (ApplicationExtensions.GetEnvironment().OsType == RuntimeOsType.Linux)
                cmd.WorkingDirectory = string.Empty;
            
            return cmd;
        }

    }

    
}
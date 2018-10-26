using System;
using System.Collections.Generic;
using System.IO;
using kd.domainmodel.Attachment;

namespace kd.services.attachment.Extensions
{
    public static class AttachmentExtensions
    {
        public static FileAttachmentType GetAttachmentType(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return FileAttachmentType.Common;

            if (name.EndsWith(".jpg",StringComparison.InvariantCultureIgnoreCase)
                || name.EndsWith(".jpeg", StringComparison.InvariantCultureIgnoreCase)
                || name.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
                return FileAttachmentType.Image;


            if (name.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase)
                || name.EndsWith(".doc", StringComparison.InvariantCultureIgnoreCase)
                || name.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase)
                || name.EndsWith(".rtf", StringComparison.InvariantCultureIgnoreCase)
                || name.EndsWith(".odt", StringComparison.InvariantCultureIgnoreCase)
                || name.EndsWith(".xls", StringComparison.InvariantCultureIgnoreCase)
                || name.EndsWith(".xlsx", StringComparison.InvariantCultureIgnoreCase)
                || name.EndsWith(".docx", StringComparison.InvariantCultureIgnoreCase))
                return FileAttachmentType.Document;

            return FileAttachmentType.Common;
        }

        public static string GetattachmentMimeType(string name)
        {
            var ext = Path.GetExtension(name.ToLower());
            return MimeTypes.ContainsKey(ext) ? MimeTypes[ext] : "application/octet-stream";
        }

        private static readonly Dictionary<string, string> MimeTypes = new Dictionary<string, string>()
        {
            {".aac", "audio/aac" },
            {".abw", "application/x-abiword" },
            {".avi", "video/x-msvideo" },
            {".azw", "application/vnd.amazon.ebook" },
            {".bin", "application/octet-stream" },
            {".bz", "application/x-bzip" },
            {".bz2", "application/x-bzip2" },
            {".css", "text/css" },
            {".csv", "text/csv" },
            {".doc", "application/msword" },
            {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            {".epub", "application/epub+zip" },
            {".gif", "image/gif" },
            {".htm", "text/html" },
            {".html", "text/html" },
            {".ico", "image/x-icon" },
            {".jpeg", "image/jpeg" },
            {".jpg", "image/jpeg" },
            {".js", "application/javascript" },
            {".json", "application/json" },
            {".mpeg", "video/mpeg" },
            {".odp", "application/vnd.oasis.opendocument.presentation" },
            {".ods", "application/vnd.oasis.opendocument.spreadsheet" },
            {".odt", "application/vnd.oasis.opendocument.text" },
            {".otf", "font/otf" },
            {".png", "image/png" },
            {".pdf", "application/pdf" },
            {".svg", "image/svg+xml" },
            {".tif", "image/tiff" },
            {".tiff", "image/tiff" },
            {".ttf", "font/ttf" },
            {".wav", "audio/x-wav" },
            {".weba", " audio/webm" },
            {".webm", "video/webm" },
            {".webp", "image/webp" },
            {".woff", "font/woff" },
            {".woff2", "font/woff2" },
            {".xls", "application/vnd.ms-excel" },
            {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            {".xml", "application/xml" },
            {".zip", "application/zip" },
            {".7z", "application/x-7z-compressed" },
        };
    }
}
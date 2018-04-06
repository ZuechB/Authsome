using Authsome.Portable.Models;

namespace Authsome.Portable.Extentions
{
    public static class MediaTypeExtention
    {
        public static string GetMediaType(this MediaType mediaType)
        {
            var _mediaType = "application/json"; // default since most common :)
            switch (mediaType)
            {
                case MediaType.application_graphql:
                    _mediaType = "application/graphql";
                    break;
                case MediaType.application_javascript:
                    _mediaType = "application/javascript";
                    break;
                case MediaType.application_octetstream:
                    _mediaType = "application/octet-stream";
                    break;
                case MediaType.application_json:
                    _mediaType = "application/json";
                    break;
                case MediaType.application_ldjson:
                    _mediaType = "application/ld+json";
                    break;
                case MediaType.application_msword:
                    _mediaType = "application/msword";
                    break;
                case MediaType.application_pdf:
                    _mediaType = "application/pdf";
                    break;
                case MediaType.application_sql:
                    _mediaType = "application/sql";
                    break;
                case MediaType.application_vndmsexcel:
                    _mediaType = "application/vnd.ms-excel";
                    break;
                case MediaType.application_vndmspowerpoint:
                    _mediaType = "application/vnd.ms-powerpoint";
                    break;
                case MediaType.application_vndoasisopendocumenttext:
                    _mediaType = "application/vnd.oasis.opendocument.text";
                    break;
                case MediaType.application_vndopenxmlformatsofficedocumentpresentationmlpresentation:
                    _mediaType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    break;
                case MediaType.application_vndopenxmlformatsofficedocumentspreadsheetmlsheet:
                    _mediaType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case MediaType.application_vndopenxmlformatsofficedocumentwordprocessingmldocument:
                    _mediaType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case MediaType.application_xml:
                    _mediaType = "application/xml";
                    break;
                case MediaType.application_xwwwformurlencoded:
                    _mediaType = "application/x-www-form-urlencoded";
                    break;
                case MediaType.application_zip:
                    _mediaType = "application/zip";
                    break;
                case MediaType.audio_mpeg:
                    _mediaType = "audio/mpeg";
                    break;
                case MediaType.audio_vorbis:
                    _mediaType = "audio/vorbis";
                    break;
                case MediaType.image_gif:
                    _mediaType = "image/gif";
                    break;
                case MediaType.image_jpeg:
                    _mediaType = "image/jpeg";
                    break;
                case MediaType.image_png:
                    _mediaType = "image/png";
                    break;
                case MediaType.multipart_formdata:
                    _mediaType = "multipart/form-data";
                    break;
                case MediaType.text_css:
                    _mediaType = "text/css";
                    break;
                case MediaType.text_csv:
                    _mediaType = "text/csv";
                    break;
                case MediaType.text_html:
                    _mediaType = "text/html";
                    break;
                case MediaType.text_plain:
                    _mediaType = "text/plain";
                    break;
            }
            return _mediaType;
        }
    }
}

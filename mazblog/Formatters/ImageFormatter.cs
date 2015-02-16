using System;
using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using mazblog.Models;

namespace mazblog.Formatters
{
    public class ImageFormatter : MediaTypeFormatter
    {
        private const string Png = "image/png";
        private const string Jpeg = "image/jpeg";
        private const string Gif = "image/gif";

        public ImageFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(Png));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(Jpeg));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(Gif));
        }

        #region Overrides of MediaTypeFormatter

        public override bool CanReadType(Type type)
        {
            return typeof(BlogImage) == type;
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }

        #endregion

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            if (type != typeof(BlogImage)) return Task.FromResult<object>(null);
            var postImage = (BlogImage)Activator.CreateInstance(type);
            MediaTypeHeaderValue contentType = content.Headers.ContentType;
            string fileExtension = contentType.MediaType.Replace("image/", string.Empty);
            postImage.ContentType = contentType.MediaType;
            //TODO: The name is in a "Slug" header... but always (from Message Handler) is dropper!! :@
            postImage.Title = Guid.NewGuid().ToString("N");
            postImage.FileName = string.Format("{0}.{1}", postImage.Title, fileExtension);
            using (var memoryStream = new MemoryStream())
            {
                readStream.CopyTo(memoryStream);
                postImage.BytesImage = memoryStream.ToArray();
            }
            return Task.FromResult<object>(postImage);
        }
    }
}
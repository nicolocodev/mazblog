using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using mazblog.Models;

namespace mazblog.Formatters
{
    public class AtomImageFormatter : MediaTypeFormatter
    {
        private const string Atom = "application/atom+xml";

        public AtomImageFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(Atom));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return typeof (BlogImage) == type;
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            var image = (BlogImage)value;
            var item = new SyndicationItem
            {
                Title = new TextSyndicationContent(image.Title),
                BaseUri = new Uri(image.Url),
                LastUpdatedTime = image.PublishDate,
                Content = new UrlSyndicationContent(new Uri(image.Url), image.ContentType)
            };
            item.Authors.Add(new SyndicationPerson { Name = BlogSettings.Author });
            using (XmlWriter writer = XmlWriter.Create(writeStream))
            {
                var atomformatter = new Atom10ItemFormatter(item);
                atomformatter.WriteTo(writer);
                return Task.FromResult(writer);
            }
        }
    }
}
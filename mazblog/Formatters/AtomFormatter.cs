using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using mazblog.Models;
using mazblog.ViewModels;

namespace mazblog.Formatters
{
    public class AtomFormatter : MediaTypeFormatter
    {
        private const string Atom = "application/atom+xml";
        private const string Rss = "application/rss+xml";

        public AtomFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(Atom));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(Rss));
        }

        public override bool CanReadType(Type type)
        {
            return typeof (BlogEntryViewModel) == type;
        }

        public override bool CanWriteType(Type type)
        {
            return typeof (IEnumerable<BlogEntryViewModel>) == type || typeof (BlogEntryViewModel) == type;
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            //TODO: Get this fuckin'  URL from the fuckin' content parametter :@!!
            //TODO: The same prob w/ Message Handler :c
            var host = "http://dev.mazblog.net";
            if (CanWriteType(type))
            {
                var feed = BuildSyndicationFeed(value, writeStream, content.Headers.ContentType.MediaType, host);
                return Task.FromResult(feed);
            }
            return Task.FromResult<object>(null);
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            if (type != typeof (BlogEntryViewModel)) return Task.FromResult<object>(null);
            string xml;
            using (var sr = new StreamReader(readStream))
            {
                xml = sr.ReadToEnd();
                sr.Close();
            }
            var xDocument = XDocument.Parse(xml);
            var formatter = new Atom10ItemFormatter();
            formatter.ReadFrom(xDocument.CreateReader());
            var item = formatter.Item;
            XNamespace aw = "http://www.w3.org/2007/app";
            var drafNode = xDocument.Descendants().FirstOrDefault(x => x.Name.NamespaceName == aw.NamespaceName);
            var isDraft = drafNode != null &&
                          string.Equals(drafNode.Value, "yes", StringComparison.InvariantCultureIgnoreCase);
            var post = new BlogEntryViewModel
                       {
                           Title = item.Title.Text,
                           Categories = item.Categories.Select(x => x.Name),
                           Content = ((TextSyndicationContent) item.Content).Text,
                           PublishDate = item.PublishDate.DateTime,
                           IsDraft = isDraft
                       };
            return Task.FromResult<object>(post);
        }

        private static XmlWriter BuildSyndicationFeed(object models, Stream stream, string contenttype, string host)
        {
            var items = new List<SyndicationItem>();
            var entries = models as IEnumerable<BlogEntryViewModel>;
            if (entries != null) items.AddRange(entries.Select(blogEntry => BuildSyndicationItem(blogEntry, host)));
            else items.Add(BuildSyndicationItem((BlogEntryViewModel) models, host));
            var feed = new SyndicationFeed
                       {
                           Title = new TextSyndicationContent(BlogSettings.FeedTitle),
                           Items = items
                       };
            using (var writer = XmlWriter.Create(stream))
            {
                if (contenttype == Atom)
                {
                    var atomformatter = new Atom10FeedFormatter(feed);
                    atomformatter.WriteTo(writer);
                }
                else
                {
                    var rssformatter = new Rss20FeedFormatter(feed);
                    rssformatter.WriteTo(writer);
                }
                return writer;
            }
        }

        private static SyndicationItem BuildSyndicationItem(BlogEntryViewModel post, string host)
        {

            var item = new SyndicationItem
                       {
                           Title = new TextSyndicationContent(post.Title),
                           BaseUri = new Uri(host + "/feed"),
                           LastUpdatedTime = post.PublishDate,
                           Content = new TextSyndicationContent(post.Content, TextSyndicationContentKind.Html),
                           Summary = new TextSyndicationContent(post.Summary, TextSyndicationContentKind.Html),
                           PublishDate = post.PublishDate,
                           Id = post.Url
                       };
            item.Authors.Add(new SyndicationPerson {Name = BlogSettings.Author});
            item.Links.Add(new SyndicationLink(new Uri(post.Url)));
            return item;
        }
    }
}
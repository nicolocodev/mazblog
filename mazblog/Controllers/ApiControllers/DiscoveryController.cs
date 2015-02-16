using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;
using System.Web.Http;
using System.Xml;
using mazblog.Filters;
using mazblog.Models;

namespace mazblog.Controllers.ApiControllers
{
    [BasicAuth]
    public class DiscoveryController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var serviceDocument = new ServiceDocument();
            var workSpace = new Workspace
            {
                Title = new TextSyndicationContent(BlogSettings.BlogTitle),
                BaseUri = new Uri(Request.RequestUri.GetLeftPart(UriPartial.Authority)),
            };
            var posts = new ResourceCollectionInfo(BlogSettings.PostsCollectionName, new Uri(Url.Link("DefaultApi", new { controller = "Blog" })));
            posts.Accepts.Add("application/atom+xml;type=entry");

            var categoriesUri = new Uri(Url.Link("DefaultApi", new { controller = "Category", format = "atomcat" }));
            var categories = new ReferencedCategoriesDocument(categoriesUri);
            posts.Categories.Add(categories);

            var images = new ResourceCollectionInfo(BlogSettings.ImagesCollectionName, new Uri(Url.Link("DefaultApi", new { controller = "Image" })));
            images.Accepts.Add("image/png");
            images.Accepts.Add("image/jpeg");
            images.Accepts.Add("image/gif");

            workSpace.Collections.Add(posts);
            workSpace.Collections.Add(images);

            serviceDocument.Workspaces.Add(workSpace);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var formatter = new AtomPub10ServiceDocumentFormatter(serviceDocument);
            var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream)) formatter.WriteTo(writer);
            stream.Position = 0;
            var content = new StreamContent(stream);
            response.Content = content;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/atomsvc+xml");
            return response;
        }
    }
}
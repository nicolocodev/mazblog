using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using mazblog.Formatters;
using mazblog.Models;
using NUnit.Framework;

namespace Mazblog.Tests
{
    [TestFixture]
    public class AtomImageFormatterTest
    {
        [Test]
        public void ShouldSupportAtomMediaType()
        {
            var formatter = new AtomImageFormatter();
            Assert.True(formatter.SupportedMediaTypes.Any(x=>x.MediaType == "application/atom+xml"));
        }

        [Test]
        public void ShoulNotReadAnyType()
        {
            var formatter = new AtomImageFormatter();
            Assert.False(formatter.CanReadType(typeof(object)));
        }

        [Test]
        public void ShouldWriteBlogImageType()
        {
            var formatter = new AtomImageFormatter();
            Assert.True(formatter.CanWriteType(typeof(BlogImage)));
        }

        [Test]
        public void ShouldSerializeAsAtom()
        {
            var formatter = new AtomImageFormatter();
            var blogImage = new BlogImage()
            {
                Title = "Image",
                BytesImage = null,
                Url = "http://dev.mazblog.net/image.png",
                PublishDate = DateTime.Now,
                ContentType = "image/png",
                FileName = "Image.png"
            };

            var stream = new MemoryStream();
            var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/atom+xml");
            Task task = formatter.WriteToStreamAsync(typeof(BlogImage), blogImage, stream, content, new FakeTransport());
            task.Wait();
            stream.Seek(0, SeekOrigin.Begin);
            var atomFormatter = new Atom10ItemFormatter();
            atomFormatter.ReadFrom(XmlReader.Create(stream));
            Assert.NotNull(atomFormatter.Item);
        }
    }
}

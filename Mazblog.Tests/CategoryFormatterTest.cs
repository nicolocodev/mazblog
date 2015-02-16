using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using mazblog.Formatters;
using mazblog.ViewModels;
using NUnit.Framework;

namespace Mazblog.Tests
{
    [TestFixture]
    public class CategoryFormatterTest
    {
        [Test]
        public void ShouldSupportAtom()
        {
            var formatter = new CategoryFormatter();
            Assert.True(formatter.SupportedMediaTypes.Any(s => s.MediaType == "application/atom+xml"));
        }
        [Test]
        public void ShouldNotReadAnyType()
        {
            var formatter = new CategoryFormatter();
            var canRead = formatter.CanReadType(typeof(object));
            Assert.False(canRead);
        }

        [Test]
        public void ShouldWriteCategoryViewModelType()
        {
            var formatter = new CategoryFormatter();
            var canWrite = formatter.CanWriteType(typeof(CategoryViewModel));
            Assert.True(canWrite);
        }

        [Test]
        [Ignore]
        public void ShouldSerializeAsAtom()
        {
            var formatter = new CategoryFormatter();
            var blogEntryViewModel = new CategoryViewModel
                                     {
                                         Label = "Test",
                                         Name = "Test"
                                     };
            var stream = new MemoryStream();
            var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/atom+xml");
            Task task = formatter.WriteToStreamAsync(typeof(CategoryViewModel), blogEntryViewModel, stream, content, new FakeTransport());
            task.Wait();
            stream.Seek(0, SeekOrigin.Begin);
            var atomDoc = new InlineCategoriesDocument();
            var atomFormatter = new AtomPub10CategoriesDocumentFormatter(atomDoc);
            atomFormatter.ReadFrom(XmlReader.Create(stream));

            //TODO: Find a way to build the atomDoc obj and assert this test :'(
            Assert.AreEqual(atomDoc.Categories.Count, 1);
        }
    }
}
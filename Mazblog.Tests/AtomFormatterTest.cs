using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using mazblog.Formatters;
using mazblog.ViewModels;
using NUnit.Framework;

namespace Mazblog.Tests
{
    [TestFixture]
    public class AtomFormatterTest
    {
        [Test]
        public void ShouldSupportAtom()
        {
            var formatter = new AtomFormatter();
            Assert.True(formatter.SupportedMediaTypes.Any(s => s.MediaType == "application/atom+xml"));
        }

        [Test]
        public void ShouldSupportRss()
        {
            var formatter = new AtomFormatter();
            Assert.True(formatter.SupportedMediaTypes.Any(s => s.MediaType == "application/rss+xml"));
        }

        [Test]
        public void ShouldReadBlogEntryViewModelType()
        {
            var formatter = new AtomFormatter();
            var canRead = formatter.CanReadType(typeof (BlogEntryViewModel));
            Assert.True(canRead);
        }

        [Test]
        public void ShouldWriteBlogEntryViewModelType()
        {
            var formatter = new AtomFormatter();
            var canWrite = formatter.CanWriteType(typeof (BlogEntryViewModel));
            Assert.True(canWrite);
        }

        [TestCase(
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<atom:entry xmlns:atom=\"http://www.w3.org/2005/Atom\">" +
            "<atom:id>urn:uuid:873eb355-b486-4fc1-b80f-09230bdda10b</atom:id>" +
            "<atom:title>Foo</atom:title>" +
            "<atom:content type=\"html\">&lt;p&gt;Bar&lt;/p&gt;</atom:content>" +
            "</atom:entry>")]
        public void ShouldReadAsBlogEntryViewModelFromAtomFormatStream(string doc)
        {
            var formatter = new AtomFormatter();
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(doc));
            var content = new StreamContent(stream);
            var task = formatter.ReadFromStreamAsync(typeof (BlogEntryViewModel), stream, content, null);
            var blogEntryViewModel = task.Result as BlogEntryViewModel;
            Assert.NotNull(blogEntryViewModel);
            Assert.AreEqual(blogEntryViewModel.Title, "Foo");
            Assert.AreEqual(blogEntryViewModel.Content, "<p>Bar</p>");
        }

        [TestCase(
            "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<atom:entry xmlns:atom=\"http://www.w3.org/2005/Atom\">" +
            "<atom:id>urn:uuid:873eb355-b486-4fc1-b80f-09230bdda10b</atom:id>" +
            "<atom:title>Foo</atom:title>" +
            "<atom:content type=\"html\">&lt;p&gt;Bar&lt;/p&gt;</atom:content>" +
            "<app:control xmlns:app=\"http://www.w3.org/2007/app\">" +
            "<app:draft>yes</app:draft>" +
            "</app:control>" +
            "</atom:entry>")]
        public void ShouldSetIsDraftAsTrueWhenCustomAppControlIsPresentAndHaveDraftYesValue(string doc)
        {
            var formatter = new AtomFormatter();
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(doc));
            var content = new StreamContent(stream);
            var task = formatter.ReadFromStreamAsync(typeof (BlogEntryViewModel), stream, content, null);
            var blogEntryViewModel = task.Result as BlogEntryViewModel;
            Assert.NotNull(blogEntryViewModel);
            Assert.IsTrue(blogEntryViewModel.IsDraft);
        }

        [Test(Description = "When you pass an object different from BlogEntryViewModel should return NULL")]
        public void ShouldReadAsNullFromAtomFormatStream()
        {
            var formatter = new AtomFormatter();
            var task = formatter.ReadFromStreamAsync(typeof (string), null, null, null);
            var blogEntryViewModel = task.Result as BlogEntryViewModel;
            Assert.IsNull(blogEntryViewModel);
        }

        [Test]
        public void ShouldSerializeAsAtom()
        {
            var formatter = new AtomFormatter();
            var blogEntryViewModel = new BlogEntryViewModel
                                     {
                                         Title = "Foo",
                                         Content = "<p>Bar</p>",
                                         Url = "http://dev.mazblog.net/foo",
                                         PublishDate = DateTime.Now
                                     };
            var stream = new MemoryStream();
            var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/atom+xml");
            Task task = formatter.WriteToStreamAsync(typeof (BlogEntryViewModel), blogEntryViewModel, stream, content,
                new FakeTransport());
            task.Wait();
            stream.Seek(0, SeekOrigin.Begin);
            var atomFormatter = new Atom10FeedFormatter();
            atomFormatter.ReadFrom(XmlReader.Create(stream));
            Assert.AreEqual(1, atomFormatter.Feed.Items.Count());
        }
    }
}
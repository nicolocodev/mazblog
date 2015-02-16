using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Resources;
using mazblog.Formatters;
using mazblog.Models;
using NUnit.Framework;

namespace Mazblog.Tests
{
    [TestFixture]
    public class ImageFormatterTest
    {
        [TestCase("image/png", "image/jpeg", "image/gif")]
        public void ShouldSupportImageMediaType(string png, string jpeg, string gif)
        {
            var formatter = new ImageFormatter();
            Assert.True(formatter.SupportedMediaTypes.Any(s => s.MediaType == png));
            Assert.True(formatter.SupportedMediaTypes.Any(s => s.MediaType == jpeg));
            Assert.True(formatter.SupportedMediaTypes.Any(s => s.MediaType == gif));
        }

        [Test]
        public void ShouldReadBlogImageType()
        {
            var formatter = new ImageFormatter();
            var canRead = formatter.CanReadType(typeof (BlogImage));
            Assert.True(canRead);
        }

        [Test]
        public void ShouldNotWriteAnyType()
        {
            var formatter = new ImageFormatter();
            var canWrite = formatter.CanWriteType(typeof (object));
            Assert.False(canWrite);
        }

        [Test]
        public void ShouldReadAsBlogImageFromAnImage()
        {
            var rm = new ResourceManager(typeof(TestsResources));
            var image = (Image)rm.GetObject("WinAzure2");
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            stream.Position = 0;

            var formatter = new ImageFormatter();
            var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue("image/png");                
            var task = formatter.ReadFromStreamAsync(typeof(BlogImage), stream, content, null);
            var blogImage = task.Result as BlogImage;
            stream.Position = 0;
            byte[] a = stream.ToArray(); 

            Assert.NotNull(blogImage);
            Assert.IsNotNull(blogImage.BytesImage);
            Assert.AreEqual(a.Length, blogImage.BytesImage.Length);
            Assert.IsNull(blogImage.Url);
            Assert.AreEqual(blogImage.ContentType, "image/png");
        }
    }
}

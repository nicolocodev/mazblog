using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using mazblog.Filters;
using mazblog.Models;
using Microsoft.WindowsAzure.Storage.Blob;

namespace mazblog.Controllers.ApiControllers
{
    public class ImageController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok();
        }

        public IHttpActionResult Get(string id)
        {
            var container = AzureConfig.StorageAccount.CreateCloudBlobClient().GetContainerReference(BlobContainerNames.BlogImages);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(id);
            blockBlob.FetchAttributes();
            var image = new BlogImage
            {
                Title = id,
                ContentType = blockBlob.Properties.ContentType,
                FileName = id,
                Url = blockBlob.Uri.AbsoluteUri,
                PublishDate = DateTime.Now
            };
            return Ok(image);
        }

        [BasicAuth]
        public HttpResponseMessage Post(BlogImage image)
        {
            var container = AzureConfig.StorageAccount.CreateCloudBlobClient().GetContainerReference(BlobContainerNames.BlogImages);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(image.Title);
            blockBlob.Properties.ContentType = image.ContentType;
            using (var stream = new MemoryStream(image.BytesImage)) blockBlob.UploadFromStream(stream);
            image.Url = blockBlob.Uri.AbsoluteUri;
            var response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", new { controller = "image", id = image.Title }));
            return response;
        }
    }
}
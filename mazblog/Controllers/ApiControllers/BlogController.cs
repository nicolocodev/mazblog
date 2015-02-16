using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using mazblog.Filters;
using mazblog.Mappers;
using mazblog.Models;
using mazblog.QueryObjects;
using mazblog.ViewModels;
using Microsoft.WindowsAzure.Storage.Table;

namespace mazblog.Controllers.ApiControllers
{
    public class BlogController : ApiController
    {
        private readonly CloudTableClient _tableClient;

        public BlogController()
        {
            _tableClient = AzureConfig.StorageAccount.CreateCloudTableClient();
        }

        public IHttpActionResult GetById(string id)
        {
            var query = new EntryByIdQuery(_tableClient);
            var blogEntry = query.Execute(id);
            if (blogEntry == null) return NotFound();
            var location = Url.Link("Default", new {controller = "Home", action = "Index", id = blogEntry.PartitionKey});
            var viewModel = BlogEntryMapper.MapToViewModel(blogEntry, location);
            return Ok(viewModel);
        }

        public IHttpActionResult GetAllPosts()
        {
            var query = new AllEntriesQuery(_tableClient);
            var blogEntries = query.Execute();
            var viewModel = blogEntries.Select(
                blogEntry => BlogEntryMapper.MapToViewModel(blogEntry,
                        Url.Link("DefaultApi", new { controller = "Blog", id = blogEntry.PartitionKey })));
            return Ok(viewModel);
        }   

        [BasicAuth]
        public IHttpActionResult Post([FromBody]BlogEntryViewModel viewModel)
        {
            var tableClient = AzureConfig.StorageAccount.CreateCloudTableClient();
            var blogTable = tableClient.GetTableReference(TablesName.BlogEntryTable);

            var blogEntry = new BlogEntry(viewModel.Title)
            {
                IsDraft = viewModel.IsDraft,
                Content = viewModel.Content,
                PublishDate = viewModel.PublishDate
            };
            var byDate = new BlogEntry(viewModel.Title, viewModel.PublishDate.ToString("yyyy-M-d"));
            var allEntries = new BlogEntry(viewModel.Title, PartitionKeys.AllBlogEntries)
            {
                IsDraft = viewModel.IsDraft,
                Content = viewModel.Content,
                PublishDate = viewModel.PublishDate
            };

            var byCategories = viewModel.Categories.Select(category => new BlogEntry(viewModel.Title, category));

            CreateCategories(viewModel.Categories.ToArray(), tableClient);

            var results = new List<TableResult>
                          {
                              blogTable.Execute(TableOperation.Insert(blogEntry)),
                              blogTable.Execute(TableOperation.Insert(byDate)),
                              blogTable.Execute(TableOperation.Insert(allEntries))
                          };

            results.AddRange(byCategories.Select(byCategory => blogTable.Execute(TableOperation.Insert(byCategory))));

            var error = results.FirstOrDefault(x => x.HttpStatusCode > 399 && x.HttpStatusCode < 600);
            if (error != null) return StatusCode((HttpStatusCode)error.HttpStatusCode);
            viewModel.Url = Url.Link("DefaultApi", new { controller = "Blog", id = blogEntry.PartitionKey });
            return Created(viewModel.Url, viewModel);
        }

        private static void CreateCategories(string[] categories, CloudTableClient tableClient)
        {
            if (!categories.Any()) return;
            var categoryTable = tableClient.GetTableReference(TablesName.CategoryTable);
            var batch = new TableBatchOperation();
            foreach (var category in categories)
            {
                var bCategory = new Category(category);
                batch.Insert(bCategory);
            }
            //TODO:Search for error
            categoryTable.ExecuteBatch(batch);
        }
    }
}
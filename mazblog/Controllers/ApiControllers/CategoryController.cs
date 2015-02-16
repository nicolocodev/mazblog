using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using mazblog.Models;
using mazblog.ViewModels;
using Microsoft.WindowsAzure.Storage.Table;

namespace mazblog.Controllers.ApiControllers
{
    public class CategoryController : ApiController
    {
        public IHttpActionResult Get()
        {
            var tableClient = AzureConfig.StorageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(TablesName.CategoryTable);
            var query = new TableQuery<Category>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKeys.Categories));
            var viewModel = table.ExecuteQuery(query).Select(x => new CategoryViewModel {Label = x.Label, Name = x.Name});
            return Ok(viewModel);
        }

        public async Task<IHttpActionResult> Post([FromBody]CategoryViewModel viewModel)
        {
            var category = new Category(viewModel.Name);
            var tableClient = AzureConfig.StorageAccount.CreateCloudTableClient();
            var categoryTable = tableClient.GetTableReference(TablesName.CategoryTable);
            var insert = TableOperation.InsertOrReplace(category);
            var result = await categoryTable.ExecuteAsync(insert);
            if (result.HttpStatusCode == 200) return Created(Url.Link("DefaultApi", new { controller="Category", id = category.Name }), viewModel);
            return InternalServerError();
        }
    }
}
using System.Web.Mvc;
using mazblog.Mappers;
using mazblog.QueryObjects;

namespace mazblog.Controllers
{
    public class ArticleController : Controller
    {
        public ActionResult Index(string id)
        {
            var tableClient = AzureConfig.StorageAccount.CreateCloudTableClient();
            var query = new EntryByIdQuery(tableClient);
            var blogEntry = query.Execute(id);
            if (blogEntry == null) return HttpNotFound();
            var viewModel = BlogEntryMapper.MapToViewModel(blogEntry);
            return View(viewModel);
        }
    }
}
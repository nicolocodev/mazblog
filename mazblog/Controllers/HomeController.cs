using System.Web.Mvc;
using mazblog.Mappers;
using mazblog.Models;
using mazblog.QueryObjects;

namespace mazblog.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = BlogSettings.BlogTitle;
            var tableClient = AzureConfig.StorageAccount.CreateCloudTableClient();
            var query = new AllEntriesButDraftsQuery(tableClient);
            var viewModel = BlogEntryMapper.MapToViewModel(query.Excecute());
            return View(viewModel);
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using mazblog.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace mazblog
{
    public class AzureTableLogging : ExceptionLogger
    {
        public override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            var log = new ErrorLog
                      {
                          Message = string.Format("Request:{0};Error:{1};", context.Request, context.Exception)
                      };
            CloudTableClient tableClient = AzureConfig.StorageAccount.CreateCloudTableClient();
            CloudTable errorTable = tableClient.GetTableReference(TablesName.ErrorLog);

            TableOperation insertOperation = TableOperation.Insert(log);
            errorTable.Execute(insertOperation);
            return Task.FromResult<object>(null);
        }
    }
}
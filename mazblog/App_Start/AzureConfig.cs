using System.Configuration;
using mazblog.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace mazblog
{
    public class AzureConfig
    {
        private static CloudStorageAccount _cloudStorageAccount;

        public static CloudStorageAccount StorageAccount
        {
            get
            {
                return _cloudStorageAccount ?? (_cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString));
            }
        }

        public static void ConfigureTables()
        {
            CloudTableClient client = StorageAccount.CreateCloudTableClient();
            CreateTablesFromModel(client);
        }

        public static void ConfigureBlob()
        {
            var blobClient = StorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(BlobContainerNames.BlogImages);
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
        }

        private static void CreateTablesFromModel(CloudTableClient cloudTableClient)
        {
            cloudTableClient.GetTableReference(TablesName.BlogEntryTable).CreateIfNotExists();
            cloudTableClient.GetTableReference(TablesName.CategoryTable).CreateIfNotExists();
            cloudTableClient.GetTableReference(TablesName.ErrorLog).CreateIfNotExists();
        }
    }
}
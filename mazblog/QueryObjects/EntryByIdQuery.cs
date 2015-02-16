using System;
using mazblog.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace mazblog.QueryObjects
{
    public class EntryByIdQuery
    {
        private readonly CloudTableClient _tableClient;

        public EntryByIdQuery(CloudTableClient tableClient)
        {
            if (tableClient == null) throw new ArgumentNullException("tableClient");
            _tableClient = tableClient;
        }

        public BlogEntry Execute(string id)
        {
            if (id == null) throw new ArgumentNullException("id");
            var table = _tableClient.GetTableReference(TablesName.BlogEntryTable);
            var retrieveOperation = TableOperation.Retrieve<BlogEntry>(id, id);
            var retrievedResult = table.Execute(retrieveOperation);
            return retrievedResult.Result as BlogEntry;
        }
    }
}
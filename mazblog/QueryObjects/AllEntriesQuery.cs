using System.Collections.Generic;
using mazblog.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace mazblog.QueryObjects
{
    public class AllEntriesQuery
    {
        private readonly CloudTableClient _tableClient;
        public AllEntriesQuery(CloudTableClient tableClient)
        {
            _tableClient = tableClient;
        }

        public IEnumerable<BlogEntry> Execute()
        {
            var table = _tableClient.GetTableReference(TablesName.BlogEntryTable);
            var query = new TableQuery<BlogEntry>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKeys.AllBlogEntries));
            var blogEntries = table.ExecuteQuery(query);
            return blogEntries;
        }
    }
}
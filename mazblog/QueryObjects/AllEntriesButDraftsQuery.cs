using System.Collections.Generic;
using mazblog.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace mazblog.QueryObjects
{
    public class AllEntriesButDraftsQuery
    {
        private readonly CloudTableClient _tableClient;

        public AllEntriesButDraftsQuery(CloudTableClient tableClient)
        {
            _tableClient = tableClient;
        }

        public IEnumerable<BlogEntry> Excecute()
        {
            var table = _tableClient.GetTableReference(TablesName.BlogEntryTable);
            var query = new TableQuery<BlogEntry>().Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKeys.AllBlogEntries),
                TableOperators.And,
                TableQuery.GenerateFilterConditionForBool("IsDraft", QueryComparisons.Equal, false)));
            return table.ExecuteQuery(query);
        }
    }
}
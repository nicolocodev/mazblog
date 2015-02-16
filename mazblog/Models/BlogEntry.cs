using System;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage.Table;

namespace mazblog.Models
{
    public class BlogEntry : TableEntity
    {

        public BlogEntry() { } //Azure

        public BlogEntry(string title, string partitionKey = null)
        {
            var url = Regex.Replace(Regex.Replace(title, @"[^a-zA-z0-9 ]+", "").Trim(), @"\s+", "-");
            PartitionKey = partitionKey ?? url;
            RowKey = url;
            Title = title;
        }
        public string Title { get; set; }
        //public string Summary { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsDraft { get; set; }
    }
}
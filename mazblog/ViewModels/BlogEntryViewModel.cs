using System;
using System.Collections.Generic;

namespace mazblog.ViewModels
{
    public class BlogEntryViewModel
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsDraft { get; set; }
        public string Url { get; set; }
        public IEnumerable<string> Categories { get; set; }
    }
}
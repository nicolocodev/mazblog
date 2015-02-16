using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mazblog.Models;
using mazblog.ViewModels;

namespace mazblog.Mappers
{
    public static class BlogEntryMapper
    {
        public static IEnumerable<BlogEntryViewModel> MapToViewModel(IEnumerable<BlogEntry> blogEntries)
        {
            return blogEntries.Select(blogEntry => MapToViewModel(blogEntry));
        }

        public static BlogEntryViewModel MapToViewModel(BlogEntry blogEntry, string customUrl = null)
        {
            return new BlogEntryViewModel
            {
                IsDraft = blogEntry.IsDraft,
                Content = blogEntry.Content,
                PublishDate = blogEntry.PublishDate,
                Title = blogEntry.Title,
                Url = customUrl ?? blogEntry.RowKey
            };
        }
    }
}
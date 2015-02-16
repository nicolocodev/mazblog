using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using mazblog.ViewModels;

namespace mazblog.Formatters
{
    public class CategoryFormatter : MediaTypeFormatter
    {
        private const string AtomCategoryMediaType = "application/atom+xml";

        public CategoryFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(AtomCategoryMediaType));
            this.AddQueryStringMapping("format", "atomcat", AtomCategoryMediaType);
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return typeof(IEnumerable<CategoryViewModel>) == type || typeof(CategoryViewModel) == type;
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
        {
            var categories = value as IEnumerable<CategoryViewModel>;
            var atomCat = new InlineCategoriesDocument();
            if (categories == null)
            {
              var category = value as CategoryViewModel;
                if (category == null) return Task.FromResult<object>(null);
                atomCat.Categories.Add(new SyndicationCategory(category.Name));
            }
            else foreach (var category in categories) atomCat.Categories.Add(new SyndicationCategory(category.Name));
            var formatter = new AtomPub10CategoriesDocumentFormatter(atomCat);
            using (var writer = XmlWriter.Create(writeStream))
            {
                formatter.WriteTo(writer);
                return Task.FromResult(writer);
            }
        }
    }
}
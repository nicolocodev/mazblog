using Microsoft.WindowsAzure.Storage.Table;

namespace mazblog.Models
{
    public class Category : TableEntity
    {
        public Category() { }//Azure

        public Category(string name)
        {
            PartitionKey = "_blogcategory";
            RowKey = name;
            Name = Name;
            Label = Label;
        }
        public string Name { get; set; }
        public string Label { get; set; }
    }
}
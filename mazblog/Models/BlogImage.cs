using System;

namespace mazblog.Models
{
    public class BlogImage
    {
        public string Title { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] BytesImage { get; set; }
        public DateTime PublishDate { get; set; }
        public string Url { get; set; }
    }
}
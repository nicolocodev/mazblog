using System;
using System.Globalization;
using Microsoft.WindowsAzure.Storage.Table;

namespace mazblog.Models
{
    public class ErrorLog : TableEntity
    {
        public ErrorLog() : base("blogexception", DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture))
        {
        }

        public string Message { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudFlare.BulkImport
{
    public class Account
    {
        public string Id { get; set; }

        public Account(string id)
        {
            Id = id;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CloudFlare.BulkImport
{
    public class CreateZoneRequest
    {
        public string Name { get; set; }
        public Account Account { get; set; }

        [JsonPropertyName("jump_start")]
        public bool JumpStart { get; set; }

        public string Type { get; set; }

        public CreateZoneRequest(string name, Account account, bool jumpStart, string type)
        {
            Name = name;
            Account = account;
            JumpStart = jumpStart;
            Type = type;
        }
    }
}

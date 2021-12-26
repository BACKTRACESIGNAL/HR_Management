using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Management.Model
{
    [BsonIgnoreExtraElements]
    public class Account
    {
        public String AccountCode { get; set; }
        public String AccountName { get; set; }
        public String Password { get; set; }
        public String CreatedDateTime { get; set; }
        public String UpdatedDateTime { get; set; }
        public String AccountType { get; set; }
        public List<String> PermissionList { get; set; }

        public String EmailName { get; set; }
    }
}

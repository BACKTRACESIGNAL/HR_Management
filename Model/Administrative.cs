using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Management.Model
{
    [BsonIgnoreExtraElements]
    public class Administrative
    {
        public int administrative_type { get; set; }
        public int province_code { get; set; }
        public string province_name { get; set; }
        public int district_code { get; set; }
        public string district_name { get; set; }
        public int ward_code { get; set; }
        public string ward_name { get; set; }
    }
}

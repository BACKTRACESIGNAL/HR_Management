using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Management.Model
{
    [BsonIgnoreExtraElements]
    public class AdminisDistrict
    {
        public Nullable<int> administrative_type { get; set; }
        public string province_name { get; set; }
        public string district_name { get; set; }

        public override string ToString()
        {
            return this.district_name;
        }
    }
}

using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Management.Model
{
    [BsonIgnoreExtraElements]
    public class EmployeeInfoModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool   Gender { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string DetailAddress { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }
    }
}

using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace HR_Management.Model
{

    [BsonIgnoreExtraElements]
    public partial class BackHrModel
    {
        [BsonIgnoreIfNull]
        public Account Account { get; set; }
        [BsonIgnoreIfNull]
        public AdminisDistrict AdminisDistrict { get; set; }
        [BsonIgnoreIfNull]
        public AdminisProvince AdminisProvince { get; set; }
        [BsonIgnoreIfNull]
        public AdminisWard AdminisWard { get; set; }
        [BsonIgnoreIfNull]
        public Department Department { get; set; }
        [BsonIgnoreIfNull]
        public EmployeeInfo EmployeeInfo { get; set; }
        [BsonIgnoreIfNull]
        public Position Position { get; set; }
    }

    [BsonIgnoreExtraElements]
    public partial class Account
    {
        [BsonIgnoreIfNull]
        public string AccountName { get; set; }
        [BsonIgnoreIfNull]
        public string CreatedBy { get; set; }
        [BsonIgnoreIfNull]
        public string CreatedDateTime { get; set; }
        [BsonIgnoreIfNull]
        public string Password { get; set; }
        [BsonIgnoreIfNull]
        public List<Permission> Permissions { get; set; }
        public override string ToString()
        {
            return this.AccountName;
        }
    }

    [BsonIgnoreExtraElements]
    public partial class Permission
    {
        [BsonIgnoreIfNull]
        public long? PermissionCode { get; set; }
        [BsonIgnoreIfNull]
        public string PermissionName { get; set; }
        [BsonIgnoreIfNull]
        public List<long> TargetCodes { get; set; }
        public override string ToString()
        {
            return this.PermissionName;
        }
    }

    [BsonIgnoreExtraElements]
    public partial class AdminisDistrict
    {
        [BsonIgnoreIfNull]
        public long? AdminisType { get; set; }
        [BsonIgnoreIfNull]
        public string DistrictName { get; set; }
        [BsonIgnoreIfNull]
        public string ProvinceName { get; set; }
        public override string ToString()
        {
            return this.DistrictName;
        }
    }

    [BsonIgnoreExtraElements]
    public partial class AdminisProvince
    {
        [BsonIgnoreIfNull]
        public long? AdminisType { get; set; }
        [BsonIgnoreIfNull]
        public string ProvinceName { get; set; }
        public override string ToString()
        {
            return this.ProvinceName;
        }
    }

    [BsonIgnoreExtraElements]
    public partial class AdminisWard
    {
        [BsonIgnoreIfNull]
        public long? AdminisType { get; set; }
        [BsonIgnoreIfNull]
        public string DistrictName { get; set; }
        [BsonIgnoreIfNull]
        public string ProvinceName { get; set; }
        [BsonIgnoreIfNull]
        public string WardName { get; set; }
        public override string ToString()
        {
            return this.WardName;
        }
    }

    [BsonIgnoreExtraElements]
    public partial class Department
    {
        [BsonIgnoreIfNull]
        public string DepartmentCode { get; set; }
        [BsonIgnoreIfNull]
        public string DepartmentName { get; set; }
        [BsonIgnoreIfNull]
        public List<EmployeeInfo> EmployeeInfos { get; set; }
        [BsonIgnoreIfNull]
        public List<Position> Positions { get; set; }
        public override string ToString()
        {
            return this.DepartmentName;
        }
    }

    [BsonIgnoreExtraElements]
    public partial class EmployeeInfo
    {
        [BsonIgnoreIfNull]
        public string DetailAddress { get; set; }
        [BsonIgnoreIfNull]
        public string DistrictName { get; set; }
        [BsonIgnoreIfNull]
        public string Email { get; set; }
        [BsonIgnoreIfNull]
        public string EmployeeCode { get; set; }
        [BsonIgnoreIfNull]
        public string FullName { get; set; }

        /// <summary>
        /// 0. Male, 1. Female, 2. Undefined
        /// </summary>
        [BsonIgnoreIfNull]
        public long? Gender { get; set; }

        [BsonIgnoreIfNull]
        public string Phone { get; set; }
        [BsonIgnoreIfNull]
        public string PositionCode { get; set; }
        [BsonIgnoreIfNull]
        public string ProvinceName { get; set; }
        [BsonIgnoreIfNull]
        public string WardName { get; set; }
        public override string ToString()
        {
            return this.FullName;
        }
    }

    [BsonIgnoreExtraElements]
    public partial class Position
    {
        [BsonIgnoreIfNull]
        public long? Current { get; set; }
        [BsonIgnoreIfNull]
        public long? Max { get; set; }
        [BsonIgnoreIfNull]
        public string PositionCode { get; set; }
        [BsonIgnoreIfNull]
        public string PositionName { get; set; }
        public override string ToString()
        {
            return this.PositionName;
        }
    }
}

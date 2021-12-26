using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HR_Management.HR_Libs.MongoDefine;

namespace HR_Management.HR_Libs
{
    public class MongoDefine
    {
        public enum DATABASE
        {
            [Description("HR_DATA")]
            HR_DATA_DB
        }

        public enum COLLECTION
        {
            [Description("HR_EMPLOYEE_INFO")]
            HR_EMPOYEE_INFO_COLLECTION,
            [Description("ADMINISTRATIVE_VN")]
            HR_ADMINISTRATIVE_VN_COLLECTION
        }

    }
    public static class EnumExtensions
    {
        public static string ToDescriptionString(this DATABASE val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }


        public static string ToDescriptionString(this COLLECTION val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}

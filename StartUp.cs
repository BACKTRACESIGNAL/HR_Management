using HR_Management.HR_Libs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Management
{
    public class StartUp
    {
        public StartUp()
        {

        }

        public bool Load()
        {
            bool ok = true;
            MongodbRequest.Instance().Load("mongodb+srv://balebom:matkhau.273@cluster0.thcuf.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");

            return ok;
        }
    }
}

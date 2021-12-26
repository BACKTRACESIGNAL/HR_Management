using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HR_Management.HR_Libs.MongoDefine;

namespace HR_Management.HR_Libs
{
    public class MongodbRequest
    {
        private MongodbRequest()
        {

        }

        private static MongodbRequest m_instance;

        private static readonly object m_lock = new object();

        private String m_connectionString;

        public static MongodbRequest Instance()
        {
            if (m_instance == null)
            {
                lock (m_lock)
                {
                    if (m_instance == null)
                    {
                        m_instance = new MongodbRequest();
                    }
                }
            }

            return m_instance;
        }

        public void Load(String connectionString)
        {
            this.m_connectionString = connectionString;
        }

        public MongoCRUD StartDbSession(DATABASE database)
        {
            var client = new MongoClient(this.m_connectionString);
            MongoCRUD crud = new MongoCRUD(client.GetDatabase(database.ToDescriptionString()));
            return crud;
        }
    }
}

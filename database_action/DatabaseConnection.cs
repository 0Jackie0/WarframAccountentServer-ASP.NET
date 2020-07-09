using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warframeaccountant.database_action
{
    public class DatabaseConnection
    {
        private static DatabaseConnection instance = null;
        private static readonly object padlock = new object();

        private static String connectionString { get; set; }

        private DatabaseConnection(String connectionString)
        {
            DatabaseConnection.connectionString = connectionString;
        }

        public static DatabaseConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            throw new Exception();
                        }
                    }
                }
                return instance;
            }
        }

        public static DatabaseConnection getInstance (String connectionString)
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DatabaseConnection(connectionString);
                    }
                }
            }
            return instance;
        }

        

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warframeaccountant.domain;

namespace Warframeaccountant.database_action
{
    public class TypeRepo
    {
        private DatabaseConnection dbConnection = DatabaseConnection.Instance;

        public List<domain.Type> getAllItem()
        {
            List<domain.Type> typeList = new List<domain.Type>();

            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from type", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        typeList.Add(new domain.Type()
                        {
                            typeId = Convert.ToInt32(reader["typeId"]),
                            typeName = reader["typeName"].ToString()
                        });
                    }
                }
            }

            return typeList;
        }
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warframeaccountant.domain;

namespace Warframeaccountant.database_action
{
    public class ItemRepo
    {
        private DatabaseConnection dbConnection = DatabaseConnection.Instance;

        public List<Item> getAllItem()
        {
            List<Item> itemList = new List<Item>();

            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from item", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        itemList.Add(new Item()
                        {
                            itemId = Convert.ToInt32(reader["itemId"]),
                            name = reader["name"].ToString(),
                            type = Convert.ToInt32(reader["type"]),
                            imageString = reader["imageString"].ToString(),
                            quantity = Convert.ToInt32(reader["quantity"]),
                            bprice = Convert.ToInt32(reader["bprice"]),
                            eprice = Convert.ToInt32(reader["eprice"])
                        });
                    }
                }
            }

            return itemList;
        }
    
        public List<Item> getAllOrderItemList (String order)
        {
            List<Item> itemList = new List<Item>();

            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();

                String sql = $"select * from item ORDER BY {order} asc";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        itemList.Add(new Item()
                        {
                            itemId = Convert.ToInt32(reader["itemId"]),
                            name = reader["name"].ToString(),
                            type = Convert.ToInt32(reader["type"]),
                            imageString = reader["imageString"].ToString(),
                            quantity = Convert.ToInt32(reader["quantity"]),
                            bprice = Convert.ToInt32(reader["bprice"]),
                            eprice = Convert.ToInt32(reader["eprice"])
                        });
                    }
                }
            }
            return itemList;
        }

        public List<Item> getFilterOrderItemList(String order, int type)
        {
            List<Item> itemList = new List<Item>();

            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
               /* MySqlCommand cmd = new MySqlCommand($"select * from item WHERE type={type} ORDER BY {order} asc", conn);*/

                String sql = $"select * from item WHERE type=@type ORDER BY {order} asc";
                cmd.Connection = conn;
                cmd.CommandText = sql;
                
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Prepare();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        itemList.Add(new Item()
                        {
                            itemId = Convert.ToInt32(reader["itemId"]),
                            name = reader["name"].ToString(),
                            type = Convert.ToInt32(reader["type"]),
                            imageString = reader["imageString"].ToString(),
                            quantity = Convert.ToInt32(reader["quantity"]),
                            bprice = Convert.ToInt32(reader["bprice"]),
                            eprice = Convert.ToInt32(reader["eprice"])
                        });
                    }
                }
            }
            return itemList;
        }

        public bool addOne (Item newItem)
        {
            try
            {
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();

                    String sql = $"INSERT INTO item (name, imageString, quantity, type, bprice, eprice) VALUES (@name, @imageString, @quantity, @type, @bprice, @eprice)";
                    cmd.Connection = conn;
                    cmd.CommandText = sql;

                    cmd.Parameters.AddWithValue("@name", newItem.name);
                    cmd.Parameters.AddWithValue("@imageString", newItem.imageString);
                    cmd.Parameters.AddWithValue("@quantity", newItem.quantity);
                    cmd.Parameters.AddWithValue("@type", newItem.type);
                    cmd.Parameters.AddWithValue("@bprice", newItem.bprice);
                    cmd.Parameters.AddWithValue("@eprice", newItem.eprice);

                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            
            return true;
        }

        public bool updateOne (Item targetItem)
        {
            try
            {
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();

                    String sql = "UPDATE item SET name=@name, imageString=@imageString, quantity=@quantity, type=@type, bprice=@bprice, eprice=@eprice WHERE itemId=@itemId";
                    cmd.Connection = conn;
                    cmd.CommandText = sql;

                    cmd.Parameters.AddWithValue("@itemId", targetItem.itemId);
                    cmd.Parameters.AddWithValue("@name", targetItem.name);
                    cmd.Parameters.AddWithValue("@imageString", targetItem.imageString);
                    cmd.Parameters.AddWithValue("@quantity", targetItem.quantity);
                    cmd.Parameters.AddWithValue("@type", targetItem.type);
                    cmd.Parameters.AddWithValue("@bprice", targetItem.bprice);
                    cmd.Parameters.AddWithValue("@eprice", targetItem.eprice);

                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return false;
            }


            return true;
        }

        public Item removeOne(int itemId)
        {
            Item targetItem = null;
            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();

                String sql = "select * from item WHERE itemId=@itemId";

                MySqlCommand cmd = new MySqlCommand();

                cmd.CommandText = sql;
                cmd.Connection = conn;

                cmd.Parameters.AddWithValue("@itemId", itemId);
                cmd.Prepare();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        targetItem = new Item()
                        {
                            itemId = Convert.ToInt32(reader["itemId"]),
                            name = reader["name"].ToString(),
                            type = Convert.ToInt32(reader["type"]),
                            imageString = reader["imageString"].ToString(),
                            quantity = Convert.ToInt32(reader["quantity"]),
                            bprice = Convert.ToInt32(reader["bprice"]),
                            eprice = Convert.ToInt32(reader["eprice"])
                        };
                    }
                }
            }

            using (MySqlConnection conn = dbConnection.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();

                String sql = "DELETE FROM item WHERE itemId=@itemId";
                cmd.Connection = conn;
                cmd.CommandText = sql;

                cmd.Parameters.AddWithValue("@itemId", itemId);

                cmd.Prepare();

                cmd.ExecuteNonQuery();
            }

            return targetItem;
        }
    
        public bool changeOneQuantity (int targetId, int amount)
        {
            try
            {
                using (MySqlConnection conn = dbConnection.GetConnection())
                {
                    conn.Open();

                    String sql = "UPDATE item SET quantity=quantity+@amount WHERE itemid=@itemId";

                    MySqlCommand cmd = new MySqlCommand();

                    cmd.CommandText = sql;
                    cmd.Connection = conn;

                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@itemId", targetId);

                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);

                return false;
            }

            return true;
        }
    }
}

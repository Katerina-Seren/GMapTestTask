using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMap
{
    public class UnitService
    {
        private SqlConnection connection;

        public UnitService(string sqlstring)
        {
            connection = new SqlConnection(sqlstring);
            try
            {
                connection.Open();
                Console.WriteLine("Подключение открыто");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("ОШИБКА ПОДКЛЮЧЕНИЯ");
                Console.WriteLine(ex.Message);
            }
        }

        public bool IsConnectionOpen()
        {
            return connection != null && connection.State == ConnectionState.Open;
        }

        public List<Unit> GetAll()
        {
            var units = new List<Unit>();
            if (IsConnectionOpen())
            {
                SqlCommand command = new SqlCommand("SELECT * FROM units", connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var name = reader.GetString(1);
                        var lat = reader.GetDouble(2);
                        var lng = reader.GetDouble(3);

                        units.Add(new Unit(id, name, lat, lng));
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                reader.Close();
            }
            return units;
        }

        public void ChangePosition(int id, double lat, double lng)
        {
            if (IsConnectionOpen())
            {
                string query = "UPDATE units SET Lat = @lat, Lng = @lng WHERE id = @id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@lat", SqlDbType.Float).Value = lat;
                    command.Parameters.Add("@lng", SqlDbType.Float).Value = lng;
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var rowsNum = command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }
        }

        public void CloseConnection()
        {
            connection.Close();
        }
    }

}

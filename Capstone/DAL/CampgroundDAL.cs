using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    public class CampgroundDAL
    {
        private string connectionString;
        private const string SQL_GetAllCampgrounds = "SELECT * FROM campground WHERE park_id = @parkId";

        public CampgroundDAL(string dbconnectionString)
        {
            connectionString = dbconnectionString;
        }

        public List<Campground> GetCampgrounds(Park park)
        {
            List<Campground> result = new List<Campground>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(SQL_GetAllCampgrounds, connection);
                    command.Parameters.AddWithValue("@parkId", park.ParkId);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground cg = new Campground();
                        cg.ParkId = Convert.ToInt32(reader["park_id"]);
                        cg.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        cg.CampgroundName = Convert.ToString(reader["name"]);
                        cg.OpenMonth = Convert.ToInt32(reader["open_from_mm"]);
                        cg.CloseMonth = Convert.ToInt32(reader["open_to_mm"]);
                        cg.DailyFee = Convert.ToDecimal(reader["daily_fee"]);


                        result.Add(cg);
                    }
                }
            }
            catch(Exception)
            {
            }

            return result;
        }
    }
}

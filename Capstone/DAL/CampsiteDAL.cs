using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    public class CampsiteDAL
    {
        private string connectionString;
        private const string SQL_GetAllCampsites = "SELECT * FROM site";
       
        public CampsiteDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        
        public List<Campsite> GetAllCampsites()
        {
            List<Campsite> result = new List<Campsite>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(SQL_GetAllCampsites, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Campsite cs = new Campsite();
                        cs.SiteId = Convert.ToInt32(reader["site_id"]);
                        cs.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        cs.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        cs.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        cs.Accessible = Convert.ToBoolean(reader["accessible"]);
                        cs.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
                        cs.Utilities = Convert.ToBoolean(reader["utilities"]);

                        result.Add(cs);
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
 
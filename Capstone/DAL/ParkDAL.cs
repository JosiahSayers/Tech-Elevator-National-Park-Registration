using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ParkDAL
    {
        private string connectionString;
        private const string SQL_GetAllParks = "SELECT * FROM park ORDER BY name";

        public ParkDAL(string dbconnectionString)
        {
            connectionString = dbconnectionString;
        }

        public List<Park> GetAllParks()
        {
            List<Park> result = new List<Park>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(SQL_GetAllParks, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Park p = new Park();
                        p.ParkId = Convert.ToInt32(reader["park_id"]);
                        p.ParkName = Convert.ToString(reader["name"]);
                        p.Location = Convert.ToString(reader["location"]);
                        p.EstablishedDate = Convert.ToDateTime(reader["establish_date"]);
                        p.Area = Convert.ToInt32(reader["area"]);
                        p.AnnualVisitors = Convert.ToInt32(reader["visitors"]);
                        p.Description = Convert.ToString(reader["description"]);


                        result.Add(p);
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



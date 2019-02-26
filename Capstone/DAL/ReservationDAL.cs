using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ReservationDAL
    {
        private string connectionString;
        private const string SQL_GetAllReservations = "SELECT * FROM reservation";
        private const string SQL_CreateReservation = "INSERT INTO reservation (site_id, name, from_date, to_date, create_date) VALUES (@site_id, @name, @from_date, @to_date, @create_date); SELECT CAST(SCOPE_IDENTITY() as int);";
        private const string SQL_ReservationSearch = "SELECT TOP 5 s.site_id, s.site_number, s.max_occupancy, s.accessible, s.max_rv_length, s.utilities FROM site s where s.campground_id = @campground_id AND s.site_id NOT IN (SELECT s.site_id from reservation r JOIN site s on r.site_id = s.site_id WHERE s.campground_id = @campground_id AND r.to_date > @req_from_date AND r.from_date<@req_to_date)";

        public ReservationDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public int CreateReservation(Reservation newReservation)
        {
            int reservationId;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(SQL_CreateReservation, connection);
                    command.Parameters.AddWithValue("@site_id", newReservation.SiteId);
                    command.Parameters.AddWithValue("@name", newReservation.ReservationName);
                    command.Parameters.AddWithValue("@from_date", newReservation.StartDate);
                    command.Parameters.AddWithValue("@to_date", newReservation.EndDate);
                    command.Parameters.AddWithValue("@create_date", newReservation.CreateDate);

                    reservationId = (int)command.ExecuteScalar();
                }
            }
            catch
            {
                reservationId = -1;
            }
            return reservationId;
        }

        public List<SearchReservation> ReservationSearch(Campground campground, DateTime fromDate, DateTime toDate)
        {
            List<SearchReservation> output = new List<SearchReservation>();
            int totalDays = (int)(toDate - fromDate).TotalDays;


            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(SQL_ReservationSearch, connection);
                    command.Parameters.AddWithValue("@campground_id", campground.CampgroundId);
                    command.Parameters.AddWithValue("@req_from_date", fromDate);
                    command.Parameters.AddWithValue("@req_to_date", toDate);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SearchReservation sr = new SearchReservation();
                        sr.SiteId = Convert.ToInt32(reader["site_id"]);
                        sr.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        sr.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        sr.Accessible = Convert.ToBoolean(reader["accessible"]);
                        sr.MaxRvLength = Convert.ToInt32(reader["max_rv_length"]);
                        sr.Utilites = Convert.ToBoolean(reader["utilities"]);
                        sr.TotalPrice = campground.DailyFee * totalDays;
                        sr.FromDate = fromDate;
                        sr.ToDate = toDate;

                        output.Add(sr);
                    }
                }
            }
            catch (Exception)
            {

            }
            return output;
        }

    }
}



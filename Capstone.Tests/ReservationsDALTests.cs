using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.Tests
{
    [TestClass]

    public class ReservationsDALTests
    {
        private TransactionScope tran;

        private string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=NationalParkReservation;Integrated Security=True";

        int testParkId;
        int testCampgroundId;
        int testSiteId;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;

                conn.Open();
                DateTime currentDateTime = DateTime.Now;

                string createParksCmd = "INSERT INTO park(name, location, establish_date, area, visitors, description) VALUES('testPark', 'Maine', '1919-02-26', 47389, 2563129, 'Covering most of Mount Desert Island and other coastal islands, Acadia features the tallest mountain on the Atlantic coast of the United States, granite peaks, ocean shoreline, woodlands, and lakes. There are freshwater, estuary, forest, and intertidal habitats.'); SELECT CAST(SCOPE_IDENTITY() as int);";
                string createCampgroundsCmd = "INSERT INTO campground(park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES(@parkId, 'testCampground', 1, 12, 35.00); SELECT CAST(SCOPE_IDENTITY() as int);";
                string createSitesCmd = "INSERT INTO site(site_number, campground_id) VALUES(1, @campgroundId); SELECT CAST(SCOPE_IDENTITY() as int);";
                cmd = new SqlCommand(createParksCmd, conn);
                testParkId = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand(createCampgroundsCmd, conn);
                cmd.Parameters.AddWithValue("@parkId", testParkId);
                testCampgroundId = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand(createSitesCmd, conn);
                cmd.Parameters.AddWithValue("campgroundId", testCampgroundId);
                testSiteId = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand($"INSERT INTO reservation (site_id, name, from_date, to_date) VALUES ({testSiteId}, 'test1', GETDATE()+2, GETDATE()+6)", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand($"INSERT INTO reservation (site_id, name, from_date, to_date) VALUES ({testSiteId}, 'test2',  GETDATE()-6, GETDATE()-2)", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand($"INSERT INTO reservation (site_id, name, from_date, to_date) VALUES ({testSiteId}, 'test3', GETDATE()+10, GETDATE()+15)", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand($"INSERT INTO reservation (site_id, name, from_date, to_date) VALUES ({testSiteId}, 'test4', GETDATE()-15, GETDATE()-10)", conn);
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void CreateReservationTests()
        {
            string SQL_Find_Matching_Reservation = "SELECT * FROM reservation WHERE from_date = '2019-10-01' AND to_date = '2019-10-10'";
            string SQL_Remove_All_Reservations = "DELETE FROM reservation";
            ReservationDAL dal = new ReservationDAL(connectionString);

            Reservation res = new Reservation();
            res.StartDate = new DateTime(2019, 10, 01);
            res.EndDate = new DateTime(2019, 10, 10);
            res.SiteId = testSiteId;
            res.ReservationName = "Test Name";

            dal.CreateReservation(res);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL_Remove_All_Reservations, conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand(SQL_Find_Matching_Reservation, conn);

                List<Reservation> results = new List<Reservation>();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Reservation result = new Reservation();
                }

                Assert.AreEqual(0, results.Count);

                
            }
        }

        [TestMethod]
        public void ReservationSearchTests()
        {
            ReservationDAL dal = new ReservationDAL(connectionString);
            DateTime fromDate = new DateTime(2019, 02, 21);
            DateTime toDate = new DateTime(2019, 02, 23);
            Campground campground = new Campground();
            campground.CampgroundId = testCampgroundId;
            campground.DailyFee = 35.00M;

            List<SearchReservation> searchResults = dal.ReservationSearch(campground, fromDate, toDate);

            Assert.AreEqual(1, searchResults.Count);
        }
    }
}

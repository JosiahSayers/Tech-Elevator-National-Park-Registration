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
    public class CampgroundDALTests
    {
        private TransactionScope tran;

        private string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=NationalParkReservation;Integrated Security=True";

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;

                conn.Open();

                cmd = new SqlCommand("INSERT INTO park (name, location, establish_date, area, visitors, description) VALUES ('testPark', 'nowhere', '2000-01-01', 250, 5000, 'Park Description'); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                int testParkId = (int)cmd.ExecuteScalar();
                cmd = new SqlCommand($"INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES ({testParkId}, 'Wonder Lake', '6', '11', '16.00')", conn);

                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod()]
        public void GetCampgroundsTest()
        {
            CampgroundDAL campgroundDal = new CampgroundDAL(connectionString);
            Park testPark = new Park();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command;
                conn.Open();
                command = new SqlCommand($"SELECT * FROM park WHERE name = 'testPark'", conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    testPark.ParkId = Convert.ToInt32(reader["park_id"]);
                    testPark.ParkName = Convert.ToString(reader["name"]);
                    testPark.Location = Convert.ToString(reader["location"]);
                    testPark.EstablishedDate = Convert.ToDateTime(reader["establish_date"]);
                    testPark.Area = Convert.ToInt32(reader["area"]);
                    testPark.AnnualVisitors = Convert.ToInt32(reader["visitors"]);
                    testPark.Description = Convert.ToString(reader["description"]);
                }
            }

            List<Campground> campgrounds = campgroundDal.GetCampgrounds(testPark);
            Assert.AreEqual(1, campgrounds.Count);
            CollectionAssert.AllItemsAreNotNull(campgrounds);
            Assert.AreEqual(testPark.ParkId, campgrounds[0].ParkId);
        }
    }
}

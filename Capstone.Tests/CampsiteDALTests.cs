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

    public class CampsiteDALTests
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

                cmd = new SqlCommand("INSERT INTO site (campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities) VALUES ('7', '6', '10', '1', '30', '1')", conn);
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod()]
        public void GetAllCampsitesTest()
        {
            CampsiteDAL campsiteDal = new CampsiteDAL(connectionString);


            List<Campsite> campsites = campsiteDal.GetAllCampsites();


            Assert.IsNotNull(campsites);

        }

        [TestMethod()]
        public void GetCampsitesWithoutReservationsTest()
        {
            CampsiteDAL campsiteDal = new CampsiteDAL(connectionString);

            List<Campsite> campsites = campsiteDal.GetCampsitesWithoutReservations();

            bool found = false;
            foreach (Campsite cs in campsites)
            {
                if (cs.SiteNumber == 6 && cs.CampgroundId == 7)
                {
                    found = true;
                }
            }
            Assert.IsTrue(found);

        }
    }
}

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

    public class ParkDALTests
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

                cmd = new SqlCommand("INSERT INTO park (name, location, establish_date, area, visitors, description) VALUES ('Denali', 'Alaska', '1917', '4740091', '599822', 'Denali is relatively low-elevation taiga forest give way to high alpine tundra and snowy mountains')", conn);
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]

        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod()]

        public void GetAllParksTest()
        {
            ParkDAL parkDal = new ParkDAL(connectionString);


            List<Park> parks = parkDal.GetAllParks();


            Assert.IsNotNull(parks);

        }

    }
}

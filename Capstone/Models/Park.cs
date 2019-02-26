using System;
using System.Collections.Generic;
using System.Text;


namespace Capstone.Models
{
    public class Park
    {
        public int ParkId { get; set; }

        public string ParkName { get; set; }

        public string Location { get; set; }

        public DateTime EstablishedDate { get; set; }

        public double Area { get; set; }

        public int AnnualVisitors { get; set; }

        public string Description { get; set; }

    }
}

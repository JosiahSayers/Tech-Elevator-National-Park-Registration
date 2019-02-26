using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        public int SiteId { get; set; }

        public string ReservationName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreateDate { get; set; }

        public Reservation()
        {
            CreateDate = DateTime.Now;
        }
    }
}

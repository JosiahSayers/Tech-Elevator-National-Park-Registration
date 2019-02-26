using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class SearchReservation
    {
        public int SiteId { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public bool Accessible { get; set; }
        public int MaxRvLength { get; set; }
        public bool Utilites { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreationDate { get; set; }
    }
}

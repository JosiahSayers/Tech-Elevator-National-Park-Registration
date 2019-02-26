using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Campground
    {
        public int ParkId { get; set; }

        public int CampgroundId { get; set;  }

        public string CampgroundName { get; set;  }

        public int OpenMonth { get; set;  }

        public int CloseMonth { get; set;  }
       
        public decimal DailyFee { get; set;  }
    }
}

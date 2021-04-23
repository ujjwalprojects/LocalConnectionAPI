using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblLCNearByPoint
    {
        [Key]
        public long NearbyPointsID { get; set; }
        public long NearByID { get; set; }
        public long HotelID { get; set; }
        public string NearByPoints { get; set; }
        public string NearByDistance { get; set; }
    }
}
    
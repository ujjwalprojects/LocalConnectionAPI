using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblLCHotelAmenitiesMap
    {
        [Key]
        public long HotelAmenitiesMapID { get; set; }
        public long HotelID { get; set; }
        public long AmenitiesID { get; set; }
    }
}

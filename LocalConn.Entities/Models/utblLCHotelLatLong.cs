using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblLCHotelLatLong
    {
        [Key]
        public long LatLongID { get; set; }
        public long HotelID { get; set; }
        public string LatLong { get; set; }
    }
}

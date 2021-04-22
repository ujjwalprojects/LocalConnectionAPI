using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblLCMstHotelPremise
    {
        [Key]
        public long HotelPremID { get; set; }
        public string HotelPremName { get; set; }
    }
}

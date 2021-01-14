using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblLCMstAmenitie
    {
        [Key]
        public long AmenitiesID { get; set; }
        public string AmenitiesName { get; set; }
        public decimal AmenitiesBasePrice { get; set; }
    }
}

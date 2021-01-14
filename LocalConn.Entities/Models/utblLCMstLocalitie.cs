using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblLCMstLocalitie
    {
        [Key]
        public long LocalityID { get; set; }
        public string LocalityName { get; set; }
        public long CityID { get; set; }
    }
}

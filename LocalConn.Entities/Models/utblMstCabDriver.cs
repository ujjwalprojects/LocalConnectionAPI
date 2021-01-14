using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblMstCabDriver
    {
        [Key]
        public long DriverID { get; set; }
        [Required]
        public string DriverName { get; set; }
        [Required]
        public string DriverContact { get; set; }
    }
}

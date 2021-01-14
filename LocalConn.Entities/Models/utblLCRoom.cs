using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblLCRoom
    {
        [Key]
        public long RoomID { get; set; }
        public string RoomType { get; set; }
        public decimal RoomBaseFare { get; set; }
        public int TotalCapacity { get; set; }
    }
}

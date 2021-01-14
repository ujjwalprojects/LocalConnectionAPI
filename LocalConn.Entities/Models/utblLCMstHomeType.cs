using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblLCMstHomeType
    {
        [Key]
        public long HomeTypeID { get; set; }
        public string HomeTypeName { get; set; }
    }
}

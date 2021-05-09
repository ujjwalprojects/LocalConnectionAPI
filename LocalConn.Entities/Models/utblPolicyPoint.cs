using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblPolicyPoint
    {
        [Key]
        public long PolicyPointID { get; set; }
        public long PolicyID { get; set; }
        public string PolicyPoints { get; set; }
    }
}

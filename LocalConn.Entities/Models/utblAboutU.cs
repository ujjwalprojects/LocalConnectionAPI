using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblAboutU
    {
        [Key]
        public long AboutID { get; set; }
        public string AboutContent { get; set; }
    }
}

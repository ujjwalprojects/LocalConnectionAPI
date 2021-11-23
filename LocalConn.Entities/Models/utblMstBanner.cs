using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblMstBanner
    {
        [Key]
        public long BannerID { get; set; }
        [Required]
        public string BannerPath { get; set; }
        public string UserID { get; set; }
        public DateTime TransDate { get; set; }
    }
}

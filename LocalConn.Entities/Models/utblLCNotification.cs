using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblLCNotification
    {
        [Key]
        public long NotificationID { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationDesc { get; set; }
        public string NotificationImagePath { get; set; }
    }
}

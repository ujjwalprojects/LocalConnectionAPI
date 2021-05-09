using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.ViewModels
{
    public class NotificationView
    {
        public long NotificationID { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationDesc { get; set; }
        public string NotificationImagePath { get; set; }
    }
    public class NotificationVM
    {
        public IEnumerable<NotificationView> Notification { get; set; }
        public int TotalRecords { get; set; }
    }
}

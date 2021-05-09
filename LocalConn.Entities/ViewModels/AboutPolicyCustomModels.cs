using LocalConn.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.ViewModels
{
    public class AboutPolicyVM
    {
        public IEnumerable<utblAboutU> AboutUsList { get; set; }
        public IEnumerable<utblPolicie> PolicyList { get; set; }
        public IEnumerable<PolicyPointView> PolicyPointView { get; set; }
        public int TotalRecords { get; set; }
    }
    public class PolicyPointView
    {
        public long PolicyPointID { get; set; }
        public long PolicyID { get; set; }
        public string PolicyTitle { get; set; }
        public string PolicyPoints { get; set; }
    }
}

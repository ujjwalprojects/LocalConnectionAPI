using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.ViewModels
{
    public class HelpPageView
    {
        public long HelpPageID { get; set; }
        public string HelpPageTitle { get; set; }
        public string HelpPageContent { get; set; }
        public string HelpPageImgPath { get; set; }
        public string HelpPageContactNo { get; set; }
        public string HelpPageEmailID { get; set; }
    }
    public class HelpPageVM
    {
        public IEnumerable<HelpPageView> HelpPage { get; set; }
        public int TotalRecords { get; set; }
    }
}

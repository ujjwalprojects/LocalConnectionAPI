using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblLCHelpPage
    {
        [Key]
        public long HelpPageID { get; set; }
        public string HelpPageTitle { get; set; }
        public string HelpPageContent { get; set; }
        public string HelpPageImgPath { get; set; }
        public string HelpPageContactNo { get; set; }
        public string HelpPageEmailID { get; set; }
    }
}

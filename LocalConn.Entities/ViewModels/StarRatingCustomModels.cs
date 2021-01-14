using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.ViewModels
{
    public class StarRatingView
    {
        public long StarRatingID { get; set; }
        public string StarRatingName { get; set; }
    }
    public class StarRatingVM
    {
        public IEnumerable<StarRatingView> StarRatings { get; set; }
        public int TotalRecords { get; set; }
    }
}

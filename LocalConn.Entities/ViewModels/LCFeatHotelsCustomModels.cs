using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.ViewModels
{
    public class FeatHotelsView
    {
        public long FeatureID { get; set; }
        public long HotelID { get; set; }
        public string HotelName { get; set; }
        public DateTime FeatureStartDate { get; set; }
        public DateTime FeatureEndDate { get; set; }
    }
    public class FeatHotelsVM
    {
        public IEnumerable<FeatHotelsView> FeatHotels { get; set; }
        public int TotalRecords { get; set; }
    }
}

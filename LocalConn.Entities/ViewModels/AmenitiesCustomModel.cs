using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.ViewModels
{
    public class AmenitiesView
    {
        public long AmenitiesID { get; set; }
        public string AmenitiesName { get; set; }
        public string AmenitiesIconPath { get; set; }
        public decimal AmenitiesBasePrice { get; set; }
    }
    public class AmenitiesVM
    {
        public IEnumerable<AmenitiesView> Amenities { get; set; }
        public int TotalRecords { get; set; }
    }

   
    public class HotelAmenitiesVM
    {
        public IEnumerable<HotelAmenitiesMapView> HAmenitiesView { get; set; }
        public int TotalRecords { get; set; }
    }

    public class AmenitiesDD
    {
        public long AmenitiesID { get; set; }
        public string AmenitiesName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.ViewModels
{
    public class LCHotelView
    {
        public long HotelID { get; set; }
        public string HotelName { get; set; }
        public string HotelContact { get; set; }
        public string HotelEmail { get; set; }
        public string HotelTypes { get; set; }
    }
    public class LCHotelVM
    {
        public IEnumerable<LCHotelView> LCHotels { get; set; }
        public int TotalRecords { get; set; }
    }
    public class LCHotelSaveModel
    {
        public long HotelID { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public string HotelDesc { get; set; }
        public string HotelContactNo { get; set; }
        public string HotelEmail { get; set; }
        public long CountryID { get; set; }
        public long StateID { get; set; }
        public long CityID { get; set; }
        public long LocalityID { get; set; }
        public long HomeTypeID { get; set; }
        public long StarRatingID { get; set; }
        public decimal HotelBaseFare { get; set; }
        public int HotelHitCount { get; set; }
        public string MetaText { get; set; }
        public int TotalSingleRooms { get; set; }
        public int TotalDoubleRooms { get; set; }
    }
}

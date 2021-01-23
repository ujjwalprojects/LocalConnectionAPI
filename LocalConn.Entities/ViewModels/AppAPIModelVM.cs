using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.ViewModels
{
    public class HotelList
    {
        public long HotelID { get; set; }
        public long HotelImageID { get; set; }
        public bool IsHotelCover { get; set; }
        public string PhotoThumbPath { get; set; }
        public string PhotoCaption { get; set; }
    }
    public class CityList
    {
        public long CityID { get; set; }
        public string CityName { get; set; }
        public string CityIconPath { get; set; }
    }
}

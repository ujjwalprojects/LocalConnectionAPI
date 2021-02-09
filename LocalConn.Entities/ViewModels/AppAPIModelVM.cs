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
        public string HotelName { get; set; }
        public decimal HotelBaseFare { get; set; }
        public string HotelDesc { get; set; }
        public string HotelAddress { get; set; }
        public long HomeTypeID { get; set; }
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
    public class FtHotelList
    {
        public long FeatureID { get; set; }
        public long HotelID { get; set; }
        public string HotelName { get; set; }
        public string PhotoThumbPath { get; set; }
        public decimal BaseFare { get; set; }
    }

    public class HotelDtl
    {
        public long HotelID { get; set; }
        public string PhotoThumbPath { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public string HotelDesc { get; set; }
        public string LocalityName { get; set; }
        public string StarRating { get; set; }
        public decimal HotelBaseFare { get; set; }
        public int TotalSingleRooms { get; set; }
        public int TotalDoubleRooms { get; set; }
    }
    public class HotelRoomList
    {
        public long HotelID { get; set; }
        public long RoomID { get; set; }
        public string RoomType { get; set; }
        public decimal RoomBaseFare { get; set; }
        public int RoomCapacity { get; set; }
    }
    public class HotelRoomImg
    {
        public long HotelImgID { get; set; }
        public long HotelID { get; set; }
        public string PhotoNormalPath { get; set; }
    }
}

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
        public decimal RoomTypePrice { get; set; }
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
    }

    public class HotelDtl
    {
        public long HotelID { get; set; }
        public string PhotoThumbPath { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public string HotelDesc { get; set; }
        public string LocalityName { get; set; }
        public decimal RoomTypePrice { get; set; }
        //public string StarRating { get; set; }
        //public decimal HotelBaseFare { get; set; }
        //public int TotalSingleRooms { get; set; }
        //public int TotalDoubleRooms { get; set; }
        //public decimal RoomTypePrice { get; set; }
        //public decimal RatePerRoom { get; set; }
        //public decimal RatePerNight { get; set; }
        //public decimal RatePerGuest { get; set; }
        //public decimal RatePerChild { get; set; }
    }
    public class HotelRoomList
    {
        public long HotelID { get; set; }
        public long RoomID { get; set; }
        public string RoomType { get; set; }
        public decimal RoomBaseFare { get; set; }
        public int RoomCapacity { get; set; }
        public bool IsStandard { get; set; }
        public decimal RoomTypePrice { get; set; }
        public decimal RatePerRoom { get; set; }
        public decimal RatePerNight { get; set; }
        public decimal RatePerGuest { get; set; }
        public decimal RatePerChild { get; set; }
    }
    public class HotelPremisesList
    {
        public long HotelPremID { get; set; }
        public long HotelID { get; set; }
        public String HotelPremName { get; set; }
    }

    public class HotelRoomTab
    {
        public List<HotelPremisesList> premisesList { get; set; }
        public List<HotelRoomImg> roomImgList { get; set; }
    }
    public class HotelRoomImg
    {
        public long HotelImgID { get; set; }
        public long HotelID { get; set; }
        public long HotelPremID { get; set; }
        public string HotelPremName { get; set; }
        public string PhotoNormalPath { get; set; }
    }


    public class PreBookingDtl
    {
        public string BookingID { get; set; }
        public long HotelID { get; set; }
        public string CustName { get; set; }
        public string CustEmail { get; set; }
        public string CustPhNo { get; set; }
        public DateTime BookingFrom { get; set; }
        public DateTime BookingUpto { get; set; }
        public DateTime BookingDate { get; set; }
        public string CustDetails { get; set; }
        public string BookingStatus { get; set; }
        public string FinalFare { get; set; }
        public string PaymentGatewayCode { get; set; }
    }
    public class OrderList
    {
        public string BookingID { get; set; }
        public long HotelID { get; set; }
        public string PhotoThumbPath { get; set; }
        public string CustName { get; set; }
        public string CustEmail { get; set; }
        public string CustPhNo { get; set; }
        public string BookingFrom { get; set; }
        public string BookingUpto { get; set; }
        public string CustDetails { get; set; }
        public string BookingDate { get; set; }
        public string BookingStatus { get; set; }
        public decimal FinalFare { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
    }
    public class HAmenitiesList
    {
        public long HotelID { get; set; }
        public long AmenitiesID { get; set; }
        public string AmenitiesName { get; set; }
        public string AmenitiesIconPath { get; set; }
        public decimal AmenitiesBasePrice { get; set; }
    }

    public class OfferList
    {
        public long OfferID { get; set; }
        public string OfferTagLine { get; set; }
        public string OfferImagePath { get; set; }
     
    }

    public class OfferHotelsList
    {
       public List<HomeTypeOnOffer> homeTypeList { get; set; }
       public List<HotelList> hotelList { get; set; }
    }
    public class HomeTypeOnOffer
    {
        public long OfferID { get; set; }
        public long HomeTypeID { get; set; }
        public string HomeTypeName { get; set; }
    }
}

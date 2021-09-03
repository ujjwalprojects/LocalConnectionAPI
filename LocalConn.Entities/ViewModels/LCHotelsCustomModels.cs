using LocalConn.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.ViewModels
{
    //LCHotels
    public class LCHotelView
    {
        public long HotelID { get; set; }
        public string HotelName { get; set; }
        public string HotelContactNo { get; set; }
        public string HotelEmail { get; set; }
        public string HomeTypeName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string LocalityName { get; set; }
        public string StarRatingName { get; set; }
        public Int16 MaxOccupant { get; set; }
        public Int16 OverallOfferPercentage { get; set; }
        public Int16 TwoOccupantPercentage { get; set; }
        public Int16 ThreeOccupantPercentage { get; set; }
        public Int16 FourPlusOccupantPercentage { get; set; }
        public string ChildOccupantNote { get; set; }
        public bool IsActive { get; set; }
        public string RoomType { get; set; }
        public decimal RoomTypePrice { get; set; }
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
        public Int16 MaxOccupant { get; set; }
        public Int16 MaxRooms { get; set; }
        public Int16 OverallOfferPercentage { get; set; }
        public Int16 TwoOccupantPercentage { get; set; }
        public Int16 ThreeOccupantPercentage { get; set; }
        public Int16 FourPlusOccupantPercentage { get; set; }
        public string ChildOccupantNote { get; set; }
        public string LatLong { get; set; }
        public bool IsActive { get; set; }
    }
    public class LCHotelManageModel
    {
        public List<RoomTypeDD> RoomTypeList { get; set; }
        public List<long> RoomID { get; set; }
        public LCHotelSaveModel LCHotel { get; set; }
    }
   
    public class LCHotelDD
    {
        public long HotelID { get; set; }
        public string HotelName { get; set; }
    }

    //LCHotel Images
    public class LCHotelImageView
    {
        public long HotelImageID { get; set; }
        public long HotelID { get; set; }
        public bool IsHotelCover { get; set; }
        public string PhotoThumbPath { get; set; }
        public string PhotoNormalPath { get; set; }
        public string PhotoCaption { get; set; }
    }
    public class LCHotelImageVM
    {
        public IEnumerable<LCHotelImageView> LCHotelImageList { get; set; }
        public int TotalRecords { get; set; }
    }
    //Hotel to Room Type Mapping
    public class HotelRoomTypeMap
    {
        public long HotelID { get; set; }
        public long RoomID { get; set; }
        public decimal RoomTypePrice { get; set; }
        public bool IsStandard { get; set; }
        public bool IsActive { get; set; }

    }
    public class HotelRoomTypeMapView
    {
        public long HotelID { get; set; }
        public long RoomID { get; set; }
        public string RoomType { get; set; }
        public decimal RoomTypePrice { get; set; }
        public bool IsStandard { get; set; }
        public bool IsActive { get; set; }

    }

    //LCHotel Terms and cancellations
    public class HotelTerms
    {
        public long HotelTermsID { get; set; }
        public long HotelID { get; set; }
        public long TermID { get; set; }
        public string TermName { get; set; }
        public bool IsSelected { get; set; }
    }
    public class HotelCancellations
    {
        public long HotelCancID { get; set; }
        public long HotelID { get; set; }
        public long CancellationID { get; set; }
        public string CancellationDesc { get; set; }
        public bool IsSelected { get; set; }
    }
    public class HotelTermCancSaveModel
    {
        [Required]
        public long HotelID { get; set; }
        public List<HOtelTermsSave> Terms { get; set; }
        public List<HotelCancellationsSave> Cancellations { get; set; }
    }
    public class HOtelTermsSave
    {
        [Required]
        public long HotelTermsID { get; set; }
        [Required]
        public long HotelID { get; set; }
        [Required]
        public long TermID { get; set; }
    }
    public class HotelCancellationsSave
    {
        [Required]
        public long HotelCancID { get; set; }
        [Required]
        public long HotelID { get; set; }
        [Required]
        public long CancellationID { get; set; }
    }


    //LCHotel Offers
    public class HotelOffer
    {
        public long OfferID { get; set; }
        [Required]
        public string OfferTagLine { get; set; }
        [Required]
        public string OfferImagePath { get; set; }
        [Required]
        public DateTime OfferStartDate { get; set; }
        [Required]
        public DateTime OfferEndDate { get; set; }
    }
    public class SaveHotelOffer
    {
        [Required]
        public List<long> HotelID { get; set; }
        public HotelOffer HotelOffer { get; set; }
    }
    public class HotelOfferView
    {
        public long OfferID { get; set; }
        public string OfferTagLine { get; set; }
        public string HotelName { get; set; }
        public DateTime OfferStartDate { get; set; }
        public DateTime OfferEndDate { get; set; }
    }
    public class HotelOfferVM
    {
        //public IEnumerable<GenPackageOfferView> GenPackageOfferList { get; set; }
        public IEnumerable<HotelOfferView> HotelOfferList { get; set; }
        public IEnumerable<HotelDD> HotelList { get; set; }
        public int TotalRecords { get; set; }
    }
    public class EditHotelOffer
    {
        public utblLCFeatureOffer HotelOffer { get; set; }
        public List<HotelDD> HotelList { get; set; }
        public long HotelID { get; set; }
    }
    public class HotelDD
    {
        public long HotelID { get; set; }
        public string HotelName { get; set; }
    }

    //LCNearByPoints
    public class LCNearBysTypeDD
    {
        public long NearByID { get; set; }
        public string NearByName { get; set; }
    }
    public class LCNearByPoints
    {
        public long NearbyPointsID { get; set; }
        public long NearByID { get; set; }
        public long HotelID { get; set; }
        public string NearByPoints { get; set; }
        public string NearByDistance { get; set; }
    }
    public class LCNearByPointsView
    {
        public long NearbyPointsID { get; set; }
        public long NearByID { get; set; }
        public long HotelID { get; set; }
        public string NearByName { get; set; }
        public string NearByPoints { get; set; }
        public string NearByDistance { get; set; }
    }
    public class LCNearByPointsVM
    {
        public IEnumerable<LCNearByPointsView> LCNearByPointView { get; set; }
        public IEnumerable<LCNearBysTypeDD> NearBysDD { get; set; }
        public int TotalRecords { get; set; }
    }

    //customer booking model
    public class LCCustomerBookingVM
    {
        public IEnumerable<LCCustomerBookingView> LCCustomerBookingView { get; set; }
        public int TotalRecords { get; set; }
    }
    public class LCCustomerBookingView
    {
        public string BookingID { get; set; }
        public string CustName { get; set; }
        public string CustEmail { get; set; }
        public string CustPhNo { get; set; }
        public long HotelID { get; set; }
        public string HotelName { get; set; }
        public DateTime BookingFrom { get; set; }
        public DateTime BookingUpto { get; set; }
        public DateTime BookingDate { get; set; }
        public string CustDetails { get; set; }
        public string BookingStatus { get; set; }
        public decimal FinalFare { get; set; }
        public string paymentstatus { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using LocalConn.API.Helper;
using LocalConn.Entities.Dal;
using LocalConn.Entities.Models;
using LocalConn.Entities.ViewModels;

namespace LocalConn.API.Areas.General.Controllers
{
    [RoutePrefix("api/general/webrequest")]
    public class WebRequestController : ApiController
    {
        dalWebRequest objDal = new dalWebRequest();
        dalConfigurations objConfig = new dalConfigurations();


        private string AdminNo = ConfigurationManager.AppSettings["AdminNo"];
        private string AdminEmail = ConfigurationManager.AppSettings["AdminEmail"];

        [HttpGet]
        [Route("getcitylist")]//state id
        public async Task<List<CityList>> getCityList()
        {
            return await objDal.getCityMenuList();
        }

        [HttpGet]
        [Route("getcityhotelvmlist")]//city id
        public async Task<List<HotelList>> getCityHotelList(string id)
        {
            return await objDal.getCityHotelList(id);
        }

        [HttpGet]
        [Route("gethotellist")]//hometypeid
        public async Task<List<HotelList>> getHotelList(string id)
        {
            List<HotelList> obj = new List<HotelList>();
            obj = await objDal.getHotelMenuList(id);
            return obj;
        }

        [HttpGet]
        [Route("getresortlist")]//hometypeid
        public async Task<List<HotelList>> getResortList(string id)
        {   
            return await objDal.getResortMenuList(id);
        }
        [HttpGet]
        [Route("getlodgelist")]//hometypeid
        public async Task<List<HotelList>> getLodgeList(string id)
        {
            return await objDal.getLodgeMenuList(id);
        }
        [HttpGet]
        [Route("gethomestaylist")]//homeytypeid
        public async Task<List<HotelList>> getHomestayList(string id)
        {
            return await objDal.getHomestayMenuList(id);
        }
        [HttpGet]
        [Route("getghouselist")]//hometypeid
        public async Task<List<HotelList>> getGHouseList(string id)
        {
            return await objDal.getGHouseMenuList(id);
        }
        //search
        [HttpGet]
        [Route("gethotelsearch")]
        public async Task<List<HotelList>> getHotelSearchList(string Query)
        {
            return await objDal.getHotelSearchList(Query);
        }

        [HttpGet]
        [Route("gethoteldtl")]
        public async Task<HotelDtl> getHotelDtl(long HotelID)
        {
            HotelDtl obj = new HotelDtl();
            obj = await objDal.getHotelDtl(Convert.ToString(HotelID));
            return obj;
        }
        [HttpGet]
        [Route("gethamenitieslist")]//hotelid
        public async Task<List<HAmenitiesList>> getAmenitiesList(string id)
        {
            return await objDal.getAmnetieslist(id);
        }
        [HttpGet]
        [Route("gethotelpremises")]
        public async Task<List<HotelPremisesList>> getHotelPremises(string id)
        {
            return await objDal.getPremMenu(id);
        }

        [HttpGet]
        [Route("gethotelroomlist")]
        public async Task<List<HotelRoomList>> getHotelRoomList(long id)
        {
            return await objDal.getHotelRoomList(Convert.ToString(id));
        }
        //tabbed
        [HttpGet]
        [Route("gethroomimglist")]
        public async Task<HotelRoomTab> getHRoomImgList(string HotelID)
        {
            HotelRoomTab obj = new HotelRoomTab();
            obj.roomImgList = await objDal.getHRoomImgList(HotelID);
            obj.premisesList = await objDal.getHPremList(HotelID);
            return obj;
        }
        [HttpGet]
        [Route("gethotelvmlist")]
        public async Task<List<HotelList>> getHotelVMList(string HomeTypeID)
        {
            return await objDal.gethotelvmlist(HomeTypeID);
        }

        [HttpGet]
        [Route("getFHotelList")]
        public async Task<List<FtHotelList_web>> getFeaturedList()
        {
            return await objDal.getFeaturedlist_web();
        }

        [HttpGet]
        [Route("getorderlist")]
        public async Task<List<OrderList>> GetOrderList(string UserID)
        {
            List<OrderList> obj = new List<OrderList>();
            obj = await objDal.getOrderlist(UserID);
            return obj;
        }

        [HttpGet]
        [Route("getBookingDtl")]
        public async Task<PreBookingDtl> getBookingDtl(string BookingID)
        {
            return await objDal.getBookingDtl(BookingID);
        }

        //final booking with payment
        [Route("paynow")]
        [HttpPost]
        public async Task<string> PayNow(PreBookingDtl obj)
        {
            string Result = "";
            Result = objDal.preBooking(obj);
            if (!(Result.Contains("Error")))
            {
                obj.BookingID = Result;
                Result = await SendMail(obj);
                if(!(Result.Contains("Error")))
                {
                    Result = await SendMailToAdmin(obj);
                }
            }
            return Result;
        }

        //cancelbooking

        [Route("cancelbooking")]
        [HttpPost]
        public async Task<string> CancelBooking(string BookingID)
        {
            string Result = "";

            string[] subs = Result.Split('%');
            PreBookingDtl obj = new PreBookingDtl();
            obj = objDal.cancelBooking(BookingID);
            if (obj.BookingStatus == "Cancelled")
                Result = await SendMail(obj);
            return Result;
        }

        #region Terms and Cancellation Policy

        [HttpGet]
        [Route("gettermncondpolicylist")]
        public TermCondPolicyVM getTermnCondPolicyList(string HotelID)
        {
            TermCondPolicyVM obj = new TermCondPolicyVM();
            obj.termPolicyList = objDal.getTermPolicyList(HotelID);
            obj.cancelPolicyList = objDal.getCancelPolicyList(HotelID);
            return obj;
        }


        #endregion

        //notification

        [HttpGet]
        [Route("getnotificationlist")]
        public async Task<List<NotificationList>> GetNotificationList()
        {
            return await objDal.getNotificationList();
        }
        //get nearby

        [HttpGet]
        [Route("getnearbylist")]
        public NearbyVM GetNearByList(string HotelID)
        {
            NearbyVM obj = new NearbyVM();
            obj.nearbyone = objDal.getNearByList(HotelID, "1");
            obj.nearbytwo = objDal.getNearByList(HotelID, "2");
            return obj;
        }

        //helppage
        [HttpGet]
        [Route("gethelpdtl")]
        public HelpPageDtl getHelpDtl()
        {
            HelpPageDtl obj = new HelpPageDtl();
            obj = objDal.getHelpPage();
            return obj;
        }
        //About
        [HttpGet]
        [Route("getaboutusdtl")]
        public AboutUsDetails getDtl()
        {
            AboutUsDetails obj = new AboutUsDetails();
            obj = objDal.getAboutUs();
            return obj;
        }
        //Policy
        [HttpGet]
        [Route("getpolicylist")]
        public PolicyList getPolicyList()
        {
            PolicyList obj = new PolicyList();
            obj = objDal.getPolicyList();
            return obj;
        }


        #region Offer
        [HttpGet]
        [Route("getofferlist")]
        public List<OfferList> getOfferList(string Dt)
        {
            List<OfferList> obj = new List<OfferList>();
            obj = objDal.getOfferlist(Dt);
            return obj;
        }
        [HttpGet]
        [Route("getofferhotellist")]
        public async Task<List<HotelList_Offer>> getOfferHotelList(string id)
        {
            //OfferHotelsList obj = new OfferHotelsList();
           return await objDal.getOfferHotellist(Convert.ToInt64(id));
            //obj.homeTypeList = await objDal.getHomtTypeOnOffer(Convert.ToInt64(OfferID));
            //return obj;
        }
        #endregion


        [HttpPost]
        [Route("GenLCHotelSearch")]
        public async Task<GenLCHotelVM> GenLCHotelSearch(GenLCSearchModel model)
        {
            return await objDal.SearchGenLCHotelListAsync(model);
        }
        [HttpGet]
        [Route("WhereLCNames")]
        public async Task<IEnumerable<string>> WhereLCNames()
        {
            return await objDal.GetStateCityNamesAsync();
        }
        [HttpGet]
        [Route("HomeTypes")]
        public async Task<IEnumerable<utblLCMstHomeType>> HomeTypes()
        {
            return await objConfig.getHomeTypeAsync();
        }
        #region Mail and SMS
        [Route("SendEmail")]
        [HttpPost]
        public async Task<string> SendMail(PreBookingDtl obj)
        {
            HotelDtl hotelDtl = await objDal.getHotelDtl(Convert.ToString(obj.HotelID));
            PreBookingTransDtl sendmailmodel = new PreBookingTransDtl()
            {
                BookingID = obj.BookingID,
                HotelID = obj.HotelID,
                CustName = obj.CustName,
                CustEmail = obj.CustEmail,
                CustPhNo = obj.CustPhNo,
                BookingFrom = obj.BookingFrom,
                BookingUpto = obj.BookingUpto,
                BookingDate = obj.BookingDate,
                CustDetails = obj.CustDetails,
                BookingStatus = obj.BookingStatus,
                FinalFare = obj.FinalFare,
                PaymentGatewayCode = obj.PaymentGatewayCode,
                HotelName = hotelDtl.HotelName
            };
            SendMail objSendMail = new SendMail();
            string result = objSendMail.SendEmail(sendmailmodel, "Customer");
            return result;
        }

        [Route("SendEmailAdmin")]
        [HttpPost]
        public async Task<string> SendMailToAdmin(PreBookingDtl obj)
        {
            HotelDtl hotelDtl = await objDal.getHotelDtl(Convert.ToString(obj.HotelID));
            PreBookingTransDtl sendmailmodel = new PreBookingTransDtl()
            {
                BookingID = obj.BookingID,
                HotelID = obj.HotelID,
                CustEmail = obj.CustEmail,
                CustName = obj.CustName,
                CustPhNo = obj.CustPhNo,
                BookingFrom = obj.BookingFrom,
                BookingUpto = obj.BookingUpto,
                BookingDate = obj.BookingDate,
                CustDetails = obj.CustDetails,
                BookingStatus = obj.BookingStatus,
                FinalFare = obj.FinalFare,
                PaymentGatewayCode = obj.PaymentGatewayCode,
                HotelName = hotelDtl.HotelName
            };
            SendMail objSendMail = new SendMail();
            string result = objSendMail.SendEmail(sendmailmodel, "Admin");
            return result;

        }

        public IHttpActionResult SendSMS(string custName, string hotelName, string amount, string bookingID, string mobno, string Type)
        {
            try
            {

                SendConfirmationmessage.SendHttpSMSConfirmation(custName, hotelName, amount, bookingID, mobno, Type);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        public IHttpActionResult SendSMSToAdmin(string amount, string bookingID, string mobno, string Type)
        {
            try
            {

                SendConfirmationmessage.SendHttpSMSConfirmationToAdmin(amount, bookingID, mobno, Type);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        #endregion
    }
}

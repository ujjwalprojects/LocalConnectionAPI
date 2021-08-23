using System;
using System.Collections.Generic;
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



        [HttpGet]
        [Route("getcitylist")]//state id
        public async Task<List<CityList>> getCityList(string id)
        {
            return await objDal.getCityMenuList(id);
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
        public async Task<HotelDtl> getHotelDtl(string HotelID)
        {
            HotelDtl obj = new HotelDtl();
            obj = await objDal.getHotelDtl(HotelID);
            return obj;
        }
        [HttpGet]
        [Route("gethamenitieslist")]
        public async Task<List<HAmenitiesList>> getAmenitiesList(string HotelID)
        {
            return await objDal.getAmnetieslist(HotelID);
        }
        [HttpGet]
        [Route("gethotelpremises")]
        public async Task<List<HotelPremisesList>> getHotelPremises(string HotelID)
        {
            return await objDal.getPremMenu(HotelID);
        }

        [HttpGet]
        [Route("gethotelroomlist")]
        public async Task<List<HotelRoomList>> getHotelRoomList(string HotelID)
        {
            return await objDal.getHotelRoomList(HotelID);
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
        public async Task<List<FtHotelList>> getFeaturedList(string Dt)
        {
            return await objDal.getFeaturedlist(Dt);
        }

        [HttpGet]
        [Route("getorderlist")]
        public async Task<List<OrderList>> GetOrderList(string CustPhNo)
        {
            List<OrderList> obj = new List<OrderList>();
            obj = await objDal.getOrderlist(CustPhNo);
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
        public string PayNow(PreBookingDtl obj)
        {
            string Result = "";
            Result = objDal.preBooking(obj);
            if (!(Result.Contains("Error")))
            {
                obj.BookingID = Result;
                Result = SendMail(obj);
            }
            return Result;
        }

        //cancelbooking

        [Route("cancelbooking")]
        [HttpPost]
        public string CancelBooking(string BookingID)
        {
            string Result = "";

            string[] subs = Result.Split('%');
            PreBookingDtl obj = new PreBookingDtl();
            obj = objDal.cancelBooking(BookingID);
            if (obj.BookingStatus == "Cancelled")
                Result = SendMail(obj);
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
        public async Task<OfferHotelsList> getOfferHotelList(string OfferID)
        {
            OfferHotelsList obj = new OfferHotelsList();
            obj.hotelList = await objDal.getOfferHotellist(Convert.ToInt64(OfferID));
            obj.homeTypeList = await objDal.getHomtTypeOnOffer(Convert.ToInt64(OfferID));
            return obj;
        }


        #endregion



        #region Mail and SMS
        [Route("SendEmail")]
        [HttpPost]
        public string SendMail(PreBookingDtl obj)
        {
            //MailDetails email = objDal.getEmailByApplication(applicationcode);
            //email.EmailID = email.EmailID;
            //email.MobileNo = email.MobileNo;
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password);
            //Create the SMTP Client
            SmtpClient client = new SmtpClient();
            client.Host = settings.Smtp.Network.Host;
            client.Credentials = credential;
            client.Timeout = 300000;
            client.EnableSsl = true;
            MailMessage mail = new MailMessage();
            StringBuilder mailbody = new StringBuilder();
            mail.From = new MailAddress(settings.Smtp.Network.UserName, "Local Connection");
            mail.To.Add(obj.CustEmail);
            mail.Priority = MailPriority.High;

            switch (obj.BookingStatus)
            {
                case "Booked":
                    //mail.Subject = "Booking Details";
                    //mailbody.Append("<p>Dear " + obj.CustName + ",</p>");
                    //mailbody.Append("<p>" + "You have successfully Booked your stay with Booking ID :" + obj.BookingID + ".\n Please check your order details in the app booking section or order list section " + "</p>");
                    //mailbody.Append("<p>Booking Date: " + DateTime.Now.ToString("dd MMM yyyy HH:mm tt") + "</p>");
                    //mailbody.Append("<i>This is an auto generated mail, please do not reply.</i>");
                    SendSMS(obj.FinalFare.ToString(), obj.BookingID, obj.CustPhNo, "Booked");
                    break;
                case "Cancelled":
                    //mail.Subject = "Cancellation Details";
                    //mailbody.Append("<p>Dear " + obj.CustName + ",</p>");
                    //mailbody.Append("<p>" + "Your Booking ID : "+ obj.BookingID+ " Has been cancelled successfully" + "</p>");
                    //mailbody.Append("<p>Cancellation Date: " + DateTime.Now.ToString("dd MMM yyyy HH:mm tt") + "</p>");
                    //mailbody.Append("<i>This is an auto generated mail, please do not reply.</i>");
                    SendSMS(obj.PaymentGatewayCode, obj.BookingID, obj.CustPhNo, "Cancelled");
                    break;

                default:
                    break;

            }

            mail.Body = mailbody.ToString();
            mail.IsBodyHtml = true;

            try
            {
                //client.Send(mail);

                return obj.BookingID;
            }
            catch (Exception e)
            {

                return "Error: Something Went Wrong" + e;
            }

        }

        public IHttpActionResult SendSMS(string amount, string bookingID, string mobno, string Type)
        {
            try
            {

                SendConfirmationmessage.SendHttpSMSConfirmation(amount, bookingID, mobno, Type);
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

using LocalConn.API.Helper;
using LocalConn.API.Models;
using LocalConn.Entities.Dal;
using LocalConn.Entities.Models;
using LocalConn.Entities.ViewModels;
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

namespace LocalConn.API.Areas.General.Controllers
{
    [RoutePrefix("api/general/app")]
    public class AppController : ApiController
    {
        dalApp objDal = new dalApp();
        private string AdminNo = ConfigurationManager.AppSettings["AdminNo"];
        private string AdminEmail = ConfigurationManager.AppSettings["AdminEmail"];


        [HttpGet]
        [Route("getcitylist")]
        public async Task<List<CityList>> getCityList(string StateID)
        {
            return await objDal.getCityMenuList(StateID);
        }

        [HttpGet]
        [Route("getcityhotelvmlist")]
        public async Task<List<HotelList>> getCityHotelList(string CityID)
        {
            return await objDal.getCityHotelList(CityID);
        }

        [HttpGet]
        [Route("gethotellist")]
        public async Task<List<HotelList>> getHotelList(string HomeTypeID)
        {
            List<HotelList> obj = new List<HotelList>();
            obj = await objDal.getHotelMenuList(HomeTypeID);
            return obj;
        }

        [HttpGet]
        [Route("getresortlist")]
        public async Task<List<HotelList>> getResortList(string HomeTypeID)
        {
            return await objDal.getResortMenuList(HomeTypeID);
        }
        [HttpGet]
        [Route("getlodgelist")]
        public async Task<List<HotelList>> getLodgeList(string HomeTypeID)
        {
            return await objDal.getLodgeMenuList(HomeTypeID);
        }
        [HttpGet]
        [Route("gethomestaylist")]
        public async Task<List<HotelList>> getHomestayList(string HomeTypeID)
        {
            return await objDal.getHomestayMenuList(HomeTypeID);
        }
        [HttpGet]
        [Route("getghouselist")]
        public async Task<List<HotelList>> getGHouseList(string HomeTypeID)
        {
            return await objDal.getGHouseMenuList(HomeTypeID);
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
            //obj.MaxOccupantPerRoom = 3;
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
        public async  Task<string> PayNow(PreBookingDtl obj)
        {
            string Result = "";
            Result = objDal.preBooking(obj);
            if (!(Result.Contains("Error")))
            {
                obj.BookingID = Result;
                Result = await SendMail(obj);
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
        public async Task<string> SendMail(PreBookingDtl obj)
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
            HotelDtl hotelDtl = await objDal.getHotelDtl(Convert.ToString(obj.HotelID));
            string hotelName = "";
            //limit hotel name upto 10 characters
            if (hotelDtl.HotelName.Length > 10)
            {
                hotelName = hotelDtl.HotelName.Substring(0, 10);
            }
            else
            {
                hotelName = hotelDtl.HotelName;
            }
            switch (obj.BookingStatus)
            {
                case "Booked":
                    mail.Subject = "Booking Details";
                    mailbody.Append("<p>Dear " + obj.CustName + ",</p>");
                    mailbody.Append("<p>" + "Hi, "+obj.CustName+".Thank you for choosing our hotel.We have you confirmed a reservation for "+hotelName+"+.Your BookingID is "+obj.BookingID+"  + 917319079996 + LocalConnection.");
                    mailbody.Append("<p>Booking Date: " + DateTime.Now.ToString("dd MMM yyyy HH:mm tt") + "</p>");
                    mailbody.Append("<i>This is an auto generated mail, please do not reply.</i>");
                    SendSMS(obj.CustName, hotelName, obj.FinalFare.ToString(), obj.BookingID, obj.CustPhNo, "Booked");
                    SendSMSToAdmin(obj.CustName, obj.FinalFare.ToString(), obj.BookingID, AdminNo, "Booked",Convert.ToString(obj.BookingDate));
                    break;
                case "Cancelled":
                    mail.Subject = "Cancellation Details";
                    mailbody.Append("<p>Dear " + obj.CustName + ",</p>");
                    mailbody.Append("<p>" + "Hi, your booking at Local Conn. has been cancelled, as per your request. BookingID: " + obj.BookingID + ". Helpline " + 917319079996 + ". Look forward to hosting you soon!" + "</p>");
                    mailbody.Append("<p>Cancellation Date: " + DateTime.Now.ToString("dd MMM yyyy HH:mm tt") + "</p>");
                    mailbody.Append("<i>This is an auto generated mail, please do not reply.</i>");
                    //SendSMS(obj.PaymentGatewayCode, obj.BookingID, obj.CustPhNo, "Cancelled");
                    //SendSMSToAdmin(obj.PaymentGatewayCode, obj.BookingID, AdminNo, "Cancelled");
                    SendSMS(obj.CustName, hotelName, obj.FinalFare.ToString(), obj.BookingID, obj.CustPhNo, "Cancelled");
                    SendSMSToAdmin(obj.CustName,obj.FinalFare.ToString(), obj.BookingID, AdminNo, "Cancelled",Convert.ToString(obj.BookingDate));
                    break;

                default:
                    break;

            }

            mail.Body = mailbody.ToString();
            mail.IsBodyHtml = true;

            try
            {
                client.Send(mail);

                return obj.BookingID;
            }
            catch (Exception e)
            {

                return "Error: Something Went Wrong" + e;
            }

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
        public IHttpActionResult SendSMSToAdmin(string name,string amount, string bookingID, string mobno, string Type,string date)
        {
            try
            {

                SendConfirmationmessage.SendHttpSMSConfirmationToAdmin(name,amount, bookingID, mobno, Type,date);
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

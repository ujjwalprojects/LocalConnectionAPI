using LocalConn.Entities.Dal;
using LocalConn.Entities.ViewModels;
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

namespace LocalConn.API.Areas.General.Controllers
{
    [RoutePrefix("api/general/app")]
    public class AppController : ApiController
    {
        dalApp objDal = new dalApp();

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
            return await objDal.getOrderlist(CustPhNo);
        }

        [HttpGet]
        [Route("getBookingDtl")]
        public async Task<PreBookingDtl> getBookingDtl(string Dt)
        {
            return await objDal.getBookingDtl(Dt);
        }


        [Route("paynow")]
        [HttpPost]
        public async Task<string> PayNow(PreBookingDtl obj)
        {
            string Result = "";
            Result = objDal.preBooking(obj);
            //if (Result.Contains("B"))
            //{
            //  Result= await SendMail(obj);
            //}
            return Result;
        }
        #region Terms and Cancellation Policy

        [HttpGet]
        [Route("gettermpolicylist")]
        public async Task<List<TermsPolicyList>> GetTermPolicyList(string HotelID)
        {
            return await objDal.getTermPolicyList(HotelID);
        }

        [HttpGet]
        [Route("getcancelpolicylist")]
        public async Task<List<CancellationPolicyList>> GetCancelPolicyList(string HotelID)
        {
            return await objDal.getCancelPolicyList(HotelID);
        }

        #endregion

        //notification

        [HttpGet]
        [Route("getnotificationlist")]
        public async Task<List<NotificationList>> GetNotificationList()
        {
            return await objDal.getNotificationList();
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
        public  async Task<OfferHotelsList> getOfferHotelList(string OfferID)
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

            switch (obj.BookingStatus)
            {
                case "Booked":
                    mail.Subject = "Booking Details";
                    mailbody.Append("<p>Dear " + obj.CustName + ",</p>");
                    mailbody.Append("<p>" +"You have successfully Booked your stay.... please check your order details in the apps Booking Section or Order List section " + "</p>");
                    mailbody.Append("<p>Timestamp: " + DateTime.Now.ToString("dd MMM yyyy HH:mm tt") + "</p>");
                    mailbody.Append("<i>This is an auto generated mail, please do not reply.</i>");
                    //await SendSMS(obj.CustPhNo, "Your Booking Has been Made "+"Successfully with Payment Code" + obj.PaymentGatewayCode);
                    break;
                //case "Rejected":
                //    mail.Subject = "Booking Details";
                //    mailbody.Append("<p>Dear " + obj.CustName + ",</p>");
                //    mailbody.Append("<p>" + "You have c" + "</p>");
                //    mailbody.Append("<p>Timestamp: " + DateTime.Now.ToString("dd MMM yyyy HH:mm tt") + "</p>");
                //    mailbody.Append("<i>This is an auto generated mail, please do not reply.</i>");
                //    break;
                //case "Approved":
                //    mail.Subject = "TET Sikkim - Approved";
                //    mailbody.Append("<p>Dear " + email.ApplicantName + ",</p>");
                //    mailbody.Append("<p>" + msg + "</p>");
                //    mailbody.Append("<p>Timestamp: " + DateTime.Now.ToString("dd MMM yyyy HH:mm tt") + "</p>");
                //    mailbody.Append("<i>This is an auto generated mail, please do not reply.</i>");
                //    break;

                default:
                    break;

            }

            mail.Body = mailbody.ToString();
            mail.IsBodyHtml = true;

            try
            {
                await client.SendMailAsync(mail);

                return "success";
            }
            catch (Exception e)
            {

                return "Error: Something Went Wrong"+e;
            }

        }
        //public async Task<bool> SendSMS(string mobno, string message)
        //{
        //    bool sms = false;
        //    try
        //    {
        //        SendSMS objSMS = new SendSMS();
        //        await objSMS.SendHttpSMSRequest("TET", message, mobno);
        //        //objSMS.sendSingleSMS("skmportalsms", "s1Kk1m@12", "SKMGOV", mobno, message, "");
        //        //SMSConfirmation.SendSMS(mobno, message);
        //        sms = true;
        //        return sms;
        //    }
        //    catch (Exception ex)
        //    {
        //        sms = false;
        //        throw ex;
        //    }
        //}
        #endregion

    }
}

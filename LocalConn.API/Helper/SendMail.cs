using LocalConn.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace LocalConn.API.Helper
{
    public class SendMail
    {

        public string SendEmail(PreBookingTransDtl model, string User="")
        {
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(System.Web.HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential(settings.Smtp.Network.UserName, settings.Smtp.Network.Password);
            //Create the SMTP Client
            SmtpClient client = new SmtpClient();
            client.Host = settings.Smtp.Network.Host;
            client.UseDefaultCredentials = false;
            client.Credentials = credential;
            client.Timeout = 300000;
            client.EnableSsl = true;

            MailMessage mail = new MailMessage();
            StringBuilder mailbody = new StringBuilder();
            mail.From = new MailAddress(settings.Smtp.Network.UserName, "Local Connection");
           

            if(User == "Customer")
            {
                mail.To.Add(model.CustEmail);
                mail.Priority = MailPriority.High;

                switch (model.BookingStatus)
                {
                    case "Booked":
                        mail.Subject = "Booking Details";
                        mailbody.Append("<p>Dear " + model.CustName + ",</p>");
                        mailbody.Append("<p>" + "Thank you for choosing our hotel! We have you confirmed a reservation for" + model.HotelName + ". Your BookingID is " + model.BookingID + "." + "</p>");
                        mailbody.Append("<p>" + "Booking From - Upto: " + model.BookingFrom.ToString("dd MMM yyyy")+" - "+model.BookingUpto.ToString("dd MMM yyyy") + "</P>");
                        mailbody.Append("<p>" + "Amount: Rs." + model.FinalFare + "</P>");
                        mailbody.Append("<p>" + "Helpline: + 91-73190-79996" + "</P>");
                        mailbody.Append("<p>" + "enquiry at: localconnectiontmc@gmail.com" + "</p>");
                        mailbody.Append("<p>" + "Local Connection." + "</p>");
                        mailbody.Append("<p>Booking Date: " + DateTime.Now.ToString("dd MMM yyyy HH:mm tt") + "</p>");
                        mailbody.Append("<i>This is an auto generated mail, please do not reply.</i>");
                        break;
                    case "Cancelled":
                        mail.Subject = "Cancellation Details";
                        mailbody.Append("<p>Dear " + model.CustName + ",</p>");
                        mailbody.Append("<p>" + "Your booking at" + model.HotelName + " has been cancelled,  as per your request. BookingID: " + model.BookingID + ".Look forward to hosting you soon!" + "</p>");
                        mailbody.Append("<p>" + "Helpline: + 91-73190-79996" + "</P>");
                        mailbody.Append("<p>" + "enquiry at: localconnectiontmc@gmail.com" + "</p>");
                        mailbody.Append("<p>" + "Local Connection." + "</p>");
                        mailbody.Append("<p>Cancellation Date: " + DateTime.Now.ToString("dd MMM yyyy HH:mm tt") + "</p>");
                        mailbody.Append("<i>This is an auto generated mail, please do not reply.</i>");
                        break;

                    default:
                        break;

                }
            }
            else if(User == "Admin")
            {
                mail.To.Add("localconnectiontmc@gmail.com");
                mail.Priority = MailPriority.High;

                switch (model.BookingStatus)
                {
                    case "Booked":
                        mail.Subject = "Booking Details";
                        mailbody.Append("<p>Dear " + "Admin" + ",</p>");
                        mailbody.Append("<p>" + "Customer " + model.CustName + " has made a new booking for " + model.HotelName + ".</p>");
                        mailbody.Append("<p>" + "BookingID: " + model.BookingID + "</P>");
                        mailbody.Append("<p>" + "Booking From - Upto: " + model.BookingFrom.ToString("dd MMM yyyy") + " - " + model.BookingUpto.ToString("dd MMM yyyy") + "</P>");
                        mailbody.Append("<p>" + "Amount: Rs." + model.FinalFare + "</P>");
                        mailbody.Append("<p>" + "Customer Ph.No.: " + model.CustPhNo + "</P>");

                        //mailbody.Append("<p>" + "Regards," + "</P>");
                        mailbody.Append("<p>" + "Regards, <br />Local Connection." + "</p>");
                        mailbody.Append("<p>Booking Date: " + DateTime.Now.ToString("dd MMM yyyy HH:mm tt") + "</p>");
                        mailbody.Append("<i>This is an auto generated mail, please do not reply.</i>");

                        break;
                    case "Cancelled":
                        mail.Subject = "Cancellation Details";
                        mailbody.Append("<p>Dear " + "Admin" + ",</p>");
                        mailbody.Append("<p>" + "Customer " + model.CustName + " has cancelled the booking for " + model.HotelName + ".</p>");
                        mailbody.Append("<p>" + "BookingID: " + model.BookingID + "</P>");
                        mailbody.Append("<p>" + "Amount: " + model.FinalFare + "</P>");
                        mailbody.Append("<p>" + "Booking From - Upto: " + model.BookingFrom.ToString("dd MMM yyyy") + " - " + model.BookingUpto.ToString("dd MMM yyyy") + "</P>");
                        mailbody.Append("<p>" + "Please check the refund status of the customer and do the needful." + "</P>");
                        mailbody.Append("<p>" + "Regards, <br />Local Connection." + "</p>");
                        mailbody.Append("<p>Cancellation Date: " + DateTime.Now.ToString("dd MMM yyyy HH:mm tt") + "</p>");
                        mailbody.Append("<i>This is an auto generated mail, please do not reply.</i>");
                      
                        break;

                    default:
                        break;

                }
            }
           
            mail.Body = mailbody.ToString();
            mail.IsBodyHtml = true;

            try
            {
                client.Send(mail);

                return model.BookingID;
            }
            catch (Exception e)
            {

                return "Error: Something Went Wrong" + e;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace LocalConn.API.Helper
{
    public static class OTPGenerator
    {
        private static string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        public static string GenerateRandomOTP(int iOTPLength)
        {

            string sOTP = String.Empty;

            string sTempChars = String.Empty;

            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)
            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }

            return sOTP;

        }
    }
    public static class SendOTPMessage
    {
        public static string SendHttpSMSRequest(string otp, string mobNo, string type)
        {
            string message = "";
            if (type == "Register")
            {
                message = HttpUtility.UrlEncode("Your OTP for LocalConnection is " + otp + "%nRegards,%nLocalConnection");
            }
            if (type == "ForgotPass")
            {
                message = HttpUtility.UrlEncode("Your OTP for LocalConnection is " + otp + "%nRegards,%nLocalConnection");
            }
            try
            {
                using (var wb = new WebClient())
                {
                    byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                {"apikey" , "NzE4ZTJlZTAyOTBlNjgyZjNkZGMwNmY0YzBhYjE1ZjY="},
                {"numbers" , "91"+mobNo},
                {"message" , message},
                {"sender" , "LocCon"}
                });
                    string result = System.Text.Encoding.UTF8.GetString(response);
                    return result;
                }

            }
            catch (Exception e)
            {

                return e + "error";
            }


        }



    }

    public static class SendConfirmationmessage
    {
        public static string SendHttpSMSConfirmation(string cusName,string hotelName, string amount, string bookingid, string mobNo, string type)
        {
            string message = "";
            if (type == "Booked")
            {
                //message = HttpUtility.UrlEncode("Your Payment of " + amount + " has been made successfully with BookingID: " + bookingid + ". Enjoy you stay !");
                message = HttpUtility.UrlEncode("Hi, "+cusName+". Thank you for choosing our hotel. We have you confirmed a reservation for "+hotelName+". Your BookingID is "+bookingid+ ". Helpline " + 917319079996 + ", LocalConnection.");
            }
            if (type == "Cancelled")
            {
                message = HttpUtility.UrlEncode("Hi, your booking at Local Conn. has been cancelled, as per your request. BookingID: "+bookingid+ ". Helpline "+917319079996+ ". Look forward to hosting you soon!");
            }
            try
            {
                using (var wb = new WebClient())
                {
                    byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                {"apikey" , "NzE4ZTJlZTAyOTBlNjgyZjNkZGMwNmY0YzBhYjE1ZjY="},
                {"numbers" , "91"+mobNo},
                {"message" , message},
                {"sender" , "LocCon"}
                });
                    string result = System.Text.Encoding.UTF8.GetString(response);
                    return result;
                }

            }
            catch (Exception e)
            {

                return e + "error";
            }
        }

        public static string SendHttpSMSConfirmationToAdmin(string name, string amount, string bookingid, string mobNo, string type,string bookingDate)
        {
            string message = "";
            if (type == "Booked")
            {
                message = HttpUtility.UrlEncode("Hi, Customer "+name+" has made a new booking. BookingID: "+bookingid+", Amount: "+ amount + ", Booking Date: "+bookingDate+". LocalConnection");
            }
            if (type == "Cancelled")
            {
                message = HttpUtility.UrlEncode("Hi, Customer "+name+" has cancelled the booking. BookingID: "+bookingid+", Amount: "+amount+". LocalConnection");

            }
            try
            {
                using (var wb = new WebClient())
                {
                    byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                {"apikey" , "NzE4ZTJlZTAyOTBlNjgyZjNkZGMwNmY0YzBhYjE1ZjY="},
                {"numbers" , "91"+mobNo},
                {"message" , message},
                {"sender" , "LocCon"}
                });
                    string result = System.Text.Encoding.UTF8.GetString(response);
                    return result;
                }
            }
            catch (Exception e)
            {

                return e + "error";
            }
        }



    }
}
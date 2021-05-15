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
        public static string SendHttpSMSRequest(string otp, string mobNo,string type)
        {
            string message = "";
            if (type == "Register")
            {
               message = HttpUtility.UrlEncode("Your OTP for LocalConnection is "+otp+ "%nRegards,%nLocalConnection");
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

                return e +"error";
            }


        }


   
    }

    public static class SendConfirmationmessage
    {
        public static string SendHttpSMSConfirmation(string amount,string bookingid, string mobNo, string type)
        {
            string message = "";
            if (type == "Booked")
            {
                message = HttpUtility.UrlEncode("Your Payment of "+amount+" has been made successfully with BookingID: "+bookingid+". Enjoy you stay !");
            }
            if (type == "Cancelled")
            {
                message = HttpUtility.UrlEncode("Your Bookiing for "+ amount + " has been successfully cancelled. Your refund will be initiated within 24 hrs. Regards LocalConnection");
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
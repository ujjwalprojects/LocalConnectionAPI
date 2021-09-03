using System;
using System.Collections.Generic;
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
        public static void SendHttpSMSRequest(string otp, string mobNo)
        {
            string messageID = "";
            string message = "OTP to Login in Mall App is - " + otp + ". Do not share it with anyone. Valid for 1 hour.";
            string url = "http://login.netspeq.com/api/sendsms.php?user=hrdd&apikey=qvUXYp4BgVImGlsUWkpQ&mobile=" + mobNo + "&message=" + message + "&senderid=HRDSAM&type=txt";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream stream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            messageID = reader.ReadToEnd();
                        }
                    }

                }

            }
            catch (Exception)
            {

                messageID = "error";
            }


        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblTrnUserOTP
    {
        [Key]
        public long OTPID { get; set; }
        public string UserMobileNo { get; set; }
        public string OTPNo { get; set; }
        public DateTime GeneratedDateTime { get; set; }
        public bool IsVerified { get; set; }
    }
    public class OTPModel
    {
        public string MobileNo { get; set; }
        public string Type { get; set; }
        public string OTP { get; set; }
    }

}

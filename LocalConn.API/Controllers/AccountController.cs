﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using LocalConn.API.Models;
using LocalConn.API.Providers;
using LocalConn.API.Results;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Configuration;
using System.Web.Configuration;
using System.Net.Mail;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using LocalConn.Entities.Models;
using LocalConn.API.Helper;

namespace LocalConn.API.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private string domainUrl = ConfigurationManager.AppSettings["WebUrl"];
        EFDBContext db = new EFDBContext();

        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
            //var provider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("StatePortal");
            //UserManager.UserTokenProvider = new Microsoft.AspNet.Identity.Owin.DataProtectorTokenProvider<ApplicationUser>(
            //    provider.Create("ResetPassword"));
            var dataProtectionProvider = Startup.DataProtectionProvider;
            UserManager.UserTokenProvider =
            new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("tourtravels123"));
        }
        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                UserName = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                UserName = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<string> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return "Error: Missing required fields.";
            }

            ApplicationUser user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
                return "Invalid User details.";

            IdentityResult result = await UserManager.ChangePasswordAsync(user.Id, model.OldPassword,
                model.NewPassword);
            if (result.Succeeded)
                return "Success";

            return "Error while changing password. Try again later";
        }
        [AllowAnonymous]
        [Route("ForgotPassword")]
        [HttpPost]
        public async Task<string> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return "Error: Missing required fields.";
                }
                ApplicationUser user = await UserManager.FindByEmailAsync(model.Email);

                if (user == null)
                    return "No user registered with the given Email";
                //if (!user.IsActivated)
                //    return "Account has not been activated, reset link cannot be sent";
                if (!user.IsActive)
                    return "Account has been disabled by admin";

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = domainUrl + "/Account/ResetPassword?userId=" + user.Id + "&code=" + System.Web.HttpUtility.UrlEncode(code);
                string msg = "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>";
                await SendMail(model.Email, msg, "Reset");
                return "Success";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        [AllowAnonymous]
        [Route("ForgotPasswordWeb")]
        [HttpPost]
        public async Task<string> ForgotPasswordWeb(ForgotPasswordModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return "Error: Missing required fields.";
                }
                ApplicationUser user = await UserManager.FindByNameAsync(model.MobileNo);

                if (user == null)
                    return "No user registered with the given Email";
                if (!user.PhoneNumberConfirmed)
                {
                    return "Verify Mobile No";
                }
                   
                if (!user.IsActive)
                    return "Account has been disabled by admin";
                else
                {
                    IdentityResult result;
                    result = await UserManager.RemovePasswordAsync(user.Id);
                    result = await UserManager.AddPasswordAsync(user.Id, model.Password);
                    IHttpActionResult errorResult = GetErrorResult(result);

                    if (errorResult != null)
                    {
                        return "Error"+errorResult;
                    }

                }
                return "Success";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<string> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return "Invalid Parameters";
                }
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    return "No user found for the given email.";
                }
                //if (!user.IsActivated)
                //    return "Account has not been activated, please activate to reset password";
                if (!user.IsActive)
                    return "Account has been disabled by admin";

                var code = System.Web.HttpUtility.UrlDecode(model.Code).Replace(' ', '+');
                var result = await UserManager.ResetPasswordAsync(user.Id, code, model.Password);
                if (result.Succeeded)
                {
                    return "Success";
                }
                return "Error: " + result.Errors.ElementAt(0).ToString();
            }
            catch (Exception)
            {
                return "Network Error.";
            }
        }
        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            IdentityUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            //if (hasRegistered)
            //{
            //    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            //    ClaimsIdentity oAuthIdentity = await UserManager.CreateIdentityAsync(user,
            //        OAuthDefaults.AuthenticationType);
            //    ClaimsIdentity cookieIdentity = await UserManager.CreateIdentityAsync(user,
            //        CookieAuthenticationDefaults.AuthenticationType);
            //    AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
            //    Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            //}
            //else
            //{
            //    IEnumerable<Claim> claims = externalLogin.GetClaims();
            //    ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
            //    Authentication.SignIn(identity);
            //}

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register from admin section
        [AllowAnonymous]
        [Route("Register")]
        public async Task<string> Register(RegisterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return "Error: Missing required fields.";
                }

                ApplicationUser existingUser = await UserManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    return "Email already in use by other user, please choose another email";
                }

                ApplicationUser user = new ApplicationUser() { Email = model.Email, ProfileName = model.ProfileName, UserName = model.Email, PhoneNumber = model.MobileNo, IsActive = true };

                //string password = Membership.GeneratePassword(6, 1);

                IdentityResult result = await UserManager.CreateAsync(user, model.Email);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "Customer");

                    //send email..
                    //var code = await UserManager.GenerateUserTokenAsync("New", user.Id);
                    //var callbackUrl = domainUrl + "/Account/New?userId=" + user.Id + "&code=" + System.Web.HttpUtility.UrlEncode(code) + "&role=" + StringEncodeDecoder.Encrypt(model.RoleName, "userrole");
                    //string msg = "Please activate your " + model.RoleName + " account by clicking <a href=\"" + callbackUrl + "\">here</a>";
                    //await SendMail(model.Email, msg, "New");

                    return "User Registered.";
                }

                return "Error: " + string.Join("; ", result.Errors
                                         .Select(x => x));

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }


        //Customer Registers him/her self from app
        [AllowAnonymous]
        [Route("registercustomer")]
        public async Task<string> RegisterCustomer(RegisterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return "Error: Missing required fields.";
                }

                ApplicationUser existingUser = UserManager.FindByName(model.MobileNo);
                ApplicationUser emailCheck = UserManager.FindByEmail(model.Email);
                if (existingUser != null)
                {
                    return "Phone Number already in use by other user, please choose another Phone Number";
                }
                if (emailCheck != null)
                {
                    return "Email already in use by other user, please choose another Email";
                }

                ApplicationUser user = new ApplicationUser() { Email = model.Email,RoleName= "Customer", ProfileName = model.ProfileName, UserName = model.MobileNo, PhoneNumber = model.MobileNo, IsActive = true };

                //string password = Membership.GeneratePassword(6, 1);

                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "Customer");

                    //send email..
                    //var code = await UserManager.GenerateUserTokenAsync("New", user.Id);
                    //var callbackUrl = domainUrl + "/Account/New?userId=" + user.Id + "&code=" + System.Web.HttpUtility.UrlEncode(code) + "&role=" + StringEncodeDecoder.Encrypt(model.RoleName, "userrole");
                    //string msg = "Please activate your " + model.RoleName + " account by clicking <a href=\"" + callbackUrl + "\">here</a>";
                    //await SendMail(model.Email, msg, "New");

                    return "Success";
                }

                return "Error: " + string.Join("; ", result.Errors
                                         .Select(x => x));

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }


        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            ApplicationUser user = new ApplicationUser
            {
                UserName = model.UserName
            };
            user.Logins.Add(new IdentityUserLogin
            {
                LoginProvider = externalLogin.LoginProvider,
                ProviderKey = externalLogin.ProviderKey
            });
            IdentityResult result = await UserManager.CreateAsync(user);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserManager.Dispose();
            }

            base.Dispose(disposing);
        }
        private async Task SendMail(string email, string msg, string mailFor = "")
        {
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
            mail.From = new MailAddress(settings.Smtp.Network.UserName, "Tour Travels");
            mail.To.Add(email);
            mail.Priority = MailPriority.High;

            switch (mailFor)
            {
                case "Reset":
                    mail.Subject = "Link To Reset Password - Tour Travels";
                    mailbody.Append("<h4>Link To Reset Password</h4>");
                    mailbody.Append("<p>" + msg + ". You will be redirected to reset password page, please choose new password for your account.</p>");
                    mailbody.Append("<p>Timestamp: " + DateTime.Now.ToString("dd MMM yyyy HH:mm tt") + "</p>");
                    mailbody.Append("<i>This is an auto generated mail, please do not reply.</i>");
                    break;
                case "New":
                    mail.Subject = "New Account Registration - Tour Travels";
                    mailbody.Append("<h4>Your account has been registered in the Tour Travels</h4>");
                    mailbody.Append("<p>" + msg + ".</p>");
                    mailbody.Append("<p>You can login in your account only after activation. After successful activation, you will be asked to set new password for your account.</p>");
                    mailbody.Append("<p>Regards, <br/> Timestamp: " + DateTime.Now.ToString("dd MMM yyyy HH:mm tt") + "</p>");
                    mailbody.Append("<i>This is an auto generated mail, please do not reply.</i>");
                    break;
                default:
                    break;

            }

            mail.Body = mailbody.ToString();
            mail.IsBodyHtml = true;

            try
            {
                await client.SendMailAsync(mail);
            }
            catch (Exception)
            {
                //throw;
            }

        }

        #region App

        [AllowAnonymous]
        [Route("getprofiledtl")]
        [HttpGet]
        public async Task<UserDetailModel> getProfileDtl(string UserID)
        {
            var user = User.Identity.GetUserId();
            UserDetailModel obj = new UserDetailModel();
            var parID = new SqlParameter("@UserID", UserID);
            obj = await db.Database.SqlQuery<UserDetailModel>("udspLCAppGetUserProfile @UserID", parID).FirstOrDefaultAsync();
            return obj;
        }

        [AllowAnonymous]
        [Route("updateprofile")]
        [HttpPost]
        public async Task<string> UpdateProfile(UserDetailModel obj)
        {
            try
            {
                string Results = "";
                var parID = new SqlParameter("@UserID", obj.UserID);
                var parPName = new SqlParameter("@ProfileName", obj.ProfileName);
                var parEmail = new SqlParameter("@Email", obj.Email);
                Results = await db.Database.SqlQuery<string>("udspLCAppUpdateProfile @UserID, @ProfileName,@Email",
                    parID, parPName, parEmail).FirstOrDefaultAsync();

                return Results;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [Route("RegisterDevice")]
        [HttpPost]
        public async Task<string> RegisterDevice(string UserName, string DeviceID)
        {
            try
            {
                string Results = "";
                var par = new SqlParameter("@UserName", DBNull.Value);
                if (UserName != null)
                    par = new SqlParameter("@UserName", UserName);
                var parDevice = new SqlParameter("@DeviceCode", DeviceID);

                Results = await db.Database.SqlQuery<string>("utblLCMstUserDeviceCode @UserName, @DeviceCode", par, parDevice).FirstOrDefaultAsync();

                return Results;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
   
        #region otp
        [AllowAnonymous]
        [Route("RequestOTP")]
        [HttpPost]
        public async Task<string> RequestOTP(string MobileNo,string Type)
        {
            try
            {
                string otp = OTPGenerator.GenerateRandomOTP(6);
                utblTrnUserOTP model = new utblTrnUserOTP()
                {
                    UserMobileNo = MobileNo,
                    OTPNo = otp,
                    GeneratedDateTime = DateTime.Now,
                    IsVerified = false
                };
                if (Type == "ForgotPass")
                {
                    ApplicationUser user = UserManager.FindByName(MobileNo);
                    if (user == null)
                    {
                        return "User Not Registered";
                    }
                    db.utblTrnUserOTPs.Add(model);
                    await db.SaveChangesAsync();

                }
                else if(Type=="Register")
                {
                    db.utblTrnUserOTPs.Add(model);
                    await db.SaveChangesAsync();
                }
                
                SendOTPMessage.SendHttpSMSRequest(otp, MobileNo, Type);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        [AllowAnonymous]
        [Route("VerifyOTP")]
        [HttpPost]
        public async Task<IHttpActionResult> VerifyOTP(string MobileNo, string OTP)
        {
            try
            {
                utblTrnUserOTP curOTP = db.utblTrnUserOTPs.Where(x => x.UserMobileNo == MobileNo && x.OTPNo == OTP).FirstOrDefault();
                if (curOTP == null)
                {
                    return NotFound();
                }
                else
                {
                    TimeSpan timespan = DateTime.Now - curOTP.GeneratedDateTime;
                    int minDiff = timespan.Seconds;
                    if (minDiff > 300)
                    {
                        return BadRequest("OTP_Expired");
                    }
                    curOTP.IsVerified = true;
                    await db.SaveChangesAsync();
                    ApplicationUser user = await UserManager.FindByNameAsync(MobileNo);
                    user.PhoneNumberConfirmed = true;
                    //user.isregistered = true;
                    await UserManager.UpdateAsync(user);

                    return Ok();
                }
            }
            catch (Exception e)
            {

                return InternalServerError(e);
            }
          
        }
        [AllowAnonymous]
        [Route("RequestOTPWeb")]
        [HttpPost]
        public async Task<string> RequestOTPWeb(OTPModel obj)
        {
            try
            {
                string otp = OTPGenerator.GenerateRandomOTP(6);
                utblTrnUserOTP model = new utblTrnUserOTP()
                {
                    UserMobileNo = obj.MobileNo,
                    OTPNo = otp,
                    GeneratedDateTime = DateTime.Now,
                    IsVerified = false
                };
                if (obj.Type == "ForgotPass")
                {
                    ApplicationUser user = UserManager.FindByName(obj.MobileNo);
                    if (user == null)
                    {
                        return "User Not Registered";
                    }
                    db.utblTrnUserOTPs.Add(model);
                    await db.SaveChangesAsync();

                }
                else if (obj.Type == "Register")
                {
                    db.utblTrnUserOTPs.Add(model);
                    await db.SaveChangesAsync();
                }

                SendOTPMessage.SendHttpSMSRequest(otp, obj.MobileNo, obj.Type);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        [AllowAnonymous]
        [Route("VerifyOTPWeb")]
        [HttpPost]
        public async Task<string> VerifyOTPWeb(OTPModel model)
        {
            try
            {
                utblTrnUserOTP curOTP = db.utblTrnUserOTPs.Where(x => x.UserMobileNo == model.MobileNo && x.OTPNo == model.OTP).FirstOrDefault();
                if (curOTP == null)
                {
                    return "User Not Registered";
                }
                else
                {
                    TimeSpan timespan = DateTime.Now - curOTP.GeneratedDateTime;
                    int minDiff = timespan.Seconds;
                    if (minDiff > 300)
                    {
                        return "OTP_Expired";
                    }
                    curOTP.IsVerified = true;
                    await db.SaveChangesAsync();
                    ApplicationUser user = await UserManager.FindByNameAsync(model.MobileNo);
                    user.PhoneNumberConfirmed = true;
                    //user.isregistered = true;
                    await UserManager.UpdateAsync(user);

                    return "Success";
                }
            }
            catch (Exception e)
            {

                return "Error"+e;
            }

        }



        [Route("setpasswordapp")]
        [HttpPost]
        public async Task<IHttpActionResult> SetPasswordApp(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser user = UserManager.FindByName(model.MobileNo);
            IdentityResult result;
            result = await UserManager.RemovePasswordAsync(user.Id);
            result = await UserManager.AddPasswordAsync(user.Id, model.Password);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }
        //used in case of forgot password from web
        [Route("webpasswordset")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> WebPasswordSet(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser user = UserManager.FindByName(model.MobileNo);
            IdentityResult result;
            result = await UserManager.RemovePasswordAsync(user.Id);
            result = await UserManager.AddPasswordAsync(user.Id, model.Password);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [Route("changepasswordapp")]
        [HttpPost]
        public async Task<string> ChangePasswordApp(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return "Error: Missing required fields.";
            }
            //email contains mobile no
            ApplicationUser user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
                return "Invalid User details.";

            IdentityResult result = await UserManager.ChangePasswordAsync(user.Id, model.OldPassword,
                model.NewPassword);
            if (result.Succeeded)
                return "Success";

            return "Error while changing password. Try again later";
        }




        //version code checker
        [HttpGet]
        [Route("VersionCodeUpdate")]
        public string GetVersionCode(int ObjVersion)
        {
            //1st parameter set less than 2 if update pop up to show to user
            //2nt parameter - Version Code to display to the user
            //3rd Parameter = Type of Prompt to update app force or optional
            //manipulate from here not from the  app optional if minor changes 
            if (ObjVersion == 2)
            {
                return "2/1.0.4/Force";
                //return "2/1.0.4/Optional";
            }
            else
            {
                return "2/1.1.0/Force";
                //return "2/1.0.4/Optional";
            }
        }



        #endregion
        #endregion


        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}

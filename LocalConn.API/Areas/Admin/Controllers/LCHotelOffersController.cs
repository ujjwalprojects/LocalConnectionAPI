using LocalConn.Entities.Dal;
using LocalConn.Entities.Models;
using LocalConn.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LocalConn.API.Areas.Admin.Controllers
{
    [Authorize]
    [RoutePrefix("api/Admin/lchoteloffer")]
    public class LCHotelOffersController : ApiController
    {
        dalLCHotel objLCHotel = new dalLCHotel();
        private string FileUrl = ConfigurationManager.AppSettings["FileURL"];

        [HttpGet]
        [Route("HotelOfferList")]
        public async Task<HotelOfferVM> Hotelofferlist(int PageNo, int PageSize, string SearchTerm)
        {
            return await objLCHotel.GetHotelOfferListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("SaveHotelOffer")]
        public async Task<string> SaveHotelOffer(SaveHotelOffer model)
        {
            if (ModelState.IsValid)
            {
                if (!model.HotelOffer.OfferImagePath.Contains(".jpg"))
                {
                    Random rand = new Random();
                    string name =  "FeatureOffer_" + DateTime.Now.ToString("yyyyMMdd") + "_" + rand.Next(50) + ".jpg";
                    string mappath = "~/Uploads/FeatureOffers";
                    string normal_result = SaveImage(model.HotelOffer.OfferImagePath, name, mappath);
                    if (normal_result.Contains("Error"))
                    {
                        string stringerror = normal_result;
                        return "Unable to upload image" + stringerror;
                    }
                    model.HotelOffer.OfferImagePath = FileUrl + "FeatureOffers/" + normal_result;
                }

                return await objLCHotel.SaveHotelOfferAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("HotelOfferByID")]
        public async Task<utblLCFeatureOffer> HotelOfferByID(long OfferID)
        {
            return await objLCHotel.GetHotelOfferByIDAsync(OfferID);
        }
        [HttpGet]
        [Route("OfferHotellist")]
        public async Task<IEnumerable<long>> OfferHotellist(long id)
        {
            return await objLCHotel.GetOfferHotelsAsync(id);
        }


        #region Helper
        private string SaveImage(string imageStrNormal, string name, string mappath)
        {
            try
            {
                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(mappath), name);
                var folderpath = System.Web.HttpContext.Current.Server.MapPath(mappath);

                //Check if normal directory exist
                if (!System.IO.Directory.Exists(folderpath))
                {
                    System.IO.Directory.CreateDirectory(folderpath); //Create directory if it doesn't exist
                }
                string x = imageStrNormal.Replace("data:image/jpeg;base64,", "");
                byte[] imageBytes = Convert.FromBase64String(x);

                System.IO.File.WriteAllBytes(path, imageBytes);

                //var thumb_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads/Cities"), name);
                //var thumb_folderpath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/Cities");

                ////Check if thumb directory exist
                //if (!System.IO.Directory.Exists(thumb_folderpath))
                //{
                //    System.IO.Directory.CreateDirectory(thumb_folderpath); //Create directory if it doesn't exist
                //}
                //string thumb_x = imageStrThumb.Replace("data:image/jpeg;base64,", "");
                //byte[] thumb_imageBytes = Convert.FromBase64String(thumb_x);

                //System.IO.File.WriteAllBytes(thumb_path, thumb_imageBytes);

                return name;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        private void DeleteFile(string filepath)
        {
            try
            {
                string[] fileArr = filepath.Split('/');
                string filename = fileArr[fileArr.Length - 1];
                string serverPath = "";
                serverPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads/FeatureOffers/Normal"), filename);
                if (System.IO.File.Exists(serverPath))
                {
                    System.IO.File.Delete(serverPath);
                }

                string thumb_serverPath = "";
                thumb_serverPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads/FeatureOffers/Normal"), filename);
                if (System.IO.File.Exists(thumb_serverPath))
                {
                    System.IO.File.Delete(thumb_serverPath);
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }
        private void DeleteAlbumFiles(long id)
        {
            try
            {
                string serverNormalPath = @"" + System.Web.HttpContext.Current.Server.MapPath("~/Uploads/FeatureOffers/Normal");
                string albumfilename = @"" + id + "_";
                string[] NormalfileList = System.IO.Directory.GetFiles(serverNormalPath, albumfilename + "*.jpg");
                foreach (string file in NormalfileList)
                {
                    //System.Diagnostics.Debug.WriteLine(file + "will be deleted");
                    System.IO.File.Delete(file);
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }
        #endregion
    }
}

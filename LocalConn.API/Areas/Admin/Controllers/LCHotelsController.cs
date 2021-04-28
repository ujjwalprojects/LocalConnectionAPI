using LocalConn.Entities.Dal;
using LocalConn.Entities.Models;
using LocalConn.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
//using System.Web;
//using System.Web.Mvc;


namespace LocalConn.API.Areas.Admin.Controllers
{

    [Authorize]
    [RoutePrefix("api/Admin/lchotelconfig")]
    public class LCHotelsController : ApiController
    {
        dalLCHotel objLCHotel = new dalLCHotel();
        private string FileUrl = ConfigurationManager.AppSettings["FileURL"];

        #region Hotels
        [HttpGet]
        [Route("LCHotels")]
        public async Task<LCHotelVM> LCHotels(int PageNo, int PageSize, string SearchTerm)
        {
            return await objLCHotel.GetLCHotelsAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("SaveLCHotel")]
        public async Task<string> SaveHotel(LCHotelManageModel model)
        {
            if (ModelState.IsValid)
            {
                return await objLCHotel.SaveLCHotelAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpPost]
        [Route("UpdateLCHotelRate")]
        public async Task<string> UpdateLCHotelRate(LCHotelSaveModel model)
        {
            if (ModelState.IsValid)
            {
                return await objLCHotel.UpdateLCHotelRateAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("LCHotelByID")]
        public async Task<utblLCHotel> HotelByID(long id)
        {
            return await objLCHotel.GetLCHotelByIDAsync(id);
        }
        [HttpDelete]
        [Route("DeleteLCHotel")]
        public async Task<string> DeleteLCHotel(long id)
        {
            return await objLCHotel.DeleteLCHotelAsync(id);
        }
        [HttpGet]
        [Route("LCHotelDD")]
        public async Task<IEnumerable<LCHotelDD>> AllLCHotels()
        {
            return await objLCHotel.GetAllLCHotelAsync();
        }
        #endregion

        #region Feature Hotels
        [HttpGet]
        [Route("FeatHotels")]
        public async Task<FeatHotelsVM> FeatHotels(int PageNo, int PageSize, string SearchTerm)
        {
            return await objLCHotel.GetFeatHotelsAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("SaveFeatHotels")]
        public async Task<string> SaveFeatHotels(utblLCFeaturedHotel model)
        {
            if (ModelState.IsValid)
            {
                return await objLCHotel.SaveFeatHotelsAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("FeatHotelsByID")]
        public async Task<utblLCFeaturedHotel> FeatHotelsByID(long id)
        {
            return await objLCHotel.GetFeatHotelsByIDAsync(id);
        }
        [HttpDelete]
        [Route("DeleteFeatHotels")]
        public async Task<string> DeleteFeatHotels(long id)
        {
            return await objLCHotel.DeleteFeatHotelsAsync(id);
        }
        #endregion

        #region Hotel Images
        [HttpGet]
        [Route("LCHotelImages")]
        public async Task<LCHotelImageVM> LCHotelImages(long HotelID, int PageNo, int PageSize)
        {
            return await objLCHotel.GetHotelImagesAsync(HotelID, PageNo, PageSize);
        }
        [HttpPost]
        [Route("SaveLCHotelImages")]
        public async Task<string> SaveLCHotelImages(utblLCHotelImage model)
        {
            if (ModelState.IsValid)
            {
                if (!model.PhotoThumbPath.Contains(".jpg"))
                {
                    Random rand = new Random();
                    string name = model.HotelID + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + rand.Next(50) + ".jpg";
                    string normal_result = SaveImage(model.PhotoNormalPath, model.PhotoThumbPath, name);
                    if (normal_result.Contains("Error"))
                    {
                        string stringerror = normal_result;
                        return "Unable to upload image" + stringerror;
                    }
                    model.PhotoNormalPath = FileUrl + "HotelImages/Normal/" + normal_result;
                    model.PhotoThumbPath = FileUrl + "HotelImages/Thumb/" + normal_result;
                }
                string result = await objLCHotel.SaveHotelImagesAsync(model);
                //if (result.ToLower().Contains("error"))
                //{
                //    DeleteFile(name);
                //}
                return result;
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("LCHotelImagesByID")]
        public async Task<utblLCHotelImage> LCHotelImagesByID(long id)
        {
            return await objLCHotel.GetLCHotelImagesByIDAsync(id);
        }
        [HttpDelete]
        [Route("DeleteLCHotelImages")]
        public async Task<string> DeleteLCHotelImages(long id)
        {
            string result = await objLCHotel.DeleteLCHotelImagesAsync(id);
            if (result.ToLower().Contains("error"))
            {
                DeleteFile(result);
            }
            return result;
        }
        [HttpPost]
        [Route("MakeCoverImage")]
        public async Task<string> MakeCoverImage(long hotelid, long imageid)
        {
            return await objLCHotel.MakeCoverImageAsync(hotelid, imageid);
        }
        [HttpGet]
        [Route("HotelPremisesDD")]
        public async Task<IEnumerable<utblLCMstHotelPremise>> HotelPremisesDD()
        {
            return await objLCHotel.GetAllLCHotelPremisesAsync();
        }
        [HttpGet]
        [Route("RoomTypeDD")]
        public async Task<IEnumerable<RoomTypeDD>> GetRoomTypeByID(long id)
        {
            return await objLCHotel.GetRoomTypeByID(id);
        }
      
        #endregion

        #region HotelRoomTypeMap
        [HttpGet]
        [Route("GetHotelRoomTypeMapList")]
        public async Task<IEnumerable<HotelRoomTypeMapView>> GetHotelRoomTypeMapList(long id)
        {
            return await objLCHotel.GetHotelRoomTypeAsync(id);
        }
        [HttpPost]
        [Route("SaveHotelRoomTypeMap")]
        public async Task<string> SaveHotelRoomTypeMap(HotelRoomTypeMap model)
        {
            if (ModelState.IsValid)
            {
                return await objLCHotel.SaveHotelRoomTypeMapAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("GetHotelRoomTypeMapByID")]
        public async Task<HotelRoomTypeMap> GetHotelRoomTypeMapByID(long id, long rid)
        {
            return await objLCHotel.GetHotelRoomTypeMapByIDAsync(id, rid);
        }
        [HttpDelete]
        [Route("DeleteHotelRoomTypeMap")]
        public async Task<string> DeleteHotelRoomTypeMap(long id, long rid)
        {
            return await objLCHotel.DeleteHotelRoomTypeMapAsync(id,rid);
        }
        #endregion

        #region HotelAmenitiesMap
        [HttpGet]
        [Route("GetHotelAmenitiesMapList")]
        public async Task<IEnumerable<HotelAmenitiesMapView>> GetHotelAmenitiesMapList(long id)
        {
            return await objLCHotel.GetAllHotelAmenitiesMap(id);
        }
        [HttpPost]
        [Route("SaveHotelAmenitiesMap")]
        public async Task<string> SaveHotelAmenitiesMap(utblLCHotelAmenitiesMap model)
        {
            if (ModelState.IsValid)
            {
                return await objLCHotel.SaveHotelAmenitiesMapAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [Route("GetHotelAmenitiesMapByID")]
        public async Task<utblLCHotelAmenitiesMap> GetHotelAmenitiesMapByID(long id)
        {
            return await objLCHotel.GetHotelAmenitiesMapByIDAsync(id);
        }
        [HttpDelete]
        [Route("DeleteHotelAmenitiesMap")]
        public async Task<string> DeleteHotelAmenitiesMap(long id)
        {
            return await objLCHotel.DeleteHotelAmenitiesMapAsync(id);
        }
         
        #endregion

        #region HotelTerms&Cancellations
        [HttpGet]
        [Route("HotelTerms")]
        public async Task<IEnumerable<HotelTerms>> HotelTerms(long id)
        {
            return await objLCHotel.GetHotelTermsAsync(id);
        }
        [HttpGet]
        [Route("HotelCancellations")]
        public async Task<IEnumerable<HotelCancellations>> HotelCancellations(long id)
        {
            return await objLCHotel.GetHotelCancellationsAsync(id);
        }
        [HttpPost]
        [Route("SaveTermsCancellations")]
        public async Task<string> SaveTermsCancellations(HotelTermCancSaveModel model)
        {
            if (ModelState.IsValid)
            {
                return await objLCHotel.SaveTermsCancAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        #endregion

        #region NearByPoints
        [HttpGet]
        [Route("GetNearByPoints")]


        public async Task<LCNearByPointsVM> GetNearByPoints(int PageNo, int PageSize, string SearchTerm)
        {
            return await objLCHotel.GetLCNearByPointsAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("SaveNearByPoints")]
        public async Task<string> SaveNearByPoints(utblLCNearByPoint model)
        {
            if (ModelState.IsValid)
            {
                return await objLCHotel.SaveLCNearByPointsAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("NearByPointsByID")]
        public async Task<utblLCNearByPoint> NearByPointsByID(long id)
        {
            return await objLCHotel.GetLCNearByPointsByIDAsync(id);
        }
        [HttpDelete]
        [Route("DeleteNearByPoints")]
        public async Task<string> DeleteNearByPoints(long id)
        {
            return await objLCHotel.DeleteLCNearByPointsAsync(id);
        }
        [HttpGet]
        [Route("GetLCNearByPointsMapList")]
        public async Task<IEnumerable<LCNearByPointsView>> GetLCNearByPointsMapList(long id)
        {
            return await objLCHotel.GetLCNearByPointsAsync(id);
        }
        [HttpGet]
        [Route("NearByDD")]
        public async Task<IEnumerable<LCNearBysTypeDD>> NearByDD()
        {
            return await objLCHotel.GetLCNearBysTypeDDAsync();
        }
        #endregion  

        #region Helper
        private string SaveImage(string imageStrNormal, string imageStrThumb, string name)
        {
            try
            {
                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads/HotelImages/Normal"), name);
                var folderpath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/HotelImages/Normal");

                //Check if normal directory exist
                if (!System.IO.Directory.Exists(folderpath))
                {
                    System.IO.Directory.CreateDirectory(folderpath); //Create directory if it doesn't exist
                }
                string x = imageStrNormal.Replace("data:image/jpeg;base64,", "");
                byte[] imageBytes = Convert.FromBase64String(x);

                System.IO.File.WriteAllBytes(path, imageBytes);

                var thumb_path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads/HotelImages/Thumb"), name);
                var thumb_folderpath = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/HotelImages/Thumb");

                //Check if thumb directory exist
                if (!System.IO.Directory.Exists(thumb_folderpath))
                {
                    System.IO.Directory.CreateDirectory(thumb_folderpath); //Create directory if it doesn't exist
                }
                string thumb_x = imageStrThumb.Replace("data:image/jpeg;base64,", "");
                byte[] thumb_imageBytes = Convert.FromBase64String(thumb_x);

                System.IO.File.WriteAllBytes(thumb_path, thumb_imageBytes);

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
                serverPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads/HotelImages/Normal"), filename);
                if (System.IO.File.Exists(serverPath))
                {
                    System.IO.File.Delete(serverPath);
                }

                string thumb_serverPath = "";
                thumb_serverPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads/HotelImages/Thumb"), filename);
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
                string serverNormalPath = @"" + System.Web.HttpContext.Current.Server.MapPath("~/Uploads/HotelImages/Normal");
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

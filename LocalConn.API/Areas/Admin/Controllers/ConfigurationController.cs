using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using LocalConn.Entities.Dal;
using LocalConn.Entities.Models;
using LocalConn.Entities.ViewModels;

namespace LocalConn.API.Areas.Admin.Controllers
{
    [Authorize]
    [RoutePrefix("api/Admin/configuration")]
    public class ConfigurationController : ApiController
    {
        private string FileUrl = ConfigurationManager.AppSettings["FileURL"];
        //
        // GET: /Admin/Configuration/
        dalConfigurations objDal = new dalConfigurations();

        #region Countries
        [Route("CountriesList")]
        [HttpGet]
        public async Task<IEnumerable<utblMstCountries>> CountriesList()
        {
            return await objDal.getCountriesAsync();
        }

        [HttpPost]
        [Route("addEditCountries")]
        public async Task<string> AddCountries(utblMstCountries model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.addCountriesAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("CountriesByID")]
        public async Task<utblMstCountries> CountriesByID(long id)
        {
            return await objDal.getCountryByIDAsync(id);
        }
        [HttpDelete]
        [Route("DeleteCountries")]
        public async Task<string> DeleteCountries(long id)
        {
            return await objDal.deleteCountriesAsync(id);
        }
        #endregion

        #region States
        [Route("StateList")]
        [HttpGet]
        public async Task<IEnumerable<StateView>> StateList()
        {
            return await objDal.GetStatesAsync();
        }

        [HttpPost]
        [Route("SaveState")]
        public async Task<string> SaveState(utblMstState model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveStatesAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("StateByID")]
        public async Task<utblMstState> StateByID(long id)
        {   
            return await objDal.GetStateByIDAsync(id);
        }
        [HttpGet]
        [Route("StateByCountry")]
        public async Task<IEnumerable<utblMstState>> StateByCountry(long id)
        {
            return await objDal.GetStateByCountryAsync(id);
        }
        [HttpDelete]
        [Route("DeleteState")]
        public async Task<string> DeleteState(long id)
        {
            return await objDal.DeleteStateAsync(id);
        }
        #endregion

        #region Cities
        [Route("CitiesList")]
        [HttpGet]
        public async Task<IEnumerable<utblLCMstCitie>> CitiesList()
        {
            return await objDal.getCitiesAsync();
        }
        [HttpGet]
        [Route("Cities")]
        public async Task<CitiesVM> Cities(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetCitiesAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("SaveCities")]
        public async Task<string> SaveCities(utblLCMstCitie model)
        {
            if (ModelState.IsValid)
            {
                if(!model.CityIconPath.Contains(".jpg"))
                {
                    Random rand = new Random();
                    string name = model.CityName + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + rand.Next(50) + ".jpg";
                    string mappath = "~/Uploads/Cities";
                    string normal_result = SaveImage(model.CityIconPath, name,mappath);
                    if (normal_result.Contains("Error"))
                    {
                        string stringerror = normal_result;
                        return "Unable to upload image" + stringerror;
                    }
                    model.CityIconPath = FileUrl + "Cities/" + normal_result;
                }
            

                string result = await objDal.SaveCitiesAsync(model);
                //if (result.ToLower().Contains("error"))
                //{
                //    DeleteFile(name);
                //}
                return result;

                //return await objDal.SaveCitiesAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("CitiesByID")]
        public async Task<utblLCMstCitie> CitiesByID(long id)
        {
            return await objDal.GetCitiesByIDAsync(id);
        }
        [HttpGet]
        [Route("AllCities")]
        public async Task<IEnumerable<utblLCMstCitie>> AllCities()
        {
            return await objDal.GetAllCitiesAsync();
        }
        [HttpGet]
        [Route("CitiesByState")]
        public async Task<IEnumerable<CitiesDD>> CitiesByState(long id)
        {
            return await objDal.GetCitiesByStateAsync(id);
        }
        [HttpDelete]
        [Route("DeleteCities")]
        public async Task<string> DeleteCities(long id)
        {
            return await objDal.DeleteCitiesAsync(id);
        }
        #endregion

        #region Localities
        [HttpGet]
        [Route("Localities")]
        public async Task<LocalitiesVM> Localities(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetLocalitiesAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("SaveLocalities")]
        public async Task<string> SaveLocalities(utblLCMstLocalitie model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveLocalitiesAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("LocalitiesByID")]
        public async Task<utblLCMstLocalitie> LocalitiesByID(long id)
        {
            return await objDal.GetLocalitiesByIDAsync(id);
        }
        [HttpGet]
        [Route("AllLocalities")]
        public async Task<IEnumerable<utblLCMstLocalitie>> AllLocalities()
        {
            return await objDal.GetAllLocalitiesAsync();
        }
        [HttpDelete]
        [Route("DeleteLocalities")]
        public async Task<string> DeleteLocalities(long id)
        {
            return await objDal.DeleteLocalitiesAsync(id);
        }
        [HttpGet]
        [Route("LocalitiesByCity")]
        public async Task<IEnumerable<LocalitiesDD>> LocalitiesByCity(long id)
        {
            return await objDal.GetLocalitiesByCityAsync(id);
        }
        #endregion

        #region Amenities
        [HttpGet]
        [Route("Amenities")]
        public async Task<AmenitiesVM> Amenities(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetAmenitiesAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("SaveAmenities")]
        public async Task<string> SaveAmenities(utblLCMstAmenitie model)
        {
            if (ModelState.IsValid)
            {
                if (!model.AmenitiesIconPath.Contains(".jpg"))
                {
                    Random rand = new Random();
                    string name = model.AmenitiesName + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + rand.Next(50) + ".jpg";
                    string mappath = "~/Uploads/AmenitiesIcon";
                    string normal_result = SaveImage(model.AmenitiesIconPath, name, mappath);
                    if (normal_result.Contains("Error"))
                    {
                        string stringerror = normal_result;
                        return "Unable to upload image" + stringerror;
                    }
                    model.AmenitiesIconPath = FileUrl + "AmenitiesIcon/" + normal_result;
                }


                //string result = await objDal.SaveCitiesAsync(model);
                //if (result.ToLower().Contains("error"))
                //{
                //    DeleteFile(name);
                //}
                //return result;

                return await objDal.SaveAmenitiesAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("AmenitiesByID")]
        public async Task<utblLCMstAmenitie> AmenitiesByID(long id)
        {
            return await objDal.GetAmenitiesByIDAsync(id);
        }
        [HttpGet]
        [Route("AllAmenities")]
        public async Task<IEnumerable<utblLCMstAmenitie>> AllAmenities()
        {
            return await objDal.GetAllAmenitiesAsync();
        }
        [HttpDelete]
        [Route("DeleteAmenities")]
        public async Task<string> DeleteAmenities(long id)
        {
            return await objDal.DeleteAmenitiesAsync(id);
        }
        #endregion

        #region StarRating
        [HttpGet]
        [Route("StarRating")]
        public async Task<StarRatingVM> StarRating(int PageNo, int PageSize, string SearchTerm)
        {
            StarRatingVM model = new StarRatingVM();
            model = await objDal.GetStarRatingAsync(PageNo, PageSize, SearchTerm);
            return model;
        }
        [HttpPost]
        [Route("SaveStarRating")]
        public async Task<string> SaveStarRating(utblLCMstStarRating model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveStarRatingAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("StarRatingByID")]
        public async Task<utblLCMstStarRating> StarRatingByID(long id)
        {
            return await objDal.GetStarRatingByIDAsync(id);
        }
        [HttpGet]
        [Route("AllStarRating")]
        public async Task<IEnumerable<utblLCMstStarRating>> AllStarRating()
        {
            return await objDal.GetAllStarRatingAsync();
        }
        [HttpDelete]
        [Route("DeleteStarRating")]
        public async Task<string> DeleteStarRating(long id)
        {
            return await objDal.DeleteStarRatingAsync(id);
        }
        #endregion

        #region HomeType
        [HttpGet]
        [Route("HomeTypes")]
        public async Task<HomeTypeVM> HomeTypes(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetHomeTypeAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("SaveHomeTypes")]
        public async Task<string> SaveHomeTypes(utblLCMstHomeType model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveHomeTypeAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("HomeTypesByID")]
        public async Task<utblLCMstHomeType> HomeTypesByID(long id)
        {
            return await objDal.GetHomeTypeByIDAsync(id);
        }
        [HttpGet]
        [Route("AllHomeTypes")]
        public async Task<IEnumerable<utblLCMstHomeType>> AllHomeTypes()
        {
            return await objDal.GetAllHomeTypeAsync();
        }
        [HttpDelete]
        [Route("DeleteHomeTypes")]
        public async Task<string> DeleteHomeTypes(long id)
        {
            return await objDal.DeleteHomeTypeAsync(id);
        }
        #endregion

        #region LCRooms
        [HttpGet]
        [Route("Rooms")]
        public async Task<RoomsVM> Rooms(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetRoomsAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("SaveRooms")]
        public async Task<string> SaveRooms(utblLCRoom model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveRoomsAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("RoomsByID")]
        public async Task<utblLCRoom> RoomsByID(long id)
        {
            return await objDal.GetRoomsByIDAsync(id);
        }
        [HttpGet]
        [Route("AllRooms")]
        public async Task<IEnumerable<utblLCRoom>> AllRooms()
        {
            return await objDal.GetAllRoomsAsync();
        }
        [HttpGet]
        [Route("RoomTypeDD")]
        public List<RoomTypeDD> RoomTypeDD()
        {
            return objDal.GetRoomDDAsync();
        }
        [HttpDelete]
        [Route("DeleteRooms")]
        public async Task<string> DeleteRooms(long id)
        {
            return await objDal.DeleteRoomsAsync(id);
        }
        [HttpGet]
        [Route("HotelRoomTypeList")]
        public async Task<IEnumerable<long>> HotelRoomTypeList(long id)
        {
            return await objDal.GetHotelRoomTypeAsync(id);
        }
        #endregion

        #region Destination
        [HttpGet]
        [Route("Destinations")]
        public async Task<DestinationVM> Destinations(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetDestinationsAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("SaveDestination")]
        public async Task<string> SaveDestination(utblMstDestination model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveDestinationAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("DestinationByID")]
        public async Task<utblMstDestination> DestinationByID(long id)
        {
            return await objDal.GetDestinationByIDAsync(id);
        }
        [HttpGet]
        [Route("AllDestinations")]
        public async Task<IEnumerable<utblMstDestination>> AllDestinations()
        {
            return await objDal.GetAllDestinationsAsync();
        }
        [HttpDelete]
        [Route("DeleteDestination")]
        public async Task<string> DeleteDestination(long id)
        {
            return await objDal.DeleteDestinationAsync(id);
        }
        #endregion

        #region Package Type

        [HttpGet]
        [Route("packagelist")]
        public async Task<PackageTypeVM> PackageList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetPackageTypeListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("savepackagetype")]
        public async Task<string> SavePackage(utblMstPackageType model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SavePackageTypeAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("packagetypebyid")]
        public async Task<utblMstPackageType> PackageTypeByID(long PackageTypeID)
        {
            return await objDal.GetPackageTypeByIDAsync(PackageTypeID);
        }
        [HttpDelete]
        [Route("deletepackagetype")]
        public async Task<string> DeletePackage(long PackageTypeID)
        {
            return await objDal.DeletePackageTypeAsync(PackageTypeID);
        }
        [HttpGet]
        [Route("AllPackageTypes")]
        public async Task<IEnumerable<utblMstPackageType>> AllPackageTypes()
        {
            return await objDal.GetAllPackageTypesAsync();
        }
        #endregion

        #region Itinerary


        [HttpGet]
        [Route("destionationDD")]
        public async Task<IEnumerable<utblMstDestination>> DestionationList()
        {
            return await objDal.GetDestinationListAsync();
        }

        [HttpGet]
        [Route("itinerarylist")]
        public async Task<ItineraryVM> ItineraryList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetItineraryListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("saveitinerary")]
        public async Task<string> SaveItinerary(utblMstItinerarie model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveItineraryAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("itinerarybyid")]
        public async Task<utblMstItinerarie> ItineraryByID(long ItineraryID)
        {
            return await objDal.GetItineraryByIDAsync(ItineraryID);
        }
        [HttpDelete]
        [Route("deleteitinerary")]
        public async Task<string> Deleteitinerary(long ItineraryID)
        {
            return await objDal.DeleteItineraryAsync(ItineraryID);
        }
        [HttpGet]
        [Route("AllItineraries")]
        public async Task<IEnumerable<utblMstItinerarie>> AllItineraries()
        {
            return await objDal.GetAllItinerariesAsync();
        }
        #endregion

        #region Inclusions

        [HttpGet]
        [Route("inclusionlist")]
        public async Task<InclusionVM> InclusionsList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetInclusionListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("saveinclusion")]
        public async Task<string> SaveInclusions(utblMstInclusion model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveInclusionAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("inclusionbyid")]
        public async Task<utblMstInclusion> InclusionsByID(long InclusionID)
        {
            return await objDal.GetInclusionByIDAsync(InclusionID);
        }
        [HttpDelete]
        [Route("deleteinclusion")]
        public async Task<string> Deleteinclusions(long InclusionID)
        {
            return await objDal.DeleteInclusionAsync(InclusionID);
        }
        #endregion

        #region Exclusions

        [HttpGet]
        [Route("exclusionlist")]
        public async Task<ExclusionVM> ExclusionList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetExclusionListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("saveexclusion")]
        public async Task<string> SaveExclusions(utblMstExclusion model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveExclusionAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("exclusionbyid")]
        public async Task<utblMstExclusion> ExclusionByID(long ExclusionID)
        {
            return await objDal.GetExclusionByIDAsync(ExclusionID);
        }
        [HttpDelete]
        [Route("deleteexclusion")]
        public async Task<string> DeleteExclusion(long ExclusionID)
        {
            return await objDal.DeleteExclusionAsync(ExclusionID);
        }
        #endregion

        #region Terms
        [HttpGet]
        [Route("termlist")]
        public async Task<TermVM> TermList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetTermListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("saveterm")]
        public async Task<string> SaveExclusions(utblMstTerm model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveTermAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("termbyid")]
        public async Task<utblMstTerm> TermByID(long TermID)
        {
            return await objDal.GetTermByIDAsync(TermID);
        }
        [HttpDelete]
        [Route("deleteterm")]
        public async Task<string> DeleteTerm(long TermID)
        {
            return await objDal.DeleteTermAsync(TermID);
        }

        #endregion

        #region Banner

        [HttpGet]
        [Route("bannerlist")]
        public async Task<BannerVM> BannerList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetBannerListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("savebanner")]
        public async Task<string> SaveBanner(utblMstBanner model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveBannerAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("bannerbyid")]
        public async Task<utblMstBanner> BannerByID(long BannerID)
        {
            return await objDal.GetBannerByIDAsync(BannerID);
        }
        [HttpDelete]
        [Route("deletebanner")]
        public async Task<string> DeleteBanner(long BannerID)
        {
            return await objDal.DeleteBannerAsync(BannerID);
        }

       

        #endregion

        #region Activity
        [HttpGet]
        [Route("activitylist")]
        public async Task<ActivityVM> ActivityList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetActivityListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("saveactivity")]
        public async Task<string> SaveActivity(utblMstActivitie model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveActivityAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("activitybyid")]
        public async Task<utblMstActivitie> ActivityByID(long id)
        {
            return await objDal.GetActivityByIDAsync(id);
        }
        [HttpDelete]
        [Route("deleteactivity")]
        public async Task<string> DeleteActivity(long ActicityID)
        {
            return await objDal.DeleteActivityAsync(ActicityID);
        }
        #endregion

        #region Tour Cancellation
        [HttpGet]
        [Route("tourcancellist")]
        public async Task<TourCancelVM> TourCancelList(int PageNo, int PageSize, string SearchTerm)
        {
            return await objDal.GetTourCancelListAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("savetourcancel")]
        public async Task<string> SaveTourCancel(utblMstTourCancellation model)
        {
            if (ModelState.IsValid)
            {
                return await objDal.SaveTourCancelAsync(model);
            }
            string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
            return "Operation Error: " + messages;
        }
        [HttpGet]
        [Route("tourcancelbyid")]
        public async Task<utblMstTourCancellation> TourCancelByID(long CancellationID)
        {
            return await objDal.GetTourCancelByIDAsync(CancellationID);
        }
        [HttpDelete]
        [Route("deletetourcancel")]
        public async Task<string> DeleteTourCancel(long CancellationID)
        {
            return await objDal.DeleteTourCancelAsync(CancellationID);
        }


        #endregion

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
                serverPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads/HotelImages/Normal"), filename);
                if (System.IO.File.Exists(serverPath))
                {
                    System.IO.File.Delete(serverPath);
                }

                string thumb_serverPath = "";
                thumb_serverPath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Uploads/Cities"), filename);
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
                string serverNormalPath = @"" + System.Web.HttpContext.Current.Server.MapPath("~/Uploads/Cities");
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
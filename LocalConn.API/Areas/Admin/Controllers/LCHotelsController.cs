using LocalConn.Entities.Dal;
using LocalConn.Entities.Models;
using LocalConn.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LocalConn.API.Areas.Admin.Controllers
{

    [Authorize]
    [RoutePrefix("api/Admin/lchotelconfig")] 
    public class LCHotelsController : ApiController
    {
        dalLCHotel objLCHotel = new dalLCHotel();

        #region Hotels
        [HttpGet]
        [Route("LCHotels")]
        public async Task<LCHotelVM> LCHotels(int PageNo, int PageSize, string SearchTerm)
        {
            return await objLCHotel.GetLCHotelsAsync(PageNo, PageSize, SearchTerm);
        }
        [HttpPost]
        [Route("SaveLCHotel")]
        public async Task<string> SaveHotel(LCHotelSaveModel model)
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
        #endregion
    }
}

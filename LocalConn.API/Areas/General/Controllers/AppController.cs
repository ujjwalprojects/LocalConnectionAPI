using LocalConn.Entities.Dal;
using LocalConn.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LocalConn.API.Areas.General.Controllers
{
    [RoutePrefix("api/general/app")]
    public class AppController : ApiController
    {
        dalApp objDal = new dalApp();

        [HttpGet]
        [Route("getcitylist")]
        public async Task<List<CityList>> getCityList(string StateID)
        {
            return await objDal.getCityMenuList(StateID);
        }

        [HttpGet]
        [Route("gethotellist")]
        public async Task<List<HotelList>> getHotelList(string HotelID)
        {
            return await objDal.getHotelMenuList(HotelID);
        }
    }
}

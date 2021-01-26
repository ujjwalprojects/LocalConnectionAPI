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
        [Route("getftHotellist")]
        public async Task<List<HotelList>> getFtHotelList(string HomeTypeID)
        {
            return await objDal.getFtHotelList(HomeTypeID);
        }

        [HttpGet]
        [Route("gethotellist")]
        public async Task<List<HotelList>> getHotelList(string HomeTypeID)
        {
            List<HotelList> obj = new List<HotelList>();
            obj=  await objDal.getHotelMenuList(HomeTypeID);
            return obj;
        }

        [HttpGet]
        [Route("getresortlist")]
        public async Task<List<HotelList>> getResortList(string HomeTypeID)
        {
            return await objDal.getResortMenuList(HomeTypeID);
        }
        [HttpGet]
        [Route("getlodgelist")]
        public async Task<List<HotelList>> getLodgeList(string HomeTypeID)
        {
            return await objDal.getLodgeMenuList(HomeTypeID);
        }
        [HttpGet]
        [Route("gethomestaylist")]
        public async Task<List<HotelList>> getHomestayList(string HomeTypeID)
        {
            return await objDal.getHomestayMenuList(HomeTypeID);
        }
        [HttpGet]
        [Route("getghouselist")]
        public async Task<List<HotelList>> getGHouseList(string HomeTypeID)
        {
            return await objDal.getGHouseMenuList(HomeTypeID);
        }

        [HttpGet]
        [Route("gethoteldtl")]
        public async Task<HotelDtl> getHotelDtl(string HotelID)
        {
            return await objDal.getHotelDtl(HotelID);
        }

    }
}

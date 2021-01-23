using LocalConn.Entities.Models;
using LocalConn.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Dal
{
    public class dalApp
    {
        EFDBContext objDB = new EFDBContext();

        public async Task<List<CityList>> getCityMenuList(string StateID)
        {
            try
            {
                var parID = new SqlParameter("@StateID", StateID);
                return await objDB.Database.SqlQuery<CityList>("udspLCAppGetCityList @StateID", parID).ToListAsync();

            }
            catch (Exception e)
            {

                throw e;
            }
            

        }


        public async Task<List<HotelList>> getHotelMenuList(string HotelID)
        {
            try
            {
                var parID = new SqlParameter("@HotelID", HotelID);
                return await objDB.Database.SqlQuery<HotelList>("udspLCAppGetHotelList @HotelID", parID).ToListAsync();

            }
            catch (Exception e)
            {

                throw e;
            }


        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalConn.Entities.Models;
using System.Data.Entity;
using System.Data.SqlClient;
using LocalConn.Entities.ViewModels;
using System.Data;
using LocalConn.Entities.Utility;

namespace LocalConn.Entities.Dal
{
    public class dalLCHotel
    {
        EFDBContext db = new EFDBContext();
        public async Task<LCHotelVM> GetLCHotelsAsync(int pageno, int pagesize, string sterm)
        {
            LCHotelVM model = new LCHotelVM();
            var parStart = new SqlParameter("@Start", (pageno - 1) * pagesize);
            var parEnd = new SqlParameter("@PageSize", pagesize);

            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(sterm == null || sterm == ""))
                parSearchTerm.Value = sterm;
            // setting stored procedure OUTPUT value
            // This return total number of rows, and avoid two database call for data and total number of rows 
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            model.LCHotels = await db.Database.SqlQuery<LCHotelView>("udspLCHotelPaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveLCHotelAsync(LCHotelSaveModel model)
        {
            try
            {
                var parHotelID = new SqlParameter("@HotelID", model.HotelID);
                var parHotelName = new SqlParameter("@HotelName", model.HotelName);
                var parHotelAddress = new SqlParameter("@HotelAddress", model.HotelAddress);
                var parHotelDesc = new SqlParameter("@HotelDesc", model.HotelDesc);
                var parHotelContactNo = new SqlParameter("@HotelContactNo", model.HotelContactNo);
                var parHotelEmail = new SqlParameter("@HotelEmail", model.HotelEmail ?? "");
                var parCountryID = new SqlParameter("@CountryID", model.CountryID);
                var parStateID = new SqlParameter("@StateID", model.StateID);
                var parCityID = new SqlParameter("@CityID", model.CityID);
                var parLocalityID = new SqlParameter("@LocalityID", model.LocalityID);
                var parHomeTypeID = new SqlParameter("@HomeTypeID", model.HomeTypeID);
                var parStarRatingID = new SqlParameter("@StarRatingID", model.StarRatingID);
                var parHotelBaseFare = new SqlParameter("@HotelBaseFare", model.HotelBaseFare);
                //var parHotelHitCount = new SqlParameter("@HotelHitCount", model.HotelHitCount);
                var parMetaText = new SqlParameter("@MetaText", model.MetaText);
                var parTotalSingleRooms = new SqlParameter("@TotalSingleRooms", model.TotalSingleRooms);
                var parTotalDoubleRooms = new SqlParameter("@TotalDoubleRooms", model.TotalDoubleRooms);

                return await db.Database.SqlQuery<string>("udspLCHotelSave @HotelID, @HotelName, @HotelAddress, @HotelDesc, @HotelContactNo, @HotelEmail, @CountryID,@StateID,@CityID,@LocalityID,@HomeTypeID,@StarRatingID,@HotelBaseFare,@MetaText,@TotalSingleRooms,@TotalDoubleRooms",
                    parHotelID, parHotelName, parHotelAddress, parHotelDesc, parHotelContactNo, parHotelEmail, parCountryID, parStateID, parCityID, parLocalityID, parHomeTypeID, parStarRatingID, parHotelBaseFare, parMetaText, parTotalSingleRooms, parTotalDoubleRooms).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblLCHotel> GetLCHotelByIDAsync(long id)
        {
            return await db.utblLCHotels.Where(x => x.HotelID == id).FirstOrDefaultAsync();
        }
        public async Task<string> DeleteLCHotelAsync(long id)
        {
            try
            {
                var parHotelID = new SqlParameter("@HotelID", id);

                return await db.Database.SqlQuery<string>("udspLCHotelDelete @HotelID", parHotelID).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}

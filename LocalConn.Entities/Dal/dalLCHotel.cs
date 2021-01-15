﻿using System;
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
        #region LCHotels
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
        public async Task<IEnumerable<LCHotelDD>> GetAllLCHotelAsync()
        {
            string query = "select HotelID, HotelName from utblLCHotels";
            return await db.Database.SqlQuery<LCHotelDD>(query).ToListAsync();
        }
        #endregion

        #region Feature Hotels
        public async Task<FeatHotelsVM> GetFeatHotelsAsync(int pageno, int pagesize, string sterm)
        {
            FeatHotelsVM model = new FeatHotelsVM();
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

            model.FeatHotels = await db.Database.SqlQuery<FeatHotelsView>("udspLCFeaturedHotelsPaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveFeatHotelsAsync(utblLCFeaturedHotel model)
        {
            try
            {
                var parFeatureID = new SqlParameter("@FeatureID", model.FeatureID);
                var parHotelID = new SqlParameter("@HotelID", model.HotelID);
                var parFeatureStartDate = new SqlParameter("@FeatureStartDate", model.FeatureStartDate);
                var parFeatureEndDate = new SqlParameter("@FeatureEndDate", model.FeatureEndDate);

                return await db.Database.SqlQuery<string>("udspLCFeaturedHotelsSave @FeatureID, @HotelID, @FeatureStartDate,@FeatureEndDate",
                    parFeatureID, parHotelID, parFeatureStartDate, parFeatureEndDate).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblLCFeaturedHotel> GetFeatHotelsByIDAsync(long id)
        {
            return await db.utblLCFeaturedHotels.Where(x => x.FeatureID == id).FirstOrDefaultAsync();
        }
        public async Task<string> DeleteFeatHotelsAsync(long id)
        {
            try
            {
                utblLCFeaturedHotel curObj = await db.utblLCFeaturedHotels.FindAsync(id);
                db.utblLCFeaturedHotels.Remove(curObj);
                await db.SaveChangesAsync();
                return "Feature Hotels Details Removed";
            }
            catch (SqlException ex)
            {
                if (ex.Errors.Count > 0) // Assume the interesting stuff is in the first error
                {
                    switch (ex.Errors[0].Number)
                    {
                        case 547: // Foreign Key violation
                            return "This record has dependencies on other records, so cannot be removed.";
                        default:
                            return "Error: " + ex.Message;
                    }
                }
                return "Error while operation. Error Message: " + ex.Message;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        #endregion
    }
}

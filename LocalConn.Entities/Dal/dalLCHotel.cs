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
        public async Task<string> SaveLCHotelAsync(LCHotelManageModel model)
        {
            try
            {
                //ConvertListToDT objDT = new ConvertListToDT();
                //DataTable typeDt = new DataTable();

                var parHotelID = new SqlParameter("@HotelID", model.LCHotel.HotelID);
                var parHotelName = new SqlParameter("@HotelName", model.LCHotel.HotelName);
                var parHotelAddress = new SqlParameter("@HotelAddress", model.LCHotel.HotelAddress);
                var parHotelDesc = new SqlParameter("@HotelDesc", model.LCHotel.HotelDesc);
                var parHotelContactNo = new SqlParameter("@HotelContactNo", model.LCHotel.HotelContactNo);
                var parHotelEmail = new SqlParameter("@HotelEmail", model.LCHotel.HotelEmail ?? "");
                var parCountryID = new SqlParameter("@CountryID", model.LCHotel.CountryID);
                var parStateID = new SqlParameter("@StateID", model.LCHotel.StateID);
                var parCityID = new SqlParameter("@CityID", model.LCHotel.CityID);
                var parLocalityID = new SqlParameter("@LocalityID", model.LCHotel.LocalityID);
                var parHomeTypeID = new SqlParameter("@HomeTypeID", model.LCHotel.HomeTypeID);
                var parStarRatingID = new SqlParameter("@StarRatingID", model.LCHotel.StarRatingID);
                var parHotelBaseFare = new SqlParameter("@HotelBaseFare", model.LCHotel.HotelBaseFare);
                var parHotelOfferPrice = new SqlParameter("@HotelOfferPrice", model.LCHotel.HotelOfferPrice);
                var parOfferPercentage = new SqlParameter("@OfferPercentage", model.LCHotel.OfferPercentage);
                var parRatePerNight = new SqlParameter("@RatePerNight", model.LCHotel.RatePerNight);
                var parRatePerRoom = new SqlParameter("@RatePerRoom", model.LCHotel.RatePerRoom);
                var parRatePerGuest = new SqlParameter("@RatePerGuest", model.LCHotel.RatePerGuest);
                var parRatePerChild = new SqlParameter("@RatePerChild", model.LCHotel.RatePerChild);

                //var parHotelHitCount = new SqlParameter("@HotelHitCount", model.HotelHitCount);
                var parMetaText = new SqlParameter("@MetaText", model.LCHotel.MetaText);
                var parTotalSingleRooms = new SqlParameter("@TotalSingleRooms", model.LCHotel.TotalSingleRooms);
                var parTotalDoubleRooms = new SqlParameter("@TotalDoubleRooms", model.LCHotel.TotalDoubleRooms);

                //List<IDModel> tyepList = model.RoomID.Select(x => new IDModel()
                //{
                //    ID = Convert.ToInt64(x)
                //}).ToList();
                //typeDt = objDT.ConvertIEnumerableToDataTable(tyepList);

                //var parSubDT = new SqlParameter("@HotelRoomTypeTable", typeDt);
                //parSubDT.SqlDbType = SqlDbType.Structured;
                //parSubDT.TypeName = "dbo.IDType";

                return await db.Database.SqlQuery<string>("udspLCHotelSave @HotelID, @HotelName, @HotelAddress, @HotelDesc, @HotelContactNo, @HotelEmail, @CountryID,@StateID,@CityID,@LocalityID,@HomeTypeID,@StarRatingID,@HotelBaseFare,@HotelOfferPrice,@OfferPercentage,@RatePerNight,@RatePerRoom,@RatePerGuest,@RatePerChild,@MetaText,@TotalSingleRooms,@TotalDoubleRooms",
                    parHotelID, parHotelName, parHotelAddress, parHotelDesc, parHotelContactNo, parHotelEmail, parCountryID, parStateID, parCityID, parLocalityID, parHomeTypeID, parStarRatingID, parHotelBaseFare, parHotelOfferPrice, parOfferPercentage, parRatePerNight, parRatePerRoom, parRatePerGuest,parRatePerChild, parMetaText, parTotalSingleRooms, parTotalDoubleRooms).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<string> UpdateLCHotelRateAsync(LCHotelSaveModel model)
        {
            try
            {

                var parHotelID = new SqlParameter("@HotelID", model.HotelID);
                var parHotelBaseFare = new SqlParameter("@HotelBaseFare", model.HotelBaseFare);
                var parHotelOfferPrice = new SqlParameter("@HotelOfferPrice", model.HotelOfferPrice);

                return await db.Database.SqlQuery<string>("udspLCHotelRateUpdate @HotelID,@HotelBaseFare,@HotelOfferPrice",
                    parHotelID,parHotelBaseFare, parHotelOfferPrice).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblLCHotel> GetLCHotelByIDAsync(long id)
        {
            try
            {
                return await db.utblLCHotels.Where(x => x.HotelID == id).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
         
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

        #region HotelImages
        public async Task<LCHotelImageVM> GetHotelImagesAsync(long hotelid, int pageno, int pagesize)
        {
            LCHotelImageVM model = new LCHotelImageVM();
            var parHotelID = new SqlParameter("@HotelID", hotelid);
            var parStart = new SqlParameter("@Start", (pageno - 1) * pagesize);
            var parEnd = new SqlParameter("@PageSize", pagesize);


            // setting stored procedure OUTPUT value
            // This return total number of rows, and avoid two database call for data and total number of rows 
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            model.LCHotelImageList = await db.Database.SqlQuery<LCHotelImageView>("udspLCHotelImagesPaged @HotelID, @Start, @PageSize, @TotalCount out",
                parHotelID, parStart, parEnd, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveHotelImagesAsync(utblLCHotelImage model)
        {
            try
            {
                var parHotelImageID = new SqlParameter("@HotelImageID", model.HotelImageID);
                var parHotelID = new SqlParameter("@HotelID", model.HotelID);
                var parIsHotelCover = new SqlParameter("@IsHotelCover", model.IsHotelCover);
                var parPhotoThumbPath = new SqlParameter("@PhotoThumbPath", model.PhotoThumbPath);
                var parPhotoNormalPath = new SqlParameter("@PhotoNormalPath", model.PhotoNormalPath);
                var parPhotoCaption = new SqlParameter("@PhotoCaption", model.PhotoCaption);

                return await db.Database.SqlQuery<string>("udspLCHotelImagesSave @HotelImageID, @HotelID, @IsHotelCover,@PhotoThumbPath,@PhotoNormalPath,@PhotoCaption",
                    parHotelImageID, parHotelID, parIsHotelCover, parPhotoThumbPath, parPhotoNormalPath, parPhotoCaption).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblLCHotelImage> GetLCHotelImagesByIDAsync(long id)
        {
            return await db.utblLCHotelImages.Where(x => x.HotelImageID == id).FirstOrDefaultAsync();
        }
        public async Task<string> DeleteLCHotelImagesAsync(long id)
        {
            try
            {
                utblLCHotelImage curObj = await db.utblLCHotelImages.FindAsync(id);
                db.utblLCHotelImages.Remove(curObj);
                await db.SaveChangesAsync();
                return "Hotel Images Details Removed";
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
        public async Task<string> MakeCoverImageAsync(long hotelid, long imageid)
        {

            var parImgID = new SqlParameter("@HotelImageID", imageid);
            var parhotelID = new SqlParameter("@HotelID", hotelid);

            return await db.Database.SqlQuery<string>("udspLCHotelMakeCover @HotelImageID,@HotelID", parImgID, parhotelID).FirstOrDefaultAsync();
        }
        #endregion

        #region HotelRoomTypeMap

        public async Task<IEnumerable<HotelRoomTypeMapView>> GetAllHotelRoomTypeMap(long id)
        {   
            var parHotelID = new SqlParameter("@HotelID", id);
            return await db.Database.SqlQuery<HotelRoomTypeMapView>("udspLCHotelDelete @HotelID", parHotelID).ToListAsync();
        }
        public async Task<string> SaveHotelRoomTypeMapAsync(HotelRoomTypeMap model)
        {
            try
            {
                var parHotelID = new SqlParameter("@HotelID", model.HotelID);
                var parRoomID = new SqlParameter("@RoomID", model.RoomID);
                var parRoomTypePrice = new SqlParameter("@RoomTypePrice", model.RoomTypePrice);
                var parIsStandard =   new SqlParameter("@IsStandard", model.IsStandard);

                return await db.Database.SqlQuery<string>("udspLCHotelRoomTypeMapSave @HotelID, @RoomID, @RoomTypePrice,@IsStandard ",
                    parHotelID, parRoomID, parRoomTypePrice, parIsStandard).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<IEnumerable<HotelRoomTypeMapView>> GetHotelRoomTypeAsync(long id)
        {
            var parHotelID = new SqlParameter("@HotelID", id);
            List<HotelRoomTypeMapView> model = new List<HotelRoomTypeMapView>();
            model = await db.Database.SqlQuery<HotelRoomTypeMapView>("udspLCHotelRoomTypeMapList @HotelID", parHotelID).ToListAsync();
            return model;
        }
        public async Task<HotelRoomTypeMap> GetHotelRoomTypeMapByIDAsync(long id, long rid)
        {
            try
            {
                string query = "select HotelID, RoomID, RoomTypePrice, IsStandard from utblLCHotelRoomTypeMaps where HotelID=" + id + "and RoomID =" + rid;
                return await db.Database.SqlQuery<HotelRoomTypeMap>(query).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<string> DeleteHotelRoomTypeMapAsync(long id, long rid)
        {
            try
            {
                var parHotelID = new SqlParameter("@HotelID", id);
                var parRoomID = new SqlParameter("@RoomID", rid);
                return await db.Database.SqlQuery<string>("udspLCHotelRoomTypeMapDelete @HotelID,@RoomID ", parHotelID, parRoomID).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        #endregion
    }
}

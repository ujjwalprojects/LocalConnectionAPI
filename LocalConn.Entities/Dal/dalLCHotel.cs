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
                var parMaxOccupant = new SqlParameter("@MaxOccupant", model.LCHotel.MaxOccupant);
                var parMaxRooms = new SqlParameter("@MaxRooms", model.LCHotel.MaxRooms);
                var parOverallOfferPercentage = new SqlParameter("@OverallOfferPercentage", model.LCHotel.OverallOfferPercentage);
                var parTwoOccupantPercentage = new SqlParameter("@TwoOccupantPercentage", model.LCHotel.TwoOccupantPercentage);
                var parThreeOccupantPercentage = new SqlParameter("@ThreeOccupantPercentage", model.LCHotel.ThreeOccupantPercentage);
                var parFourPlusOccupantPercentage = new SqlParameter("@FourPlusOccupantPercentage", model.LCHotel.FourPlusOccupantPercentage);
                var parChildOccupantNote = new SqlParameter("@ChildOccupantNote", model.LCHotel.ChildOccupantNote);
                var parIsActive = new SqlParameter("@IsActive", model.LCHotel.IsActive);


                return await db.Database.SqlQuery<string>("udspLCHotelSave @HotelID, @HotelName, @HotelAddress, @HotelDesc, @HotelContactNo, @HotelEmail, @CountryID,@StateID,@CityID,@LocalityID,@HomeTypeID,@StarRatingID,@MaxOccupant,@MaxRooms,@OverallOfferPercentage,@TwoOccupantPercentage,@ThreeOccupantPercentage,@FourPlusOccupantPercentage,@ChildOccupantNote,@IsActive",
                    parHotelID, parHotelName, parHotelAddress, parHotelDesc, parHotelContactNo, parHotelEmail, parCountryID, parStateID, parCityID, parLocalityID, parHomeTypeID, parStarRatingID, parMaxOccupant,parMaxRooms, parOverallOfferPercentage, parTwoOccupantPercentage, parThreeOccupantPercentage, parFourPlusOccupantPercentage, parChildOccupantNote,parIsActive).FirstOrDefaultAsync();
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

                //var parHotelID = new SqlParameter("@HotelID", model.HotelID);
                //var parHotelBaseFare = new SqlParameter("@HotelBaseFare", model.HotelBaseFare);
                //var parHotelOfferPrice = new SqlParameter("@HotelOfferPrice", model.HotelOfferPrice);

                //return await db.Database.SqlQuery<string>("udspLCHotelRateUpdate @HotelID,@HotelBaseFare,@HotelOfferPrice",
                //    parHotelID, parHotelBaseFare, parHotelOfferPrice).FirstOrDefaultAsync();

                return "sdf";
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
        //RoomType mapped with hotel list
        public async Task<IEnumerable<RoomTypeDD>> GetRoomTypeByID(long id)
        {
            var parHotelID = new SqlParameter("@HotelID", id);
            return await db.Database.SqlQuery<RoomTypeDD>("udspLCRoomTypeListByID @HotelID", parHotelID).ToListAsync();
        }
        //Hotel Premises list for Drop Down
        public async Task<IEnumerable<utblLCMstHotelPremise>> GetAllLCHotelPremisesAsync()
        {
            string query = "select HotelPremID, HotelPremName from utblLCMstHotelPremises";
            return await db.Database.SqlQuery<utblLCMstHotelPremise>(query).ToListAsync();
        }
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
                var parHotelPremID = new SqlParameter("@HotelPremID", model.HotelPremID);
                var parRoomsID = new SqlParameter("@RoomID", DBNull.Value);
                if (model.RoomID != null)
                {
                    parRoomsID = new SqlParameter("@RoomID", model.RoomID);
                }
                var parIsRoomCover = new SqlParameter("@IsRoomCover", DBNull.Value);
                //if(model.IsRoomCover != false)
                //{
                    
                //}
                parIsRoomCover = new SqlParameter("@IsRoomCover", model.IsRoomCover);
                var parIsHotelCover = new SqlParameter("@IsHotelCover", model.IsHotelCover);
                var parPhotoThumbPath = new SqlParameter("@PhotoThumbPath", model.PhotoThumbPath);
                var parPhotoNormalPath = new SqlParameter("@PhotoNormalPath", model.PhotoNormalPath);
                var parPhotoCaption = new SqlParameter("@PhotoCaption", model.PhotoCaption);

                return await db.Database.SqlQuery<string>("udspLCHotelImagesSave @HotelImageID, @HotelID,@HotelPremID,@RoomID,@IsRoomCover, @IsHotelCover,@PhotoThumbPath,@PhotoNormalPath,@PhotoCaption",
                    parHotelImageID, parHotelID, parHotelPremID, parRoomsID, parIsRoomCover, parIsHotelCover, parPhotoThumbPath, parPhotoNormalPath, parPhotoCaption).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblLCHotelImage> GetLCHotelImagesByIDAsync(long id)
        {
            try
            {
                var parHotelImageID = new SqlParameter("@HotelImageID", id);
                return await db.Database.SqlQuery<utblLCHotelImage>("udspLCHotelImagesByID @HotelImageID", parHotelImageID).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
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
                var parIsStandard = new SqlParameter("@IsStandard", model.IsStandard);

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

        #region HotelTems&Cancellations
        public async Task<IEnumerable<HotelTerms>> GetHotelTermsAsync(long id)
        {
            var parHotelID = new SqlParameter("@HotelID", id);
            return await db.Database.SqlQuery<HotelTerms>("select * from dbo.udfGetLCHotelTerms(@HotelID)", parHotelID).ToListAsync();
        }
        public async Task<IEnumerable<HotelCancellations>> GetHotelCancellationsAsync(long id)
        {
            var parHotelID = new SqlParameter("@HotelID", id);
            return await db.Database.SqlQuery<HotelCancellations>("select * from dbo.udfGetLCHotelCancellations(@HotelID)", parHotelID).ToListAsync();
        }

        public async Task<string> SaveTermsCancAsync(HotelTermCancSaveModel model)
        {
            try
            {
                var parHotelID = new SqlParameter("@HotelID", model.HotelID);
                ConvertListToDT objList = new ConvertListToDT();
                DataTable termdt = new DataTable();
                DataTable candt = new DataTable();

                //Converting subject list to datatable if record is present else send empty datatable
                if (model.Terms != null)
                {
                    termdt = objList.ConvertIEnumerableToDataTable(model.Terms);
                }
                else
                {
                    if (termdt.Columns.Count == 0)
                    {
                        DataColumn col = new DataColumn();
                        col.ColumnName = "HotelTermsID";
                        termdt.Columns.Add(col);
                        DataColumn col1 = new DataColumn();
                        col1.ColumnName = "HotelID";
                        termdt.Columns.Add(col1);
                        DataColumn col2 = new DataColumn();
                        col2.ColumnName = "TermID";
                        termdt.Columns.Add(col2);
                    }
                }

                var parTermDT = new SqlParameter("@LCTermTable", termdt);
                parTermDT.SqlDbType = SqlDbType.Structured;
                parTermDT.TypeName = "dbo.LCHotelTermType";


                if (model.Cancellations != null)
                {
                    candt = objList.ConvertIEnumerableToDataTable(model.Cancellations);
                }
                else
                {
                    if (candt.Columns.Count == 0)
                    {
                        DataColumn col = new DataColumn();
                        col.ColumnName = "HotelCancID";
                        candt.Columns.Add(col);
                        DataColumn col1 = new DataColumn();
                        col1.ColumnName = "HotelID";
                        candt.Columns.Add(col1);
                        DataColumn col2 = new DataColumn();
                        col2.ColumnName = "CancellationID";
                        candt.Columns.Add(col2);
                    }
                }

                var parCanDT = new SqlParameter("@LCCancellationTable", candt);
                parCanDT.SqlDbType = SqlDbType.Structured;
                parCanDT.TypeName = "dbo.LCHotelCancType";

                return await db.Database.SqlQuery<string>("udspLCHotelTermCancellationSave @HotelID, @LCTermTable,@LCCancellationTable",
                    parHotelID, parTermDT, parCanDT).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        #endregion

        #region HotelOffers
        public async Task<utblLCFeatureOffer> GetHotelOfferByIDAsync(long id)
        {
            return await db.utblLCFeatureOffers.Where(x => x.OfferID == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<long>> GetOfferHotelsAsync(long id)
        {
            string query = "select HotelID from utblLCFeatureOfferHotelMapping where OfferID=" + id;
            return await db.Database.SqlQuery<long>(query).ToListAsync();
        }
        public async Task<HotelOfferVM> GetHotelOfferListAsync(int pageno, int pagesize, string sterm)
        {
            HotelOfferVM model = new HotelOfferVM();
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

            model.HotelOfferList = await db.Database.SqlQuery<HotelOfferView>("udspLCFeatureOfferspaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveHotelOfferAsync(SaveHotelOffer model)
        {
            try
            {
                ConvertListToDT objDT = new ConvertListToDT();
                DataTable typeDt = new DataTable();

                var parOffID = new SqlParameter("@OfferID", model.HotelOffer.OfferID);
                var parOfferTag = new SqlParameter("@OfferTagLine", model.HotelOffer.OfferTagLine);
                var parOfferImgPath = new SqlParameter("@OfferImagePath", model.HotelOffer.OfferImagePath);
                var parStart = new SqlParameter("@OfferStartDate", model.HotelOffer.OfferStartDate);
                var parEnd = new SqlParameter("@OfferEndDate", model.HotelOffer.OfferEndDate);

                List<IDModel> tyepList = model.HotelID.Select(x => new IDModel()
                {
                    ID = Convert.ToInt64(x)
                }).ToList();
                typeDt = objDT.ConvertIEnumerableToDataTable(tyepList);

                var parSubDT = new SqlParameter("@LCOfferTable", typeDt);
                parSubDT.SqlDbType = SqlDbType.Structured;
                parSubDT.TypeName = "dbo.IDType";

                return await db.Database.SqlQuery<string>("udspLCFeatureOffersSave @OfferID , @OfferTagLine, @OfferImagePath, @OfferStartDate, @OfferEndDate,@LCOfferTable",
                     parOffID, parOfferTag, parOfferImgPath, parStart, parEnd, parSubDT).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        #endregion

        #region LCNearByPoints
        public async Task<LCNearByPointsVM> GetLCNearByPointsAsync(int pageno, int pagesize, string sterm)
        {
            LCNearByPointsVM model = new LCNearByPointsVM();
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

            model.LCNearByPointView = await db.Database.SqlQuery<LCNearByPointsView>("udspLCNearByPointsPaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveLCNearByPointsAsync(utblLCNearByPoint model)
        {
            try
            {
                var parNearbyPointsID = new SqlParameter("@NearbyPointsID", model.NearbyPointsID);
                var parNearByID = new SqlParameter("@NearByID", model.NearByID);
                var parHotelID = new SqlParameter("@HotelID", model.HotelID);
                var parNearByPoints = new SqlParameter("@NearByPoints", model.NearByPoints);
                var parNearByDistance = new SqlParameter("@NearByDistance", model.NearByDistance);

                return await db.Database.SqlQuery<string>("udspLCNearByPointsSave @NearbyPointsID, @NearByID,@HotelID, @NearByPoints,@NearByDistance",
                    parNearbyPointsID, parNearByID, parHotelID, parNearByPoints, parNearByDistance).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblLCNearByPoint> GetLCNearByPointsByIDAsync(long id)
        {
            return await db.utblLCNearByPoints.Where(x => x.NearbyPointsID == id).FirstOrDefaultAsync();
        }
        public async Task<string> DeleteLCNearByPointsAsync(long id)
        {
            try
            {
                utblLCNearByPoint obj = await db.utblLCNearByPoints.FindAsync(id);
                db.utblLCNearByPoints.Remove(obj);
                await db.SaveChangesAsync();
                return "Near Point Details Removed";
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
        public async Task<IEnumerable<LCNearByPointsView>> GetLCNearByPointsAsync(long id)
        {
            var parHotelID = new SqlParameter("@HotelID", id);
            List<LCNearByPointsView> model = new List<LCNearByPointsView>();
            model = await db.Database.SqlQuery<LCNearByPointsView>("udspLCHotelNearByPointMapList @HotelID", parHotelID).ToListAsync();
            return model;
        }
        public async Task<IEnumerable<LCNearBysTypeDD>> GetLCNearBysTypeDDAsync()
        {
            string query = "select NearByID, NearByName from utblLCMstNearBys";
            return await db.Database.SqlQuery<LCNearBysTypeDD>(query).ToListAsync();
        }
        #endregion

       

    }
}

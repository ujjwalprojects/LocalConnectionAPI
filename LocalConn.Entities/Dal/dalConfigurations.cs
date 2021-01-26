using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;
using LocalConn.Entities.Models;
using LocalConn.Entities.ViewModels;

namespace LocalConn.Entities.Dal
{
    public class dalConfigurations
    {
        EFDBContext objDB = new EFDBContext();


        #region Countries
        public async Task<IEnumerable<utblMstCountries>> getCountriesAsync()
        {
            return await objDB.utblMstCountries.ToListAsync();
        }
        public async Task<string> addCountriesAsync(utblMstCountries model)
        {
            try
            {
                if (model.CountryID == 0)
                {
                    objDB.utblMstCountries.Add(model);
                    await objDB.SaveChangesAsync();
                    return "New Country Details Added";
                }
                else
                {
                    utblMstCountries curObj = await objDB.utblMstCountries.FindAsync(model.CountryID);
                    curObj.CountryName = model.CountryName;
                    await objDB.SaveChangesAsync();
                    return "Country Details Updated";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public async Task<utblMstCountries> getCountryByIDAsync(long id)
        {
            return await objDB.utblMstCountries.Where(x => x.CountryID == id).FirstOrDefaultAsync();
        }

        public async Task<string> deleteCountriesAsync(long id)
        {
            try
            {
                utblMstCountries curObj = await objDB.utblMstCountries.FindAsync(id);
                objDB.utblMstCountries.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Country Details Removed";
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

        #endregion Countries

        #region States
        public async Task<IEnumerable<StateView>> GetStatesAsync()
        {
            return await objDB.utblMstStates
                .Join(objDB.utblMstCountries, x => x.CountryID, y => y.CountryID, (x, y) => new StateView()
                {
                    StateID = x.StateID,
                    CountryID = x.CountryID,
                    CountryName = y.CountryName,
                    StateName = x.StateName
                }).ToListAsync();
        }
        public async Task<string> SaveStatesAsync(utblMstState model)
        {
            try
            {
                if (model.StateID == 0)
                {
                    objDB.utblMstStates.Add(model);
                    await objDB.SaveChangesAsync();
                    return "New State Added";
                }
                else
                {
                    utblMstState curState = await objDB.utblMstStates.FindAsync(model.StateID);
                    curState.CountryID = model.CountryID;
                    curState.StateName = model.StateName;
                    await objDB.SaveChangesAsync();
                    return "State Details Updated";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstState> GetStateByIDAsync(long id)
        {
            return await objDB.utblMstStates.Where(x => x.StateID == id).FirstOrDefaultAsync();
        }
        public async Task<string> DeleteStateAsync(long id)
        {
            try
            {
                utblMstState curObj = await objDB.utblMstStates.FindAsync(id);
                objDB.utblMstStates.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "State Details Removed";
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
        public async Task<IEnumerable<utblMstState>> GetStateByCountryAsync(long id)
        {
            return await objDB.utblMstStates.Where(x => x.CountryID == id).ToListAsync();
        }
        #endregion

        #region LCCities
        public async Task<IEnumerable<utblLCMstCitie>> getCitiesAsync()
        {
            return await objDB.utblLCMstCities.ToListAsync();
        }
        public async Task<CitiesVM> GetCitiesAsync(int pageno, int pagesize, string sterm)
        {
            CitiesVM model = new CitiesVM();
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

            model.Cities = await objDB.Database.SqlQuery<CitiesView>("udspLCMstCitiesPaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveCitiesAsync(utblLCMstCitie model)
        {
            try
            {
                var parCityID = new SqlParameter("@CityID", model.CityID);
                var parCountryID = new SqlParameter("@CountryID", model.CountryID);
                var parStateID = new SqlParameter("@StateID", model.StateID);
                var parCityName = new SqlParameter("@CityName", model.CityName);
                var parCityIconPath = new SqlParameter("@CityIconPath", model.CityIconPath ?? "");
                var parIsPopular = new SqlParameter("@IsPopular", model.IsPopular);

                return await objDB.Database.SqlQuery<string>("udspLCMstCitiesSave @CityID, @CountryID, @StateID, @CityName, @CityIconPath, @IsPopular",
                    parCityID, parCountryID, parStateID, parCityName, parCityIconPath,parIsPopular).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblLCMstCitie> GetCitiesByIDAsync(long id)
        {
            return await objDB.utblLCMstCities.Where(x => x.CityID == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<utblLCMstCitie>> GetAllCitiesAsync()
        {
            return await objDB.utblLCMstCities.OrderBy(x => x.CityName).ToListAsync();
        }
        public async Task<string> DeleteCitiesAsync(long id)
        {
            try
            {
                utblLCMstCitie curObj = await objDB.utblLCMstCities.FindAsync(id);
                objDB.utblLCMstCities.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Cities Details Removed";
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
        public async Task<IEnumerable<CitiesDD>> GetCitiesByStateAsync(long id)
        {
            try
            {
                 string query = "select CityID, CityName from utblLCMstCities where StateID=" + id;
                 return await objDB.Database.SqlQuery<CitiesDD>(query).ToListAsync();

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        #endregion

        #region LCAmenities
        public async Task<IEnumerable<utblLCMstAmenitie>> getAmenitiesAsync()
        {
            return await objDB.utblLCMstAmenities.ToListAsync();
        }
        public async Task<AmenitiesVM> GetAmenitiesAsync(int pageno, int pagesize, string sterm)
        {
            AmenitiesVM model = new AmenitiesVM();
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

            model.Amenities = await objDB.Database.SqlQuery<AmenitiesView>("udspLCMstAmenitiesPaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveAmenitiesAsync(utblLCMstAmenitie model)
        {
            try
            {
                var parAmenitiesID = new SqlParameter("@AmenitiesID", model.AmenitiesID);
                var parAmenitiesName = new SqlParameter("@AmenitiesName", model.AmenitiesName);
                var parAmenitiesBasePrice = new SqlParameter("@AmenitiesBasePrice", model.AmenitiesBasePrice);

                return await objDB.Database.SqlQuery<string>("udspLCMstAmenitiesSave @AmenitiesID, @AmenitiesName, @AmenitiesBasePrice",
                    parAmenitiesID, parAmenitiesName, parAmenitiesBasePrice).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblLCMstAmenitie> GetAmenitiesByIDAsync(long id)
        {
            return await objDB.utblLCMstAmenities.Where(x => x.AmenitiesID == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<utblLCMstAmenitie>> GetAllAmenitiesAsync()
        {
            return await objDB.utblLCMstAmenities.OrderBy(x => x.AmenitiesName).ToListAsync();
        }
        public async Task<string> DeleteAmenitiesAsync(long id)
        {
            try
            {
                utblLCMstAmenitie curObj = await objDB.utblLCMstAmenities.FindAsync(id);
                objDB.utblLCMstAmenities.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Amenities Details Removed";
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

        #region LCStarRating
        public async Task<IEnumerable<utblLCMstStarRating>> getStarRatingAsync()
        {
            return await objDB.utblLCMstStarRatings.ToListAsync();
        }
        public async Task<StarRatingVM> GetStarRatingAsync(int pageno, int pagesize, string sterm)
        {
            StarRatingVM model = new StarRatingVM();
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

            model.StarRatings = await objDB.Database.SqlQuery<StarRatingView>("udspLCMstStarRatingsPaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveStarRatingAsync(utblLCMstStarRating model)
        {
            try
            {
                var parStarRatingID = new SqlParameter("@StarRatingID", model.StarRatingID);
                var parStarRatingName = new SqlParameter("@StarRatingName", model.StarRatingName);

                return await objDB.Database.SqlQuery<string>("udspLCMstStarRatingSave @StarRatingID, @StarRatingName",
                    parStarRatingID, parStarRatingName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblLCMstStarRating> GetStarRatingByIDAsync(long id)
        {
            return await objDB.utblLCMstStarRatings.Where(x => x.StarRatingID == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<utblLCMstStarRating>> GetAllStarRatingAsync()
        {
            return await objDB.utblLCMstStarRatings.OrderBy(x => x.StarRatingName).ToListAsync();
        }
        public async Task<string> DeleteStarRatingAsync(long id)
        {
            try
            {
                utblLCMstStarRating curObj = await objDB.utblLCMstStarRatings.FindAsync(id);
                objDB.utblLCMstStarRatings.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Star Rating Details Removed";
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

        #region LCHomeType
        public async Task<IEnumerable<utblLCMstHomeType>> getHomeTypeAsync()
        {
            return await objDB.utblLCMstHomeTypes.ToListAsync();
        }
        public async Task<HomeTypeVM> GetHomeTypeAsync(int pageno, int pagesize, string sterm)
        {
            HomeTypeVM model = new HomeTypeVM();
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

            model.HomeTypes = await objDB.Database.SqlQuery<HomeTypeView>("udspLCMstHomeTypesPaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveHomeTypeAsync(utblLCMstHomeType model)
        {
            try
            {
                var parHomeTypeID = new SqlParameter("@HomeTypeID", model.HomeTypeID);
                var parHomeTypeName = new SqlParameter("@HomeTypeName", model.HomeTypeName);

                return await objDB.Database.SqlQuery<string>("udspLCMstHomeTypesSave @HomeTypeID, @HomeTypeName",
                    parHomeTypeID, parHomeTypeName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblLCMstHomeType> GetHomeTypeByIDAsync(long id)
        {
            return await objDB.utblLCMstHomeTypes.Where(x => x.HomeTypeID == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<utblLCMstHomeType>> GetAllHomeTypeAsync()
        {
            return await objDB.utblLCMstHomeTypes.OrderBy(x => x.HomeTypeName).ToListAsync();
        }
        public async Task<string> DeleteHomeTypeAsync(long id)
        {
            try
            {
                utblLCMstHomeType curObj = await objDB.utblLCMstHomeTypes.FindAsync(id);
                objDB.utblLCMstHomeTypes.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Home Type Details Removed";
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

        #region LCLocalities
        public async Task<LocalitiesVM> GetLocalitiesAsync(int pageno, int pagesize, string sterm)
        {
            LocalitiesVM model = new LocalitiesVM();
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

            model.Localities = await objDB.Database.SqlQuery<LocalitiesView>("udspLCMstLocalitiesPaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveLocalitiesAsync(utblLCMstLocalitie model)
        {
            try
            {
                var parLocalityID = new SqlParameter("@LocalityID", model.LocalityID);
                var parLocalityName = new SqlParameter("@LocalityName", model.LocalityName);
                var parCityID = new SqlParameter("@CityID", model.CityID);

                return await objDB.Database.SqlQuery<string>("udspLCMstLocalitiesSave @LocalityID, @LocalityName, @CityID",
                    parLocalityID, parLocalityName, parCityID).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblLCMstLocalitie> GetLocalitiesByIDAsync(long id)
        {
            return await objDB.utblLCMstLocalities.Where(x => x.LocalityID == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<utblLCMstLocalitie>> GetAllLocalitiesAsync()
        {
            return await objDB.utblLCMstLocalities.OrderBy(x => x.LocalityName).ToListAsync();
        }
        public async Task<string> DeleteLocalitiesAsync(long id)
        {
            try
            {
                utblLCMstLocalitie curObj = await objDB.utblLCMstLocalities.FindAsync(id);
                objDB.utblLCMstLocalities.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Cities Details Removed";
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
        public async Task<IEnumerable<LocalitiesDD>> GetLocalitiesByCityAsync(long id)
        {
            string query = "select LocalityID, LocalityName from utblLCMstLocalities where CityID=" + id;
            return await objDB.Database.SqlQuery<LocalitiesDD>(query).ToListAsync();
        }
        #endregion

        #region LCRooms
        public async Task<IEnumerable<utblLCRoom>> getRoomsAsync()
        {
            return await objDB.utblLCRooms.ToListAsync();
        }
        public async Task<RoomsVM> GetRoomsAsync(int pageno, int pagesize, string sterm)
        {
            RoomsVM model = new RoomsVM();
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

            model.Rooms = await objDB.Database.SqlQuery<RoomsView>("udspLCRoomsPaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveRoomsAsync(utblLCRoom model)
        {
            try
            {
                var parRoomID = new SqlParameter("@RoomID", model.RoomID);
                var parRoomType = new SqlParameter("@RoomType", model.RoomType);
                var parRoomBaseFare = new SqlParameter("@RoomBaseFare", model.RoomBaseFare);
                var parTotalCapacity = new SqlParameter("@TotalCapacity", model.TotalCapacity);
                return await objDB.Database.SqlQuery<string>("udspLCRoomsSave @RoomID, @RoomType, @RoomBaseFare, @TotalCapacity",
                    parRoomID, parRoomType, parRoomBaseFare, parTotalCapacity).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblLCRoom> GetRoomsByIDAsync(long id)
        {
            return await objDB.utblLCRooms.Where(x => x.RoomID == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<utblLCRoom>> GetAllRoomsAsync()
        {
            return await objDB.utblLCRooms.OrderBy(x => x.RoomType).ToListAsync();
        }
        public List<RoomTypeDD> GetRoomDDAsync()
        {
            List<RoomTypeDD> obj = new List<RoomTypeDD>();
            obj = objDB.Database.SqlQuery<RoomTypeDD>("select RoomID,RoomType from utblLCRooms").ToList();
            return obj;
        }
        public async Task<IEnumerable<long>> GetHotelRoomTypeAsync(long id)
        {
            string query = "select RoomID from utblLCHotelRoomTypeMaps where HotelID=" + id;
            return await objDB.Database.SqlQuery<long>(query).ToListAsync();
        }
        public async Task<string> DeleteRoomsAsync(long id)
        {
            try
            {
                utblLCRoom curObj = await objDB.utblLCRooms.FindAsync(id);
                objDB.utblLCRooms.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Rooms Details Removed";
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

        #region Destinations
        public async Task<DestinationVM> GetDestinationsAsync(int pageno, int pagesize, string sterm)
        {
            DestinationVM model = new DestinationVM();
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

            model.Destinations = await objDB.Database.SqlQuery<DestinationView>("udspMstDestinationPaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveDestinationAsync(utblMstDestination model)
        {
            try
            {
                var parDestID = new SqlParameter("@DestinationID", model.DestinationID);
                var parCountryID = new SqlParameter("@CountryID", model.CountryID);
                var parStateID = new SqlParameter("@StateID", model.StateID);
                var parDestName = new SqlParameter("@DestinationName", model.DestinationName);
                var parDestDesc = new SqlParameter("@DestinationDesc", model.DestinationDesc ?? "");
                var parDestImg = new SqlParameter("@DestinationImagePath", model.DestinationImagePath ?? "");

                return await objDB.Database.SqlQuery<string>("udspMstDestinationSave @DestinationID, @CountryID, @StateID, @DestinationName, @DestinationDesc,@DestinationImagePath",
                    parDestID, parCountryID, parStateID, parDestName, parDestDesc,parDestImg).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstDestination> GetDestinationByIDAsync(long id)
        {
            return await objDB.utblMstDestinations.Where(x => x.DestinationID == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<utblMstDestination>> GetAllDestinationsAsync()
        {
            return await objDB.utblMstDestinations.OrderBy(x => x.DestinationName).ToListAsync();
        }
        public async Task<string> DeleteDestinationAsync(long id)
        {
            try
            {
                utblMstDestination curObj = await objDB.utblMstDestinations.FindAsync(id);
                objDB.utblMstDestinations.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Destination Details Removed";
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

        #region Package Type
        public async Task<PackageTypeVM> GetPackageTypeListAsync(int pageno, int pagesize, string sterm)
        {
            PackageTypeVM model = new PackageTypeVM();
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

            model.PackageList = await objDB.Database.SqlQuery<PackageTypeView>("udspMstPackageTypeList @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SavePackageTypeAsync(utblMstPackageType model)
        {
            try
            {
                var parPID = new SqlParameter("@PackageTypeID", model.PackageTypeID);
                var parPName = new SqlParameter("@PackageTypeName", model.PackageTypeName ?? "");
                var parIsVis = new SqlParameter("@IsVisible", model.IsVisible);

                return await objDB.Database.SqlQuery<string>("udspMstPackageTypeAddEdit @PackageTypeID, @PackageTypeName,@IsVisible",
                    parPID, parPName,parIsVis).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstPackageType> GetPackageTypeByIDAsync(long id)
        {
            utblMstPackageType obj = new utblMstPackageType();
            obj = await objDB.utblMstPackageTypes.Where(x => x.PackageTypeID == id).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<string> DeletePackageTypeAsync(long PackageTypeID)
        {
            try
            {
                utblMstPackageType curObj = await objDB.utblMstPackageTypes.FindAsync(PackageTypeID);
                objDB.utblMstPackageTypes.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Package Details Removed";
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
        public async Task<IEnumerable<utblMstPackageType>> GetAllPackageTypesAsync()
        {
            return await objDB.utblMstPackageTypes.ToListAsync();
        }
        #endregion

        #region Itineraries
        public async Task<ItineraryVM> GetItineraryListAsync(int pageno, int pagesize, string sterm)
        {
            ItineraryVM model = new ItineraryVM();
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

            model.ItinenaryList = await objDB.Database.SqlQuery<ItineraryView>("udspMstItineraryList @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }

        public async Task<IEnumerable<utblMstDestination>> GetDestinationListAsync()
        {
            return await objDB.utblMstDestinations.ToListAsync();
        }
        public async Task<string> SaveItineraryAsync(utblMstItinerarie model)
        {
            try
            {
                var parPID = new SqlParameter("@ItineraryID", model.ItineraryID);
                var parItName = new SqlParameter("@ItineraryName", model.ItineraryName);
                var parItDesc = new SqlParameter("@ItineraryDesc", model.ItineraryDesc);
                var parOvDest = new SqlParameter("@OvernightDestinationID", DBNull.Value);
                if (model.OvernightDestinationID != null)
                    parOvDest = new SqlParameter("@OvernightDestinationID", model.OvernightDestinationID);

                return await objDB.Database.SqlQuery<string>("udspMstItineraryAddEdit @ItineraryID, @ItineraryName,@ItineraryDesc,@OvernightDestinationID",
                    parPID, parItName, parItDesc, parOvDest).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstItinerarie> GetItineraryByIDAsync(long id)
        {
            utblMstItinerarie obj = new utblMstItinerarie();
            obj = await objDB.utblMstItineraries.Where(x => x.ItineraryID == id).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<string> DeleteItineraryAsync(long ItineraryID)
        {
            try
            {
                utblMstItinerarie curObj = await objDB.utblMstItineraries.FindAsync(ItineraryID);
                objDB.utblMstItineraries.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Itinerary Details Removed";
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
        public async Task<IEnumerable<utblMstItinerarie>> GetAllItinerariesAsync()
        {
            return await objDB.utblMstItineraries.ToListAsync();
        }

        #endregion

        #region Inclusions
        public async Task<InclusionVM> GetInclusionListAsync(int pageno, int pagesize, string sterm)
        {
            InclusionVM model = new InclusionVM();
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

            model.InclusionList = await objDB.Database.SqlQuery<InclusionView>("udspMstInclusionList @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveInclusionAsync(utblMstInclusion model)
        {
            try
            {
                var parPID = new SqlParameter("@InclusionID", model.InclusionID);
                var parIName = new SqlParameter("@InclusionName", model.InclusionName);
                var parIDesc = new SqlParameter("@InclusionType", model.InclusionType);

                return await objDB.Database.SqlQuery<string>("udspMstInclusionAddEdit @InclusionID, @InclusionName,@InclusionType",
                    parPID, parIName, parIDesc).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstInclusion> GetInclusionByIDAsync(long id)
        {
            utblMstInclusion obj = new utblMstInclusion();
            obj = await objDB.utblMstInclusions.Where(x => x.InclusionID == id).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<string> DeleteInclusionAsync(long InclustionID)
        {
            try
            {
                utblMstInclusion curObj = await objDB.utblMstInclusions.FindAsync(InclustionID);
                objDB.utblMstInclusions.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Inclusion Details Removed";
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

        #region Exclusions
        public async Task<ExclusionVM> GetExclusionListAsync(int pageno, int pagesize, string sterm)
        {
            ExclusionVM model = new ExclusionVM();
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

            model.ExclusionList = await objDB.Database.SqlQuery<ExclusionView>("udspMstExclusionList @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveExclusionAsync(utblMstExclusion model)
        {
            try
            {
                var parPID = new SqlParameter("@ExclusionID", model.ExclusionID);
                var parIName = new SqlParameter("@ExclusionName", model.ExclusionName);

                return await objDB.Database.SqlQuery<string>("udspMstExclusionAddEdit @ExclusionID, @ExclusionName",
                    parPID, parIName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstExclusion> GetExclusionByIDAsync(long id)
        {
            utblMstExclusion obj = new utblMstExclusion();
            obj = await objDB.utblMstExclusions.Where(x => x.ExclusionID == id).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<string> DeleteExclusionAsync(long ExclusionID)
        {
            try
            {
                utblMstExclusion curObj = await objDB.utblMstExclusions.FindAsync(ExclusionID);
                objDB.utblMstExclusions.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Exclusion Details Removed";
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

        #region Terms

        public async Task<TermVM> GetTermListAsync(int pageno, int pagesize, string sterm)
        {
            TermVM model = new TermVM();
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

            model.TermList = await objDB.Database.SqlQuery<TermView>("udspMstTermList @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveTermAsync(utblMstTerm model)
        {
            try
            {
                var parPID = new SqlParameter("@TermID", model.TermID);
                var parIName = new SqlParameter("@TermName", model.TermName);

                return await objDB.Database.SqlQuery<string>("udspMstTermAddEdit @TermID, @TermName",
                    parPID, parIName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstTerm> GetTermByIDAsync(long id)
        {
            utblMstTerm obj = new utblMstTerm();
            obj = await objDB.utblMstTerms.Where(x => x.TermID == id).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<string> DeleteTermAsync(long TermID)
        {
            try
            {
                utblMstTerm curObj = await objDB.utblMstTerms.FindAsync(TermID);
                objDB.utblMstTerms.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Term Details Removed";
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

        #region Banner

        public async Task<BannerVM> GetBannerListAsync(int pageno, int pagesize, string sterm)
        {
            BannerVM model = new BannerVM();
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

            model.BannerList = await objDB.Database.SqlQuery<BannerView>("udspMstBannerList @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveBannerAsync(utblMstBanner model)
        {
            try
            {
                var parPID = new SqlParameter("@BannerID", model.BannerID);
                var parIName = new SqlParameter("@BannerPath", model.BannerPath);

                return await objDB.Database.SqlQuery<string>("udspMstBannerAddEdit @BannerID, @BannerPath",
                    parPID, parIName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstBanner> GetBannerByIDAsync(long id)
        {
            utblMstBanner obj = new utblMstBanner();
            obj = await objDB.utblMstBanners.Where(x => x.BannerID == id).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<string> DeleteBannerAsync(long BannerID)
        {
            try
            {
                utblMstBanner curObj = await objDB.utblMstBanners.FindAsync(BannerID);
                objDB.utblMstBanners.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Banner Details Removed";
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

        public  List<utblMstBanner> getBannerList()
        {
            return  objDB.utblMstBanners.ToList();
        }
        #endregion

        #region Activities
        public async Task<ActivityVM> GetActivityListAsync(int pageno, int pagesize, string sterm)
        {
            try
            {
                ActivityVM model = new ActivityVM();
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

                model.ActivityList = await objDB.Database.SqlQuery<ActivityView>("udspMstActivitiesList @Start, @PageSize,@SearchTerm, @TotalCount out",
                    parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
                model.TotalRecords = int.Parse(spOutput.Value.ToString());
                return model;
            }
            catch (Exception ex)
            {
               throw ex;
            }
        }

        public async Task<string> SaveActivityAsync(utblMstActivitie model)
        {
            try
            {
                var parAID = new SqlParameter("@ActivityID", model.ActivityID);
                var parAName = new SqlParameter("@ActivityName", model.ActivityName);
                var parADesc = new SqlParameter("@ActivityDesc", model.ActivityDesc);
                var parDestinationID = new SqlParameter("@DestinationID", model.DestinationID);
                var parBaseFare = new SqlParameter("@BaseFare", model.BaseFare);

                return await objDB.Database.SqlQuery<string>("udspMstActivitiesAddEdit @ActivityID, @ActivityName,@ActivityDesc,@DestinationID,@BaseFare",
                    parAID, parAName, parADesc, parDestinationID, parBaseFare).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstActivitie> GetActivityByIDAsync(long id)
        {
            utblMstActivitie obj = new utblMstActivitie();
            obj = await objDB.utblMstActivities.Where(x => x.ActivityID == id).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<string> DeleteActivityAsync(long ActivityID)
        {
            try
            {
                utblMstActivitie curObj = await objDB.utblMstActivities.FindAsync(ActivityID);
                objDB.utblMstActivities.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Activity Details Removed";
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
        public async Task<IEnumerable<utblMstActivitie>> GetAllActivityAsync()
        {
            return await objDB.utblMstActivities.OrderBy(x => x.ActivityName).ToListAsync();
        }
        #endregion


        #region Tour Cancellation

        public async Task<TourCancelVM> GetTourCancelListAsync(int pageno, int pagesize, string sterm)
        {
            TourCancelVM model = new TourCancelVM();
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

            model.tourCancelList = await objDB.Database.SqlQuery<TourCancelView>("udspMstTourCancelList @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<string> SaveTourCancelAsync(utblMstTourCancellation model)
        {
            try
            {
                var parPID = new SqlParameter("@CancellationID", model.CancellationID);
                var parIName = new SqlParameter("@CancellationDesc", model.CancellationDesc);

                return await objDB.Database.SqlQuery<string>("udspMstTourCancelAddEdit @CancellationID, @CancellationDesc",
                    parPID, parIName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblMstTourCancellation> GetTourCancelByIDAsync(long id)
        {
            utblMstTourCancellation obj = new utblMstTourCancellation();
            obj = await objDB.utblMstTourCancellations.Where(x => x.CancellationID == id).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<string> DeleteTourCancelAsync(long CancellationID)
        {
            try
            {
                utblMstTourCancellation curObj = await objDB.utblMstTourCancellations.FindAsync(CancellationID);
                objDB.utblMstTourCancellations.Remove(curObj);
                await objDB.SaveChangesAsync();
                return "Tour Cancellation Details Removed";
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

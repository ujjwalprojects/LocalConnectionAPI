using LocalConn.Entities.Models;
using LocalConn.Entities.Utility;
using LocalConn.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Dal
{
    public class dalWebRequest
    { 
        EFDBContext objDB = new EFDBContext();

        public async Task<List<CityList>> getCityMenuList()
        {
            try
            {
                long tempstateid = 0;
                List<CityList> list = new List<CityList>();
                var parID = new SqlParameter("@StateID", tempstateid);
                list=  await objDB.Database.SqlQuery<CityList>("udspLCAppGetCityList @StateID", parID).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        public async Task<List<HotelList>> getFtHotelList(string HomeTypeID)
        {
            try
            {
                var parID = new SqlParameter("@HomeTypeID", HomeTypeID);
                return await objDB.Database.SqlQuery<HotelList>("udspLCAppGetHotelList @HomeTypeID", parID).ToListAsync();

            }
            catch (Exception e)
            {

                throw e;
            }


        }

        public async Task<List<HotelList>> getHotelMenuList(string HomeTypeID)
        {
            try
            {
                var parID = new SqlParameter("@HomeTypeID", HomeTypeID);
                return await objDB.Database.SqlQuery<HotelList>("udspLCAppGetHotelList @HomeTypeID", parID).ToListAsync();

            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<HotelList>> getCityHotelList(string cityID)
        {
            try
            {
                var parID = new SqlParameter("@CityID", cityID);
                return await objDB.Database.SqlQuery<HotelList>("udspLCAppGetCityHotelList @CityID", parID).ToListAsync();

            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<HotelList>> getResortMenuList(string HomeTypeID)
        {
            try
            {
                var parID = new SqlParameter("@HomeTypeID", HomeTypeID);
                return await objDB.Database.SqlQuery<HotelList>("udspLCAppGetHotelList @HomeTypeID", parID).ToListAsync();

            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<HotelList>> getHomestayMenuList(string HomeTypeID)
        {
            try
            {
                var parID = new SqlParameter("@HomeTypeID", HomeTypeID);
                return await objDB.Database.SqlQuery<HotelList>("udspLCAppGetHotelList @HomeTypeID", parID).ToListAsync();

            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<HotelList>> getLodgeMenuList(string HomeTypeID)
        {
            try
            {
                var parID = new SqlParameter("@HomeTypeID", HomeTypeID);
                return await objDB.Database.SqlQuery<HotelList>("udspLCAppGetHotelList @HomeTypeID", parID).ToListAsync();

            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<HotelList>> getGHouseMenuList(string HomeTypeID)
        {
            try
            {
                var parID = new SqlParameter("@HomeTypeID", HomeTypeID);
                return await objDB.Database.SqlQuery<HotelList>("udspLCAppGetHotelList @HomeTypeID", parID).ToListAsync();

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        //search
        public async Task<List<HotelList>> getHotelSearchList(string query)
        {
            try
            {
                var parID = new SqlParameter("@query", query);
                return await objDB.Database.SqlQuery<HotelList>("udspLCAppGetHotelSearchList @query", parID).ToListAsync();

            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<HotelDtl> getHotelDtl(string HotelID)
        {
            try
            {
                var parID = new SqlParameter("@HotelID", HotelID);
                return await objDB.Database.SqlQuery<HotelDtl>("udspLCAppGetHotelDtl @HotelID", parID).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<HAmenitiesList>> getAmnetieslist(string HotelID)
        {
            try
            {
                var parID = new SqlParameter("@HotelID", HotelID);
                return await objDB.Database.SqlQuery<HAmenitiesList>("udspLCAppGetHAmenitiesList @HotelID", parID).ToListAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<HotelRoomList>> getHotelRoomList(string HotelID)
        {
            try
            {
                var parID = new SqlParameter("@HotelID", HotelID);
                return await objDB.Database.SqlQuery<HotelRoomList>("udspLCAppGetHotelRoomTypeList @HotelID", parID).ToListAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<HotelRoomImg>> getHRoomImgList(string HotelID)
        {
            try
            {
                var parID = new SqlParameter("@HotelID", HotelID);
                return await objDB.Database.SqlQuery<HotelRoomImg>("udspLCAppGetHotelRoomImgList @HotelID", parID).ToListAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<HotelPremisesList>> getHPremList(string HotelID)
        {
            try
            {
                var parID = new SqlParameter("@HotelID", HotelID);
                return await objDB.Database.SqlQuery<HotelPremisesList>("udspLCAppGetHotelPremName @HotelID", parID).ToListAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<HotelPremisesList>> getPremMenu(string HotelID)
        {
            try
            {
                var parID = new SqlParameter("@HotelID", HotelID);
                return await objDB.Database.SqlQuery<HotelPremisesList>("udspLCAppGetHotelPremName @HotelID", parID).ToListAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<HotelList>> gethotelvmlist(string HomeTypeID)
        {
            try
            {
                var parID = new SqlParameter("@HomeTypeID", HomeTypeID);
                return await objDB.Database.SqlQuery<HotelList>("udspLCAppGetHotelVMList @HomeTypeID", parID).ToListAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<FtHotelList>> getFeaturedlist(string Date)
        {
            try
            {
                var parID = new SqlParameter("@Date", Date);
                return await objDB.Database.SqlQuery<FtHotelList>("udspLCAppGetFeaturedList @Date", parID).ToListAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<FtHotelList_web>> getFeaturedlist_web()
        {
            try
            {
                //var pardate = new SqlParameter("@Date", Date);
                return await objDB.Database.SqlQuery<FtHotelList_web>("udspLCAppGetFeaturedList_Web").ToListAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<OrderList>> getOrderlist(string UserID)
        {
            try
            {
                var parID = new SqlParameter("@UserID", UserID);
                return await objDB.Database.SqlQuery<OrderList>("udspLCAppGetOrderList @UserID", parID).ToListAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<PreBookingDtl> getBookingDtl(string BookingID)
        {
            try
            {
                var parID = new SqlParameter("@BookingID", BookingID);
                return await objDB.Database.SqlQuery<PreBookingDtl>("udspLCAppGetBookingDtl @BookingID", parID).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public string preBooking(PreBookingDtl obj)
        {
            try
            {
                string Results = "";
                var parCName = new SqlParameter("@CustName", obj.CustName);
                var parCMail = new SqlParameter("@CustEmail", obj.CustEmail);
                var parCustPhNo = new SqlParameter("@CustPhNo", obj.CustPhNo);
                var parBHID = new SqlParameter("@HotelID", obj.HotelID);
                var parBFrom = new SqlParameter("@BookingFrom", obj.BookingFrom);
                var parBUpto = new SqlParameter("@BookingUpto", obj.BookingUpto);
                var parCDtl = new SqlParameter("@CustDetails", obj.CustDetails);
                var parBStatue = new SqlParameter("@BookingStatus", obj.BookingStatus);
                var parFare = new SqlParameter("@FinalFare", obj.FinalFare);
                var parPGCode = new SqlParameter("@PaymentGatewayCode", obj.PaymentGatewayCode);
                var parUserID = new SqlParameter("@UserID", obj.UserID);
                var parUserName = new SqlParameter("@UserName", obj.UserName);

                Results = objDB.Database.SqlQuery<string>("udspLCAppBookingRooms @CustName, @CustEmail,@CustPhNo,@HotelID,@BookingFrom,@BookingUpto,@CustDetails,@BookingStatus,@FinalFare,@PaymentGatewayCode,@UserID,@UserName",
                    parCName, parCMail, parCustPhNo, parBHID, parBFrom, parBUpto, parCDtl, parBStatue, parFare, parPGCode, parUserID, parUserName).FirstOrDefault();

                return Results;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public PreBookingDtl cancelBooking(string BookingID)
        {
            try
            {
                var parID = new SqlParameter("@BookingID", BookingID);
                return objDB.Database.SqlQuery<PreBookingDtl>("udspLCAppCancelBooking @BookingID", parID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public List<OfferList> getOfferlist(string Date)
        {
            try
            {
                var parID = new SqlParameter("@Date", Date);
                return objDB.Database.SqlQuery<OfferList>("udspLCAppGetOfferList @Date", parID).ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<HotelList_Offer>> getOfferHotellist(long OfferID)
        {
            try
            {
                var parID = new SqlParameter("@OfferID", OfferID);
                return await objDB.Database.SqlQuery<HotelList_Offer>("udspLCAppGetOfferHotelList_Web @OfferID", parID).ToListAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
       

        public async Task<List<HomeTypeOnOffer>> getHomtTypeOnOffer(long OfferID)
        {
            var parOfferID = new SqlParameter("@OfferID", OfferID);
            return await objDB.Database.SqlQuery<HomeTypeOnOffer>("udspLCAppGetOfferHomeTypeList @OfferID", parOfferID).ToListAsync();
        }
        public List<TermsPolicyList> getTermPolicyList(string HotelID)
        {
            try
            {
                var parID = new SqlParameter("@HotelID", HotelID);
                return objDB.Database.SqlQuery<TermsPolicyList>("udspLCAppGetTermPolicyList @HotelID", parID).ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public List<CancellationPolicyList> getCancelPolicyList(string HotelID)
        {
            try
            {
                var parID = new SqlParameter("@HotelID", HotelID);
                return objDB.Database.SqlQuery<CancellationPolicyList>("udspLCAppGetCancelPolicyList @HotelID", parID).ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public async Task<List<NotificationList>> getNotificationList()
        {
            try
            {
                return await objDB.Database.SqlQuery<NotificationList>("udspLCAppGetNotificationList").ToListAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public List<NearbyList> getNearByList(string HotelID, string NearByID)
        {
            try
            {
                var parHID = new SqlParameter("@HotelID", HotelID);
                var parNID = new SqlParameter("@NearByID", NearByID);
                return objDB.Database.SqlQuery<NearbyList>("udspLCAppGetNearbyList @HotelID,@NearByID", parHID, parNID).ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public HelpPageDtl getHelpPage()
        {
            try
            {
                return objDB.Database.SqlQuery<HelpPageDtl>("udspLCAppGetHelpPageDtl").FirstOrDefault();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public AboutUsDetails getAboutUs()
        {
            try
            {
                return objDB.Database.SqlQuery<AboutUsDetails>("udspLCAppGetDtl").FirstOrDefault();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public PolicyList getPolicyList()
        {
            try
            {
                PolicyList obj = new PolicyList();
                obj.PolicyTitle = objDB.Database.SqlQuery<PolicyTitle>("udspLCAppGetPolicyTitle").ToList();
                obj.PolicyData = objDB.Database.SqlQuery<PolicyData>("udspLCAppGetPolicyData").ToList();
                return obj;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        //General Page for web
        public async Task<GenLCHotelVM> SearchGenLCHotelListAsync(GenLCSearchModel search)
        {
            GenLCHotelVM model = new GenLCHotelVM();
            ConvertListToDT objList = new ConvertListToDT();
            DataTable subdt = new DataTable();
            try
            {
                //Converting subject list to datatable if record is present else send empty datatable
                if (search.HomeType != null)
                {
                    List<IDModel> villList = search.HomeType.Select(x => new IDModel()
                    {
                        ID = Convert.ToInt64(x)
                    }).ToList();
                    subdt = objList.ConvertIEnumerableToDataTable(villList);
                }
                else
                {
                    if (subdt.Columns.Count == 0)
                    {
                        DataColumn col = new DataColumn();
                        col.ColumnName = "ID";
                        subdt.Columns.Add(col);
                    }
                }
                var parSubDT = new SqlParameter("@HomeTypeTable", subdt);
                parSubDT.SqlDbType = SqlDbType.Structured;
                parSubDT.TypeName = "dbo.IDType";

                var parStart = new SqlParameter("@Start", (search.PageNo - 1) * search.PageSize);
                var parEnd = new SqlParameter("@PageSize", search.PageSize);

                var parWhere = new SqlParameter("@Where", DBNull.Value);
                if (!(search.Where == null || search.Where == ""))
                    parWhere.Value = search.Where;
                // setting stored procedure OUTPUT value
                // This return total number of rows, and avoid two database call for data and total number of rows 
                var spOutput = new SqlParameter
                {
                    ParameterName = "@TotalCount",
                    SqlDbType = System.Data.SqlDbType.BigInt,
                    Direction = System.Data.ParameterDirection.Output
                };

                model.HotelList = await objDB.Database.SqlQuery<HotelList>("udspGenLCHotelSearch @Start, @PageSize,@Where, @HomeTypeTable, @TotalCount out",
                    parStart, parEnd, parWhere, parSubDT, spOutput).ToListAsync();
                model.TotalRecords = int.Parse(spOutput.Value.ToString());
                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async Task<IEnumerable<string>> GetStateCityNamesAsync()
        {
            List<string> names = new List<string>();
            names = await objDB.utblLCMstCities.Select(x => x.CityName).Distinct().ToListAsync();
            names.AddRange(await objDB.utblMstStates.Select(x => x.StateName).Distinct().ToListAsync());
            return names;
        }
    }
}

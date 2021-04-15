﻿using LocalConn.Entities.Models;
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
        public async Task<List<OrderList>> getOrderlist(string CustPhNo)
        {
            try
            {
                var parID = new SqlParameter("@CustPhNo", CustPhNo);
                return await objDB.Database.SqlQuery<OrderList>("udspLCAppGetOrderList @CustPhNo", parID).ToListAsync();
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
                Results = objDB.Database.SqlQuery<string>("udspLCAppBookingRooms @CustName, @CustEmail,@CustPhNo,@HotelID,@BookingFrom,@BookingUpto,@CustDetails,@BookingStatus,@FinalFare,@PaymentGatewayCode",
                    parCName, parCMail, parCustPhNo, parBHID, parBFrom, parBUpto, parCDtl, parBStatue, parFare, parPGCode).FirstOrDefault();

                return Results;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public async Task<string> confirmBooking()
        //{

        //}


        public List<OfferList> getOfferlist(string Date)
        {
            try
            {
                var parID = new SqlParameter("@Date", Date);
                return  objDB.Database.SqlQuery<OfferList>("udspLCAppGetOfferList @Date", parID).ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<List<HotelList>> getOfferHotellist(long OfferID)
        {
            try
            {
                var parID = new SqlParameter("@OfferID", OfferID);
                return await objDB.Database.SqlQuery<HotelList>("udspLCAppGetOfferHotelList @OfferID", parID).ToListAsync();
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



    }
}

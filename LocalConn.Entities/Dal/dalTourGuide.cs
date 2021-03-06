﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LocalConn.Entities.Models;
using System.Data.Entity;
using LocalConn.Entities.ViewModels;

namespace LocalConn.Entities.Dal
{
    public class dalTourGuide
    {
        EFDBContext db = new EFDBContext();

        public async Task<string> SaveGuideAsync(utblTourGuide model)
        {
            try
            {
                var parGuideID = new SqlParameter("@TourGuideID", model.TourGuideID);
                var parGuideName = new SqlParameter("@TourGuideName", model.TourGuideName);
                var parGuideDesc = new SqlParameter("@TourGuideDesc", model.TourGuideDesc);
                var parGuideLink = new SqlParameter("@TourGuideLink", model.TourGuideLink);

                return await db.Database.SqlQuery<string>("udspTourGuideSave @TourGuideID, @TourGuideName, @TourGuideDesc, @TourGuideLink",
                    parGuideID, parGuideName, parGuideDesc, parGuideLink).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblTourGuide> GetGuideByIDAsync(long id)
        {
            return await db.utblTourGuides.Where(x => x.TourGuideID == id).FirstOrDefaultAsync();
        }
        public async Task<string> DeleteGuideAsync(long id)
        {
            try
            {
                utblTourGuide obj = await db.utblTourGuides.FindAsync(id);
                db.utblTourGuides.Remove(obj);
                await db.SaveChangesAsync();
                return "Tour Guide Details Removed";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<TourGuideVM> GetGuidesPagedAsync(int pageno, int pagesize, string searchterm)
        {
            TourGuideVM model = new TourGuideVM();
            var parStart = new SqlParameter("@Start", (pageno - 1) * pagesize);
            var parEnd = new SqlParameter("@PageSize", pagesize);

            var parSearchTerm = new SqlParameter("@SearchTerm", DBNull.Value);
            if (!(searchterm == null || searchterm == ""))
                parSearchTerm.Value = searchterm;
            // setting stored procedure OUTPUT value
            // This return total number of rows, and avoid two database call for data and total number of rows 
            var spOutput = new SqlParameter
            {
                ParameterName = "@TotalCount",
                SqlDbType = System.Data.SqlDbType.BigInt,
                Direction = System.Data.ParameterDirection.Output
            };

            model.TourGuides = await db.Database.SqlQuery<TourGuideListView>("udspTourGuidesPaged @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public async Task<IEnumerable<TourGuideListView>> GetAllGuidesAsync()
        {
            return await db.utblTourGuides.Select(x => new TourGuideListView()
                {
                    TourGuideID = x.TourGuideID,
                    TourGuideLink = x.TourGuideLink,
                    TourGuideName = x.TourGuideName
                }).ToListAsync();
        }
    }
}

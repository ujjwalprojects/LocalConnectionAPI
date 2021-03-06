﻿using System;
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
    public class dalAgentConfig
    {
        EFDBContext objDB = new EFDBContext();

        #region Agent Registration

        public async Task<AgentVM> GetAgentListAsync(int pageno, int pagesize, string sterm)
        {
            AgentVM model = new AgentVM();
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

            model.AgentList = await objDB.Database.SqlQuery<AgentView>("udspAgentList @Start, @PageSize,@SearchTerm, @TotalCount out",
                parStart, parEnd, parSearchTerm, spOutput).ToListAsync();
            model.TotalRecords = int.Parse(spOutput.Value.ToString());
            return model;
        }
        public string SaveAgentAsync(utblAgent model)
        {
            try
            {
                var parPID = new SqlParameter("@AgentID", model.AgentID);
                var parAName = new SqlParameter("@AgentName", model.AgentName);
                var parAAddress = new SqlParameter("@AgentAddress", model.AgentAddress);
                var parAEmail = new SqlParameter("@AgentEmail", model.AgentEmail);
                var parAMobile = new SqlParameter("@AgentMobile", DBNull.Value);
                if(model.AgentMobile!=null)
                parAMobile = new SqlParameter("@AgentMobile", model.AgentMobile);
                var parADoc = new SqlParameter("@AgentDocumentPath", DBNull.Value);
                if(model.AgentDocumentPath!=null)
                parADoc = new SqlParameter("@AgentDocumentPath", model.AgentDocumentPath);
                var parAStatus = new SqlParameter("@Status", model.Status);
                return  objDB.Database.SqlQuery<string>("udspAgentAddEdit @AgentID, @AgentName,@AgentAddress,@AgentEmail,@AgentMobile,@AgentDocumentPath,@Status",
                    parPID, parAName, parAAddress, parAEmail, parAMobile,parADoc,parAStatus).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public async Task<utblAgent> GetAgentByIDAsync(long id)
        {
            utblAgent obj = new utblAgent();
            obj = await objDB.utblAgents.Where(x => x.AgentID == id).FirstOrDefaultAsync();
            return obj;
        }
        public async Task<string> DeleteAgentAsync(long AgentID)
        {
            var parID = new SqlParameter("@AgentID", AgentID);
            return await objDB.Database.SqlQuery<string>("udspAgentDelete @AgentID", parID).FirstOrDefaultAsync();
        }
        #endregion 

    }
}

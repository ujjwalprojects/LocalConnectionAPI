﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblClientEnquirie
    {
        [Key]
        public string EnquiryCode { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhoneNo { get; set; }
        public long RefPackageID { get; set; }
        public int NoOfDays { get; set; }
        public string Remarks { get; set; }
        public DateTime DateOfArrival { get; set; }
        public string NoOfAdult { get; set; }
        public string NoOfChildren { get; set; }
        public long? HotelTypeID { get; set; }
        public long? CabTypeID { get; set; }
        public bool IsDirectBooking { get; set; }
        public string Status { get; set; }
        public DateTime TransDate { get; set; }
    }
}

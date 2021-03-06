﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.ViewModels
{
    public class DestinationView
    {
        public long DestinationID { get; set; }
        public long CountryID { get; set; }
        public long StateID { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string DestinationName { get; set; }
        public string DestinationDesc { get; set; }
        public string DestinationImagePath { get; set; }
    }
    public class DestinationVM
    {
        public IEnumerable<DestinationView> Destinations { get; set; }
        public int TotalRecords { get; set; }
    }
}

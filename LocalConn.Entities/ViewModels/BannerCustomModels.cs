﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.ViewModels
{
    public class BannerView
    {
        public long BannerID { get; set; }
        public string BannerPath { get; set; }
        public string UserID { get; set; }
        public DateTime TransDate { get; set; }
    }
    public class BannerVM
    {
        public IEnumerable<BannerView> BannerList { get; set; }
        public int TotalRecords { get; set; }
    }
}

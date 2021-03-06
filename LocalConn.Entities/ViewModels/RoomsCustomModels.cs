﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.ViewModels
{
    public class RoomsView
    {
        public long RoomID { get; set; }
        public string RoomType { get; set; }
        public decimal RoomBaseFare { get; set; }
        public int TotalCapacity { get; set; }
    }
    public class RoomsVM
    {
        public IEnumerable<RoomsView> Rooms { get; set; }
        public int TotalRecords { get; set; }
    }

    public class RoomTypeDD
    {
        public long RoomID { get; set; }
        public string RoomType { get; set; }
    }
}

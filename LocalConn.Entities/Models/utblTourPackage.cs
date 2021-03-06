﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblTourPackage
    {
        [Key]
        public long PackageID { get; set; }
        [Required]
        public string PackageName { get; set; }
        [Required]
        public long PackageTypeID { get; set; }
        public string PackageRouting { get; set; }
        public string PickupPoint { get; set; }
        public string DropPoint { get; set; }
        [Required]
        public int TotalDays { get; set; }
        public decimal BaseFare { get; set; }
        public string PackageDesc { get; set; }
        public decimal FinalFare { get; set; }
        public string FarePer { get; set; }
        public long PackageHitCount { get; set; }
        public bool IsActive { get; set; }
        public string LinkText { get; set; }
        public string MetaText { get; set; }
        public string MetaDesc { get; set; }
        public bool ShowPackage { get; set; }
    }

  
}

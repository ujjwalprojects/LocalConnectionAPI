using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblLCFeatureOffer
    {
        [Key]
        public long OfferID { get; set; }
        public string OfferTagLine { get; set; }
        public string OfferImagePath { get; set; }
        public DateTime OfferStartDate { get; set; }
        public DateTime OfferEndDate { get; set; }
    }
}

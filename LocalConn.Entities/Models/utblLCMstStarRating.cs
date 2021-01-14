using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.Models
{
    public class utblLCMstStarRating
    {
        [Key]
        public long StarRatingID { get; set; }
        public string StarRatingName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.ViewModels
{
    public class LocalitiesView
    {
        public long LocalityID { get; set; }
        public string LocalityName { get; set; }
        public long CityID { get; set; }
        public string  CityName { get; set; }
    }
    public class LocalitiesVM
    {
        public IEnumerable<LocalitiesView> Localities { get; set; }
        public int TotalRecords { get; set; }
    }
    public class LocalitiesDD
    {
        public long LocalityID { get; set; }
        public string LocalityName { get; set; }
    }
}

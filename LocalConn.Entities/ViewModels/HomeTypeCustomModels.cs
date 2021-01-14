using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConn.Entities.ViewModels
{

    public class HomeTypeView
    {
        public long HomeTypeID { get; set; }
        public string HomeTypeName { get; set; }
    }
    public class HomeTypeVM
    {
        public IEnumerable<HomeTypeView> HomeTypes { get; set; }
        public int TotalRecords { get; set; }
    }
}

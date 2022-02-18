using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iurec_Alexandra_Proiect_Master.Models
{
    public class BookedEquipment
    {
        public int EquipmentID { get; set; }
        public int BeautyServiceID { get; set; }
        public Equipment Equipment { get; set; }
        public BeautyService BeautyService { get; set; }
    }
}

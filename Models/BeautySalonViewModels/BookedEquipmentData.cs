using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iurec_Alexandra_Proiect_Master.Models.BeautySalonViewModels
{
   
    public class BookedEquipmentData
    {
        public int BeautyServiceID { get; set; }
        public string Title { get; set; }
        public bool IsBooked { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iurec_Alexandra_Proiect_Master.Models.BeautySalonViewModels
{
    // aceasta clasa a fost creata pentru ca vrem sa afisam date din tabele multiple
    public class EquipmentIndexData
    {
        public IEnumerable<Equipment> Equipments { get; set; }
        public IEnumerable<BeautyService> BeautyServices { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
    }
}

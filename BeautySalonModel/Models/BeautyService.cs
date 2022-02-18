using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Iurec_Alexandra_Proiect_Master.Models
{
    public class BeautyService
    {
        public int BeautyServiceID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<BookedEquipment> BookedEquipments { get; set; }
    }
}

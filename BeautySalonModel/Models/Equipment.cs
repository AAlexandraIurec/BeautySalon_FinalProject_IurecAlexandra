using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Iurec_Alexandra_Proiect_Master.Models
{
    public class Equipment
    {
        public int EquipmentID { get; set; }
        [Required]
        [Display(Name = "Equipment Name")]
        [StringLength(50)]
        public string EquipmentName { get; set; }
        [Display(Name = "Year of Manufacture")]
        public int ManufactureYear { get; set; }
        [StringLength(70)]
        public string Producer { get; set; }
        public ICollection<BookedEquipment> BookedEquipments { get; set; }
    }
}

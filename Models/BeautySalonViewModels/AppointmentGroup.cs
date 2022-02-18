using System;
using System.ComponentModel.DataAnnotations;

namespace Iurec_Alexandra_Proiect_Master.Models.BeautySalonViewModels
{
    public class AppointmentGroup
    {
        [DataType(DataType.Date)]
        public DateTime? AppointmentDate { get; set; }
        public int BeautyServiceCount { get; set; }
    }
}

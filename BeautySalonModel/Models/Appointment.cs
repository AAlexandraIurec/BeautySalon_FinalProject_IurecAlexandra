using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iurec_Alexandra_Proiect_Master.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int EmployeeID { get; set; }
        public int BeautyServiceID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public Employee Employee { get; set; }
        public BeautyService BeautyService { get; set; }
    }
}

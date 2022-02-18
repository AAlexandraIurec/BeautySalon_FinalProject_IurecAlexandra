using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Iurec_Alexandra_Proiect_Master.Models
{
    public class Employee
    {
       // [DatabaseGenerated(DatabaseGeneratedOption.None)] -> mi-ar fi dat posibilitatea sa dau eu un id angajatului (adica sa nu fie generat automat de baza de date)
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }

        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
        public ICollection<Appointment> Appointments { get; set; } //navigation property care poate contine mai multe entitati relationate
    }
}

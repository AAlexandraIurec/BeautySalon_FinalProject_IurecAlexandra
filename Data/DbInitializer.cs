using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iurec_Alexandra_Proiect_Master.Models;

namespace Iurec_Alexandra_Proiect_Master.Data
{
    // clasa realizata pentru crearea bazei de date si inserarea unui set de date
    public class DbInitializer
    {
        public static void Initialize(BeautySalonContext context)
        {
            context.Database.EnsureCreated();
            if (context.BeautyServices.Any())
            {
                return; // BD a fost creata anterior
            }
            var beautyServices = new BeautyService[]
            {
                new BeautyService{Title="Manichiura Semipermanenta",Description="Se foloseste oja semipermanenta si lampa UV", Price=Decimal.Parse("75")},
                new BeautyService{Title="Manichiura simpla",Description="Se foloseste oja normala", Price=Decimal.Parse("40")},
                new BeautyService{Title="Epilare definitiva picior lung",Description="Toata lungimea piciorului va fi epilata cu laser in mai multe sedinte. Pretul este pe sedinta", Price=Decimal.Parse("310")},
                new BeautyService{Title="Epilare definitiva axila",Description="Epilare cu laser.Pretul este pe sedinta", Price=Decimal.Parse("100")},
                new BeautyService{Title="Tuns par lung",Description="Include spalatul parului", Price=Decimal.Parse("50")},
                new BeautyService{Title="Tuns par mediu",Description="Include spalatul parului", Price=Decimal.Parse("35")},
                new BeautyService{Title="Tratament facial de hidratare",Description="Creme hodratante si masaj", Price=Decimal.Parse("80")},
                new BeautyService{Title="Pensat",Description="Pentru spramcene si mustata", Price=Decimal.Parse("40")},
            };
            foreach (BeautyService bs in beautyServices)
            {
                context.BeautyServices.Add(bs);
            }
            context.SaveChanges();

            var employees = new Employee[]
            {
                new Employee{Name="Popescu Maria", JobDescription="Cosmetician", BirthDate=DateTime.Parse("1989-06-15")},
                new Employee{Name="Ionescu Elena", JobDescription="Manichiurist", BirthDate=DateTime.Parse("1995-08-02")},
                new Employee{Name="Enescu Livia", JobDescription="Cosmetician", BirthDate=DateTime.Parse("1997-04-28")},
                new Employee{Name="Marinescu Silvia", JobDescription="Hair Stylist", BirthDate=DateTime.Parse("1991-02-02")},
            };
            foreach (Employee e in employees)
            {
                context.Employees.Add(e);
            }
            context.SaveChanges();

            var appointments = new Appointment[]
            {
                new Appointment{EmployeeID=1,BeautyServiceID=3, AppointmentDate=DateTime.Parse("2021-12-15 15:30:00 GMT")},
                new Appointment{EmployeeID=1,BeautyServiceID=4, AppointmentDate=DateTime.Parse("2021-12-14 12:00:00 GMT")},
                new Appointment{EmployeeID=2,BeautyServiceID=1, AppointmentDate=DateTime.Parse("2021-12-16 09:15:00 GMT")},
                new Appointment{EmployeeID=2,BeautyServiceID=1, AppointmentDate=DateTime.Parse("2021-12-18 11:40:00 GMT")},
                new Appointment{EmployeeID=1,BeautyServiceID=5, AppointmentDate=DateTime.Parse("2021-12-18 14:00:00 GMT")},
                new Appointment{EmployeeID=4,BeautyServiceID=6, AppointmentDate=DateTime.Parse("2021-12-14 10:30:00 GMT")},
                new Appointment{EmployeeID=3,BeautyServiceID=7, AppointmentDate=DateTime.Parse("2021-12-16 16:00:00 GMT")},
                new Appointment{EmployeeID=3,BeautyServiceID=8, AppointmentDate=DateTime.Parse("2021-12-16 14:00:00 GMT")},
                new Appointment{EmployeeID=3,BeautyServiceID=8, AppointmentDate=DateTime.Parse("2021-12-19 11:00:00 GMT")},
            };
            foreach (Appointment a in appointments)
            {
                context.Appointments.Add(a);
            }
            context.SaveChanges();

            var equipments = new Equipment[]
            {
                new Equipment{EquipmentName= "Laser Dioda", ManufactureYear=2018, Producer="Mattler Toledo" },
                new Equipment{EquipmentName= "Laser IPL", ManufactureYear=2019, Producer="Philips" },
                new Equipment{EquipmentName= "Lampa UV LED Evolution", ManufactureYear=2020, Producer="LUXORISE" },
                new Equipment{EquipmentName= "Scafa Coafor Unitate", ManufactureYear=2017, Producer="Trendis" },
                new Equipment{EquipmentName= "Pat Cosmetica", ManufactureYear=2016, Producer="VidaXL" },
                new Equipment{EquipmentName= "Pat Cosmetica Ergonomic", ManufactureYear=2019, Producer="Vivre" },
            };
            foreach (Equipment eq in equipments)
            {
                context.Equipments.Add(eq);
            }
            context.SaveChanges();

            var bookedEquipments = new BookedEquipment[]
            {
                new BookedEquipment {
                    EquipmentID = equipments.Single(c => c.EquipmentName == "Lampa UV LED Evolution" ).EquipmentID,
                    BeautyServiceID = beautyServices.Single(c => c.Title == "Manichiura Semipermanenta" ).BeautyServiceID,
                },
                new BookedEquipment {
                    EquipmentID = equipments.Single(c => c.EquipmentName == "Laser Dioda" ).EquipmentID,
                    BeautyServiceID = beautyServices.Single(c => c.Title == "Epilare definitiva picior lung" ).BeautyServiceID,
                },
                new BookedEquipment {
                    EquipmentID = equipments.Single(c => c.EquipmentName == "Pat Cosmetica" ).EquipmentID,
                    BeautyServiceID = beautyServices.Single(c => c.Title == "Epilare definitiva picior lung" ).BeautyServiceID,
                },
                new BookedEquipment {
                    EquipmentID = equipments.Single(c => c.EquipmentName == "Pat Cosmetica Ergonomic" ).EquipmentID,
                    BeautyServiceID = beautyServices.Single(c => c.Title == "Epilare definitiva axila" ).BeautyServiceID,
                },
                 new BookedEquipment {
                    EquipmentID = equipments.Single(c => c.EquipmentName == "Laser IPL" ).EquipmentID,
                    BeautyServiceID = beautyServices.Single(c => c.Title == "Epilare definitiva axila" ).BeautyServiceID,
                },
                 new BookedEquipment {
                    EquipmentID = equipments.Single(c => c.EquipmentName == "Scafa Coafor Unitate" ).EquipmentID,
                    BeautyServiceID = beautyServices.Single(c => c.Title == "Tuns par lung" ).BeautyServiceID,
                },
                   new BookedEquipment {
                    EquipmentID = equipments.Single(c => c.EquipmentName == "Scafa Coafor Unitate" ).EquipmentID,
                    BeautyServiceID = beautyServices.Single(c => c.Title == "Tuns par mediu" ).BeautyServiceID,
                },
                     new BookedEquipment {
                    EquipmentID = equipments.Single(c => c.EquipmentName == "Pat Cosmetica Ergonomic" ).EquipmentID,
                    BeautyServiceID = beautyServices.Single(c => c.Title == "Pensat" ).BeautyServiceID,
                },
                    new BookedEquipment {
                    EquipmentID = equipments.Single(c => c.EquipmentName == "Pat Cosmetica Ergonomic" ).EquipmentID,
                    BeautyServiceID = beautyServices.Single(c => c.Title == "Tratament facial de hidratare" ).BeautyServiceID,
                },
            };
            foreach (BookedEquipment bkeq in bookedEquipments)
            {
                context.BookedEquipments.Add(bkeq);
            }
            context.SaveChanges();
        }
    }
}
   

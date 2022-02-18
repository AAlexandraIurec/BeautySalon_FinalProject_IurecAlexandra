using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Iurec_Alexandra_Proiect_Master.Models;

namespace Iurec_Alexandra_Proiect_Master.Data
{
    public class BeautySalonContext : DbContext
    {
        public BeautySalonContext(DbContextOptions<BeautySalonContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Appointment> Appointments { get; set; } // entity set corespunde unui tabel, entity corespunde unui rand din tabel
        public DbSet<BeautyService> BeautyServices { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<BookedEquipment> BookedEquipments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) // pentru crearea tabelelor cu nume diferit de cel dat de DbSet
        {
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<Appointment>().ToTable("Appointment");
            modelBuilder.Entity<BeautyService>().ToTable("BeautyService");
            modelBuilder.Entity<Equipment>().ToTable("Equipment");
            modelBuilder.Entity<BookedEquipment>().ToTable("BookedEquipment");
            modelBuilder.Entity<BookedEquipment>().HasKey(c => new { c.BeautyServiceID, c.EquipmentID });//configureaza cheia primara compusa
        }
    }
}

using Iurec_Alexandra_Proiect_Master.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Iurec_Alexandra_Proiect_Master.Data;
using Iurec_Alexandra_Proiect_Master.Models.BeautySalonViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Iurec_Alexandra_Proiect_Master.Controllers
{
    public class HomeController : Controller
    {
        // instantiem contextul
        private readonly BeautySalonContext _context;
        public HomeController(BeautySalonContext context)
        {
            _context = context;
        }
        
        /*private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        */

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize(Roles = "Employee,Manager")]

        // metoda care utilizeaza o interogare LINQ care grupeaza entitatile programari dupa data programarii, calculand numarul de programari realizate pentru fiecare 
        //data calendaristica si salveaza rezultatul intr-o colectie ppointmentGroup
        public async Task<ActionResult> Statistics()
        {
            IQueryable<AppointmentGroup> data = from appointment in _context.Appointments
            group appointment by appointment.AppointmentDate.Date into dateGroup
            select new AppointmentGroup()
            {
                AppointmentDate = dateGroup.Key,
                BeautyServiceCount = dateGroup.Count()
            };
            return View(await data.AsNoTracking().ToListAsync());
        }
    }
}

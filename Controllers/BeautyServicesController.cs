using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Iurec_Alexandra_Proiect_Master.Data;
using Iurec_Alexandra_Proiect_Master.Models;
using Microsoft.AspNetCore.Authorization;

namespace Iurec_Alexandra_Proiect_Master.Controllers
{
    [Authorize(Roles = "Employee")]
    public class BeautyServicesController : Controller
    {
        private readonly BeautySalonContext _context;

        public BeautyServicesController(BeautySalonContext context)
        {
            _context = context;
        }

        // GET: BeautyServices
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
           
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var beautyServices = from bs in _context.BeautyServices
                        select bs;
            if (!String.IsNullOrEmpty(searchString))
            {
                beautyServices = beautyServices.Where(s => s.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    beautyServices = beautyServices.OrderByDescending(b => b.Title);
                    break;
                case "Price":
                    beautyServices = beautyServices.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    beautyServices = beautyServices.OrderByDescending(b => b.Price);
                    break;
                default:
                    beautyServices = beautyServices.OrderBy(b => b.Title);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<BeautyService>.CreateAsync(beautyServices.AsNoTracking(), pageNumber ?? 1, pageSize));
       
        }

        // GET: BeautyServices/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /* var beautyService = await _context.BeautyServices
                 .FirstOrDefaultAsync(m => m.BeautyServiceID == id);    -> se regasea o singura entitate BeautyService */

            // regasim colectia din proprietatea Appointments
            var beautyService = await _context.BeautyServices
                .Include(bs => bs.Appointments)
                .ThenInclude(e => e.Employee) //provoaca obiectul context sa incarce BeautyService.Appointments navigation property si pentru fiecare programare, Appointment.Employee navigation property.
                .AsNoTracking() // imbunatateste performanta in scenarii in care entitatile returnate nu vor fi actualizate pe durata de viata a contextului curent
                .FirstOrDefaultAsync(m => m.BeautyServiceID == id);

            if (beautyService == null)
            {
                return NotFound();
            }

            return View(beautyService);
        }

        // GET: BeautyServices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BeautyServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken] //previne atacurile cu cereri false cross-site.
        public async Task<IActionResult> Create([Bind("Title,Description,Price")] BeautyService beautyService) 
        // cu Bind datele din formular sunt convertite si trimise ca si parametri la action method
        // se instantiaza o entitate BeautyService utilizand valorile din formular
        {
            try
            {
                if (ModelState.IsValid) //Daca modelul e validat
                {
                    _context.Add(beautyService); // se adauga entitatea la entity set-ul BeautyServices
                    await _context.SaveChangesAsync(); // se salveaza modificarile in baza de date
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {
                ModelState.AddModelError("", "Unable to save changes. " +"Try again  if the problem persists ");
            }
            return View(beautyService);
        }

        // GET: BeautyServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beautyService = await _context.BeautyServices.FindAsync(id);
            if (beautyService == null)
            {
                return NotFound();
            }
            return View(beautyService);
        }

        // POST: BeautyServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceToUpdate = await _context.BeautyServices.FirstOrDefaultAsync(s => s.BeautyServiceID == id);
            if (await TryUpdateModelAsync<BeautyService>(
            serviceToUpdate,
            "",
            s => s.Description, s => s.Title, s => s.Price))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +"Try again if the problem persists");
                }
            }
            
            return View(serviceToUpdate);
        }

        // GET: BeautyServices/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var beautyService = await _context.BeautyServices
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.BeautyServiceID == id);
            if (beautyService == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] ="Delete failed. Try again";
            }

            return View(beautyService);
        }

        // POST: BeautyServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var beautyService = await _context.BeautyServices.FindAsync(id);
            if (beautyService == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.BeautyServices.Remove(beautyService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool BeautyServiceExists(int id)
        {
            return _context.BeautyServices.Any(e => e.BeautyServiceID == id);
        }
    }
}

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
    public class AppointmentsController : Controller
    {
        private readonly BeautySalonContext _context;

        public AppointmentsController(BeautySalonContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var beautySalonContext = _context.Appointments.Include(a => a.BeautyService).Include(a => a.Employee);
            //return View(await beautySalonContext.ToListAsync());
  
            int pageSize = 4;
            return View(await PaginatedList<Appointment>.CreateAsync(beautySalonContext.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.BeautyService)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["BeautyServiceID"] = new SelectList(_context.BeautyServices, "BeautyServiceID", "BeautyServiceID");
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "EmployeeID");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentID,EmployeeID,BeautyServiceID,AppointmentDate")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BeautyServiceID"] = new SelectList(_context.BeautyServices, "BeautyServiceID", "BeautyServiceID", appointment.BeautyServiceID);
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "EmployeeID", appointment.EmployeeID);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["BeautyServiceID"] = new SelectList(_context.BeautyServices.ToHashSet<BeautyService>(), "BeautyServiceID", "Title");
            //ViewData["BeautyServiceID"] = new SelectList(_context.BeautyServices, "BeautyServiceID", "BeautyServiceID", appointment.BeautyServiceID);
            ViewData["EmployeeID"] = new SelectList(_context.Employees.ToHashSet<Employee>(), "EmployeeID", "Name");
            //ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "EmployeeID", appointment.EmployeeID);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AppointmentID,EmployeeID,BeautyServiceID,AppointmentDate")] Appointment appointment)
        {
            if (id != appointment.AppointmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointmentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BeautyServiceID"] = new SelectList(_context.BeautyServices, "BeautyServiceID", "BeautyServiceID", appointment.BeautyServiceID);
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "EmployeeID", appointment.EmployeeID);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.BeautyService)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentID == id);
        }
    }
}

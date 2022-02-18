using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Iurec_Alexandra_Proiect_Master.Data;
using Iurec_Alexandra_Proiect_Master.Models;
using Iurec_Alexandra_Proiect_Master.Models.BeautySalonViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Iurec_Alexandra_Proiect_Master.Controllers
{
    [Authorize(Policy = "ManagerAcess")]
    public class EquipmentsController : Controller
    {
        private readonly BeautySalonContext _context;

        public EquipmentsController(BeautySalonContext context)
        {
            _context = context;
        }

        // GET: Equipments
        public async Task<IActionResult> Index(int? id, int? beautyServiceID)
        {
            var viewModel = new EquipmentIndexData();
            viewModel.Equipments = await _context.Equipments
            .Include(i => i.BookedEquipments)
            .ThenInclude(i => i.BeautyService)
            .ThenInclude(i => i.Appointments)
            .ThenInclude(i => i.Employee)
            .AsNoTracking()
            .OrderBy(i => i.EquipmentName)
            .ToListAsync();
            if (id != null)
            {
                ViewData["EquipmentID"] = id.Value;
                Equipment equipment = viewModel.Equipments.Where(
                i => i.EquipmentID == id.Value).Single();
                viewModel.BeautyServices = equipment.BookedEquipments.Select(s => s.BeautyService);
            }
            if (beautyServiceID!= null)
            {
                ViewData["BeautyServiceID"] = beautyServiceID.Value;
                viewModel.Appointments = viewModel.BeautyServices.Where(
                x => x.BeautyServiceID == beautyServiceID).Single().Appointments;
            }
            return View(viewModel);
        }

        // GET: Equipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipments
                .FirstOrDefaultAsync(m => m.EquipmentID == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: Equipments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Equipments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EquipmentID,EquipmentName,ManufactureYear,Producer")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(equipment);
        }

        // GET: Equipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipments
                .Include(i => i.BookedEquipments).ThenInclude(i => i.BeautyService)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.EquipmentID == id);
            if (equipment == null)
            {
                return NotFound();
            }
            PopulateBookedEquipmnetData(equipment);
            return View(equipment);
        }

        private void PopulateBookedEquipmnetData(Equipment equipment)
        {
            var allBeautyServices = _context.BeautyServices;
            var bookedEquipments = new HashSet<int>(equipment.BookedEquipments.Select(c => c.BeautyServiceID));
            var viewModel = new List<BookedEquipmentData>();
            foreach (var beautyService in allBeautyServices)
            {
                viewModel.Add(new BookedEquipmentData
                {
                    BeautyServiceID = beautyService.BeautyServiceID,
                    Title = beautyService.Title,
                    IsBooked = bookedEquipments.Contains(beautyService.BeautyServiceID)
                });
            }
            ViewData["BeautyServices"] = viewModel;
        }

        // POST: Equipments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedBeautyServices)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipmentToUpdate = await _context.Equipments
                .Include(i => i.BookedEquipments)
                .ThenInclude(i => i.BeautyService)
                .FirstOrDefaultAsync(m => m.EquipmentID == id);

            if (await TryUpdateModelAsync<Equipment>(equipmentToUpdate,"",i=> i.EquipmentName, i => i.ManufactureYear, i => i.Producer))
            {
                UpdateBookedEquipments(selectedBeautyServices, equipmentToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +"Try again if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateBookedEquipments(selectedBeautyServices, equipmentToUpdate);
            PopulateBookedEquipmnetData(equipmentToUpdate);
            return View(equipmentToUpdate);

        }

        public void UpdateBookedEquipments(string[] selectedBeautyServices, Equipment equipmentToUpdate)
        {
            if (selectedBeautyServices == null)
            {
                equipmentToUpdate.BookedEquipments = new List<BookedEquipment>();
                return;
            }

            var selectedBeautyServiceHS = new HashSet<string>(selectedBeautyServices);
            var bookedEquipments = new HashSet<int>(equipmentToUpdate.BookedEquipments.Select(c => c.BeautyService.BeautyServiceID));
            foreach (var beautyService in _context.BeautyServices)
            {
                if (selectedBeautyServiceHS.Contains(beautyService.BeautyServiceID.ToString()))
                {
                    if (!bookedEquipments.Contains(beautyService.BeautyServiceID))
                    {
                        equipmentToUpdate.BookedEquipments.Add(new BookedEquipment { EquipmentID = equipmentToUpdate.EquipmentID, BeautyServiceID = beautyService.BeautyServiceID });
                    }
                }
                else
                {
                    if (bookedEquipments.Contains(beautyService.BeautyServiceID))
                    {
                        BookedEquipment beautyServiceToRemove = equipmentToUpdate.BookedEquipments.FirstOrDefault(i => i.BeautyServiceID == beautyService.BeautyServiceID);
                        _context.Remove(beautyServiceToRemove);
                    }
                }
            }
        }

        // GET: Equipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipments
                .FirstOrDefaultAsync(m => m.EquipmentID == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipment = await _context.Equipments.FindAsync(id);
            _context.Equipments.Remove(equipment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipments.Any(e => e.EquipmentID == id);
        }
    }
}

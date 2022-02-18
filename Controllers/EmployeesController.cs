using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Iurec_Alexandra_Proiect_Master.Data;
using Iurec_Alexandra_Proiect_Master.Models;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json; // utilizat pentru deserializare
using Microsoft.AspNetCore.Authorization;

namespace Iurec_Alexandra_Proiect_Master.Controllers
{
    [Authorize(Policy = "ManagerAcess")]
    public class EmployeesController : Controller
    {
        private readonly BeautySalonContext _context;
        private string _baseUrl = "http://localhost:56983/api/Employees";

        public EmployeesController(BeautySalonContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient(); // utilizat pentru a primi raspunsuri de la serviciul web
            var response = await client.GetAsync(_baseUrl);
            if (response.IsSuccessStatusCode)
            {
                var employees = JsonConvert.DeserializeObject<List<Employee>>(await response.Content. 
                ReadAsStringAsync()); //utilizat pentru deserializare din Json
                return View(employees);
            }
            return NotFound();
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var employee = JsonConvert.DeserializeObject<Employee>(
                await response.Content.ReadAsStringAsync());
                return View(employee);
            }
            return NotFound();
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,JobDescription,BirthDate")] Employee employee)
        {
            if (!ModelState.IsValid) return View(employee);
            try
            {
                var client = new HttpClient();
                string json = JsonConvert.SerializeObject(employee);
                var response = await client.PostAsync(_baseUrl,
                new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to create record: {ex.Message}");
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var employee = JsonConvert.DeserializeObject<Employee>(
                await response.Content.ReadAsStringAsync());
                return View(employee);
            }
            return new NotFoundResult();
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,Name,JobDescription,BirthDate")] Employee employee)
        {
            if (!ModelState.IsValid) return View(employee);
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(employee);
            var response = await client.PutAsync($"{_baseUrl}/{employee.EmployeeID}",
            new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }

            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var employee = JsonConvert.DeserializeObject<Employee>(await response.Content.ReadAsStringAsync());
                return View(employee);
            }
            return new NotFoundResult();
        }

        // POST: Employees/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("EmployeeID")] Employee employee)
        {
            try
            {
                var client = new HttpClient();
                HttpRequestMessage request =
                new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{employee.EmployeeID}")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json")
                };
                var response = await client.SendAsync(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to delete record: {ex.Message}");
            }
            return View(employee);
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeID == id);
        }
    }
}

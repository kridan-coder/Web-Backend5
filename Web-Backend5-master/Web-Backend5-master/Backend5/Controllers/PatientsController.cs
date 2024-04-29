using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend5.Data;
using Backend5.Models;

namespace Backend5.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Patients.ToListAsync());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .SingleOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Birthday,Gender")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                var patients = await _context.Patients.ToListAsync();
                if (patients.Any (x => x.Name == patient.Name && x.Address == patient.Address && x.Birthday == patient.Birthday && x.Gender == patient.Gender))
                {
                    this.ModelState.AddModelError("", "User with same data already exists");
                    return this.View(patient);
                }
                if (patient.Birthday > DateTime.Now)
                {
                    this.ModelState.AddModelError("Birthday", "Set real birthday");
                    return this.View(patient);
                }
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.SingleOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Birthday,Gender")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var patients = await _context.Patients.ToListAsync();
                if (patients.Any(x => x.Name == patient.Name && x.Address == patient.Address && x.Birthday == patient.Birthday && x.Gender == patient.Gender &&  x.Id != patient.Id))
                {
                    this.ModelState.AddModelError("", "User with same data already exists");
                    return this.View(patient);
                }
                if (patient.Birthday > DateTime.Now)
                {
                    this.ModelState.AddModelError("Birthday", "Set real birthday");
                    return this.View(patient);
                }

                /*                    _context.Update(patient);*/
                var pat = await this._context.Patients.SingleOrDefaultAsync(m => m.Id == patient.Id);

                pat.Name = patient.Name;
                pat.Address = patient.Address;
                pat.Birthday = patient.Birthday;
                pat.Gender = patient.Gender;
                await this._context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .SingleOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.SingleOrDefaultAsync(m => m.Id == id);
            var placements = await this._context.Placements.Where(x => x.PatientId == patient.Id).ToListAsync();
            if (placements != null)
            {
                foreach (var placement in placements)
                {
                    placement.PatientId = null;
                }
            }
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}

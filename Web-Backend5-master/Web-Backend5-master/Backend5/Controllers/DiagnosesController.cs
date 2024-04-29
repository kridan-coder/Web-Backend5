using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend5.Data;
using Backend5.Models;
using Backend5.Models.ViewModels;

namespace Backend5.Controllers
{
    public class DiagnosesController : Controller
    {
        private readonly ApplicationDbContext context;

        public DiagnosesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> ShowWholeList()
        {
            return View(await context.Diagnoses
                .Include(w => w.Patient)
                .ToListAsync());
        }


        public async Task<IActionResult> Index(Int32? patientId)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this.context.Patients
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Patient = patient;
            var diagnosiss = await this.context.Diagnoses
                .Include(w => w.Patient)
                .Where(x => x.PatientId == patientId)
                .ToListAsync();

            return this.View(diagnosiss);
        }

        // GET: Diagnoses/Details/5
        public async Task<IActionResult> Details(Int32? diagnosisId, Int32? patientId)
        {
            if (diagnosisId == null || patientId == null)
            {
                return this.NotFound();
            }

            var diagnosis = await this.context.Diagnoses
                .Include(w => w.Patient)
                .SingleOrDefaultAsync(m => m.Id == diagnosisId && m.PatientId == patientId);
            if (diagnosis == null)
            {
                return this.NotFound();
            }

            return this.View(diagnosis);
        }

        // GET: Diagnoses/Create
        public async Task<IActionResult> Create(Int32? patientId)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this.context.Patients
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }



            this.ViewBag.Patient = patient;
            return this.View(new DiagnosisCreateModel() { PatientId = (Int32)patientId });
        }

        // POST: Diagnoses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? patientId, DiagnosisCreateModel model)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this.context.Patients
                .Include(x => x.Diagnoses)
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var diagnosisId = patient.Diagnoses.Any() ? patient.Diagnoses.Max(x => x.Id) + 1 : 1;
                var diagnosis = new Diagnosis
                {
                    Id = diagnosisId,
                    PatientId = patient.Id,
                    Type = model.Type,
                    Complications = model.Complications,
                    Details = model.Details
                };


                this.context.Add(diagnosis);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { patientId = patient.Id });
            }

            this.ViewBag.Patient = patient;
            return this.View(model);



        }

        // GET: Diagnoses/Edit/5
        public async Task<IActionResult> Edit(Int32? diagnosisId, Int32? patientId)
        {
            if (diagnosisId == null || patientId == null)
            {
                return this.NotFound();
            }



            var diagnosis = await this.context.Diagnoses.SingleOrDefaultAsync(m => m.Id == diagnosisId && m.PatientId == patientId);
            if (diagnosis == null)
            {
                return this.NotFound();
            }

            var model = new DiagnosisEditModel
            {
                Type = diagnosis.Type,
                Complications = diagnosis.Complications,
                Details = diagnosis.Details,
                PatientId = diagnosis.PatientId,
                Id = diagnosis.Id
            };

            return this.View(model);
        }

        // POST: Diagnoses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? diagnosisId, Int32? patientId, DiagnosisEditModel model)
        {
            if (diagnosisId == null || patientId == null)
            {
                return this.NotFound();
            }

            var diagnosis = await this.context.Diagnoses.SingleOrDefaultAsync(m => m.Id == diagnosisId && m.PatientId == patientId);
            if (diagnosis == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                diagnosis.Type = model.Type;
                diagnosis.Complications = model.Complications;
                diagnosis.Details = model.Details;
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { patientId });
            }

            return this.View(model);
        }

        // GET: Diagnoses/Delete/5
        public async Task<IActionResult> Delete(Int32? diagnosisId, Int32? patientId)
        {
            if (diagnosisId == null || patientId == null)
            {
                return this.NotFound();
            }

            var diagnosis = await this.context.Diagnoses
                .Include(w => w.Patient)
                .SingleOrDefaultAsync(m => m.Id == diagnosisId && m.PatientId == patientId);
            if (diagnosis == null)
            {
                return this.NotFound();
            }

            return this.View(diagnosis);
        }

        // POST: Diagnoses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32? diagnosisId, Int32? patientId)
        {
            var diagnosis = await this.context.Diagnoses.SingleOrDefaultAsync(m => m.Id == diagnosisId && m.PatientId == patientId);
            this.context.Diagnoses.Remove(diagnosis);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { patientId = diagnosis.PatientId });
        }
    }
}

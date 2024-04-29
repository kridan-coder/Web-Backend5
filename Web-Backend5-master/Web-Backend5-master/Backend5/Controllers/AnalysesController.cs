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
    public class AnalysesController : Controller
    {
        private readonly ApplicationDbContext context;

        public AnalysesController(ApplicationDbContext context)
        {
            this.context = context;
        }


        public async Task<IActionResult> ShowWholeList()
        {
            return View(await context.Analyses
                .Include(w => w.Lab)
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
            var analyses = await this.context.Analyses
                .Include(w => w.Lab)
                .Include(w => w.Patient)
                .Where(x => x.PatientId == patientId)
                .ToListAsync();

            return this.View(analyses);
        }

        // GET: Analyses/Details/5
        public async Task<IActionResult> Details(Int32? analysisId, Int32? patientId)
        {
            if (analysisId == null || patientId == null)
            {
                return this.NotFound();
            }

            var analysis = await this.context.Analyses
                .Include(w => w.Patient)
                .Include(w => w.Lab)
                .SingleOrDefaultAsync(m => m.Id == analysisId && m.PatientId == patientId);
            if (analysis == null)
            {
                return this.NotFound();
            }

            return this.View(analysis);
        }

        // GET: Analyses/Create
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
            return this.View(new AnalysisCreateModel() { PatientId = (Int32)patientId });
        }

        // POST: Analyses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? patientId, AnalysisCreateModel model)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this.context.Patients
                .Include(x => x.Analyses)
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var analysisId = patient.Analyses.Any() ? patient.Analyses.Max(x => x.Id) + 1 : 1;
                var analysis = new Analysis
                {
                    Id = analysisId,
                    PatientId = patient.Id,
                    Type = model.Type,
                    Date = model.Date,
                    Status = model.Type
                };

                if (model.LabId != 0)
                {
                    analysis.LabId = model.LabId;
                }

                this.context.Add(analysis);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { patientId = patient.Id });
            }

            this.ViewBag.Patient = patient;
            return this.View(model);



        }

        // GET: Analyses/Edit/5
        public async Task<IActionResult> Edit(Int32? analysisId, Int32? patientId)
        {
            if (analysisId == null || patientId == null)
            {
                return this.NotFound();
            }



            var analysis = await this.context.Analyses.SingleOrDefaultAsync(m => m.Id == analysisId && m.PatientId == patientId);
            if (analysis == null)
            {
                return this.NotFound();
            }

            var model = new AnalysisEditModel
            {
                Type = analysis.Type,
                Date = analysis.Date,
                Status = analysis.Type,
                Id = analysis.Id,
                PatientId = analysis.PatientId
            };
            this.ViewData["LabId"] = new SelectList(this.context.Labs, "Id", "Name");

            return this.View(model);
        }

        // POST: Analyses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? analysisId, Int32? patientId, AnalysisEditModel model)
        {
            if (analysisId == null || patientId == null)
            {
                return this.NotFound();
            }

            var analysis = await this.context.Analyses.SingleOrDefaultAsync(m => m.Id == analysisId && m.PatientId == patientId);
            if (analysis == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                analysis.Type = model.Type;
                analysis.Date = model.Date;
                analysis.Status = model.Status;
                analysis.LabId = model.LabId;
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { patientId });
            }

            return this.View(model);
        }

        // GET: Analyses/Delete/5
        public async Task<IActionResult> Delete(Int32? analysisId, Int32? patientId)
        {
            if (analysisId == null || patientId == null)
            {
                return this.NotFound();
            }

            var analysis = await this.context.Analyses
                .Include(w => w.Patient)
                .Include(w => w.Lab)
                .SingleOrDefaultAsync(m => m.Id == analysisId && m.PatientId == patientId);
            if (analysis == null)
            {
                return this.NotFound();
            }

            return this.View(analysis);
        }

        // POST: Analyses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32? analysisId, Int32? patientId)
        {
            var analysis = await this.context.Analyses.SingleOrDefaultAsync(m => m.Id == analysisId && m.PatientId == patientId);
            this.context.Analyses.Remove(analysis);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { patientId = analysis.PatientId });
        }
    }
    }

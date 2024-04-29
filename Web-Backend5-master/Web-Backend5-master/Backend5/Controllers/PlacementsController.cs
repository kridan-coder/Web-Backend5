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
    public class PlacementsController : Controller
    {
        private readonly ApplicationDbContext context;

        public PlacementsController(ApplicationDbContext context)
        {
            this.context = context;
        }


        public async Task<IActionResult> ShowWholeList()
        {
            return View(await context.Placements
                .Include(w => w.Ward)
                .Include(w => w.Patient)
                .Include(w => w.Ward.Hospital)
                .ToListAsync());
        }


        public async Task<IActionResult> Index(Int32? wardId)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this.context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Ward = ward;
            var placements = await this.context.Placements
                .Include(w => w.Patient)
                .Include(w => w.Ward)
                .Include(w => w.Ward.Hospital)
                .Where(x => x.WardId == wardId)
                .ToListAsync();

            return this.View(placements);
        }

        // GET: Placements/Details/5
        public async Task<IActionResult> Details(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var placement = await this.context.Placements
                .Include(w => w.Ward)
                .Include(w => w.Patient)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return this.NotFound();
            }

            return this.View(placement);
        }

        // GET: Placements/Create
        public async Task<IActionResult> Create(Int32? wardId)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this.context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }



            this.ViewBag.Ward = ward;
            return this.View(new PlacementCreateModel() { WardId = (Int32)wardId });
        }

        // POST: Placements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? wardId, PlacementCreateModel model)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }


            var ward = await this.context.Wards
                .Include(x => x.Placements)
                .SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }
            this.ViewBag.Ward = ward;

            if (model.Bed < 0)
            {
                this.ModelState.AddModelError("Bed", "Value cannot be negative");
                return this.View(model);
            }

            if (ward.Placements.Any(x => x.Bed == model.Bed))
            {
                this.ModelState.AddModelError("Bed", "This bed already exists");
                return this.View(model);
            }


            if (this.ModelState.IsValid)
            {
                var placement = new Placement
                {
                    WardId = ward.Id,
                    Bed = model.Bed
                };

                this.context.Add(placement);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { wardId = ward.Id });
            }

            this.ViewBag.Ward = ward;
            return this.View(model);



        }

        // GET: Placements/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var placement = await this.context.Placements.SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return this.NotFound();
            }
            this.ViewData["PatientId"] = new SelectList(this.context.Patients, "Id", "Name");
            var model = new PlacementEditModel
            {
                Bed = placement.Bed,
                PatientId = placement.PatientId,
                WardId = (Int32)placement.WardId
            };

            return this.View(model);
        }

        // POST: Placements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, PlacementEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var placement = await this.context.Placements
                .SingleOrDefaultAsync(m => m.Id == id);
            model.WardId = (Int32)placement.WardId;
            var ward = await this.context.Wards
    .Include(x => x.Placements)
    .SingleOrDefaultAsync(x => x.Id == placement.WardId);
            if (placement == null)
            {
                return this.NotFound();
            }
            this.ViewData["PatientId"] = new SelectList(this.context.Patients, "Id", "Name");
            if (model.Bed < 0)
            {
                this.ModelState.AddModelError("Bed", "Value cannot be negative");
                return this.View(model);
            }

            if (ward.Placements.Any(x => x.Bed == model.Bed && x.Id != id))
            {
                this.ModelState.AddModelError("Bed", "This bed already exists");
                return this.View(model);
            }

            if (this.ModelState.IsValid)
            {
                placement.Bed = model.Bed;
                placement.PatientId = model.PatientId;
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { placement.WardId });
            }

            return this.View(model);
        }

        // GET: Placements/Delete/5
        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var placement = await this.context.Placements
                .Include(w => w.Ward)
                .Include(w => w.Patient)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return this.NotFound();
            }

            return this.View(placement);
        }

        // POST: Placements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32? id)
        {
            var placement = await this.context.Placements.SingleOrDefaultAsync(m => m.Id == id);
            this.context.Placements.Remove(placement);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { wardId = placement.WardId });
        }
    }
}

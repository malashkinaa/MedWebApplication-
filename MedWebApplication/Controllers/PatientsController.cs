using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MedWebApplication;

namespace MedWebApplication.Controllers
{
    public class PatientsController : Controller
    {
        private readonly DbmedContext _context;

        public PatientsController(DbmedContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index()
        {
            var dbmedContext = _context.Patients.Include(p => p.BloodGroup).Include(p => p.Gender);
            return View(await dbmedContext.ToListAsync());
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.BloodGroup)
                .Include(p => p.Gender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            ViewData["BloodGroupId"] = new SelectList(_context.BloodGroups, "Id", "Name");
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,GenderId,BirthDate,BloodGroupId,Address,PhoneNumber,Email,AnyMajorDiseaseSufferedEarlier")] Patient patient)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["BloodGroupId"] = new SelectList(_context.BloodGroups, "Id", "Name", patient.BloodGroupId);
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", patient.GenderId);
            return View(patient);
        }

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Import(IFormFile fileExcel)
		//{

		//}

		// GET: Patients/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            ViewData["BloodGroupId"] = new SelectList(_context.BloodGroups, "Id", "Name", patient.BloodGroupId);
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", patient.GenderId);
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,GenderId,BirthDate,BloodGroupId,Address,PhoneNumber,Email,AnyMajorDiseaseSufferedEarlier")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            ViewData["BloodGroupId"] = new SelectList(_context.BloodGroups, "Id", "Name", patient.BloodGroupId);
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "Name", patient.GenderId);
            return View(patient);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Patients == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .Include(p => p.BloodGroup)
                .Include(p => p.Gender)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.Patients == null)
            {
                return Problem("Entity set 'DbmedContext.Patients'  is null.");
            }
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(int id)
        {
          return (_context.Patients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

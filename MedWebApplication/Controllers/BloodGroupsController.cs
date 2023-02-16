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
    public class BloodGroupsController : Controller
    {
        private readonly DbmedContext _context;

        public BloodGroupsController(DbmedContext context)
        {
            _context = context;
        }

        // GET: BloodGroups
        public async Task<IActionResult> Index()
        {
              return _context.BloodGroups != null ? 
                          View(await _context.BloodGroups.ToListAsync()) :
                          Problem("Entity set 'DbmedContext.BloodGroups'  is null.");
        }

        // GET: BloodGroups/Details/5
        public async Task<IActionResult> Details(byte? id)
        {
            if (id == null || _context.BloodGroups == null)
            {
                return NotFound();
            }

            var bloodGroup = await _context.BloodGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bloodGroup == null)
            {
                return NotFound();
            }

            return View(bloodGroup);
        }

        // GET: BloodGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BloodGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] BloodGroup bloodGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bloodGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bloodGroup);
        }

        // GET: BloodGroups/Edit/5
        public async Task<IActionResult> Edit(byte? id)
        {
            if (id == null || _context.BloodGroups == null)
            {
                return NotFound();
            }

            var bloodGroup = await _context.BloodGroups.FindAsync(id);
            if (bloodGroup == null)
            {
                return NotFound();
            }
            return View(bloodGroup);
        }

        // POST: BloodGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(byte id, [Bind("Id,Name")] BloodGroup bloodGroup)
        {
            if (id != bloodGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bloodGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BloodGroupExists(bloodGroup.Id))
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
            return View(bloodGroup);
        }

        // GET: BloodGroups/Delete/5
        public async Task<IActionResult> Delete(byte? id)
        {
            if (id == null || _context.BloodGroups == null)
            {
                return NotFound();
            }

            var bloodGroup = await _context.BloodGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bloodGroup == null)
            {
                return NotFound();
            }

            return View(bloodGroup);
        }

        // POST: BloodGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(byte id)
        {
            if (_context.BloodGroups == null)
            {
                return Problem("Entity set 'DbmedContext.BloodGroups'  is null.");
            }
            var bloodGroup = await _context.BloodGroups.FindAsync(id);
            if (bloodGroup != null)
            {
                _context.BloodGroups.Remove(bloodGroup);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BloodGroupExists(byte id)
        {
          return (_context.BloodGroups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

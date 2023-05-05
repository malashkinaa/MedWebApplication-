using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MedWebApplication;
using ClosedXML.Excel;
using System.IO;
using System.Reflection.Metadata.Ecma335;

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

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Import(IFormFile fileExcel)
		{
			if (fileExcel != null)
			{
				using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
				{
					await fileExcel.CopyToAsync(stream);
					using (var workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
					{
						//перегляд усіх листів (в даному випадку категорій)
						foreach (var worksheet in workBook.Worksheets)
						{
							foreach (var row in worksheet.RowsUsed().Skip(1))
							{

								var patientName = row.Cell(1).Value.ToString();
								var birthDate = DateTime.Parse(row.Cell(2).Value.ToString());
								var adress = row.Cell(3).Value.ToString();
								var phoneNumber = int.Parse(row.Cell(4).Value.ToString());
								var email = row.Cell(5).Value.ToString();
								var anyMajorDiseaseSufferedEarlier = row.Cell(6).Value.ToString();
								var bloodGroup = row.Cell(7).Value.ToString();
								var genderName = row.Cell(8).Value.ToString();

								var patient = new Patient { };
								patient.Name = patientName;
								patient.BirthDate = birthDate;
								patient.Address = adress;
								patient.PhoneNumber = phoneNumber;
								patient.Email = email;
								patient.AnyMajorDiseaseSufferedEarlier = anyMajorDiseaseSufferedEarlier;
								


								int genderId = -1;
								foreach (var g in _context.Genders)
								{
									if (g.Name == genderName) 
									{
										genderId = g.Id; 
										break;
									}	
								}
								//int genderId = (from gender in _context.Genders where gender.Name == genderName select gender).FirstOrDefault().Id;
								//_context.Genders()\							    
								patient.GenderId = genderId;


								byte bloodGroupId = 0;
								foreach (var b in _context.BloodGroups)
								{
									if (b.Name.Trim() == bloodGroup)
									{ 
										bloodGroupId = b.Id;
										break;	
									}
								}
								patient.BloodGroupId = bloodGroupId;

								_context.Add(patient);
								await _context.SaveChangesAsync();



							}
							//							//worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
							//							Patient newcat;
							//							var c = (from cat in _context.Patients)
							//									 where cat.Name.Contains(worksheet.Name)
							//									 select cat).ToList();
							//							if (c.Count & gt; 0)
							//{
							//								newcat = c[0];
							//							}
							//else
							//							{
							//								newcat = new Category();
							//								newcat.Name = worksheet.Name;
							//								newcat.Info = &quot; from EXCEL&quot; ;
							//								//додати в контекст
							//								_context.Categories.Add(newcat);
							//							}
						}
					}
				}
			}
			return RedirectToAction(nameof(Index));
		}

		public ActionResult Export()
		{
			using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
			{
				var worksheet = workbook.Worksheets.Add("Пацієнти");

				worksheet.Cell(1, 1).Value = "Ім'я пацієнта";
				worksheet.Cell(1, 2).Value = "Дата народження";
				worksheet.Cell(1, 3).Value = "Адреса";
				worksheet.Cell(1, 4).Value = "Номер телефону";
				worksheet.Cell(1, 5).Value = "Пошта";
				worksheet.Cell(1, 6).Value = "Попередні хвороби";
				worksheet.Cell(1, 7).Value = "Група крові";
				worksheet.Cell(1, 8).Value = "Стать";
				worksheet.Row(1).Style.Font.Bold = true;

				var bloodGroups = _context.BloodGroups.ToList();
				var genders = _context.Genders.ToList();
				int i = 2; //skip header
				foreach (var p in _context.Patients)
				{ 
					worksheet.Cell(i, 1).Value = p.Name;
					worksheet.Cell(i, 2).Value = p.BirthDate;
					worksheet.Cell(i, 3).Value = p.Address;
					worksheet.Cell(i, 4).Value = p.PhoneNumber;
					worksheet.Cell(i, 5).Value = p.Email;
					worksheet.Cell(i, 6).Value = p.AnyMajorDiseaseSufferedEarlier;
					foreach (var b in bloodGroups)
					{
						if (b.Id == p.BloodGroupId)
						{
							worksheet.Cell(i, 7).Value = b.Name;
							break;
						}
								
					}
					foreach (var g in genders)
					{
						if (g.Id == p.GenderId)
						{
							worksheet.Cell(i, 8).Value = g.Name;
							break;
						}
					
					}
			
					i++;
				}
				


				using (var stream = new MemoryStream())
				{ 
					workbook.SaveAs(stream);
					stream.Flush();

					return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
					{
						FileDownloadName = $"patients_{DateTime.UtcNow.ToShortDateString()}.xlsx"
					};
				}

			}

			


		}



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

using ClosedXML.Excel;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Mvc;

namespace MedWebApplication.ExcelIntegration
{
	public class ExcelExport
	{
		DbmedContext _context;
		public ExcelExport(DbmedContext _context)
		{
			this._context = _context;
		}

		public ActionResult ProcessFile()
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
					ExportPatient(worksheet, bloodGroups, genders, i, p);

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

		private static void ExportPatient(IXLWorksheet worksheet, List<BloodGroup> bloodGroups, List<Gender> genders, int i, Patient p)
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
		}
	}
}

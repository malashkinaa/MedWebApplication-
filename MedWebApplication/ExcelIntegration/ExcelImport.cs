using ClosedXML.Excel;
using MedWebApplication;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MedWebApplication.ExcelIntegration
{
	public class ExcelImport
	{
		private IFormFile fileExcel;
		private List<BloodGroup> bloodGroups;
		private List<Gender> genders;

		public ExcelImport(IFormFile fileExcel, List<BloodGroup> bloodGroups, List<Gender> genders)
		{
			this.fileExcel = fileExcel;
			this.bloodGroups = bloodGroups;
			this.genders = genders;
		}

		public List<Patient> ProcessFile()
		{
			var list = new List<Patient>();
			using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
			{
				fileExcel.CopyTo(stream);
				using (var workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
				{
					//перегляд усіх листів (в даному випадку категорій)
					foreach (var worksheet in workBook.Worksheets)
					{
						foreach (var row in worksheet.RowsUsed().Skip(1))
						{
							var patient = ImportPatient(row);
							list.Add(patient);
						}
					}
				}
			}
			return list;
		}


		private Patient ImportPatient(IXLRow? row)
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
			foreach (var g in genders)
			{
				if (g.Name == genderName)
				{
					genderId = g.Id;
					break;
				}
			}					    
			patient.GenderId = genderId;


			byte bloodGroupId = 0;
			foreach (var b in bloodGroups)
			{
				if (b.Name.Trim() == bloodGroup)
				{
					bloodGroupId = b.Id;
					break;
				}
			}
			patient.BloodGroupId = bloodGroupId;
			return patient;
		}	
	}
}

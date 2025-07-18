using AccountManagementSystem.Models;
using AccountManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace AccountManagementSystem.Pages.ChartOfAccounts
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public List<AccountView> ChartOfAccounts { get; set; } = new();

        public IndexModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void OnGet()
        {
            ChartOfAccounts = context.ChartOfAccount.OrderByDescending(i => i.Id).ToList();
        }

        public IActionResult OnGetExportToExcel()
        {
            var accounts = context.ChartOfAccount.ToList(); // use correct source

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Chart of Accounts");

            // Headers
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Email";
            worksheet.Cells[1, 4].Value = "Phone";
            worksheet.Cells[1, 5].Value = "Parent ID";
            worksheet.Cells[1, 6].Value = "Status";
            worksheet.Cells[1, 7].Value = "Created On";
            worksheet.Cells[1, 8].Value = "Created By";

            using (var range = worksheet.Cells[1, 1, 1, 8])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            int row = 2;
            foreach (var acc in accounts)
            {
                worksheet.Cells[row, 1].Value = acc.Id;
                worksheet.Cells[row, 2].Value = acc.Name;
                worksheet.Cells[row, 3].Value = acc.Email;
                worksheet.Cells[row, 4].Value = acc.PhoneNumber;
                worksheet.Cells[row, 5].Value = acc.ParentId?.ToString() ?? "—";
                worksheet.Cells[row, 6].Value = acc.IsActive ? "Active" : "Inactive";
                worksheet.Cells[row, 7].Value = acc.CreatedAt.ToString("dd MMM yyyy");
                worksheet.Cells[row, 8].Value = acc.CreatedBy;

                row++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            var stream = new MemoryStream(package.GetAsByteArray());
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ChartOfAccounts.xlsx");
        }
    }
}

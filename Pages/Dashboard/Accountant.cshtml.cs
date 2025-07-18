using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace AccountManagementSystem.Pages.Dashboard
{
    public class AccountantModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountantModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public List<IdentityUser> Accountants { get; set; } = new();

        public async Task OnGetAsync()
        {
            var users = _userManager.Users.ToList(); // Avoids multiple readers
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Accountant"))
                {
                    Accountants.Add(user);
                }
            }
        }

        public async Task<IActionResult> OnGetExportToExcel()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var accountants = new List<IdentityUser>();

            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, "Accountant"))
                {
                    accountants.Add(user);
                }
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Accountants");

            worksheet.Cells[1, 1].Value = "Email";
            worksheet.Cells[1, 2].Value = "Username";

            using (var headerRange = worksheet.Cells[1, 1, 1, 2])
            {
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            int row = 2;
            foreach (var acc in accountants)
            {
                worksheet.Cells[row, 1].Value = acc.Email;
                worksheet.Cells[row, 2].Value = acc.UserName;
                row++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            var stream = new MemoryStream(package.GetAsByteArray());
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Accountants.xlsx");
        }



        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToPage();
        }
    }
}

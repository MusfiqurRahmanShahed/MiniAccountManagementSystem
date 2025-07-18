using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace AccountManagementSystem.Pages.Dashboard.ViewerManagement
{
    public class ViewerIndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ViewerIndexModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public List<IdentityUser> Viewers { get; set; } = new();

        public async Task OnGetAsync()
        {
            var allUsers = await _userManager.Users.ToListAsync(); // Ensure DataReader is closed
            Viewers = new();

            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, "Viewer"))
                {
                    Viewers.Add(user);
                }
            }
        }

        public async Task<IActionResult> OnGetExportToExcel()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var viewers = new List<IdentityUser>();

            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, "Viewer"))
                {
                    viewers.Add(user);
                }
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Viewers");

            worksheet.Cells[1, 1].Value = "Email";
            worksheet.Cells[1, 2].Value = "Username";

            using (var header = worksheet.Cells[1, 1, 1, 2])
            {
                header.Style.Font.Bold = true;
                header.Style.Fill.PatternType = ExcelFillStyle.Solid;
                header.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            int row = 2;
            foreach (var viewer in viewers)
            {
                worksheet.Cells[row, 1].Value = viewer.Email;
                worksheet.Cells[row, 2].Value = viewer.UserName;
                row++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            var stream = new MemoryStream(package.GetAsByteArray());
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Viewers.xlsx");
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

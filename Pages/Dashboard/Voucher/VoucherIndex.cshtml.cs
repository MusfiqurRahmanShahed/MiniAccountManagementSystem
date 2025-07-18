using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System.ComponentModel;
using System.Data;

namespace AccountManagementSystem.Pages.Dashboard.Voucher
{
    public class VoucherIndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public VoucherIndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<VoucherViewModel> Vouchers { get; set; } = new();

        public void OnGet()
        {
            LoadVouchers();
        }

        private void LoadVouchers()
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new SqlConnection(connStr);
            conn.Open();

            using var cmd = new SqlCommand("SELECT * FROM Vouchers", conn);
            using var reader = cmd.ExecuteReader();

            Vouchers.Clear();
            while (reader.Read())
            {
                Vouchers.Add(new VoucherViewModel
                {
                    Id = (int)reader["Id"],
                    Date = (DateTime)reader["Date"],
                    ReferenceNo = reader["ReferenceNo"].ToString() ?? ""
                });
            }
        }

        public async Task<IActionResult> OnPostExportAsync()
        {
            LoadVouchers();
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Vouchers");

            // Header
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Date";
            worksheet.Cells[1, 3].Value = "Reference No";

            // Data
            int row = 2;
            foreach (var v in Vouchers)
            {
                worksheet.Cells[row, 1].Value = v.Id;
                worksheet.Cells[row, 2].Value = v.Date.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 3].Value = v.ReferenceNo;
                row++;
            }

            worksheet.Cells.AutoFitColumns();

            var stream = new MemoryStream();
            await package.SaveAsAsync(stream);
            stream.Position = 0;

            var fileName = $"Vouchers_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

            return File(stream,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }

        public class VoucherViewModel
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public string ReferenceNo { get; set; } = "";
        }
    }
}

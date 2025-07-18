using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AccountManagementSystem.Models;
using System.Data;
using System.Data.SqlClient;

namespace AccountManagementSystem.Pages.Dashboard.VoucherManagement
{
    public class AddVoucherModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public AddVoucherModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public VoucherEntry Voucher { get; set; } = new();

        public IActionResult OnGet()
        {
            // Add a default empty line
            Voucher.Lines.Add(new VoucherLine());
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection");

            using var conn = new SqlConnection(connStr);
            using var cmd = new SqlCommand("sp_SaveVoucher", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Date", Voucher.Date);
            cmd.Parameters.AddWithValue("@ReferenceNo", Voucher.ReferenceNo);

            var table = new DataTable();
            table.Columns.Add("AccountId", typeof(int));
            table.Columns.Add("Debit", typeof(decimal));
            table.Columns.Add("Credit", typeof(decimal));

            foreach (var line in Voucher.Lines)
            {
                table.Rows.Add(line.AccountId, line.Debit, line.Credit);
            }

            var param = cmd.Parameters.AddWithValue("@VoucherLines", table);
            param.SqlDbType = SqlDbType.Structured;
            param.TypeName = "dbo.VoucherLineType";

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            return RedirectToPage("/Dashboard/Voucher/VoucherIndex");
        }
    }
}

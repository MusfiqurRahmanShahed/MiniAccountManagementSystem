using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AccountManagementSystem.Pages.Dashboard.Voucher
{
    public class VoucherDetailsModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public VoucherDetailsModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public VoucherHeader Header { get; set; } = new();
        public List<VoucherLine> Lines { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection");

            using var conn = new SqlConnection(connStr);
            await conn.OpenAsync();

            // Get voucher header
            using (var cmd = new SqlCommand("SELECT * FROM Vouchers WHERE Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using var reader = await cmd.ExecuteReaderAsync();

                if (!reader.HasRows)
                    return NotFound();

                await reader.ReadAsync();

                Header = new VoucherHeader
                {
                    Id = (int)reader["Id"],
                    Date = (DateTime)reader["Date"],
                    ReferenceNo = reader["ReferenceNo"].ToString() ?? ""
                };
            }

            // Get voucher details
            using (var cmd = new SqlCommand("SELECT * FROM VoucherDetails WHERE VoucherId = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Lines.Add(new VoucherLine
                    {
                        AccountId = (int)reader["AccountId"],
                        Debit = (decimal)reader["Debit"],
                        Credit = (decimal)reader["Credit"]
                    });
                }
            }

            return Page();
        }

        public class VoucherHeader
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public string ReferenceNo { get; set; } = "";
        }

        public class VoucherLine
        {
            public int AccountId { get; set; }
            public decimal Debit { get; set; }
            public decimal Credit { get; set; }
        }
    }
}

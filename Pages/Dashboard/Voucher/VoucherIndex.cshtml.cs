using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new SqlConnection(connStr);
            conn.Open();

            using var cmd = new SqlCommand("SELECT * FROM Vouchers", conn);
            using var reader = cmd.ExecuteReader();

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

        public class VoucherViewModel
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public string ReferenceNo { get; set; } = "";
        }
    }
}

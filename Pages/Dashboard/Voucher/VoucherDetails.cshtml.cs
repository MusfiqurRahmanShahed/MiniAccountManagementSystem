using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Runtime.Loader;

namespace AccountManagementSystem.Pages.Dashboard.Voucher
{
    public class VoucherDetailsModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IConverter _pdfConverter;

        public VoucherDetailsModel(IConfiguration configuration, IConverter pdfConverter)
        {
            _configuration = configuration;
            _pdfConverter = pdfConverter;
        }

        public VoucherHeader Header { get; set; } = new();
        public List<VoucherLine> Lines { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await LoadVoucherAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPostDownloadPdfAsync(int id)
        {
            await LoadVoucherAsync(id);

            string html = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; }}
                        table {{ width: 100%; border-collapse: collapse; margin-top: 20px; }}
                        th, td {{ border: 1px solid #ddd; padding: 8px; text-align: center; }}
                        th {{ background-color: #f2f2f2; }}
                    </style>
                </head>
                <body>
                    <h2>Voucher Details</h2>
                    <p><strong>ID:</strong> {Header.Id}</p>
                    <p><strong>Date:</strong> {Header.Date:yyyy-MM-dd}</p>
                    <p><strong>Reference No:</strong> {Header.ReferenceNo}</p>
                    
                    <h4>Voucher Lines</h4>
                    <table>
                        <thead>
                            <tr>
                                <th>Account ID</th>
                                <th>Debit</th>
                                <th>Credit</th>
                            </tr>
                        </thead>
                        <tbody>
                            {string.Join("", Lines.Select(line => $@"
                                <tr>
                                    <td>{line.AccountId}</td>
                                    <td>{line.Debit}</td>
                                    <td>{line.Credit}</td>
                                </tr>"))}
                        </tbody>
                    </table>
                </body>
                </html>";

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                    DocumentTitle = $"Voucher_{Header.Id}",
                },
                Objects = {
                    new ObjectSettings {
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            try
            {
                byte[] pdfBytes = _pdfConverter.Convert(doc);
                return File(pdfBytes, "application/pdf", $"Voucher_{Header.Id}.pdf");
            }
            catch (Exception ex)
            {
                var errorMessage = ex is AggregateException aggr
                    ? aggr.InnerException?.Message ?? aggr.Message
                    : ex.Message;

                return BadRequest("PDF generation failed: " + errorMessage);
            }
        }

        private async Task LoadVoucherAsync(int id)
        {
            Lines.Clear();
            string connStr = _configuration.GetConnectionString("DefaultConnection");

            using var conn = new SqlConnection(connStr);
            await conn.OpenAsync();

            // Load header
            using (var cmd = new SqlCommand("SELECT * FROM Vouchers WHERE Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using var reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    Header = new VoucherHeader
                    {
                        Id = (int)reader["Id"],
                        Date = (DateTime)reader["Date"],
                        ReferenceNo = reader["ReferenceNo"].ToString() ?? ""
                    };
                }
            }

            // Load lines
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

    // Load native libwkhtmltox.dll
    public class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDllFromPath(absolutePath);
        }
    }
}

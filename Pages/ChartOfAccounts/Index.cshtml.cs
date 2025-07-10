using AccountManagementSystem.Models;
using AccountManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccountManagementSystem.Pages.ChartOfAccounts
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public List<AccountView> ChartOfAccounts = new();

        public IndexModel(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void OnGet()
        {
            ChartOfAccounts = context.ChartOfAccount.OrderByDescending(i=>i.Id).ToList();
        }
    }
}

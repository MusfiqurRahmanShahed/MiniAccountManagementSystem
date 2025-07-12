using AccountManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccountManagementSystem.Pages.ChartOfAccounts
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext context;
        public DeleteModel(ApplicationDbContext context)
        {
            this.context = context;
        }


        public IActionResult OnGet(int id)
        {
            var account = context.ChartOfAccount.Find(id);
            if (account != null)
            {
                context.ChartOfAccount.Remove(account);
                context.SaveChanges();
            }

            return RedirectToPage("/ChartOfAccounts/Index");
        }
    }
}

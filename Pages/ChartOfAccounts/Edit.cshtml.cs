using AccountManagementSystem.Models;
using AccountManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccountManagementSystem.Pages.ChartOfAccounts
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public accountDto accountDto { get; set; } = new accountDto();

        public AccountView AccountView { get; set; } = new();

        private readonly ApplicationDbContext context;
        public EditModel(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult OnGet(int id)
        {
            var account = context.ChartOfAccount.Find(id);
            if (account == null)
            {
                return RedirectToPage("/ChartOfAccounts/Index");
            }
            AccountView = account;

            accountDto.Name = account.Name;
            accountDto.Email = account.Email;
            accountDto.PhoneNumber = account.PhoneNumber;
            accountDto.ParentId = account.ParentId;
            accountDto.IsActive = account.IsActive;
            accountDto.CreatedAt = account.CreatedAt;
            accountDto.CreatedBy = account.CreatedBy;
            return Page();
        }

        public string successMessage = "";

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                // Handle invalid model state
                return Page();
            }
            var account = context.ChartOfAccount.Find(id);
            if (account == null)
            {
                return RedirectToPage("/ChartOfAccounts/Index");
            }
            account.Name = accountDto.Name;
            account.Email = accountDto.Email;
            account.PhoneNumber = accountDto.PhoneNumber;
            account.ParentId = accountDto.ParentId;
            account.IsActive = accountDto.IsActive;
            account.CreatedAt = DateTime.Now; // Assuming you want to update the CreatedAt field
            account.CreatedBy = accountDto.CreatedBy;
            context.ChartOfAccount.Update(account);
            context.SaveChanges();
            successMessage = "Account updated successfully!";

            // Redirect to a confirmation page or the list of accounts
            return Page();
        }
    }

}

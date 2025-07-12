using AccountManagementSystem.Models;
using AccountManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccountManagementSystem.Pages.ChartOfAccounts
{
    public class CreateModel : PageModel
    {
        private ApplicationDbContext context;

        [BindProperty]
        public accountDto accountDto { get; set; } = new();

        public CreateModel(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost() { 
            if(!ModelState.IsValid)
            {
                // Handle invalid model state
                return Page();
            }

            var account = new AccountView
            {
                Name = accountDto.Name,
                Email = accountDto.Email,
                PhoneNumber = accountDto.PhoneNumber,
                ParentId = accountDto.ParentId,
                IsActive = accountDto.IsActive,
                CreatedAt = DateTime.Now,
                CreatedBy = accountDto.CreatedBy
            };
            context.ChartOfAccount.Add(account);
            context.SaveChanges();

            // Redirect to a confirmation page or the list of accounts
            return RedirectToPage("/ChartOfAccounts/Index");

        }
    }
}

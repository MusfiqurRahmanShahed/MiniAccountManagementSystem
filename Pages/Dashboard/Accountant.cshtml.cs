using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

namespace AccountManagementSystem.Pages.Dashboard
{
    public class AccountantModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountantModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public List<IdentityUser> Accountants { get; set; } = new();

        public async Task OnGetAsync()
        {
            var users = _userManager.Users.ToList(); // Avoids multiple readers
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Accountant"))
                {
                    Accountants.Add(user);
                }
            }
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

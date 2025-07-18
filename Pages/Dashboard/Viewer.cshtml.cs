using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountManagementSystem.Pages.Dashboard
{
    public class ViewerModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ViewerModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public List<IdentityUser> Viewers { get; set; } = new();

        public async Task OnGetAsync()
        {
            var users = _userManager.Users.ToList(); // Materialize to memory first
            Viewers = new List<IdentityUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Viewer"))
                {
                    Viewers.Add(user);
                }
            }
        }


        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && await _userManager.IsInRoleAsync(user, "Viewer"))
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToPage();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AccountManagementSystem.Pages.Dashboard.ViewerManagement
{
    public class ViewerIndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ViewerIndexModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public List<IdentityUser> Viewers { get; set; } = new();

        public async Task OnGetAsync()
        {
            var allUsers = await _userManager.Users.ToListAsync(); // Ensure DataReader is closed
            Viewers = new();

            foreach (var user in allUsers)
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
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToPage();
        }
    }
}

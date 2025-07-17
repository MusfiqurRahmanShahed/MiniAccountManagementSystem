using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AccountManagementSystem.Pages.Dashboard.ViewerManagement
{
    [Authorize(Roles = "Admin,Accountant")]
    public class EditViewerModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public EditViewerModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public ViewerEditModel Input { get; set; } = new();

        public string Id { get; set; } = "";

        public class ViewerEditModel
        {
            [Required]
            public string UserName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            Id = id;
            Input = new ViewerEditModel
            {
                UserName = user.UserName ?? "",
                Email = user.Email ?? ""
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            user.UserName = Input.UserName;
            user.Email = Input.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return RedirectToPage("ViewerIndex");

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return Page();
        }
    }
}

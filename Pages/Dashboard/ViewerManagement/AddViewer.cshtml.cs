using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace AccountManagementSystem.Pages.Dashboard.ViewerManagement
{
    [Authorize(Roles = "Admin,Accountant")]
    public class AddViewerModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AddViewerModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public ViewerInputModel Input { get; set; } = new();

        public class ViewerInputModel
        {
            [Required]
            public string UserName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var newUser = new IdentityUser
            {
                UserName = Input.UserName,
                Email = Input.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser, Input.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "Viewer");
                return RedirectToPage("ViewerIndex");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return Page();
        }
    }
}

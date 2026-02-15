using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MyChat.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public IndexModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }
        [BindProperty, Required(ErrorMessage = "Användarnamn krävs.")]
        public string UserName { get; set; }
        [BindProperty, Required(ErrorMessage = "Lösenord krävs.")]
        public string Password { get; set; }
        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Users/Messages");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {


            if (!ModelState.IsValid)
            {
                return Page();
            }
            var result = await _signInManager.PasswordSignInAsync(UserName, Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToPage("/Users/Messages");
            }
            ModelState.AddModelError(string.Empty, "Ogiltigt användarnamn eller lösenord.");
            return Page();
        }

    }
}

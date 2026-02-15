using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyChat.BLL.Services;
using System.ComponentModel.DataAnnotations;

namespace MyChat.UI.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMessageService _messageService;

        public DetailsModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IMessageService messageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _messageService = messageService;
        }

        [BindProperty]
        public string NewUserName { get; set; }
        [BindProperty]
        public string CurrentPassword { get; set; }
        [BindProperty, DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [BindProperty, DataType(DataType.Password), Compare("NewPassword", ErrorMessage = "Lösenorden matchar inte.")]
        public string ConfirmPassword { get; set; }



        public async Task OnGetAsync()
        {
            //var user = await _userManager.GetUserAsync(User);

        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove(nameof(CurrentPassword)); // Ta bort validering för CurrentPassword när vi byter användarnamn
            ModelState.Remove(nameof(NewPassword)); // Ta bort validering för NewPassword när vi byter användarnamn
            ModelState.Remove(nameof(ConfirmPassword));

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("Användaren kunde inte hittas.");
            }

            if (!string.IsNullOrWhiteSpace(NewUserName))
            {
                var existingUser = await _userManager.FindByNameAsync(NewUserName);

                if (existingUser != null && existingUser.Id != user.Id)
                {
                    ModelState.AddModelError(string.Empty, "Användarnamnet är redan taget.");
                    return Page();
                }

                var result = await _userManager.SetUserNameAsync(user, NewUserName);

                if (result.Succeeded)
                {
                    await _messageService.UpdateMessageAsync(NewUserName, user.Id);
                    await _signInManager.RefreshSignInAsync(user);
                    TempData["SuccessMessage"] = "Användarnamnet har uppdaterats.";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Något gick fel vid uppdatering av användarnamn.");
                    return Page();
                }
            }


            return Page();
        }

        public async Task<IActionResult> OnPostChangePasswordAsync()
        {
            ModelState.Remove(nameof(NewUserName)); // Ta bort validering för NewUserName när vi byter lösenord

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Användaren kunde inte hittas.");
            }
            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                ModelState.AddModelError(string.Empty, "Lösenord kan inte vara tomt.");
                return Page();
            }

            if (NewPassword != ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Lösenorden matchar inte.");
                return Page();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, CurrentPassword, NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            else
            {

                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Lösenordet har ändrats.";
            }


            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAccountAsync()
        {
            ModelState.Remove(nameof(NewUserName)); // Ta bort validering för NewUserName när vi raderar konto
            ModelState.Remove(nameof(CurrentPassword)); // Ta bort validering för CurrentPassword när vi raderar konto
            ModelState.Remove(nameof(NewPassword)); // Ta bort validering för NewPassword när vi raderar konto
            ModelState.Remove(nameof(ConfirmPassword)); // Ta bort validering för ConfirmPassword när vi raderar konto

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Användaren kunde inte hittas.");
            }

            await _messageService.DeleteUserIdFromMessagesAsync(user.Id);

            await _signInManager.SignOutAsync();
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                TempData["ErrorMessage"] = "Kunde inte radera kontot. Försök igen senare.";
                return Page();
            }

            TempData["SuccessMessage"] = "Ditt konto har raderats.";
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostClearHistoryAsync()
        {
            ModelState.Remove(nameof(NewUserName)); // Ta bort validering för NewUserName när vi rensar historik
            ModelState.Remove(nameof(CurrentPassword)); // Ta bort validering för CurrentPassword när vi rensar historik
            ModelState.Remove(nameof(NewPassword)); // Ta bort validering för NewPassword när vi rensar historik
            ModelState.Remove(nameof(ConfirmPassword)); // Ta bort validering för ConfirmPassword när vi rensar historik
            
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Användaren kunde inte hittas.");
            }
            
            var result = await _messageService.DeleteAllMessagesByUserIdAsync(user.Id);
            if (!result)
            {
                TempData["ErrorMessage"] = "Du hade inga meddelanden att rensa.";
                return Page();
            }

            TempData["SuccessMessage"] = "Chathistoriken har rensats.";

            return Page();

        }
    }
}
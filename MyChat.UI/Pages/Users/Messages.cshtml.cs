using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using MyChat.BLL.DTO;
using MyChat.BLL.Services;
using MyChat.DAL.Models;

namespace MyChat.UI.Pages.Users
{
    public class MessagesModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMessageService _messageService;

        public MessagesModel(UserManager<IdentityUser> userManager, IMessageService messageService)
        {
            _userManager = userManager;
            _messageService = messageService;
        }

        public IdentityUser CurrentUser { get; set; }
        public IEnumerable<MessageModel> AllMessages { get; set; }

        [BindProperty]
        public string MessageContent { get; set; } = string.Empty;




        public async Task OnGetAsync()
        {
            AllMessages = await _messageService.GetAllMessagesAsync();
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(MessageContent))
            {
                ModelState.AddModelError(string.Empty, "Meddelande kan inte vara tomt.");
                return Page();
            }

            await _messageService.AddMessageAsync(User, MessageContent);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int messageId)
        {
            var success = await _messageService.DeleteMessageAsync(User, messageId);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Kunde inte ta bort meddelandet. Kontrollera att du har rätt att ta bort det.");
            }
            AllMessages = await _messageService.GetAllMessagesAsync();
            return RedirectToPage();
        }
    }
}

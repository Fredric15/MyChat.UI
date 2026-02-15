using Microsoft.AspNetCore.Identity;
using MyChat.BLL.DTO;
using MyChat.DAL.Models;
using MyChat.DAL.Repositories;
using System.Security.Claims;

namespace MyChat.BLL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _repo;
        private readonly UserManager<IdentityUser> _userManager;
        public MessageService(IMessageRepository repo, UserManager<IdentityUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }
        public async Task<IEnumerable<MessageDto>> GetAllMessagesAsync()
        {
            var messages = await _repo.GetAllMessagesAsync();
            var messageDtos = new List<MessageDto>();
            //Mappa varje MessageModel till en MessageDto
            foreach (var m in messages)
            {
                var dto = new MessageDto
                {
                    Id = m.Id,
                    Content = m.Message,
                    SenderName = m.Username,
                    UserId = m.UserId,
                    Timestamp = m.Date
                };
                
                messageDtos.Add(dto);
            }


            return messageDtos;
        }

        public async Task<bool> AddMessageAsync(ClaimsPrincipal user, string message)
        {

            if (user.Identity?.IsAuthenticated != true || string.IsNullOrWhiteSpace(message))
            {
                return false;
            }
            var messageModel = new MessageModel
            {
                Username = user.Identity!.Name!,
                UserId = _userManager.GetUserId(user)!,
                Message = message,
                Date = DateTime.Now
            };

            await _repo.AddMessageAsync(messageModel);
            return true;
        }

        public async Task<bool> DeleteMessageAsync(ClaimsPrincipal user, int messageId)
        {
            // Hämta användarens alla meddelanden
            var messages = await _repo.GetMessagesByUserId(_userManager.GetUserId(user)!);
            var messageToDelete = messages.FirstOrDefault(m => m.Id == messageId);

            if (messageToDelete == null)
            {
                return false; // Meddelandet finns inte
            }

            if (messageToDelete.UserId != _userManager.GetUserId(user))
            {
                return false; // AnvändarId matchar ej med meddelandets UserId. Har inte rätt att ta bort detta meddelande
            }

            await _repo.DeleteMessageAsync(messageToDelete);
            return true;
        }

        public async Task<bool> UpdateMessageAsync(string newName, string userId)
        {
            // Hämta alla meddelanden som tillhör den aktuella användaren
            var messages = await _repo.GetMessagesByUserId(userId);

            //Enkel validering för det nya användarnamnet
            if (string.IsNullOrWhiteSpace(newName))
            {
                return false;
            }

            //Uppdatera användarnamnet i varje meddelande
            foreach (var message in messages)
            {
                message.Username = newName;
            }

            //Spara ändringarna i databasen
            return await _repo.UpdateAllMessagesAsync(messages);
        }

        public async Task<bool> DeleteAllMessagesByUserIdAsync(string id)
        {
            var messages = await _repo.GetMessagesByUserId(id);
            if (messages == null || !messages.Any())
            {
                return false;
            }

            foreach (var message in messages)
            {
                await _repo.DeleteMessageAsync(message);
            }

            return true;
        }

        public async Task<bool> DeleteUserIdFromMessagesAsync(string id)
        {
            var messages = await _repo.GetMessagesByUserId(id);

            if (messages == null || !messages.Any())
            {
                return false;
            }

            foreach (var message in messages)
            {
                message.UserId = null;
            }
            await _repo.UpdateAllMessagesAsync(messages);
            return true;
        }
    }
}

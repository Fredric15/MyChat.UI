using MyChat.BLL.DTO;
using MyChat.DAL.Models;
using System.Security.Claims;

namespace MyChat.BLL.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDto>> GetAllMessagesAsync();

        Task<bool> AddMessageAsync(ClaimsPrincipal user, string message );

        Task<bool> DeleteMessageAsync(ClaimsPrincipal user, int messageId);

        Task<bool> UpdateMessageAsync(string newName, string userId);
        Task<bool> DeleteAllMessagesByUserIdAsync(string id);
        Task<bool> DeleteUserIdFromMessagesAsync(string id);
    }
}

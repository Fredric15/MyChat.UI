using MyChat.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyChat.DAL.Repositories
{
    public interface IMessageRepository
    {
        Task<IEnumerable<MessageModel>> GetAllMessagesAsync();
        Task AddMessageAsync(MessageModel message);
        Task<bool> UpdateAllMessagesAsync(IEnumerable<MessageModel> messages);
        Task DeleteMessageAsync(MessageModel message);

        Task<IEnumerable<MessageModel>> GetMessagesByUserId(string userId);
    }
}

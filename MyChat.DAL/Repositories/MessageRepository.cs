using Microsoft.EntityFrameworkCore;
using MyChat.DAL.Repositories;
using MyChat.DAL.Data;
using MyChat.DAL.Models;


namespace MyChat.DAL.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;
        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddMessageAsync(MessageModel message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MessageModel>> GetAllMessagesAsync()
        {
             return await _context.Messages.OrderByDescending(m => m.Date).ToListAsync();
        }

        public async Task DeleteMessageAsync(MessageModel message)
        {
            await _context.Messages.Where(m => m.Id == message.Id).ExecuteDeleteAsync();
        }

        public async Task<bool> UpdateAllMessagesAsync(IEnumerable<MessageModel> messages)
        {

            _context.Messages.UpdateRange(messages);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<IEnumerable<MessageModel>> GetMessagesByUserId(string userId)
        {
            var messages = await _context.Messages.Where(m => m.UserId == userId).ToListAsync();
            return messages;
        }
    }
}

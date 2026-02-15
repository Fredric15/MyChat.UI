using System;
using System.Collections.Generic;
using System.Text;

namespace MyChat.DAL.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}

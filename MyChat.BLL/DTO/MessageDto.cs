using System;
using System.Collections.Generic;
using System.Text;

namespace MyChat.BLL.DTO
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public string SenderName { get; set; }

        public string? UserId { get; set; }

        public DateTime Timestamp { get; set; }
    }
}

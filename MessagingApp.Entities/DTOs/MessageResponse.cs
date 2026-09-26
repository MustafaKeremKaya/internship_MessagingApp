using System;
using MessagingApp.Core.Entities;

namespace MessagingApp.Entities.DTOs
{

    public class MessageResponse : IDto
    {

        public int Id { get; set; }

        public int ConversationId { get; set; }

        public string Sender { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; }
    }
}

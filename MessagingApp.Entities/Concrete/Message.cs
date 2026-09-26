using System;
using MessagingApp.Core.Entities;

namespace MessagingApp.Entities.Concrete
{

    public class Message : IEntity
    {

        public Message()
        {
            SentAt = DateTime.UtcNow;
        }

        public int Id { get; set; }

        public int ConversationId { get; set; }

        public string Sender { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; }

        public virtual Conversation? Conversation { get; set; }
    }
}

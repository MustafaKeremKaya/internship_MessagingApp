using System;
using System.Collections.Generic;
using MessagingApp.Core.Entities;

namespace MessagingApp.Entities.Concrete
{

    public class Conversation : IEntity
    {

        public Conversation()
        {
            Messages = new List<Message>();
            CreatedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}

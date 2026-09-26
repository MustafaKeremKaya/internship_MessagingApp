using System;
using MessagingApp.Core.Entities;

namespace MessagingApp.Entities.DTOs
{

    public class ConversationSummaryResponse : IDto
    {

        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}

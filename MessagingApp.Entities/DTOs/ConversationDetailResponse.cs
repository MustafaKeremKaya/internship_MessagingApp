using System;
using System.Collections.Generic;
using MessagingApp.Core.Entities;

namespace MessagingApp.Entities.DTOs
{

    public class ConversationDetailResponse : IDto
    {

        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public int MessageCount { get; set; }

        public List<string> Participants { get; set; } = new List<string>();
    }
}

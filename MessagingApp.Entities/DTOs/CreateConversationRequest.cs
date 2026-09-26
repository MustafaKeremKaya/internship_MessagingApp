using MessagingApp.Core.Entities;

namespace MessagingApp.Entities.DTOs
{

    public class CreateConversationRequest : IDto
    {

        public string Title { get; set; } = string.Empty;
    }
}

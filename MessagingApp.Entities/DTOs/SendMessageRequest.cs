using MessagingApp.Core.Entities;

namespace MessagingApp.Entities.DTOs
{

    public class SendMessageRequest : IDto
    {

        public string Sender { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
    }
}

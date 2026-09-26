using System.Linq;
using Microsoft.EntityFrameworkCore;
using MessagingApp.Core.DataAccess.EntityFramework;
using MessagingApp.DataAccess.Abstract;
using MessagingApp.Entities.Concrete;
using MessagingApp.Entities.DTOs;

namespace MessagingApp.DataAccess.Concrete.EntityFramework
{

    public class EfConversationDal : EfEntityRepositoryBase<Conversation, AppDbContext>, IConversationDal
    {

        public ConversationDetailResponse? GetConversationDetail(int conversationId)
        {
            using (var context = new AppDbContext())
            {

                var conversation = context.Conversations
                    .Include(c => c.Messages)
                    .SingleOrDefault(c => c.Id == conversationId);

                if (conversation == null)
                {
                    return null;
                }

                return new ConversationDetailResponse
                {
                    Id = conversation.Id,
                    Title = conversation.Title,
                    CreatedAt = conversation.CreatedAt,
                    MessageCount = conversation.Messages.Count,
                    Participants = conversation.Messages
                        .Select(m => m.Sender)
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Distinct()
                        .ToList()
                };
            }
        }
    }
}

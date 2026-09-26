using MessagingApp.Core.DataAccess;
using MessagingApp.Entities.Concrete;
using MessagingApp.Entities.DTOs;

namespace MessagingApp.DataAccess.Abstract
{

    public interface IConversationDal : IEntityRepository<Conversation>
    {

        ConversationDetailResponse? GetConversationDetail(int conversationId);
    }
}

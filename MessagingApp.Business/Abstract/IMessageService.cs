using System.Collections.Generic;
using MessagingApp.Core.Utilities.Results;
using MessagingApp.Entities.DTOs;

namespace MessagingApp.Business.Abstract
{

    public interface IMessageService
    {

        IDataResult<MessageResponse> Send(int conversationId, SendMessageRequest request);

        IDataResult<List<MessageResponse>> GetMessagesByConversationId(int conversationId);
    }
}

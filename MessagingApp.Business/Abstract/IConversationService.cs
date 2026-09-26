using System.Collections.Generic;
using MessagingApp.Core.Utilities.Results;
using MessagingApp.Entities.DTOs;

namespace MessagingApp.Business.Abstract
{

    public interface IConversationService
    {

        IDataResult<ConversationSummaryResponse> Create(CreateConversationRequest request);

        IDataResult<List<ConversationSummaryResponse>> GetAll();

        IDataResult<ConversationDetailResponse> GetById(int id);
    }
}

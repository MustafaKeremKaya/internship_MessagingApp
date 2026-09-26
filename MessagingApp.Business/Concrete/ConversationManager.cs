using System;
using System.Collections.Generic;
using System.Linq;
using MessagingApp.Business.Abstract;
using MessagingApp.Core.Constants;
using MessagingApp.Core.Utilities.Results;
using MessagingApp.DataAccess.Abstract;
using MessagingApp.Entities.Concrete;
using MessagingApp.Entities.DTOs;

namespace MessagingApp.Business.Concrete
{

    public class ConversationManager : IConversationService
    {
        private readonly IConversationDal _conversationDal;

        public ConversationManager(IConversationDal conversationDal)
        {
            _conversationDal = conversationDal;
        }

        public IDataResult<ConversationSummaryResponse> Create(CreateConversationRequest request)
        {

            if (request == null || string.IsNullOrWhiteSpace(request.Title))
            {
                return new ErrorDataResult<ConversationSummaryResponse>(Messages.ConversationTitleEmpty);
            }

            if (request.Title.Trim().Length > 100)
            {
                return new ErrorDataResult<ConversationSummaryResponse>(Messages.ConversationTitleTooLong);
            }

            var conversation = new Conversation
            {
                Title = request.Title.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _conversationDal.Add(conversation);

            var response = new ConversationSummaryResponse
            {
                Id = conversation.Id,
                Title = conversation.Title,
                CreatedAt = conversation.CreatedAt
            };

            return new SuccessDataResult<ConversationSummaryResponse>(response, Messages.ConversationCreated);
        }

        public IDataResult<List<ConversationSummaryResponse>> GetAll()
        {
            var conversations = _conversationDal.GetAll();

            var responseList = conversations
                .OrderBy(c => c.CreatedAt)
                .Select(c => new ConversationSummaryResponse
                {
                    Id = c.Id,
                    Title = c.Title,
                    CreatedAt = c.CreatedAt
                })
                .ToList();

            return new SuccessDataResult<List<ConversationSummaryResponse>>(responseList, Messages.ConversationsListed);
        }

        public IDataResult<ConversationDetailResponse> GetById(int id)
        {
            var detail = _conversationDal.GetConversationDetail(id);

            if (detail == null)
            {
                return new ErrorDataResult<ConversationDetailResponse>(Messages.ConversationNotFound);
            }

            return new SuccessDataResult<ConversationDetailResponse>(detail, Messages.ConversationDetailListed);
        }
    }
}

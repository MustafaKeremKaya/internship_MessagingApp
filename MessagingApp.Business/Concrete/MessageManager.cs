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

    public class MessageManager : IMessageService
    {
        private readonly IMessageDal _messageDal;
        private readonly IConversationDal _conversationDal;

        public MessageManager(IMessageDal messageDal, IConversationDal conversationDal)
        {
            _messageDal = messageDal;
            _conversationDal = conversationDal;
        }

        public IDataResult<MessageResponse> Send(int conversationId, SendMessageRequest request)
        {

            if (request == null)
            {
                return new ErrorDataResult<MessageResponse>(Messages.MessageRequestEmpty);
            }

            if (string.IsNullOrWhiteSpace(request.Sender))
            {
                return new ErrorDataResult<MessageResponse>(Messages.SenderEmpty);
            }

            if (request.Sender.Trim().Length > 50)
            {
                return new ErrorDataResult<MessageResponse>(Messages.SenderTooLong);
            }

            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return new ErrorDataResult<MessageResponse>(Messages.ContentEmpty);
            }

            if (request.Content.Trim().Length > 500)
            {
                return new ErrorDataResult<MessageResponse>(Messages.ContentTooLong);
            }

            var conversation = _conversationDal.Get(c => c.Id == conversationId);
            if (conversation == null)
            {
                return new ErrorDataResult<MessageResponse>(Messages.ConversationForMessageNotFound);
            }

            var message = new Message
            {
                ConversationId = conversationId,
                Sender = request.Sender.Trim(),
                Content = request.Content.Trim(),
                SentAt = DateTime.UtcNow
            };

            _messageDal.Add(message);

            var response = new MessageResponse
            {
                Id = message.Id,
                ConversationId = message.ConversationId,
                Sender = message.Sender,
                Content = message.Content,
                SentAt = message.SentAt
            };

            return new SuccessDataResult<MessageResponse>(response, Messages.MessageSent);
        }

        public IDataResult<List<MessageResponse>> GetMessagesByConversationId(int conversationId)
        {

            var conversation = _conversationDal.Get(c => c.Id == conversationId);
            if (conversation == null)
            {
                return new ErrorDataResult<List<MessageResponse>>(Messages.ConversationNotFound);
            }

            var messages = _messageDal.GetAll(m => m.ConversationId == conversationId);

            var responseList = messages
                .OrderBy(m => m.SentAt)
                .Select(m => new MessageResponse
                {
                    Id = m.Id,
                    ConversationId = m.ConversationId,
                    Sender = m.Sender,
                    Content = m.Content,
                    SentAt = m.SentAt
                })
                .ToList();

            return new SuccessDataResult<List<MessageResponse>>(responseList, Messages.MessagesListed);
        }
    }
}

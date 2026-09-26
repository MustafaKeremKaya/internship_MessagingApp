using MessagingApp.Core.DataAccess;
using MessagingApp.Entities.Concrete;

namespace MessagingApp.DataAccess.Abstract
{

    public interface IMessageDal : IEntityRepository<Message>
    {
    }
}

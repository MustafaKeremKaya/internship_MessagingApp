using MessagingApp.Core.DataAccess.EntityFramework;
using MessagingApp.DataAccess.Abstract;
using MessagingApp.Entities.Concrete;

namespace MessagingApp.DataAccess.Concrete.EntityFramework
{

    public class EfMessageDal : EfEntityRepositoryBase<Message, AppDbContext>, IMessageDal
    {
    }
}

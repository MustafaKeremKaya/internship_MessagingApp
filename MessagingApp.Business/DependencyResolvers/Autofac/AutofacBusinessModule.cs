using Autofac;
using MessagingApp.Business.Abstract;
using MessagingApp.Business.Concrete;
using MessagingApp.DataAccess.Abstract;
using MessagingApp.DataAccess.Concrete.EntityFramework;

namespace MessagingApp.Business.DependencyResolvers.Autofac
{

    public class AutofacBusinessModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<ConversationManager>().As<IConversationService>().InstancePerLifetimeScope();

            builder.RegisterType<EfConversationDal>().As<IConversationDal>().InstancePerLifetimeScope();

            builder.RegisterType<MessageManager>().As<IMessageService>().InstancePerLifetimeScope();

            builder.RegisterType<EfMessageDal>().As<IMessageDal>().InstancePerLifetimeScope();

            builder.RegisterType<AppDbContext>().InstancePerLifetimeScope();
        }
    }
}

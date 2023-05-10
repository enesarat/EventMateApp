using Autofac;
using EventMate.Core.Repository;
using EventMate.Core.Service;
using EventMate.Core.UnitOfWork;
using EventMate.Repository.Context;
using EventMate.Repository.Repository;
using EventMate.Repository.UnitOfWork;
using EventMate.Service.Mapper;
using EventMate.Service.Service;
using System.Reflection;
using Module = Autofac.Module;

namespace EventMate.Web.Modules
{
    public class RepositoryAndServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(GenericService<,>)).As(typeof(IGenericService<,>)).InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();



            var apiAssembly = Assembly.GetExecutingAssembly();
            var repoAssembly = Assembly.GetAssembly(typeof(ApplicationDbContext));
            var serviceAssembly = Assembly.GetAssembly(typeof(MappingProfile));

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}

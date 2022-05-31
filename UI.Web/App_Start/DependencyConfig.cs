using Autofac;
using Autofac.Integration.Mvc;
using Sky.CMS.IServices;
using Sky.CMS.Services;
using Sky.Common.Provider.Pagination;
using Sky.Generic.IServices;
using Sky.Generic.Services;
using Sky.Log.IServices;
using Sky.Log.Services;
using Sky.News.IServices;
using Sky.News.Services;
using Sky.Newspaper.IServices;
using Sky.Newspaper.Services;
using Sky.Repository.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Web.App_Start
{
    public class DependencyConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            //  DB Connection
            builder.Register(ctx =>
            {
                var connectionStringGLog = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringGLog"];
                if (string.IsNullOrWhiteSpace(connectionStringGLog.ConnectionString))
                    throw new InvalidOperationException(
                        "ConnectionStringGLog can not be null or empty");

                var connectionStringCMS = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringCMS"];
                if (string.IsNullOrWhiteSpace(connectionStringCMS.ConnectionString))
                    throw new InvalidOperationException(
                        "ConnectionStringCMS can not be null or empty");

                var connectionStringNews = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringNews"];
                if (string.IsNullOrWhiteSpace(connectionStringNews.ConnectionString))
                    throw new InvalidOperationException(
                        "ConnectionStringNews can not be null or empty");

                var connectionStringNewspaper = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringNewspaper"];
                if (string.IsNullOrWhiteSpace(connectionStringNewspaper.ConnectionString))
                    throw new InvalidOperationException(
                        "ConnectionStringNewspaper can not be null or empty");

                var connectionStringGeneric = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringGeneric"];
                if (string.IsNullOrWhiteSpace(connectionStringGeneric.ConnectionString))
                    throw new InvalidOperationException(
                        "ConnectionStringGeneric can not be null or empty");

                return new DapperRepository(
                    contextCMS: new System.Data.SqlClient.SqlConnection(connectionStringCMS.ConnectionString),
                    contextGLog: new System.Data.SqlClient.SqlConnection(connectionStringGLog.ConnectionString),
                    contextGeneric: new System.Data.SqlClient.SqlConnection(connectionStringGeneric.ConnectionString),
                    contextNews: new System.Data.SqlClient.SqlConnection(connectionStringNews.ConnectionString),
                    contextNewspaper: new System.Data.SqlClient.SqlConnection(connectionStringNewspaper.ConnectionString)
                    );

            }).As<IMicroOrmRepository>().InstancePerLifetimeScope();

            builder.RegisterType<NavigationService>().As<INavigationService>().InstancePerLifetimeScope();
            builder.RegisterType<AppService>().As<IAppService>().InstancePerLifetimeScope();
            builder.RegisterType<RedirectService>().As<IRedirectService>().InstancePerLifetimeScope();

            builder.RegisterType<NewsService>().As<INewsService>().InstancePerLifetimeScope();
            builder.RegisterType<NewspaperService>().As<INewspaperService>().InstancePerLifetimeScope();

            builder.RegisterType<LogService>().As<ILogService>().InstancePerLifetimeScope();

            builder.RegisterType<Pagination>().As<IPaginationProvider>().InstancePerLifetimeScope();

            builder.RegisterType<GenericService>().As<IGenericService>().InstancePerLifetimeScope();

            builder.RegisterType<Infrastructure.NewsHelper>().InstancePerLifetimeScope();

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
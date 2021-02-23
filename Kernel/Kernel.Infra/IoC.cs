using Kernel.Domain.Model.Enums;
using Kernel.Domain.Model.Helpers;
using Kernel.Domain.Model.Settings;
using Kernel.Domain.Repositories;
using Kernel.Domain.Services;
using Kernel.Infra.Email;
using Kernel.Infra.Jwt;
using Kernel.Infra.Repositories;
using Kernel.Infra.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using System;
using System.IO;

namespace Kernel.Infra
{
    public static class IoC
    {
        public static Container Container { get; set; }
        public static AppSettings AppSettings { get; set; }

        public static void InitializeContainer()
        {
            if (Container == null)
            {
                Container = new Container();
                Container.Options.EnableAutoVerification = false;
                Container.Options.ResolveUnregisteredConcreteTypes = true;
            }
        }

        public static void Start<TDbContext>(Context context = Context.IntegratedTest) where TDbContext : DbContext
        {
            InitializeContainer();

            if (Container.IsLocked) return;

            RegisterCommonDependencies();

            if (context == Context.UnitTest)
                RegisterMockDependencies();
            else
                RegisterInfraDependencies<TDbContext>();
        }

        public static void RegisterCommonDependencies(string environment = "Development")
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{environment}.json");

            var configuration = builder.Build();

            AppSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
            Container.RegisterInstance(AppSettings);

            var factory = new LoggerFactory();
            Container.RegisterInstance<ILoggerFactory>(factory);
            Container.RegisterSingleton(typeof(ILogger<>), typeof(Logger<>));

            Container.Register<ITokenHelper, JwtTokenHelper>();
            Container.Register<IEmailService, EmailService>();
            Container.Register<IBlobStorage, AzureBlobStorage>();
        }

        public static void RegisterMockDependencies()
        {
            //Container.Register<ISessionHelper, MockSessionHelper>();
            //Container.Register(typeof(IDocumentRepository<>),
            //    typeof(MockDocumentRepository<>));
        }

        public static void RegisterInfraDependencies<TDbContext>() where TDbContext : DbContext
        {
            Container.Register<DbContext, TDbContext>();
            Container.Register<ISessionFactory, SessionFactory>();
        }

        public static void Register<TFrom, TTo>() 
            where TTo : class, TFrom
            where TFrom : class
        {
            if (Container == null)
                throw new Exception("SimpleInjector Container was not initialized!");

            Container.Register<TFrom, TTo>();
        }

        public static void RegisterScoped<TFrom, TTo>()
            where TTo : class, TFrom
            where TFrom : class
        {
            if (Container == null)
                throw new Exception("SimpleInjector Container was not initialized!");

            Container.Register<TFrom, TTo>(Lifestyle.Scoped);
        }

        public static void Register<T>() where T : class
        {
            if (Container == null)
                throw new Exception("SimpleInjector Container was not initialized!");

            Container.Register<T>();
        }

        public static void RegisterInstance<T>(T instance) where T : class
        {
            if (Container == null)
                throw new Exception("SimpleInjector Container was not initialized!");

            Container.RegisterInstance(instance);
        }

        public static T Get<T>() where T : class
        {
            if (Container == null)
                throw new Exception("SimpleInjector Container was not initialized!");

            return Container.GetInstance<T>();
        }

        public static object Get(Type type)
        {
            if (Container == null)
                throw new Exception("SimpleInjector Container was not initialized!");

            return Container.GetInstance(type);
        }
    }
}
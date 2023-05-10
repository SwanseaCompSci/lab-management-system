using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Respawn;
using Respawn.Graph;
using SwanseaCompSci.LabManagementSystem.Core.Application;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Models;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;
using SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Infrastructure.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application
{
    [SetUpFixture]
    public partial class Testing
    {
        private static IConfigurationRoot Configuration { get; set; } = null!;
        private static IServiceScopeFactory ScopeFactory { get; set; } = null!;
        private static Respawner Respawner { get; set; } = null!;

        private static ClaimsPrincipal? User { get; set; }
        private static DateTime TestDateTime { get; set; } = DateTime.UtcNow;

        [OneTimeSetUp]
        public async Task RunBeforeAnyTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddUserSecrets(GetType().Assembly, true, true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddLogging();

            services.AddApplication();
            services.AddPersistenceInfrastructure(Configuration);
            services.AddSharedInfrastructure(Configuration);

            ReplaceICurrentUserService(services);
            ReplaceIDateTimeService(services);
            ReplaceIIdentityProvider(services);

            ScopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();

            EnsureDatabase();

            Respawner = await Respawner.CreateAsync(Configuration.GetConnectionString("DbConnection"), new RespawnerOptions
            {
                CheckTemporalTables = true,
                TablesToIgnore = new Table[]
                {
                    "__EFMigrationsHistory",
                }
            });
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            using var scope = ScopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.EnsureDeleted();
        }
    }

    public partial class Testing
    {
        private static void ReplaceICurrentUserService(ServiceCollection services)
        {
            var descriptor = services.FirstOrDefault(d =>
                d.ServiceType == typeof(ICurrentUserService));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            if (User is null)
            {
                User = Users.GetDefaultUser();
            }

            services.AddTransient(provider =>
            {
                var mock = new Mock<ICurrentUserService>();
                mock.SetupGet(d => d.User).Returns(User);
                mock.SetupGet(d => d.UserId).Returns(LabManagementSystem.Core.Application.Authorization.Helpers.GetUserId(User));
                mock.SetupGet(d => d.UserName).Returns(LabManagementSystem.Core.Application.Authorization.Helpers.GetUserName(User));
                mock.SetupGet(d => d.UserFirstName).Returns(LabManagementSystem.Core.Application.Authorization.Helpers.GetUserFirstName(User));
                mock.SetupGet(d => d.UserSurname).Returns(LabManagementSystem.Core.Application.Authorization.Helpers.GetUserSurname(User));
                mock.SetupGet(d => d.UserApplicationRole).Returns(LabManagementSystem.Core.Application.Authorization.Helpers.GetApplicationRole(User));
                return mock.Object;
            });
        }
        private static void ReplaceIDateTimeService(ServiceCollection services)
        {
            var descriptor = services.FirstOrDefault(d =>
                d.ServiceType == typeof(IDateTimeService));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddTransient(provider =>
            {
                var today = new DateTime(year: TestDateTime.ToUniversalTime().Year,
                                         month: TestDateTime.ToUniversalTime().Month,
                                         day: TestDateTime.ToUniversalTime().Day);

                var mock = new Mock<IDateTimeService>();
                mock.Setup(x => x.UtcNow).Returns(TestDateTime.ToUniversalTime());
                mock.Setup(x => x.Today).Returns(today);
                return mock.Object;
            });
        }
        private static void ReplaceIIdentityProvider(ServiceCollection services)
        {
            var descriptor = services.FirstOrDefault(d =>
                d.ServiceType == typeof(IIdentityProvider));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddTransient(provider =>
            {
                var mock = new Mock<IIdentityProvider>();
                return mock.Object;
            });
        }

        private static void EnsureDatabase()
        {
            using var scope = ScopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();
        }

        public static void RunAsUser(ClaimsPrincipal user)
        {
            User = user;
        }
        public static void RunOnDateTime(DateTime dateTime)
        {
            TestDateTime = dateTime;
        }

        public static TService? GetService<TService>()
        {
            var scope = ScopeFactory.CreateScope();
            return scope.ServiceProvider.GetService<TService>();
        }

        public static async Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using var scope = ScopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Add(entity);

            await context.SaveChangesAsync();

            return entity;
        }
        public static async Task<IEnumerable<TEntity>> AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            using var scope = ScopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            foreach (var item in entities)
            {
                context.Add(entity: item);
            }

            await context.SaveChangesAsync();

            return entities;
        }
        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = ScopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

            return await mediator.Send(request);
        }
        public static async Task PublishAsync<TDomainEvent>(DomainEventNotification<TDomainEvent> notification, CancellationToken cancellationToken = default) where TDomainEvent : DomainEvent
        {
            using var scope = ScopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IPublisher>();

            await mediator.Publish(notification, cancellationToken);
        }

        public static async Task ResetState()
        {
            await Respawner.ResetAsync(Configuration.GetConnectionString("DbConnection"));

            User = null;
        }
    }
}

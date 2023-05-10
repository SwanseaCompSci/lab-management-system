using MediatR;
using Microsoft.Extensions.Logging;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Models;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Events.UserEvents;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.EventHandlers.UserEvents
{
    // TODO: Add docs comments
    public sealed class UserTimeAvailabilitySubmittedDomainEventHandler : INotificationHandler<DomainEventNotification<UserTimeAvailabilitySubmittedDomainEvent>>
    {
        public UserTimeAvailabilitySubmittedDomainEventHandler(IUserRepository repository,
                                                               ILogger<UserTimeAvailabilitySubmittedDomainEventHandler> logger)
        {
            Repository = repository;
            Logger = logger;
        }

        private IUserRepository Repository { get; }
        private ILogger<UserTimeAvailabilitySubmittedDomainEventHandler> Logger { get; }

        public async Task Handle(DomainEventNotification<UserTimeAvailabilitySubmittedDomainEvent> notification, CancellationToken cancellationToken)
        {
            Logger.LogInformation($"Removing time availability token for User ({notification.Event.UserId}).");

            var user = await Repository.GetItemAsync(id: notification.Event.UserId, cancellationToken: cancellationToken);
            if (user is null)
            {
                throw new EntityNotFoundException(nameof(User), notification.Event.UserId);
            }

            user.QuestionnaireToken = null;

            _ = await Repository.UpdateItemAsync(id: notification.Event.UserId,
                                                 item: user,
                                                 cancellationToken: cancellationToken);

            Logger.LogInformation($"Time availability token removed for User ({notification.Event.UserId}).");
        }
    }
}

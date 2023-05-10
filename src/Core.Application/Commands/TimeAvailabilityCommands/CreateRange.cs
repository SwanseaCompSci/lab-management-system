using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.TimeAvailabilityModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Events.UserEvents;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.TimeAvailabilityCommands
{
    // TODO: Add docs comments
    public sealed class CreateRange
    {
        public sealed class TimeAvailabilityCommandModel
        {
            public string Day { get; set; } = null!;
            public TimeOnly StartTime { get; set; }
            public TimeOnly EndTime { get; set; }
        }
        internal sealed class TimeAvailabilityValidator : AbstractValidator<TimeAvailabilityCommandModel>
        {
            public TimeAvailabilityValidator()
            {
                RuleFor(x => x.Day)
                    .IsEnumName(typeof(WorkDayOfWeek))
                    .NotEmpty();

                RuleFor(x => x.StartTime)
                    .NotEmpty();

                RuleFor(x => x.EndTime)
                    .NotEmpty();
                RuleFor(x => x.EndTime)
                    .GreaterThan(x => x.StartTime);
            }
        }

        public sealed class Command : IRequest<Response>
        {
            public Guid UserId { get; set; }
            public Guid Token { get; set; }
            public IEnumerable<TimeAvailabilityCommandModel> TimeAvailabilities { get; set; } = null!;
        }

        public sealed class Response
        {
            public Response(IEnumerable<TimeAvailabilityModel> resource)
            {
                Resource = resource;
            }

            public IEnumerable<TimeAvailabilityModel> Resource { get; }
        }

        public sealed class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.UserId)
                    .NotEmpty();

                RuleFor(x => x.Token)
                    .NotEmpty();

                RuleFor(x => x.TimeAvailabilities)
                    .NotEmpty();
                RuleForEach(x => x.TimeAvailabilities)
                    .SetValidator(new TimeAvailabilityValidator());
            }
        }

        internal sealed class CommandHandler : IRequestHandler<Command, Response>
        {
            public CommandHandler(ITimeAvailabilityRepository repository,
                                  IMapper mapper,
                                  IDomainEventService domainEventService)
            {
                Repository = repository;
                Mapper = mapper;
                DomainEventService = domainEventService;
            }

            private ITimeAvailabilityRepository Repository { get; }
            private IMapper Mapper { get; }
            private IDomainEventService DomainEventService { get; }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entities = await Repository.AddRangeAsync(items: request.TimeAvailabilities.Select(x => new TimeAvailability(userId: request.UserId,
                                                                                                                                 day: Enum.Parse<WorkDayOfWeek>(x.Day),
                                                                                                                                 startTime: x.StartTime,
                                                                                                                                 endTime: x.EndTime)),
                                                              cancellationToken: cancellationToken);

                var models = entities.Select(x => Mapper.Map<TimeAvailability, TimeAvailabilityModel>(x));

                await DomainEventService.Publish(new UserTimeAvailabilitySubmittedDomainEvent(request.UserId)
                {
                    IsPublished = true
                });

                return new Response(resource: models);
            }
        }
    }
}

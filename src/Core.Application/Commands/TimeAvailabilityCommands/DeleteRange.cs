using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.TimeAvailabilityModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.TimeAvailabilityCommands
{
    // TODO: Add docs comments
    public sealed class DeleteRange
    {
        public sealed class Command : IRequest<Response>
        {
            public Command(Guid userId)
            {
                UserId = userId;
            }

            public Guid UserId { get; }
        }

        public sealed class Response
        {
            public Response(IEnumerable<TimeAvailabilityModel>? resource)
            {
                Resource = resource;
            }

            public IEnumerable<TimeAvailabilityModel>? Resource { get; }
        }

        public sealed class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.UserId).NotEmpty();
            }
        }

        internal sealed class CommandHandler : IRequestHandler<Command, Response>
        {
            public CommandHandler(ITimeAvailabilityRepository repository,
                                  IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private ITimeAvailabilityRepository Repository { get; }
            private IMapper Mapper { get; }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var timeAvailabilities = await Repository.DeleteRangeByUserIdAsync(userId: request.UserId,
                                                                           cancellationToken: cancellationToken);

                return timeAvailabilities.Any()
                    ? new Response(resource: timeAvailabilities.Select(x => Mapper.Map<TimeAvailability, TimeAvailabilityModel>(x)))
                    : new Response(resource: null);
            }
        }
    }
}

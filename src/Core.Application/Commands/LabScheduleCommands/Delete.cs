using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabScheduleModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabScheduleCommands
{
    // TODO: Add docs comments
    public sealed class Delete
    {
        public sealed class Command : IRequest<Response>
        {
            public Command(Guid labScheduleId)
            {
                LabScheduleId = labScheduleId;
            }

            public Guid LabScheduleId { get; }
        }

        public sealed class Response
        {
            public Response(LabScheduleModel? resource)
            {
                Resource = resource;
            }

            public LabScheduleModel? Resource { get; }
        }

        public sealed class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.LabScheduleId).NotEmpty();
            }
        }

        internal sealed class CommandHandler : IRequestHandler<Command, Response>
        {
            public CommandHandler(ILabScheduleRepository repository,
                                  IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private ILabScheduleRepository Repository { get; }
            private IMapper Mapper { get; }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var labSchedule = await Repository.DeleteItemAsync(id: request.LabScheduleId,
                                                                   cancellationToken: cancellationToken);

                return labSchedule is not null
                    ? new Response(resource: Mapper.Map<LabSchedule, LabScheduleModel>(labSchedule))
                    : new Response(resource: null);
            }
        }
    }
}

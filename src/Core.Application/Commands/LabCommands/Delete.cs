using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabCommands
{
    // TODO: Add docs comments
    public sealed class Delete
    {
        public sealed class Command : IRequest<Response>
        {
            public Command(Guid labId)
            {
                LabId = labId;
            }

            public Guid LabId { get; }
        }

        public sealed class Response
        {
            public Response(LabModel? resource)
            {
                Resource = resource;
            }

            public LabModel? Resource { get; }
        }

        public sealed class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.LabId).NotEmpty();
            }
        }

        internal sealed class CommandHandler : IRequestHandler<Command, Response>
        {
            public CommandHandler(ILabRepository repository,
                                  IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private ILabRepository Repository { get; }
            private IMapper Mapper { get; }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var lab = await Repository.DeleteItemAsync(id: request.LabId,
                                                           cancellationToken: cancellationToken);

                return lab is not null
                    ? new Response(Mapper.Map<Lab, LabModel>(lab))
                    : new Response(null);
            }
        }
    }
}

using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModuleCommands
{
    // TODO: Add docs comments
    public sealed class Delete
    {
        public sealed class Command : IRequest<Response>
        {
            public Command(Guid moduleId)
            {
                ModuleId = moduleId;
            }

            public Guid ModuleId { get; }
        }

        public sealed class Response
        {
            public Response(ModuleModel? resource)
            {
                Resource = resource;
            }

            public ModuleModel? Resource { get; }
        }

        public sealed class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ModuleId).NotEmpty();
            }
        }

        internal sealed class CommandHandler : IRequestHandler<Command, Response>
        {
            public CommandHandler(IModuleRepository repository,
                                  IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private IModuleRepository Repository { get; }
            private IMapper Mapper { get; }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var module = await Repository.DeleteItemAsync(id: request.ModuleId,
                                                              cancellationToken: cancellationToken);

                return module is not null
                    ? new Response(resource: Mapper.Map<Module, ModuleModel>(module))
                    : new Response(resource: null);
            }
        }
    }
}

using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModuleCommands
{
    // TODO: Add docs comments
    public sealed class DeleteAll
    {
        public sealed class Command : IRequest<Response> { }

        public sealed class Response
        {
            public Response(IEnumerable<ModuleModel>? resource)
            {
                Resource = resource;
            }

            public IEnumerable<ModuleModel>? Resource { get; }
        }

        public sealed class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                // There is nothing to validate
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
                var modules = await Repository.DeleteAllAsync(cancellationToken);

                return modules.Any()
                    ? new Response(resource: modules.Select(x => Mapper.Map<Module, ModuleModel>(x)))
                    : new Response(resource: null);
            }
        }
    }
}

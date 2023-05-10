using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModuleCommands
{
    // TODO: Add docs comments
    public sealed class Create
    {
        public sealed class Command : IRequest<Response>
        {
            public string Name { get; set; } = null!;
            public string Code { get; set; } = null!;
            public string Level { get; set; } = null!;
        }

        public sealed class Response
        {
            public Response(ModuleModel resource)
            {
                Resource = resource;
            }

            public ModuleModel Resource { get; }
        }

        public sealed class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name)
                    .MaximumLength(50)
                    .NotEmpty();

                RuleFor(x => x.Code)
                    .MaximumLength(10)
                    .NotEmpty();

                RuleFor(x => x.Level)
                    .IsEnumName(typeof(Level))
                    .NotEmpty();
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
                var entity = await Repository.AddItemAsync(
                    item: new Module(name: request.Name,
                                     code: request.Code,
                                     level: Enum.Parse<Level>(request.Level)),
                    cancellationToken: cancellationToken);

                var moduleModel = Mapper.Map<Module, ModuleModel>(entity);

                return new Response(resource: moduleModel);
            }
        }
    }
}

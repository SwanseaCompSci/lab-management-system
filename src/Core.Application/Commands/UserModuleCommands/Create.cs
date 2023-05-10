using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserModuleCommands
{
    // TODO: Add docs comments
    public sealed class Create
    {
        public sealed class Command : IRequest<Response>
        {
            public Guid UserId { get; set; }
            public Guid ModuleId { get; set; }
            public string Role { get; set; } = null!;
        }

        public sealed class Response
        {
            public Response(UserModuleModel resource)
            {
                Resource = resource;
            }

            public UserModuleModel Resource { get; }
        }

        public sealed class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.UserId).NotEmpty();
                RuleFor(x => x.ModuleId).NotEmpty();
                RuleFor(x => x.Role)
                    .IsEnumName(typeof(ModuleRole))
                    .NotEmpty();
            }
        }

        internal sealed class CommandHandler : IRequestHandler<Command, Response>
        {
            public CommandHandler(IUserModuleRepository repository,
                                  IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private IUserModuleRepository Repository { get; }
            private IMapper Mapper { get; }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var userModule = await Repository.AddItemAsync(item: new UserModule(userId: request.UserId,
                                                                                    moduleId: request.ModuleId,
                                                                                    role: Enum.Parse<ModuleRole>(request.Role)),
                                                               cancellationToken: cancellationToken);

                var resource = Mapper.Map<UserModule, UserModuleModel>(userModule);

                return new Response(resource: resource);
            }
        }
    }
}

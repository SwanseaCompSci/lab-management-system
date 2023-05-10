using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserModuleCommands
{
    // TODO: Add docs comments
    public sealed class Delete
    {
        public sealed class Command : IRequest<Response>
        {
            public Command(Guid userId, Guid moduleId)
            {
                UserId = userId;
                ModuleId = moduleId;
            }

            public Guid UserId { get; }
            public Guid ModuleId { get; }
        }

        public sealed class Response
        {
            public Response(UserModuleModel? resource)
            {
                Resource = resource;
            }

            public UserModuleModel? Resource { get; }
        }

        public sealed class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.UserId).NotEmpty();
                RuleFor(x => x.ModuleId).NotEmpty();
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
                var userModule = await Repository.DeleteItemAsync(userId: request.UserId,
                                                                  moduleId: request.ModuleId,
                                                                  cancellationToken: cancellationToken);

                return userModule is not null
                    ? new Response(resource: Mapper.Map<UserModule, UserModuleModel>(userModule))
                    : new Response(resource: null);
            }
        }
    }
}

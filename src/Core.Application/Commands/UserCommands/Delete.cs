using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands
{
    // TODO: Add docs comments
    public sealed class Delete
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
            public Response(UserModel? resource)
            {
                Resource = resource;
            }

            public UserModel? Resource { get; }
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
            public CommandHandler(IIdentityProvider identityProvider,
                                  IUserRepository repository,
                                  IMapper mapper)
            {
                IdentityProvider = identityProvider;
                Repository = repository;
                Mapper = mapper;
            }

            private IIdentityProvider IdentityProvider { get; }
            private IUserRepository Repository { get; }
            private IMapper Mapper { get; }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await Repository.DeleteItemAsync(id: request.UserId,
                                                            cancellationToken: cancellationToken);

                if (user is not null)
                {
                    var userRoleId = await IdentityProvider.GetRoleIdAsync(role: ApplicationRole.User, cancellationToken: cancellationToken);
                    var currentUserRoleId = await IdentityProvider.GetUserRoleIdAsync(userId: user.Id, cancellationToken: cancellationToken);

                    if (currentUserRoleId is not null && currentUserRoleId == userRoleId)
                    {
                        await IdentityProvider.RemoveFromRoleAsync(userId: user.Id, roleId: userRoleId, cancellationToken: cancellationToken);
                    }
                }

                return user is not null
                    ? new Response(resource: Mapper.Map<User, UserModel>(user))
                    : new Response(resource: null);
            }
        }
    }
}

using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands
{
    // TODO: Add docs comments
    public sealed class Create
    {
        public sealed class Command : IRequest<Response>
        {
            public Guid Id { get; set; }
            public string FirstName { get; set; } = null!;
            public string Surname { get; set; } = null!;
            public string AchievedLevel { get; set; } = null!;
            public int MaxWeeklyWorkHours { get; set; }
        }

        public sealed class Response
        {
            public Response(UserModel resource)
            {
                Resource = resource;
            }

            public UserModel Resource { get; }
        }

        public sealed class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id)
                    .NotEmpty();

                RuleFor(x => x.FirstName)
                    .MaximumLength(50)
                    .NotEmpty();

                RuleFor(x => x.Surname)
                    .MaximumLength(50)
                    .NotEmpty();

                RuleFor(x => x.AchievedLevel)
                    .IsEnumName(typeof(Level))
                    .NotEmpty();

                RuleFor(x => x.MaxWeeklyWorkHours)
                    .GreaterThanOrEqualTo(0)
                    .LessThanOrEqualTo(48);
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
                var item = new User(id: request.Id,
                                    firstName: request.FirstName,
                                    surname: request.Surname,
                                    achievedLevel: Enum.Parse<Level>(request.AchievedLevel),
                                    maxWeeklyWorkHours: request.MaxWeeklyWorkHours);

                var user = await Repository.AddItemAsync(item: item,
                                                         cancellationToken: cancellationToken);

                var currentUserRoleId = await IdentityProvider.GetUserRoleIdAsync(userId: user.Id, cancellationToken: cancellationToken);
                if (currentUserRoleId is null)
                {
                    var userRoleId = await IdentityProvider.GetRoleIdAsync(role: ApplicationRole.User, cancellationToken: cancellationToken);
                    await IdentityProvider.AddToRoleAsync(userId: user.Id, roleId: userRoleId, cancellationToken: cancellationToken);
                }

                return new Response(resource: Mapper.Map<User, UserModel>(user));
            }
        }
    }
}

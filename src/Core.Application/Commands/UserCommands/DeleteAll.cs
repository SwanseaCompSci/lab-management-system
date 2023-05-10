using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands
{
    // TODO: Add docs comments
    public sealed class DeleteAll
    {
        public sealed class Command : IRequest<Response> { }

        public sealed class Response
        {
            public Response(IEnumerable<UserModel>? resource)
            {
                Resource = resource;
            }

            public IEnumerable<UserModel>? Resource { get; }
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
            public CommandHandler(IUserRepository repository,
                                  IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private IUserRepository Repository { get; }
            private IMapper Mapper { get; }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var users = await Repository.DeleteAllAsync(cancellationToken);

                return users.Any()
                    ? new Response(resource: users.Select(x => Mapper.Map<User, UserModel>(x)))
                    : new Response(resource: null);
            }
        }
    }
}

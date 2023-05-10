using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserLabModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserLabCommands
{
    // TODO: Add docs comments
    public sealed class Create
    {
        public sealed class Command : IRequest<Response>
        {
            public Command(Guid userId, Guid labId)
            {
                UserId = userId;
                LabId = labId;
            }

            public Guid UserId { get; }
            public Guid LabId { get; }
        }

        public sealed class Response
        {
            public Response(UserLabModel resource)
            {
                Resource = resource;
            }

            public UserLabModel Resource { get; }
        }

        public sealed class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.UserId).NotEmpty();
                RuleFor(x => x.LabId).NotEmpty();
            }
        }

        internal sealed class CommandHandler : IRequestHandler<Command, Response>
        {
            public CommandHandler(IUserLabRepository repository,
                                  IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            public IUserLabRepository Repository { get; }
            public IMapper Mapper { get; }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var userLab = await Repository.AddItemAsync(item: new UserLab(userId: request.UserId,
                                                                              labId: request.LabId),
                                                            cancellationToken: cancellationToken);

                return new Response(resource: Mapper.Map<UserLab, UserLabModel>(userLab));
            }
        }
    }
}

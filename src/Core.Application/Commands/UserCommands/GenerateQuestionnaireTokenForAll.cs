using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands
{
    // TODO: Add docs comments
    public sealed class GenerateQuestionnaireTokenForAll
    {
        public sealed class Command : IRequest<Response> { }

        public sealed class Response
        {
            public Response(IEnumerable<UserModel> resource)
            {
                Resource = resource;
            }

            public IEnumerable<UserModel> Resource { get; }
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
                var specification = new GetAllUsersSpecification();

                var entities = Repository.GetItems(specification: specification);

                foreach (var item in entities)
                {
                    item.QuestionnaireToken = Guid.NewGuid();
                }

                var result = await Repository.UpdateRangeAsync(entities, cancellationToken);

                return new Response(resource: result.Select(x => Mapper.Map<User, UserModel>(x)));
            }
        }
    }
}

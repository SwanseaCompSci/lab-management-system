using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserQueries
{
    // TODO: Add docs comments
    public sealed class GetAllWithQuestionnaireToken
    {
        public sealed class Query : IRequest<Response> { }

        public sealed class Response
        {
            public Response(IEnumerable<UserModel> resource)
            {
                Resource = resource;
            }

            public IEnumerable<UserModel> Resource { get; }
        }

        public sealed class QueryValidator : AbstractValidator<Query>
        {
            // There is nothing to validate
        }

        internal sealed class QueryHandler : IRequestHandler<Query, Response>
        {
            public QueryHandler(IUserRepository repository,
                                IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private IUserRepository Repository { get; }
            private IMapper Mapper { get; }

            public Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var specification = new GetAllUsersWithQuestionnaireTokenSpecification();

                var users = Repository.GetItems(specification);

                var response = new Response(resource: users.Select(x => Mapper.Map<User, UserModel>(x)));

                return Task.FromResult(response);
            }
        }
    }
}

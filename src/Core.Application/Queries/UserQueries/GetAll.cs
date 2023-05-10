using Ardalis.Specification;
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
    public sealed class GetAll
    {
        public sealed class Query : IRequest<Response> { }

        public sealed class Response
        {
            public IEnumerable<UserModel> Resource { get; set; } = null!;
        }

        public sealed class QueryValidator : AbstractValidator<Query> { }

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
                var specification = new GetAllUsersSpecification();

                var entities = Repository.GetItems(specification: specification);

                var response = new Response()
                {
                    Resource = entities.Select(x => Mapper.Map<User, UserModel>(x))
                };

                return Task.FromResult(response);
            }
        }
    }
}

using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserLabSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Queries.LabQueries
{
    // TODO: Add docs comments
    public sealed class GetAllWhereUser
    {
        public sealed class Query : IRequest<Response>
        {
            public Query(Guid userId)
            {
                UserId = userId;
            }

            public Guid UserId { get; }
        }

        public sealed class Response
        {
            public Response(IEnumerable<LabModel> resource)
            {
                Resource = resource;
            }

            public IEnumerable<LabModel> Resource { get; }
        }

        public sealed class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.UserId).NotEmpty();
            }
        }

        internal sealed class QueryHandler : IRequestHandler<Query, Response>
        {
            public QueryHandler(IUserLabRepository repository,
                                IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private IUserLabRepository Repository { get; }
            private IMapper Mapper { get; }

            public Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var specification = new GetUserLabsWhereUserSpecification(userId: request.UserId);

                var entities = Repository.GetItems(specification: specification).Select(x => x.Lab);

                var response = new Response(resource: entities.Select(x => Mapper.Map<Lab, LabModel>(x)));

                return Task.FromResult(response);
            }
        }
    }
}

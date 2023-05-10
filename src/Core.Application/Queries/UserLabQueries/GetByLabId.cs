using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserLabModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserLabSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserLabQueries
{
    // TODO: Add docs comments
    public sealed class GetByLabId
    {
        public sealed class Query : IRequest<Response>
        {
            public Query(Guid labId)
            {
                LabId = labId;
            }

            public Guid LabId { get; }
        }

        public sealed class Response
        {
            public Response(IEnumerable<UserLabModel> resource)
            {
                Resource = resource;
            }

            public IEnumerable<UserLabModel> Resource { get; }
        }

        public sealed class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.LabId).NotEmpty();
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
                var specification = new GetUserLabsWhereLabSpecification(labId: request.LabId);

                var userLabs = Repository.GetItems(specification: specification);

                var resource = userLabs.Select(x => Mapper.Map<UserLab, UserLabModel>(x));

                return Task.FromResult(new Response(resource));
            }
        }
    }
}

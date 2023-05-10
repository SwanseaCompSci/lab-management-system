using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.AllocationModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.AllocationSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Queries.AllocationQueries
{
    public sealed class GetLabAllocations
    {
        public sealed class Query : IRequest<Response> { }

        public sealed class Response
        {
            public Response(IEnumerable<AllocationResultModel> resource)
            {
                Resource = resource;
            }

            public IEnumerable<AllocationResultModel> Resource { get; }
        }

        public sealed class QueryValidator : AbstractValidator<Query> { }

        internal sealed class QueryHandler : IRequestHandler<Query, Response>
        {
            public QueryHandler(ILabRepository repository,
                                IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private ILabRepository Repository { get; }
            private IMapper Mapper { get; }

            public Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var specification = new GetAllAllocationResultsSpecification();

                var entities = Repository.GetItems(specification);

                var response = new Response(resource: entities.Select(x => Mapper.Map<Lab, AllocationResultModel>(x)));

                return Task.FromResult(response);
            }
        }
    }
}

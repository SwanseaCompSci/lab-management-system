using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModulePreferenceModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.ModulePreferenceSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModulePreferenceQueries
{
    // TODO: Add docs comments
    public sealed class GetAll
    {
        public sealed class Query : IRequest<Response> { }

        public sealed class Response
        {
            public Response(IEnumerable<ModulePreferenceDetailModel> resource)
            {
                Resource = resource;
            }

            public IEnumerable<ModulePreferenceDetailModel> Resource { get; }
        }

        public sealed class QueryValidator : AbstractValidator<Query> { }

        internal sealed class QueryHandler : IRequestHandler<Query, Response>
        {
            public QueryHandler(IModulePreferenceRepository repository,
                IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private IModulePreferenceRepository Repository { get; }
            private IMapper Mapper { get; }

            public Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var specification = new GetAllModulePreferenceDetailsSpecification();

                var entities = Repository.GetItems(specification);

                var response = new Response(resource: entities.Select(x => Mapper.Map<ModulePreference, ModulePreferenceDetailModel>(x)));

                return Task.FromResult(response);
            }
        }
    }
}

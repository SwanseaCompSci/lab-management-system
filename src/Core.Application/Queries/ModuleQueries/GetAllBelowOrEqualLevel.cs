using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.ModuleSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModuleQueries
{
    // TODO: Add docs comments

    public sealed class GetAllBelowOrEqualLevel
    {
        public sealed class Query : IRequest<Response>
        {
            public Query(string level)
            {
                Level = level;
            }

            public string Level { get; }
        }

        public sealed class Response
        {
            public Response(IEnumerable<ModuleModel> resource)
            {
                Resource = resource;
            }

            public IEnumerable<ModuleModel> Resource { get; }
        }

        public sealed class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Level)
                    .IsEnumName(typeof(Level))
                    .NotEmpty();
            }
        }

        internal sealed class QueryHandler : IRequestHandler<Query, Response>
        {
            public QueryHandler(IModuleRepository repository,
                                IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private IModuleRepository Repository { get; }
            private IMapper Mapper { get; }

            public Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var specification = new GetAllModulesSpecification();

                var requestLevel = Enum.Parse<Level>(request.Level);
                var entities = Repository
                    .GetItems(specification: specification)
                    .Where(x => x.Level <= requestLevel);

                var response = new Response(resource: entities.Select(x => Mapper.Map<Module, ModuleModel>(x)));

                return Task.FromResult(response);
            }
        }
    }
}

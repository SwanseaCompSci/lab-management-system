using Ardalis.Specification;
using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.ModuleSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModuleQueries
{
    // TODO: Add docs comments
    public sealed class GetAll
    {
        public sealed class Query : IRequest<Response> { }

        public sealed class Response
        {
            public IEnumerable<ModuleModel> Resource { get; set; } = null!;
        }

        public sealed class QueryValidator : AbstractValidator<Query> { }

        internal sealed class QueryHandler : IRequestHandler<Query, Response>
        {
            public QueryHandler(ICurrentUserService currentUserService,
                                IModuleRepository repository,
                                IMapper mapper)
            {
                CurrentUserService = currentUserService;
                Repository = repository;
                Mapper = mapper;
            }

            private ICurrentUserService CurrentUserService { get; }
            private IModuleRepository Repository { get; }
            private IMapper Mapper { get; }

            public Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                Specification<Module> specification = CurrentUserService.UserApplicationRole == ApplicationRole.Administrator
                    ? new GetAllModulesSpecification()
                    : new GetAllModulesWherePermissionSpecification(userId: CurrentUserService.UserId!.Value);

                var entities = Repository.GetItems(specification: specification);

                var response = new Response
                {
                    Resource = entities.Select(x => Mapper.Map<Module, ModuleModel>(x))
                };

                return Task.FromResult(response);
            }
        }
    }
}

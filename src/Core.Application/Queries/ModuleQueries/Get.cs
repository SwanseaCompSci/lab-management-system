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
    public sealed class Get
    {
        public sealed class Query : IRequest<Response>
        {
            public Query(Guid moduleId)
            {
                ModuleId = moduleId;
            }

            public Guid ModuleId { get; }
        }

        public sealed class Response
        {
            public Response(ModuleModel? resource)
            {
                Resource = resource;
            }

            public ModuleModel? Resource { get; set; } = null!;
        }

        public sealed class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.ModuleId).NotEmpty();
            }
        }

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

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                ISingleResultSpecification<Module> specification = CurrentUserService.UserApplicationRole == ApplicationRole.Administrator
                    ? new GetModuleSpecification(moduleId: request.ModuleId)
                    : new GetModuleWherePermissionSpecification(moduleId: request.ModuleId,
                                                                userId: CurrentUserService.UserId!.Value);

                var module = await Repository.GetItemAsync(specification: specification,
                                                           cancellationToken: cancellationToken);

                return module is not null
                    ? new Response(resource: Mapper.Map<Module, ModuleModel>(module))
                    : new Response(resource: null);
            }
        }
    }
}

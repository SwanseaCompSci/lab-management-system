using Ardalis.Specification;
using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.LabSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Queries.LabQueries
{
    // TODO: Add docs comments
    public sealed class GetDetail
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
            public Response(LabDetailModel? resource)
            {
                Resource = resource;
            }

            public LabDetailModel? Resource { get; set; } = null!;
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
            public QueryHandler(ICurrentUserService currentUserService,
                                ILabRepository repository,
                                IMapper mapper)
            {
                CurrentUserService = currentUserService;
                Repository = repository;
                Mapper = mapper;
            }

            private ICurrentUserService CurrentUserService { get; }
            private ILabRepository Repository { get; }
            private IMapper Mapper { get; }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                ISingleResultSpecification<Lab> specification = CurrentUserService.UserApplicationRole == ApplicationRole.Administrator
                    ? new GetLabDetailSpecification(labId: request.LabId)
                    : new GetLabDetailWherePermissionSpecification(labId: request.LabId, userId: CurrentUserService.UserId!.Value);

                var lab = await Repository.GetItemAsync(specification: specification,
                                                        cancellationToken: cancellationToken);

                return lab is not null
                    ? new Response(resource: Mapper.Map<Lab, LabDetailModel>(lab))
                    : new Response(resource: null);
            }
        }
    }
}

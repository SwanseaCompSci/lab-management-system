using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserModuleSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserModuleQueries
{
    // TODO: Add docs comments
    public sealed class GetDetail
    {
        public sealed class Query : IRequest<Response>
        {
            public Query(Guid userId, Guid moduleId)
            {
                UserId = userId;
                ModuleId = moduleId;
            }

            public Guid UserId { get; }
            public Guid ModuleId { get; }
        }

        public sealed class Response
        {
            public Response(UserModuleDetailModel? resource)
            {
                Resource = resource;
            }

            public UserModuleDetailModel? Resource { get; set; } = null!;
        }

        public sealed class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.UserId).NotEmpty();
                RuleFor(x => x.ModuleId).NotEmpty();
            }
        }

        internal sealed class QueryHandler : IRequestHandler<Query, Response>
        {
            public QueryHandler(IUserModuleRepository repository,
                                IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private IUserModuleRepository Repository { get; }
            private IMapper Mapper { get; }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var specification = new GetUserModuleDetailSpecification(userId: request.UserId,
                                                                         moduleId: request.ModuleId);

                var userModule = await Repository.GetItemAsync(specification: specification,
                                                               cancellationToken: cancellationToken);

                return userModule is not null
                    ? new Response(resource: Mapper.Map<UserModule, UserModuleDetailModel>(userModule))
                    : new Response(resource: null);
            }
        }
    }
}

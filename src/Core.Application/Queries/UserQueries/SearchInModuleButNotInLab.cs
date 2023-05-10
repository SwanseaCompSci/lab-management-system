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

    public sealed class SearchInModuleButNotInLab
    {
        public sealed class Query : IRequest<Response>
        {
            public Query(Guid moduleId, Guid labId, string searchExpression)
            {
                ModuleId = moduleId;
                LabId = labId;
                SearchExpression = searchExpression;
            }

            public Guid ModuleId { get; }
            public Guid LabId { get; }
            public string SearchExpression { get; }
        }

        public sealed class Response
        {
            public Response(IEnumerable<UserModel> resource)
            {
                Resource = resource;
            }

            public IEnumerable<UserModel> Resource { get; }
        }

        public sealed class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.ModuleId)
                    .NotEmpty();

                RuleFor(x => x.LabId)
                    .NotEmpty();

                RuleFor(x => x.SearchExpression)
                    .MaximumLength(120)
                    .NotEmpty();
            }
        }

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
                var specification = new SearchForUsersInModuleButNotInLabSpecification(moduleId: request.ModuleId,
                                                                                       labId: request.LabId,
                                                                                       searchExpression: request.SearchExpression);

                var users = Repository.GetItems(specification: specification);

                var resource = users.Select(x => Mapper.Map<User, UserModel>(x));

                return Task.FromResult(new Response(resource));
            }
        }
    }
}

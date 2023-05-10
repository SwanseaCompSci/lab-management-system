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

    public sealed class Get
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
            public Response(UserModel? resource)
            {
                Resource = resource;
            }

            public UserModel? Resource { get; set; } = null!;
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
            public QueryHandler(IUserRepository repository,
                                IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private IUserRepository Repository { get; }
            private IMapper Mapper { get; }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var specification = new GetUserSpecification(request.UserId);

                var user = await Repository.GetItemAsync(specification: specification,
                                                         cancellationToken: cancellationToken);

                return user is not null
                    ? new Response(resource: Mapper.Map<User, UserModel>(user))
                    : new Response(resource: null);
            }
        }
    }
}

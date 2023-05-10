using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabScheduleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.LabScheduleSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Queries.LabScheduleQueries
{
    // TODO: Add docs comments
    public sealed class Get
    {
        public sealed class Query : IRequest<Response>
        {
            public Query(Guid labScheduleId)
            {
                LabScheduleId = labScheduleId;
            }

            public Guid LabScheduleId { get; }
        }

        public sealed class Response
        {
            public Response(LabScheduleModel? resource)
            {
                Resource = resource;
            }

            public LabScheduleModel? Resource { get; }
        }

        public sealed class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.LabScheduleId).NotEmpty();
            }
        }

        internal sealed class QueryHandler : IRequestHandler<Query, Response>
        {
            public QueryHandler(ILabScheduleRepository repository,
                                IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private ILabScheduleRepository Repository { get; }
            private IMapper Mapper { get; }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var specification = new GetLabScheduleSpecification(labScheduleId: request.LabScheduleId);

                var labSchedule = await Repository.GetItemAsync(specification: specification,
                                                                cancellationToken: cancellationToken);

                return labSchedule is not null
                    ? new Response(resource: Mapper.Map<LabSchedule, LabScheduleModel>(labSchedule))
                    : new Response(resource: null);
            }
        }
    }
}

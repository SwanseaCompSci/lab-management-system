using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.DashboardModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.DashboardSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Queries.DashboardQueries
{
    // TODO: Add docs comments
    public sealed class GetFutureWorkShifts
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
            public Response(IEnumerable<WorkShiftModel> resource)
            {
                Resource = resource;
            }

            public IEnumerable<WorkShiftModel> Resource { get; }
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
            public QueryHandler(IUserLabScheduleRepository repository,
                                IDateTimeService dateTimeService,
                                IMapper mapper)
            {
                Repository = repository;
                DateTimeService = dateTimeService;
                Mapper = mapper;
            }

            private IUserLabScheduleRepository Repository { get; }
            private IDateTimeService DateTimeService { get; }
            private IMapper Mapper { get; }

            public Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var specification = new GetWorkShiftsFromDateTimeSpecifications(userId: request.UserId, dateTime: DateTimeService.UtcNow);

                var userLabSchedules = Repository.GetItems(specification: specification);

                var response = new Response(resource: userLabSchedules.Select(x => Mapper.Map<UserLabSchedule, WorkShiftModel>(x)));

                return Task.FromResult(response);
            }
        }
    }
}

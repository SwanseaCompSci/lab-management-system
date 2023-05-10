using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabScheduleModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabScheduleCommands
{
    // TODO: Add docs comments
    public sealed class CreateRange
    {
        public sealed class Command : IRequest<Response>
        {
            public Guid LabId { get; set; }

            public DateTime? StartDate { get; set; }
            public int NumberOfOccurrences { get; set; } = 1;

            public TimeSpan? Start { get; set; }
            public TimeSpan? End { get; set; }
        }

        public sealed class Response
        {
            public Response(IEnumerable<LabScheduleModel> resource)
            {
                Resource = resource;
            }

            public IEnumerable<LabScheduleModel> Resource { get; }
        }

        public sealed class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IDateTimeService dateTimeService)
            {
                RuleFor(x => x.LabId)
                    .NotEmpty();

                RuleFor(x => x.StartDate)
                    .GreaterThanOrEqualTo(dateTimeService.Today)
                    .NotEmpty();

                RuleFor(x => x.NumberOfOccurrences)
                    .GreaterThanOrEqualTo(1);

                RuleFor(x => x.Start)
                    .NotEmpty();

                RuleFor(x => x.End)
                    .NotEmpty();
                RuleFor(x => x.End)
                    .GreaterThan(x => x.Start)
                    .When(x => x.Start is not null);
            }
        }

        internal sealed class CommandHandler : IRequestHandler<Command, Response>
        {
            public CommandHandler(ILabScheduleRepository repository,
                                  IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private ILabScheduleRepository Repository { get; }
            private IMapper Mapper { get; }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var firstStartDateTime = request.StartDate!.Value.Add(request.Start!.Value);
                var firstEndDateTime = request.StartDate!.Value.Add(request.End!.Value);

                var labSchedules = new List<LabSchedule>(capacity: request.NumberOfOccurrences);

                for (int i = 0; i < request.NumberOfOccurrences; i++)
                {
                    labSchedules.Add(new LabSchedule(labId: request.LabId,
                                                     start: firstStartDateTime.AddDays(i * 7),
                                                     end: firstEndDateTime.AddDays(i * 7)));
                }

                var entities = await Repository.AddRangeAsync(items: labSchedules,
                                                              cancellationToken: cancellationToken);
                var resource = entities.Select(x => Mapper.Map<LabSchedule, LabScheduleModel>(x));

                return new Response(resource);
            }
        }
    }
}

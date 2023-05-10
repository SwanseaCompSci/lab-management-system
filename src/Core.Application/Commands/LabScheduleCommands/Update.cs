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
    public sealed class Update
    {
        public sealed class Command : IRequest<Response>
        {
            public Guid Id { get; set; }
            public Guid LabId { get; set; }

            public DateTime? Date { get; set; }

            public TimeSpan? Start { get; set; }
            public TimeSpan? End { get; set; }
        }

        public sealed class Response
        {
            public Response(LabScheduleModel resource)
            {
                Resource = resource;
            }

            public LabScheduleModel Resource { get; }
        }

        public sealed class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(IDateTimeService dateTimeService)
            {
                RuleFor(x => x.Id)
                    .NotEmpty();

                RuleFor(x => x.LabId)
                    .NotEmpty();

                RuleFor(x => x.Date)
                    .GreaterThanOrEqualTo(dateTimeService.Today)
                    .NotEmpty();

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
                var startDateTime = request.Date!.Value.Add(request.Start!.Value);
                var endDateTime = request.Date!.Value.Add(request.End!.Value);

                var labSchedule = await Repository.UpdateItemAsync(id: request.Id,
                                                                   item: new LabSchedule(labId: request.LabId,
                                                                                         start: startDateTime,
                                                                                         end: endDateTime),
                                                                   cancellationToken: cancellationToken);

                var resource = Mapper.Map<LabSchedule, LabScheduleModel>(labSchedule);

                return new Response(resource: resource);
            }
        }
    }
}

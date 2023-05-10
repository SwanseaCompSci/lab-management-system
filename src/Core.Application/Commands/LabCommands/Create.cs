using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabCommands
{
    // TODO: Add docs comments
    public sealed class Create
    {
        public sealed class Command : IRequest<Response>
        {
            public Guid ModuleId { get; set; }
            public string Name { get; set; } = null!;
            public string Day { get; set; } = null!;
            public TimeSpan? StartTime { get; set; }
            public TimeSpan? EndTime { get; set; }
            public int MinNumberOfStaff { get; set; } = 1;
            public int MaxNumberOfStaff { get; set; } = 1;
        }

        public sealed class Response
        {
            public Response(LabModel resource)
            {
                Resource = resource;
            }

            public LabModel Resource { get; }
        }

        public sealed class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ModuleId)
                    .NotEmpty();

                RuleFor(x => x.Name)
                    .MaximumLength(50)
                    .NotEmpty();

                RuleFor(x => x.Day)
                    .IsEnumName(typeof(WorkDayOfWeek))
                    .NotEmpty();

                RuleFor(x => x.StartTime)
                    .NotEmpty();

                RuleFor(x => x.EndTime)
                    .GreaterThan(x => x.StartTime)
                    .NotEmpty();

                RuleFor(x => x.MinNumberOfStaff)
                    .GreaterThanOrEqualTo(1)
                    .NotEmpty();

                RuleFor(x => x.MaxNumberOfStaff)
                    .GreaterThanOrEqualTo(x => x.MinNumberOfStaff)
                    .NotEmpty();
            }
        }

        internal sealed class CommandHandler : IRequestHandler<Command, Response>
        {
            public CommandHandler(ILabRepository repository,
                                  IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private ILabRepository Repository { get; }
            private IMapper Mapper { get; }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await Repository.AddItemAsync(
                    item: new Lab(moduleId: request.ModuleId,
                                  name: request.Name,
                                  day: Enum.Parse<WorkDayOfWeek>(request.Day),
                                  startTime: TimeOnly.FromTimeSpan(request.StartTime!.Value),
                                  endTime: TimeOnly.FromTimeSpan(request.EndTime!.Value),
                                  minNumberOfStaff: request.MinNumberOfStaff,
                                  maxNumberOfStaff: request.MaxNumberOfStaff),
                    cancellationToken: cancellationToken);

                var labModel = Mapper.Map<Lab, LabModel>(entity);

                return new Response(resource: labModel);
            }
        }
    }
}

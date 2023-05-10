using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModulePreferenceModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModulePreferenceCommands
{
    // TODO: Add docs comments
    public sealed class Update
    {
        public sealed class Command : IRequest<Response>
        {
            public Command(Guid userId, Guid moduleId, string status)
            {
                UserId = userId;
                ModuleId = moduleId;
                Status = status;
            }

            public Guid UserId { get; }
            public Guid ModuleId { get; }
            public string Status { get; }
        }

        public sealed class Response
        {
            public Response(ModulePreferenceModel resource)
            {
                Resource = resource;
            }

            public ModulePreferenceModel Resource { get; }
        }

        public sealed class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.UserId)
                    .NotEmpty();

                RuleFor(x => x.ModuleId)
                    .NotEmpty();

                RuleFor(x => x.Status)
                    .IsEnumName(typeof(Status))
                    .NotEmpty();
            }
        }

        internal sealed class CommandHandler : IRequestHandler<Command, Response>
        {
            public CommandHandler(IModulePreferenceRepository repository,
                                  IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private IModulePreferenceRepository Repository { get; }
            private IMapper Mapper { get; }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var modulePreference = await Repository.UpdateItemAsync(userId: request.UserId,
                                                                        moduleId: request.ModuleId,
                                                                        item: new ModulePreference(userId: request.UserId, moduleId: request.ModuleId)
                                                                        {
                                                                            Status = Enum.Parse<Status>(request.Status),
                                                                        },
                                                                        cancellationToken: cancellationToken);

                var model = Mapper.Map<ModulePreference, ModulePreferenceModel>(modulePreference);

                return new Response(resource: model);
            }
        }
    }
}

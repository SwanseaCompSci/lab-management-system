using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModulePreferenceModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModulePreferenceCommands
{
    // TODO: Add docs comments
    public sealed class DeleteRange
    {
        public sealed class Command : IRequest<Response>
        {
            public Command(Guid userId)
            {
                UserId = userId;
            }

            public Guid UserId { get; }
        }

        public sealed class Response
        {
            public Response(IEnumerable<ModulePreferenceModel>? resource)
            {
                Resource = resource;
            }

            public IEnumerable<ModulePreferenceModel>? Resource { get; }
        }

        public sealed class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.UserId).NotEmpty();
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
                var modulePreferences = await Repository.DeleteRangeByUserIdAsync(userId: request.UserId,
                                                                                  cancellationToken: cancellationToken);

                return modulePreferences.Any()
                    ? new Response(resource: modulePreferences.Select(x => Mapper.Map<ModulePreference, ModulePreferenceModel>(x)))
                    : new Response(resource: null);
            }
        }
    }
}

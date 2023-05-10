using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Algorithms;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Common.Interfaces;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Models;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Events.UserLabEvents;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Commands
{
    // TODO: Add docs comments
    public sealed class Allocate
    {
        public sealed class Command : IRequest<Response>
        {
            public Command(string algorithm)
            {
                Algorithm = algorithm;
            }

            public string Algorithm { get; }
        }

        public sealed class Response
        {
            public Response(IEnumerable<AllocationModel> resource)
            {
                Resource = resource;
            }

            public IEnumerable<AllocationModel> Resource { get; }
        }

        public sealed class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Algorithm)
                    .IsEnumName(typeof(Algorithm))
                    .NotEmpty();
            }
        }

        internal sealed class CommandHandler : IRequestHandler<Command, Response>
        {
            public CommandHandler(IApplicationDbContext dbContext, IMapper mapper)
            {
                DbContext = dbContext;
                Mapper = mapper;
            }

            private IApplicationDbContext DbContext { get; }
            private IMapper Mapper { get; }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var userModels = DbContext.Users
                    .Where(x => x.QuestionnaireToken == null) // Filter users who filled in the questionnaire
                    .Include(x => x.TimeAvailabilities)
                    .Include(x => x.ModulePreferences)
                    .Select(x => Mapper.Map<User, UserModel>(x))
                    .ToList().AsReadOnly();

                var labModels = DbContext.Labs
                    .Include(x => x.Module)
                    .OrderByDescending(x => x.Module.Level)
                    .Select(x => Mapper.Map<Lab, LabModel>(x))
                    .ToList().AsReadOnly();

                var allocationModels = DbContext.UserLabs
                    .Include(x => x.Lab)
                    .Select(x => new AllocationModel(x.UserId, x.Lab.ModuleId, x.LabId))
                    .ToList().AsReadOnly();

                var algorithm = Enum.Parse<Algorithm>(request.Algorithm);

                var result = GetAllocator(algorithm).Allocate(users: userModels, labs: labModels, allocations: allocationModels);

                var trackedUserModules = new List<UserModule>();
                var trackedUserLabs = new List<UserLab>();

                foreach (var allocation in result)
                {
                    // Add User to Module
                    if ((DbContext.UserModules.Any(x => x.UserId == allocation.UserId && x.ModuleId == allocation.ModuleId)
                         || trackedUserModules.Any(x => x.UserId == allocation.UserId && x.ModuleId == allocation.ModuleId)) == false)
                    {
                        var userModule = new UserModule(userId: allocation.UserId, moduleId: allocation.ModuleId, role: ModuleRole.TeachingAssistant);

                        DbContext.UserModules.Add(userModule);
                        trackedUserModules.Add(userModule);
                    }

                    // Add User to Lab
                    if ((DbContext.UserLabs.Any(x => x.UserId == allocation.UserId && x.LabId == allocation.LabId)
                         || trackedUserLabs.Any(x => x.UserId == allocation.UserId && x.LabId == allocation.LabId)) == false)
                    {
                        var userLab = new UserLab(userId: allocation.UserId, labId: allocation.LabId);
                        userLab.DomainEvents.Add(new UserAddedToLabDomainEvent(userId: allocation.UserId, labId: allocation.LabId));

                        DbContext.UserLabs.Add(userLab);
                        trackedUserLabs.Add(userLab);
                    }
                }

                await DbContext.SaveChangesAsync(cancellationToken);

                return new Response(resource: result);
            }

            private static IAllocator GetAllocator(Algorithm algorithm)
            {
                return algorithm switch
                {
                    Algorithm.FirstMatch => new FirstMatch(),
                    Algorithm.Balanced => throw new NotImplementedException(),
                    _ => throw new NotImplementedException()
                };
            }
        }
    }
}

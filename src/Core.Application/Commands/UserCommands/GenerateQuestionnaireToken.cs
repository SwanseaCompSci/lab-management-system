﻿using AutoMapper;
using FluentValidation;
using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands
{
    // TODO: Add docs comments
    public sealed class GenerateQuestionnaireToken
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
            public Response(UserModel resource)
            {
                Resource = resource;
            }

            public UserModel Resource { get; }
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
            public CommandHandler(IUserRepository repository,
                                  IMapper mapper)
            {
                Repository = repository;
                Mapper = mapper;
            }

            private IUserRepository Repository { get; }
            private IMapper Mapper { get; }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await Repository.GetItemAsync(id: request.UserId,
                                                         cancellationToken: cancellationToken);

                if (user is null)
                {
                    throw new EntityNotFoundException(nameof(User), request.UserId.ToString());
                }

                user.QuestionnaireToken = Guid.NewGuid();

                user = await Repository.UpdateItemAsync(id: user.Id,
                                                        item: user,
                                                        cancellationToken: cancellationToken);

                return new Response(resource: Mapper.Map<User, UserModel>(user));
            }
        }
    }
}

using Ardalis.Specification.EntityFrameworkCore;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Specifications.UserSpecifications
{
    public sealed class TestsGetUserByQuestionnaireTokenSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entity_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("4bfc577b-6981-4c4f-b7ae-d6652b892fb9"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30)
                {
                    QuestionnaireToken = Guid.Parse("9bc22fe3-e502-4c7b-8729-47552898ee07"),
                },
                new User(id: Guid.Parse("cf02479d-0b5e-4510-8273-fbf146fcc5f7"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20),
            };
            await Testing.AddRangeAsync(entities: users);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetUserByQuestionnaireTokenSpecification(token: users[0].QuestionnaireToken!.Value);

            // Act
            var result = applicationDbContext.Users.WithSpecification(specification).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Id.Should().Be(users[0].Id);
        }

        [Test]
        public void Specified_Entity_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetUserByQuestionnaireTokenSpecification(token: Guid.Parse("ab67b18c-40cd-49b0-a112-6d74e660f7a5"));

            // Act
            var result = applicationDbContext.Users.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }
    }
}

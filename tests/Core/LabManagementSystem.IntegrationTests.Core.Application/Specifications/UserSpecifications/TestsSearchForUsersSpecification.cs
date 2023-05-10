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
    public sealed class TestsSearchForUsersSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entities_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("4bfc577b-6981-4c4f-b7ae-d6652b892fb9"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("cf02479d-0b5e-4510-8273-fbf146fcc5f7"), firstName: "Anna", surname: "Beck", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20),
                new User(id: Guid.Parse("ee470b65-df89-433a-a2e6-81b8be8a3c86"), firstName: "Jack", surname: "Wood", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30),

                new User(id: Guid.Parse("d6148930-d184-4d3f-9a60-ae5fd9e24325"), firstName: "Adam", surname: "Vega", achievedLevel: Level.Masters, maxWeeklyWorkHours: 40),
                new User(id: Guid.Parse("cc5d36af-c7f0-4d0b-b3a4-9d76075ac892"), firstName: "Josh", surname: "Hunt", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            await Testing.AddRangeAsync(entities: users);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new SearchForUsersSpecification(searchExpression: "e");

            // Act
            var result = applicationDbContext.Users.WithSpecification(specification).ToList();

            // Assert
            result.Should().HaveCount(3);
            result.Any(x => x.Id == users[0].Id).Should().BeTrue();
            result.Any(x => x.Id == users[1].Id).Should().BeTrue();
            result.Any(x => x.Id == users[3].Id).Should().BeTrue();
        }

        [Test]
        public void Specified_Entities_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new SearchForUsersSpecification(searchExpression: "Tomas");

            // Act
            var result = applicationDbContext.Users.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }
    }
}

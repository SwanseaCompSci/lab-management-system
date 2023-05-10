using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Queries.UserQueries
{
    public sealed class TestsGetQueryHandler : TestBase
    {
        [Test]
        public async Task Handle_Query_Entity_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("c890fcb6-176e-4ce2-9759-b62f0d98d700"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10);
            await Testing.AddAsync(entity: user);

            var query = new Get.Query(userId: user.Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.Id.Should().Be(user.Id);
            response.Resource.FirstName.Should().Be(user.FirstName);
            response.Resource.Surname.Should().Be(user.Surname);
            response.Resource.AchievedLevel.Should().Be(user.AchievedLevel.ToString());
            response.Resource.MaxWeeklyWorkHours.Should().Be(user.MaxWeeklyWorkHours);
        }

        [Test]
        public async Task Handle_Query_Entity_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var query = new Get.Query(userId: Guid.NewGuid());

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeNull();
        }
    }
}

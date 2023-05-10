using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Queries.UserQueries
{
    public sealed class TestsGetByQuestionnaireTokenQueryHandler : TestBase
    {
        [Test]
        public async Task Handle_Query_Entity_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("4bfc577b-6981-4c4f-b7ae-d6652b892fb9"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("cf02479d-0b5e-4510-8273-fbf146fcc5f7"), firstName: "Anna", surname: "Beck", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20)
                {
                    QuestionnaireToken = Guid.Parse("f6adc7cd-3796-4f1f-986f-2b1409bf44f3"),
                },
                new User(id: Guid.Parse("ee470b65-df89-433a-a2e6-81b8be8a3c86"), firstName: "Jack", surname: "Wood", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30),
            };
            await Testing.AddRangeAsync(entities: users);

            var query = new GetByQuestionnaireToken.Query(token: users[1].QuestionnaireToken!.Value);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.Id.Should().Be(users[1].Id);
            response.Resource.FirstName.Should().Be(users[1].FirstName);
            response.Resource.Surname.Should().Be(users[1].Surname);
            response.Resource.AchievedLevel.Should().Be(users[1].AchievedLevel.ToString());
            response.Resource.MaxWeeklyWorkHours.Should().Be(users[1].MaxWeeklyWorkHours);
            response.Resource.QuestionnaireToken.Should().Be(users[1].QuestionnaireToken);
        }

        [Test]
        public async Task Handle_Query_Entity_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var query = new GetByQuestionnaireToken.Query(token: Guid.NewGuid());

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeNull();
        }
    }
}

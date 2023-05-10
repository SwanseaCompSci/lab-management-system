using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModuleQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Queries.ModuleQueries
{
    public sealed class TestsGetAllBelowOrEqualLevelQueryHandler : TestBase
    {
        [Test]
        public async Task Handle_Query_Entities_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),

                new Module(name: "Database Systems", code: "CS-250", level: Level.Year2),
                new Module(name: "Computer Graphics", code: "CS-255", level: Level.Year2),

                new Module(name: "Cryptography and IT Security", code: "CSC318", level: Level.Year3),
                new Module(name: "Advanced Object Oriented Programming", code: "CSC371", level: Level.Year3),
            };
            await Testing.AddRangeAsync(entities: modules);

            var query = new GetAllBelowOrEqualLevel.Query(level: Level.Year2.ToString());

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().NotBeEmpty();
            for (int i = 0; i < 4; i++)
            {
                response.Resource.Any(x => x.Id == modules[i].Id).Should().BeTrue();
            }
        }

        [Test]
        public async Task Handle_Query_Entities_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var modules = new List<Module>()
            {
                new Module(name: "Cryptography and IT Security", code: "CSC318", level: Level.Year3),
                new Module(name: "Advanced Object Oriented Programming", code: "CSC371", level: Level.Year3),
            };
            await Testing.AddRangeAsync(entities: modules);

            var query = new GetAllBelowOrEqualLevel.Query(level: Level.Year2.ToString());

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeEmpty();
        }
    }
}

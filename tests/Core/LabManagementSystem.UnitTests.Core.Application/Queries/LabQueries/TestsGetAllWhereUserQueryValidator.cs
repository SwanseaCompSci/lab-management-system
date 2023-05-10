using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.LabQueries;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Queries.LabQueries
{
    public sealed class TestsGetAllWhereUserQueryValidator
    {
        private GetAllWhereUser.QueryValidator Validator { get; } = new();

        [Test]
        public void Query_Valid()
        {
            // Arrange
            var query = new GetAllWhereUser.Query(userId: Guid.Parse("7aea0589-abda-4e72-bef0-a942b0834f09"));

            // Act & Assert
            Validator.Validate(query).IsValid.Should().BeTrue();
        }

        [Test]
        public void Query_Invalid()
        {
            // Arrange
            var query = new GetAllWhereUser.Query(userId: Guid.Empty);

            // Act
            var result = Validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should().HaveCount(1);
            result.Errors[0].ErrorMessage.Should().Be("'User Id' must not be empty.");
        }
    }
}

using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.DashboardQueries;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Queries.DashboardQueries
{
    public class TestsGetFutureWorkShiftsQueryValidator
    {
        private GetFutureWorkShifts.QueryValidator Validator { get; } = new();

        [Test]
        public void Query_Valid()
        {
            // Arrange
            var query = new GetFutureWorkShifts.Query(userId: Guid.Parse("74b5716f-e9db-4431-a85f-a52b04bff12b"));

            // Act & Assert
            Validator.Validate(query).IsValid.Should().BeTrue();
        }

        [Test]
        public void Query_Invalid()
        {
            // Arrange
            var query = new GetFutureWorkShifts.Query(userId: Guid.Empty);

            // Act
            var result = Validator.Validate(query);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Should().HaveCount(1);
            result.Errors[0].ErrorMessage.Should().Be("'User Id' must not be empty.");
        }
    }
}

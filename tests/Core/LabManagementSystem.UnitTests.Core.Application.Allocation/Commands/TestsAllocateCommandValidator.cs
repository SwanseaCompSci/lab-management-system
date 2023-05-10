using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Commands;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Allocation.Commands
{
    public class TestsAllocateCommandValidator
    {
        private Allocate.CommandValidator Validator { get; } = new();

        [Test]
        public void Command_Valid_FirstMatch()
        {
            // Arrange
            var command = new Allocate.Command(algorithm: "FirstMatch");

            // Act
            Validator.Validate(command).IsValid.Should().BeTrue();
        }

        [Test]
        public void Command_Valid_Balanced()
        {
            // Arrange
            var command = new Allocate.Command(algorithm: "Balanced");

            // Act
            Validator.Validate(command).IsValid.Should().BeTrue();
        }

        [Test]
        public void Command_Invalid()
        {
            // Arrange
            var command = new Allocate.Command(algorithm: string.Empty);

            // Act
            var result = Validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(2);
            result.Errors[0].ErrorMessage.Should().Be("'Algorithm' has a range of values which does not include ''.");
            result.Errors[1].ErrorMessage.Should().Be("'Algorithm' must not be empty.");
        }
    }
}

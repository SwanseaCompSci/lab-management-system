using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Commands.UserCommands
{
    public class TestsGenerateQuestionnaireTokenForAllCommandValidator
    {
        private GenerateQuestionnaireTokenForAll.CommandValidator Validator = new();

        [Test]
        public void Command_Valid()
        {
            var query = new GenerateQuestionnaireTokenForAll.Command();

            Validator.Validate(query).IsValid.Should().BeTrue();
        }
    }
}

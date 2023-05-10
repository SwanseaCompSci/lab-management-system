﻿using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.TimeAvailabilityCommands;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Commands.TimeAvailabilityCommands
{
    public class TestsDeleteRangeCommandValidator
    {
        private DeleteRange.CommandValidator Validator { get; } = new();

        [Test]
        public void Command_Valid()
        {
            var command = new DeleteRange.Command(userId: Guid.NewGuid());

            Validator.Validate(command).IsValid.Should().BeTrue();
        }

        [Test]
        public void Command_Invalid()
        {
            var command = new DeleteRange.Command(userId: Guid.Empty);

            var result = Validator.Validate(command);

            result.IsValid.Should().BeFalse();
            result.Errors[0].ErrorMessage.Should().Be("'User Id' must not be empty.");
        }
    }
}

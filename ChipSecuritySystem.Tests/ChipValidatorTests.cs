using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ChipSecuritySystem.Tests
{
    [TestClass]
    public class ChipValidatorTests
    {
        [TestMethod]
        public void NullChips_Throws()
        {
            var validator = new ChipValidator(Color.Blue, Color.Green);

            Action action = () => validator.Validate(null);

            action.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("value");
        }

        [TestMethod]
        public void StartAndEndColorsSame_Throws()
        {
            var validator = new ChipValidator(Color.Blue, Color.Blue);
            var chips = new List<ColorChip> {
                new ColorChip(Color.Blue, Color.Yellow),
                new ColorChip(Color.Red, Color.Green),
                new ColorChip(Color.Yellow, Color.Red),
                new ColorChip(Color.Orange, Color.Purple)
            };

            Action action = () => validator.Validate(chips);

            action.Should().ThrowExactly<ArgumentException>()
                .WithParameterName("value")
                .WithMessage("Starting and ending colors must be different*");
        }

        [TestMethod]
        public void MissingStartChip_Throws()
        {
            var validator = new ChipValidator(Color.Blue, Color.Green);
            var chips = new List<ColorChip> {
                new ColorChip(Color.Red, Color.Green),
                new ColorChip(Color.Yellow, Color.Red),
                new ColorChip(Color.Orange, Color.Purple)
            };

            Action action = () => validator.Validate(chips);

            action.Should().ThrowExactly<ChipValidationException>()
                .WithMessage("No chips contain starting color [Blue]");
        }

        [TestMethod]
        public void MissingEndChip_Throws()
        {
            var validator = new ChipValidator(Color.Blue, Color.Green);
            var chips = new List<ColorChip> {
                new ColorChip(Color.Blue, Color.Yellow),
                new ColorChip(Color.Yellow, Color.Red),
                new ColorChip(Color.Orange, Color.Purple)
            };

            Action action = () => validator.Validate(chips);

            action.Should().ThrowExactly<ChipValidationException>()
                .WithMessage("No chips contain ending color [Green]");
        }

        [TestMethod]
        public void NoValidMatches_Good()
        {
            var validator = new ChipValidator(Color.Blue, Color.Green);
            var chips = new List<ColorChip> {
                new ColorChip(Color.Blue, Color.Yellow),
                new ColorChip(Color.Red, Color.Green),
                new ColorChip(Color.Purple, Color.Blue),
                new ColorChip(Color.Yellow, Color.Orange),
            };

            Action action = () => validator.Validate(chips);

            action.Should().ThrowExactly<ChipValidationException>()
                .WithMessage("No matching commbination found");
        }

        [TestMethod]
        public void HasContinuousLoop_Good()
        {
            var validator = new ChipValidator(Color.Blue, Color.Green);
            var chips = new List<ColorChip> {
                new ColorChip(Color.Blue, Color.Yellow),
                new ColorChip(Color.Red, Color.Green),
                new ColorChip(Color.Yellow, Color.Orange),
                new ColorChip(Color.Orange, Color.Yellow)
            };

            Action action = () => validator.Validate(chips);

            action.Should().ThrowExactly<ChipValidationException>()
                .WithMessage("No matching commbination found");
        }

        [TestMethod]
        public void HasValidStartAndEnd_Good()
        {
            var validator = new ChipValidator(Color.Blue, Color.Green);
            var chips = new List<ColorChip> {
                new ColorChip(Color.Blue, Color.Yellow),
                new ColorChip(Color.Red, Color.Green),
                new ColorChip(Color.Yellow, Color.Red),
                new ColorChip(Color.Orange, Color.Purple)
            };
            var expected = new List<ColorChip> {
                new ColorChip(Color.Blue, Color.Yellow),
                new ColorChip(Color.Yellow, Color.Red),
                new ColorChip(Color.Red, Color.Green)
            };

            var actual = validator.Validate(chips);

            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void HasMultipleValidStartAndSingleEnd_Good()
        {
            var validator = new ChipValidator(Color.Blue, Color.Green);
            var chips = new List<ColorChip> {
                new ColorChip(Color.Blue, Color.Red),
                new ColorChip(Color.Yellow, Color.Orange),
                new ColorChip(Color.Red, Color.Purple),
                new ColorChip(Color.Orange, Color.Green),
                new ColorChip(Color.Blue, Color.Yellow)
            };

            var expected = new List<ColorChip> {
                new ColorChip(Color.Blue, Color.Yellow),
                new ColorChip(Color.Yellow, Color.Orange),
                new ColorChip(Color.Orange, Color.Green),
            };

            var actual = validator.Validate(chips);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}

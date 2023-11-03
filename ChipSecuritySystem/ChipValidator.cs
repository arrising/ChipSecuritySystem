using System;
using System.Collections.Generic;
using System.Linq;

namespace ChipSecuritySystem
{
    public class ChipValidator : IValidator<ColorChip>
    {
        public ChipValidator()
        {
            StartingColor = Color.Blue;
            EndingColor = Color.Green;
        }

        public ChipValidator(Color startingColor, Color endingColor)
        {
            StartingColor = startingColor;
            EndingColor = endingColor;
        }

        public Color StartingColor;
        public Color EndingColor;

        public IEnumerable<ColorChip> Validate(IEnumerable<ColorChip> value)
        {
            var valueList = value?.ToList();

            ValidateInputs(valueList);
            return ValidateSequence(valueList.ToList());
        }

        private void ValidateInputs(List<ColorChip> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (StartingColor == EndingColor)
            {
                throw new ArgumentException("Starting and ending colors must be different", nameof(value));
            }
            if (!value.Any(x => x.StartColor == StartingColor))
            {
                throw new ChipValidationException($"No chips contain starting color [{StartingColor}]");
            }
            if (!value.Any(x => x.EndColor == EndingColor))
            {
                throw new ChipValidationException($"No chips contain ending color [{EndingColor}]");
            }
        }

        private IEnumerable<ColorChip> ValidateSequence(List<ColorChip> value)
        {
            var searchResults = new List<List<ColorChip>>();
            var startingChips = GetAllChipsWithStartingColor(value, StartingColor);

            foreach (var chip in startingChips)
            {
                var sequence = GetSequence(chip, value, new List<ColorChip>());
                searchResults.Add(sequence);
            }

            //Find first sequence which ends with Ending Color
            var result = searchResults.FirstOrDefault(x => x.Last().EndColor == EndingColor);
            if (result == null)
            {
                throw new ChipValidationException("No matching commbination found");
            }

            return result;
        }

        private List<ColorChip> GetSequence(ColorChip chipUnderTest, List<ColorChip> sourceValues, List<ColorChip> result)
        {
            // If chipUnderTest already exists, a continuous loop exists, stop searching
            if (result.Contains(chipUnderTest))
            {
                return result;
            }

            result.Add(chipUnderTest);

            // If chip matches End Color, stop searching
            if (chipUnderTest.EndColor == EndingColor)
            {
                return result;
            }

            var matchingChips = GetAllChipsWithStartingColor(sourceValues, chipUnderTest.EndColor);
            if (matchingChips.Any())
            {
                foreach(var chip in matchingChips)
                {
                    return GetSequence(chip, sourceValues, result);
                }
            } 
            return result;
        }

        private IEnumerable<ColorChip> GetAllChipsWithStartingColor(List<ColorChip> sourceValues, Color color)
        {
            return sourceValues?.Where(x => x.StartColor == color) ?? Enumerable.Empty<ColorChip>();
        }
    }
}

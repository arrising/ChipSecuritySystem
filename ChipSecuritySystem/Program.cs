using System;
using System.Collections.Generic;
using System.Linq;

namespace ChipSecuritySystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Loading chips...");
            var startingColor = Color.Blue;
            var endingColor = Color.Green;
            var chips = new List<ColorChip> {
                new ColorChip(Color.Blue, Color.Yellow),
                new ColorChip(Color.Orange, Color.Green),
                new ColorChip(Color.Red, Color.Orange),
                new ColorChip(Color.Yellow, Color.Red),
                new ColorChip(Color.Orange, Color.Purple)
            };

            try
            {
                var validator = new ChipValidator(startingColor, endingColor);
                var result = validator.Validate(chips);
                var message = string.Join(", ", result.Select(x => $"[{x}]")) ;
                Console.WriteLine("Found matching combination");
                Console.WriteLine($"{startingColor} {message} {endingColor}");
            }
            catch (ChipValidationException)
            {
                // No match found
                Console.WriteLine(Constants.ErrorMessage);
            }
            catch (Exception exception)
            {
                // Other unknown exception occured
                Console.WriteLine(exception.Message);
            }
            finally
            {
                Console.WriteLine("Press a key to exit...");
                Console.ReadLine();
            }
        }
    }
}

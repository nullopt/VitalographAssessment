using System;
using System.Linq;

namespace Encryption.CaesarCipher
{
	public class Program
	{
		// total amount of characters in usable set (a-z = 26)
		private const int TOTAL_CHARACTERS = 26;
		// minimum ascii code (A = 65)
		private const int MINIMUM_CHAR = 65;
		// maximum ascii code (Z = 90)
		private const int MAXIMUM_CHAR = 90;

		public static void Main(string[] args)
		{
			// ask the user for an input string to encrypt
			var input = GetInputString();
			Console.WriteLine($"User Input:            {input}");

			// ask the user for a number to shift the cipher by
			var shiftAmount = GetShiftAmount();
			Console.WriteLine($"Shift Amount:          {shiftAmount}");

			// pass the upper version of input to CaesarShift and store into output
			var output = CaesarShift(input, shiftAmount);
			
			Console.WriteLine($"Caesar Shifted Output: {output}");
		}
		
		/// <summary>
		/// Continuously asks the user for an input string until the input string is valid.
		/// </summary>
		/// <returns>
		/// Capitalised user input string
		/// </returns>
		private static string GetInputString()
		{
			var inputString = string.Empty;

			// continuously loop to ask the user for a valid input string
			while (string.IsNullOrEmpty(inputString))
			{
				Console.Write("Input string to encrypt with the Caesar Cipher (a-z A-Z): ");
				inputString = Console.ReadLine();

				// validate that characters are valid (a-zA-Z)
				if (inputString.Any(c => (int)c < MINIMUM_CHAR || (int)c > MAXIMUM_CHAR))
				{
					Console.WriteLine("Only a-z and A-Z characters are supported.");
					inputString = string.Empty;
				}
			}
			
			// return the capitalized version
			return inputString.ToUpperInvariant();
		}
		
		private static int GetShiftAmount()
		{
			var shiftAmount = 0;
			
			// continuously loop to ask the user for a valid number to shift by
			while (shiftAmount != 0)
			{
				Console.Write("Amount of characters to shift (1-25): ");
				// try to parse the input 
				

				// validate that the number is between 1 and 25
				if (!int.TryParse(Console.ReadLine(), out shiftAmount) || shiftAmount < 1 || shiftAmount > 25)
				{
					Console.WriteLine("Number has to be between 1 and 25.");
					shiftAmount = 0;
				}
			}
			
			return shiftAmount;
		}

		/// <summary>
		/// Caesar Shifts an input string by the shift amount and returns the encrypted string
		/// </summary>
		/// <param name="input">
		///		input string which contains characters a-zA-Z
		/// </param>
		/// <param name="shiftAmount">
		///		number between 1 - 25 which is used to shift each character's ascii code
		/// </param>
		/// <returns></returns>
		private static string CaesarShift(string input, int shiftAmount)
		{
			if (string.IsNullOrEmpty(input))
			{
				throw new ArgumentNullException("input cannot be empty or null.");
			}

			var output = string.Empty;

			foreach (var character in input)
			{				
				output += (char)((((character + shiftAmount) - MINIMUM_CHAR) % TOTAL_CHARACTERS) + MINIMUM_CHAR);
			}
			
			return output;
		}
	}
}

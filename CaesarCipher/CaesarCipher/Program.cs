using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace CaesarCipher
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

			// ask the user for a number to shift the cipher by
			var shiftAmount = GetShiftAmount();

			// pass the upper version of input to CaesarShift and store into output
			var output = CaesarShift(input, shiftAmount);

			Console.WriteLine($"User Input:            {input}");
			Console.WriteLine($"Shift Amount:          {shiftAmount}");
			Console.WriteLine($"Caesar Shifted Output: {output}");

			// benchmarking code
			//Profiler("Improved CaesarShift: ", Test);
			//Console.WriteLine("\n");
			//Profiler("Old CaesarShift: ", Test2);
			//Console.ReadLine();
		}

		/// <summary>
		///		Continuously asks the user for an input string until the input string is valid.
		/// </summary>
		/// <returns>
		///		Capitalised user input string.
		/// </returns>
		private static string GetInputString(string input = null)
		{
			var inputString = input ?? string.Empty;

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
			return inputString.ToUpper();
		}

		/// <summary>
		///		Continuously asks the user for a number until the input is valid.
		/// </summary>
		/// <returns>
		///		The number to shift characters by.
		/// </returns>
		private static int GetShiftAmount(int amount = 0)
		{
			var shiftAmount = amount;

			// continuously loop to ask the user for a valid number to shift by
			while (shiftAmount == 0)
			{
				Console.Write("Amount of characters to shift (1-25): ");

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
		///		Input string which contains characters a-zA-Z.
		/// </param>
		/// <param name="shiftAmount">
		///		Number between 1 - 25 which is used to shift each character's ascii code.
		/// </param>
		/// <returns>
		///		Returns the encrypted string.
		/// </returns>
		private static string CaesarShift(string input, int shiftAmount)
		{
			// throw exception if input is null or empty
			if (string.IsNullOrEmpty(input))
			{
				throw new ArgumentNullException("input cannot be empty or null.");
			}

			var output = string.Empty;

			for (var i = 0; i < input.Length; i++)
			{
				// shift the char by the shift amount
				// (eg. Y + 7 = 96)
				var shifted = input[i] + shiftAmount;
				// get the amount of characters it overflows by
				// (eg. (96 - 65) % 26 = 5)
				var overflow = (shifted - MINIMUM_CHAR) % TOTAL_CHARACTERS;
				// add the overflow to the minimum ascii character to get the looped character
				// (eg. 5 + 65 = 70 -> F)
				var adjusted = overflow + MINIMUM_CHAR;

				// add it to the output
				output += (char)adjusted;
			}

			return output;
		}

		#region BenchMarking
		private static string OldCaesar(string input, int amount)
		{
			var output = "";
			if (input != null)
			{
				for (int i = 0; i < input.Length; i++)
				{
					if (input[i] < 65 || input[i] > 90)
					{
						throw new Exception("Only A-Z supported.");
					}
					int shifted = input[i] + amount;
					if (shifted > 90)
					{
						shifted = 65 + shifted - 91;
					}
					output = output + (char)shifted;
				}
			}

			return output;
		}

		private static void Test()
		{
			var input = "TESTABCXYZ";
			var amount = 6;
			var t = GetInputString(input);
			var y = GetShiftAmount(amount);

			var output = CaesarShift(t, y);
		}

		private static void Test2()
		{
			var input = "TESTABCXYZ";
			var shift = 6;
			var output = OldCaesar(input, shift);
		}

		private static double Profiler(string description, Action function, int iterations = 10000)
		{
			// set process and thread priority
			Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
			Thread.CurrentThread.Priority = ThreadPriority.Highest;

			// warm up function call
			function();

			var stopWatch = new Stopwatch();

			// garbage collection clean up
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();

			// start benchmarking
			stopWatch.Start();
			for (var i = 0; i < iterations; i++)
			{
				function();
			}

			// finish benchmarking
			stopWatch.Stop();
			Console.WriteLine(description);
			var average = (double)(stopWatch.Elapsed.TotalMilliseconds / iterations);
			Console.WriteLine($"Average Time Elapsed: {average} ms over {iterations} iterations");
			return stopWatch.Elapsed.TotalMilliseconds;
		}
		#endregion BenchMarking
	}
}

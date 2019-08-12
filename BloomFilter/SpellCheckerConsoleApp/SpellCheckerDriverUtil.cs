using BloomFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpellCheckerConsoleApp
{
	public class SpellCheckerDriverUtil
	{
		private static SpellChecker SpellCheckerWithBloomFilter = new SpellChecker();
		private static SpellChecker SpellCheckerWithoutBloomFilter = new SpellChecker(false);
		private static int NumberOfIterations = 10000;
		private static int RandomWordLength = 5;

		private static Random random = new Random();

		/// <summary>
		/// Generated Random string of length n containing alphabets A-Z
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}

		public static void Main(string[] args)
		{
			int numberOfFalsePositives = 0;
			for(int i =0;i< NumberOfIterations; i++)
			{
				string input = RandomString(RandomWordLength);
				bool bloomFilterOutput = SpellCheckerWithBloomFilter.isWordValid(input);
				bool hashSetOutput = SpellCheckerWithoutBloomFilter.isWordValid(input);
				if(bloomFilterOutput == true && hashSetOutput == false)
				{
					numberOfFalsePositives++;
				}
			}
			Console.WriteLine("Number of iterations: " + NumberOfIterations);
			Console.WriteLine("Number of False Positives: " + numberOfFalsePositives);
			Console.WriteLine("False positive rate: " + (double)((double)numberOfFalsePositives / (double)NumberOfIterations));
		}
		

	}
}

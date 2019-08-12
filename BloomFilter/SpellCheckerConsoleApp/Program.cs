using BloomFilter;
using System;

namespace SpellCheckerConsoleApp
{
	class Program
	{
		public static void Main(string[] args)
		{
			SpellChecker spellChecker = new SpellChecker();
			bool exit = false;
			while (!exit)
			{
				Console.WriteLine("Please enter word to check: ");
				string word = Console.ReadLine();
				if (spellChecker.isWordValid(word))
					Console.WriteLine("Given word " + word + " has correct spelling");
				else
					Console.WriteLine("Given word " + word + " does not have correct spelling");
				Console.WriteLine("Do you want to enter more words? y or n ");
				string key = Console.ReadLine();
				exit = !(key.ToLower() == "y" || key.ToLower() == "yes");
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilter
{
	public class SpellChecker
	{
		private BloomFilter.BloomFilter<string> BloomFilter;
		private HashSet<string> WordsInDictionary;
		private string wordListPath = "misc/wordlist.txt";
		private List<string> wordsList = new List<string>();
		private bool useBloomFilter;

		/// <summary>
		/// Create Spell checker object
		/// </summary>
		/// <param name="useBloomFilter">Boolean indicating wether to use bloom filter or HashSet to store Words</param>
		public SpellChecker(bool useBloomFilter = true)
		{
			ReadFromWordsFile();
			this.useBloomFilter = useBloomFilter;
			if (useBloomFilter)
			{
				BloomFilter = new BloomFilter<string>(wordsList.Count, null, 0.01f);
				InsertIntoBloomFilter();
			}
			else
			{
				WordsInDictionary = new HashSet<string>();
				InsertToWordsSet();
			}

		}

		/// <summary>
		/// Returns true if word is correctly spelled
		/// </summary>
		/// <param name="word">Input word</param>
		/// <returns></returns>
		public bool isWordValid(string word)
		{
			if (useBloomFilter)
			{
				return BloomFilter.Contains(word);
			}
			else
			{
				return WordsInDictionary.Contains(word);
			}
		}

		/// <summary>
		/// Reads all the words line by line from wordlists file
		/// </summary>
		private void ReadFromWordsFile()
		{
			const Int32 BufferSize = 128;
			using (var fileStream = File.OpenRead(wordListPath))
			using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
			{
				String line;
				while ((line = streamReader.ReadLine()) != null)
				{
					wordsList.Add(line);
				}
			}
		}

		/// <summary>
		/// Inserts all the words read to Bloom Filter
		/// </summary>
		private void InsertIntoBloomFilter()
		{
			foreach(string word in wordsList)
			{
				BloomFilter.Add(word);
			}
		}
		
		/// <summary>
		/// Inserts all the words read to hashSet
		/// </summary>
		private void InsertToWordsSet()
		{
			foreach (string word in wordsList)
			{
				WordsInDictionary.Add(word);
			}
		}
	}
}

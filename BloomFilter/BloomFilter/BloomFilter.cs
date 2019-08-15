using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilter
{
    public class BloomFilter<T> : IBloomFilter<T>
    {
		public delegate int HashFunction(T input);

		private BitArray HashBits;
		private HashFunction SecondaryHashFunction;
		private HashFunction PrimaryHashFunction;
		private int NumberOfHashFunctions;

		/// <summary>
		/// Creates a new Bloom Filter given capacity and size of bitarray
		/// Error Rate and Number of hash functions are calculated for given parameters
		/// </summary>
		/// <param name="capacity">Number of expected elements to be inserted</param>
		/// <param name="secondaryHashFunction">Secondary hash Functions to be used to generate combination of hashes. Dont pass GetHashCode</param>
		/// <param name="hashBitsSize">Size of bit array to be stored</param>
		public BloomFilter(int capacity, HashFunction secondaryHashFunction, int hashBitsSize)
		: this(capacity, secondaryHashFunction, hashBitsSize, OptimalErrorRate(capacity), OptimalNumberOfHashes(capacity, OptimalErrorRate(capacity)))
		{
		}

		/// <summary>
		/// Creates a new Bloom Filter given capacity and error rate
		/// BitArray size and Number of hash functions are calculated for given parameters
		/// </summary>
		/// <param name="capacity">Number of expected elements to be inserted</param>
		/// <param name="secondaryHashFunction">Secondary hash Functions to be used to generate combination of hashes. Dont pass GetHashCode</param>
		/// <param name="errorRate">Expected error rate for the bloom filter</param>
		public BloomFilter(int capacity, HashFunction secondaryHashFunction, float errorRate)
		: this(capacity, secondaryHashFunction, OptimalNumberOfHashBits(capacity, errorRate), errorRate, OptimalNumberOfHashes(capacity, errorRate))
		{

		}

		/// <summary>
		/// Creates a new Bloom Filter given capacity, error rate and number of hash functions
		/// BitArray size is calculated for given parameters
		/// </summary>
		/// <param name="capacity">Number of expected elements to be inserted</param>
		/// <param name="secondaryHashFunction">Secondary hash Functions to be used to generate combination of hashes. Dont pass GetHashCode</param>
		/// <param name="errorRate">Expected error rate for the bloom filter</param>
		/// <param name="numberOfHashFunctions">Number of hash functions to be used while storing and retrieving value in bloom filter</param>
		public BloomFilter(int capacity, HashFunction secondaryHashFunction, float errorRate, int numberOfHashFunctions)
		: this(capacity, secondaryHashFunction, OptimalNumberOfHashBits(capacity, errorRate), errorRate, OptimalNumberOfHashes(capacity, errorRate))
		{

		}

		/// <summary>
		/// Creates a new Bloom Filter given capacity, error rate, bit array size and number of hash functions
		/// </summary>
		/// <param name="capacity">Number of expected elements to be inserted</param>
		/// <param name="secondaryHashFunction">Secondary hash Functions to be used to generate combination of hashes. Dont pass GetHashCode</param>
		/// <param name="hashBitsSize">Size of bit array to be stored</param>
		/// <param name="errorRate">Expected error rate for the bloom filter</param>
		/// <param name="numberOfHashFunctions">Number of hash functions to be used while storing and retrieving value in bloom filter</param>
		public BloomFilter(int capacity, HashFunction secondaryHashFunction, int hashBitsSize, float errorRate, int numberOfHashFunctions)
		{
			if(capacity < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Number of elements to be inserted in bloom filter must be > 0");
			}

			if(hashBitsSize < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(hashBitsSize), hashBitsSize, "Size of HashBits in bloom filter is expected to be >0");
			}

			if(errorRate <=0 || errorRate>=1)
			{
				throw new ArgumentOutOfRangeException(nameof(errorRate), errorRate, "Error rate is expected to be between (0.0,1.0)");
			}

			if(numberOfHashFunctions < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(numberOfHashFunctions), numberOfHashFunctions, "Number of Hash Functions in bloom filter is expected to be >0");
			}
			this.HashBits = new BitArray(hashBitsSize);
			this.NumberOfHashFunctions = numberOfHashFunctions;
			if (secondaryHashFunction != null)
			{
				this.SecondaryHashFunction = secondaryHashFunction;
			}
			else
			{
				if(typeof(T) == typeof(string))
				{
					this.SecondaryHashFunction = HashString;
				}
				else
				{
					throw new ArgumentNullException("HashFunction", "Non null values are expected for each hash function in bloom filter");
				} 
			}

		}

		/// <summary>
		/// Adds a new item to the filter. It cannot be removed.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Add(T item)
		{
			if (item == null)
				throw new ArgumentNullException(nameof(item), "Cannot insert null element in bloom Filter");
			// start flipping bits for each hash of item
			int primaryHash = item.GetHashCode();
			int secondaryHash = this.SecondaryHashFunction(item);
			for (int i = 0; i < this.NumberOfHashFunctions; i++)
			{
				int hash = this.ComputeHash(primaryHash, secondaryHash, i);
				this.HashBits[hash] = true;
			}
		}

		/// <summary>
		/// Checks for the existance of the item in the filter for a given probability.
		/// </summary>
		/// <param name="item"> The item. </param>
		/// <returns> The <see cref="bool"/>. </returns>
		public bool Contains(T item)
		{
			if (item == null)
				throw new ArgumentNullException(nameof(item), "Cannot insert null element in bloom Filter");
			int primaryHash = item.GetHashCode();
			int secondaryHash = this.SecondaryHashFunction(item);
			for (int i = 0; i < this.NumberOfHashFunctions; i++)
			{
				int hash = this.ComputeHash(primaryHash, secondaryHash, i);
				if (this.HashBits[hash] == false)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Default Hash Function for string input type
		/// Based on Jenkins One at a time method http://burtleburtle.net/bob/hash/doobs.html
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private int HashString(T input)
		{
			string s = input as string;
			int hash = 0;

			for (int i = 0; i < s.Length; i++)
			{
				hash += s[i];
				hash += (hash << 10);
				hash ^= (hash >> 6);
			}

			hash += (hash << 3);
			hash ^= (hash >> 11);
			hash += (hash << 15);
			return hash;
		}

		/// <summary>
		/// OptimalNumberOfHashes
		/// </summary>
		/// <param name="capacity"> The capacity. </param>
		/// <param name="errorRate"> The error rate. </param>
		/// <returns> The <see cref="int"/>. </returns>
		private static int OptimalNumberOfHashes(int capacity, float errorRate)
		{
			return (int)Math.Round(Math.Log(2.0) * OptimalNumberOfHashBits(capacity, errorRate) / capacity);
		}

		/// <summary>
		/// OptimalNumberOfHashBits
		/// </summary>
		/// <param name="capacity"> The capacity. </param>
		/// <param name="errorRate"> The error rate. </param>
		/// <returns> The <see cref="int"/>. </returns>
		private static int OptimalNumberOfHashBits(int capacity, float errorRate)
		{
			return (int)Math.Ceiling(capacity * Math.Log(errorRate, (1.0 / Math.Pow(2, Math.Log(2.0)))));
		}

		/// <summary>
		/// OptimalErrorRate
		/// </summary>
		/// <param name="capacity"> The capacity. </param>
		/// <returns> The <see cref="float"/>. </returns>
		private static float OptimalErrorRate(int capacity)
		{
			float c = (float)(1.0 / capacity);
			if (c != 0)
			{
				return c;
			}

			// default
			// http://www.cs.princeton.edu/courses/archive/spring02/cs493/lec7.pdf
			return (float)Math.Pow(0.6185, int.MaxValue / capacity);
		}

		/// <summary>
		/// Combines hashes from 2 hash functions to create k hash values
		/// Returns bit index to be set
		/// </summary>
		/// <param name="primaryHash">Hash value from first hash</param>
		/// <param name="secondaryHash">Hash value from second hash</param>
		/// <param name="i"></param>
		/// <returns></returns>
		private int ComputeHash(int primaryHash, int secondaryHash, int i)
		{
			int resultingHash = (primaryHash + (i * secondaryHash)) % this.HashBits.Count;
			return Math.Abs((int)resultingHash);
		}
	}
}

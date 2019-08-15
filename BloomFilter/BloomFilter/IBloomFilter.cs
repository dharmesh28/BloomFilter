using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilter
{
	interface IBloomFilter<T>
	{
		void Add(T item);

		bool Contains(T item);
	}
}

Bloom Filter Implementation in C# and its application with Spellchecker

The project contains Bloom filter implementation in C# and its application demonstrated with Spellchecker Application
Bloom Filter implemented here can be used for generic data types and allows user to specify combination of parameters 
namely capacity, hashFunctions, size of bit array and error rate. For spell checker application, words from Dictionary are inserted 
into bloom filter and spell check is done based on if word is present in bloom filter

There is a helper console app which has 2 classes, Program.cs can be used to test by inputting words in console
There is wordlists.txt file which has words used as dictionary words
SpellCheckerDriverUtil has code to run for N iterations with 5 characters random word string and compare False Positive rate of bloom filter


Here is the result of false positive rate for current implementation with 1000 iterations:

Number of iterations: 1000
Number of False Positives: 9
False positive rate: 0.009

Here is the result of false positive rate for current implementation with 1000 iterations:

Number of iterations: 10000
Number of False Positives: 97
False positive rate: 0.0097

Here is the result of false positive rate for current implementation with 100000 iterations:

Number of iterations: 100000
Number of False Positives: 902
False positive rate: 0.00902

Here is the result of false positive rate for current implementation with 1000000 iterations:

Number of iterations: 1000000
Number of False Positives: 10035
False positive rate: 0.010035

Here is the result of false positive rate for current implementation with 5000000 iterations:

Number of iterations: 5000000
Number of False Positives: 50318
False positive rate: 0.0100636
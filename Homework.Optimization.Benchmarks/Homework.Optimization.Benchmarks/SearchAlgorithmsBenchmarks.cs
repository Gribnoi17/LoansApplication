using BenchmarkDotNet.Attributes;

namespace Homework.Optimization.Benchmarks
{
    [MemoryDiagnoser(false)]
    public class SearchAlgorithmsBenchmarks
    {
        [Params(10, 1000, 1000000, 1000000000)]
        public int ArrayLength { get; set; }
        public int[] SortedArray { get; private set; }
        public int Left { get; private set; }
        public int Middle { get; private set; }
        public int Right { get; private set; }

        [GlobalSetup]
        public void Setup()
        {
            SortedArray = new int[ArrayLength];

            for (int i = 0; i < ArrayLength; i++)
            {
                SortedArray[i] = i + 1;
            }

            Left = 1;
            Middle = ArrayLength / 2;
            Right = ArrayLength;
        }
        
        [Benchmark]
        public int BenchmarkFindElementInSortedArrayLeft()
        {
            return SearchAlgorithms.FindElementInSortedArray(SortedArray, Left);
        }

        [Benchmark]
        public int BenchmarkFindElementInSortedArrayMiddle()
        {
            return SearchAlgorithms.FindElementInSortedArray(SortedArray, Middle);
        }

        [Benchmark]
        public int BenchmarkFindElementInSortedArrayRight()
        {
            return SearchAlgorithms.FindElementInSortedArray(SortedArray, Right);
        }

        [Benchmark]
        public int BenchmarkFindElementInSortedOptimizedLeft()
        {
            return SearchAlgorithms.FindElementInSortedArrayOptimized(SortedArray, Left);
        }

        [Benchmark]
        public int BenchmarkFindElementInSortedOptimizedMiddle()
        {
            return SearchAlgorithms.FindElementInSortedArrayOptimized(SortedArray, Middle);
        }

        [Benchmark]
        public int BenchmarkFindElementInSortedOptimizedRight()
        {
            return SearchAlgorithms.FindElementInSortedArrayOptimized(SortedArray, Right);
        }
    }
}
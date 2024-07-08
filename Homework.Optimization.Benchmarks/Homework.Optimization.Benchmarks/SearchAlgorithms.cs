namespace Homework.Optimization.Benchmarks
{
    public static class SearchAlgorithms
    {
        /// <summary>
        /// Найти элемент в отсортированном массиве целых чисел.
        /// Сложность алгоритма по времени: Big-O(n) 
        /// Сложность алгоритма по памяти: Big-O(1) 
        /// </summary>
        /// <remarks>
        /// Алгоритм не выполняет проверку, что массив отсортирован.
        /// </remarks>
        /// <param name="array">Отсортированный массив целых чисел.</param>
        /// <param name="value">Искомое число.</param>
        /// <returns>Искомое число. Если его нет в массиве, то -1.</returns>
        public static int FindElementInSortedArray(int[] array, int value)
        {
            for (var index = 0; index < array.Length; index++)
            {
                var item = array[index];
                if (item == value)
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Найти элемент в отсортированном массиве целых чисел.
        /// Сложность алгоритма: Big-O(log(n))
        /// Сложность алгоритма по памяти: Big-O(1) 
        /// </summary>
        /// <remarks>
        /// Алгоритм не выполняет проверку, что массив отсортирован.
        /// </remarks>
        /// <param name="array">Отсортированный массив целых чисел.</param>
        /// <param name="value">Искомое число.</param>
        /// <returns>Искомое число. Если его нет в массиве, то -1.</returns>
        public static int FindElementInSortedArrayOptimized(int[] array, int value)
        {
            var left = 0;
            var right = array.Length - 1;
            
            while (left <= right) {
                var mid = left + (right - left) / 2;
                
                if (array[mid] == value)
                    return mid;
                if (array[mid] < value)
                    left = mid + 1;
                else
                    right = mid - 1;
            }
            
            return -1;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectObjects
{
    public class CalculatingComplexity
    {
        /// <summary>
        /// Число сравнений
        /// </summary>
        public static int CountComparisons { get; set; }
        /// <summary>
        /// Количество сложений
        /// </summary>
        public static int CountAdditions { get; set; }
        /// <summary>
        /// Количество вычитаний
        /// </summary>
        public static int CountSubtractions { get; set; }
        /// <summary>
        /// Количество умножений
        /// </summary>
        public static int CountMultiplications { get; set; }
        /// <summary>
        /// Количество делений
        /// </summary>
        public static int CountDivisions { get; set; }
        /// <summary>
        /// Метод, обнуляющий счетчик операций
        /// </summary>
        public static void NullCalculatingComplexity()
        {
            CountComparisons = 0;
            CountDivisions = 0;
            CountAdditions = 0;
            CountMultiplications = 0;
            CountSubtractions = 0;
        }
    }
}

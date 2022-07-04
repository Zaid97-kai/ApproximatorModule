using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportExcel
{
    public class CalculatingComplexity
    {
        /// <summary>
        /// Число сравнений
        /// </summary>
        public int CountComparisons { get; set; }
        /// <summary>
        /// Количество сложений
        /// </summary>
        public int CountAdditions { get; set; }
        /// <summary>
        /// Количество вычитаний
        /// </summary>
        public int CountSubtractions { get; set; }
        /// <summary>
        /// Количество умножений
        /// </summary>
        public int CountMultiplications { get; set; }
        /// <summary>
        /// Количество делений
        /// </summary>
        public int CountDivisions { get; set; }
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public  CalculatingComplexity()
        { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectObjects.TemporaryClasses
{
    /// <summary>
    /// Узел таблицы
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Номер участка
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Коэффициент A линейной модели
        /// </summary>
        public double A { get; set; }
        /// <summary>
        /// Коэффициент B линейной модели
        /// </summary>
        public double B { get; set; }
        /// <summary>
        /// Номер начального узла
        /// </summary>
        public int numberInitialNode { get; set; }
        /// <summary>
        /// Номер конечного узла
        /// </summary>
        public int numberEndNode { get; set; }
    }
}

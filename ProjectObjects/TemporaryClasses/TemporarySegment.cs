using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectObjects.TemporaryClasses
{
    public class TemporarySegment
    {
        /// <summary>
        /// Длина сегмента
        /// </summary>
        public int SegmentLength { get; set; }
        /// <summary>
        /// Наихудший коэффициент детерминации
        /// </summary>
        public double WorstCoefficientDetermination { get; set; }
        /// <summary>
        /// Допустимое значение коэффициента детерминации
        /// </summary>
        public double AcceptableAccuracyValue { get; set; }
    }
}

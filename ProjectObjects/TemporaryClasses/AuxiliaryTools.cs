using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectObjects.TemporaryClasses
{
    public class AuxiliaryTools
    {
        /// <summary>
        /// Вычисление коэффициента детерминации
        /// </summary>
        /// <param name="Y">Вектор исходных значений</param>
        /// <param name="YT">Вектор значений, полученных из модели</param>
        /// <returns></returns>
        public static double CalculationDetermination(double[] Y, double[] YT)
        {
            double Average = Y.Average();
            double Numerator = 0;
            double Denominator = 0;
            for (int i = 0; i < Y.Length; i++)
            {
                Numerator += (Y[i] - YT[i]) * (Y[i] - YT[i]);
                Denominator += (Y[i] - Average) * (Y[i] - Average);
            }
            return 1 - Numerator / Denominator;
        }
        /// <summary>
        /// Допустимое значение точности
        /// </summary>
        public static double AcceptableAccuracyValue = 0.0;
        /// <summary>
        /// Вектор X
        /// </summary>
        public static double[] X;
        /// <summary>
        /// Вектор Y
        /// </summary>
        public static double[] Y;
        /// <summary>
        /// Номер рассматриваемого сегмента
        /// </summary>
        public static int NumberSegment = 0;
        /// <summary>
        /// Множество узлов
        /// </summary>
        public static List<Node> nodesTable = new List<Node>();
        /// <summary>
        /// Вектор практических значений
        /// </summary>
        public static double[] _vs;
        /// <summary>
        /// Создание нулевого сегмента
        /// </summary>
        public static void CreateInitialSegment()
        {
            SegmentContainer.segments.Add(new Segment(AuxiliaryTools.X, AuxiliaryTools.Y) { Number = NumberSegment });
            nodesTable.Add(new Node()
            {
                A = SegmentContainer.segments[0].A,
                B = SegmentContainer.segments[0].B,
                Number = SegmentContainer.segments[0].Number,
                numberEndNode = SegmentContainer.segments[0].NumberEndNode,
                numberInitialNode = SegmentContainer.segments[0].NumberInitialNode
            });
            SegmentContainer.segments[0].LeastSquaresMethod();
            SegmentContainer.segments[0].CalculatingPracticalValue();
            SegmentContainer.segments[0].Determination = AuxiliaryTools.CalculationDetermination(SegmentContainer.segments[0].Y, _vs);
        }
    }
}

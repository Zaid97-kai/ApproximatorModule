using ProjectObjects.TemporaryClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectObjects.AlgorithmClasses
{
    public static class GreedyAlgorithm
    {
        /// <summary>
        /// Сравнение сегментов
        /// </summary>
        /// <param name="segments">Множество сегментов</param>
        /// <returns>Если true - для всех сегментов выполняется условие соответствия заданного коэффициента детерминации, если false - не выполняется</returns>
        public static bool SegmentComparison(List<Segment> segments)
        {
            bool flag = true;
            for (int i = 0; i < segments.Count; i++)
            {
                if (segments[i].Determination > AuxiliaryTools.AcceptableAccuracyValue)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                    segments.RemoveRange(1, segments.Count - 1);
                    break;
                }
            }
            return flag;
        }
        /// <summary>
        /// Метод с постоянным шагом
        /// </summary>
        public static void Method()
        {
            int CountSegments = 1;

            do
            {
                CountSegments++;
                if (SegmentContainer.segments[0].NumberEndNode / CountSegments == 1)
                {
                    return;
                }
                for (int i = 0; i < CountSegments; i++)
                {
                    SegmentContainer.segments.Add(new Segment(AuxiliaryTools.X, AuxiliaryTools.Y, SegmentContainer.segments[0].NumberEndNode / CountSegments * (i + 1) - SegmentContainer.segments[0].NumberEndNode / CountSegments, SegmentContainer.segments[0].NumberEndNode / CountSegments * (i + 1), SegmentContainer.segments[SegmentContainer.segments.Count - 1])
                    {
                        Number = i + 1
                    });
                    SegmentContainer.segments[SegmentContainer.segments.Count - 1].LeastSquaresMethod();
                    SegmentContainer.segments[SegmentContainer.segments.Count - 1].CalculatingPracticalValue();
                    SegmentContainer.segments[SegmentContainer.segments.Count - 1].Determination = AuxiliaryTools.CalculationDetermination(SegmentContainer.segments[SegmentContainer.segments.Count - 1].Y, SegmentContainer.segments[SegmentContainer.segments.Count - 1].YPractical);
                }
            }
            while (!SegmentComparison(SegmentContainer.segments));
        }
    }
}

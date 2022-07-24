using ProjectObjects.TemporaryClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectObjects.AlgorithmClasses
{
    public static class DevelopedMethod
    {
        /// <summary>
        /// Разрабатываемый метод аппроксимации
        /// </summary>
        public static void Method()
        {
            AuxiliaryTools.CreateInitialSegment();
            Segment segment1;
            do
            {
                AuxiliaryTools.NumberSegment++;
                segment1 = new Segment(AuxiliaryTools.X, AuxiliaryTools.Y, SegmentContainer.segments[AuxiliaryTools.NumberSegment - 1])
                {
                    Number = AuxiliaryTools.NumberSegment
                };
                do
                {
                    if (segment1.NumberEndNode == segment1.NumberInitialNode)
                    {
                        return;
                    }
                    segment1.NumberEndNode--;
                    segment1.UpdatingMatrices(AuxiliaryTools.X, AuxiliaryTools.Y);
                    segment1.LeastSquaresMethod();
                    segment1.CalculatingPracticalValue();
                    segment1.CalculationDetermination();

                    CalculatingComplexity.CountComparisons++;
                }
                while (segment1.Determination < AuxiliaryTools.AcceptableAccuracyValue);
                segment1.NumberInitialNode++;
                SegmentContainer.segments.Add(segment1);
            }
            while (segment1.X.Length > 0);
        }
    }
}

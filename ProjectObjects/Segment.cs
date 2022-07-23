using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectObjects
{
    /// <summary>
    /// Класс Участок
    /// </summary>
    public partial class Segment
    {
        /// <summary>
        /// Таблица XY
        /// </summary>
        public double[,] T { get; set; }
        /// <summary>
        /// Вектор X
        /// </summary>
        public double[] X { get; set; }
        /// <summary>
        /// Вектор Y
        /// </summary>
        public double[] Y { get; set; }
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
        /// Практические значения Y
        /// </summary>
        public double[] YPractical { get; set; }
        /// <summary>
        /// Коэффициент детерминации
        /// </summary>
        public double Determination { get; set; }
        /// <summary>
        /// Текущая позиция
        /// </summary>
        public int currentState { get; set; }
        /// <summary>
        /// Номер начального узла
        /// </summary>
        public int numberInitialNode { get; set; }
        /// <summary>
        /// Номер конечного узла
        /// </summary>
        public int numberEndNode { get; set; }
        /// <summary>
        /// Конструктор класса Участок
        /// </summary>
        /// <param name="X">Вектор X</param>
        /// <param name="Y">Вектор Y</param>
        public Segment(double[] X, double[] Y, Segment PreviousSegment = null)
        {
            if (PreviousSegment == null || PreviousSegment.Number == 0)
            {
                numberInitialNode = 0;
                numberEndNode = X.Length;
                this.T = new double[X.Length, 2];
                this.X = new double[X.Length];
                this.Y = new double[Y.Length];
            }
            else
            {
                numberInitialNode = PreviousSegment.numberEndNode;
                numberEndNode = X.Length;
                this.T = new double[numberEndNode - numberInitialNode, 2];
                this.X = new double[numberEndNode - numberInitialNode];
                this.Y = new double[numberEndNode - numberInitialNode];
            }
            for (int i = 0; i < numberEndNode - numberInitialNode; i++)
            {
                T[i, 0] = X[numberInitialNode + i];
                T[i, 1] = Y[numberInitialNode + i];
                this.X[i] = X[numberInitialNode + i];
                this.Y[i] = Y[numberInitialNode + i];
            }
        }
        /// <summary>
        /// Конструктор класса Участок
        /// </summary>
        /// <param name="X">Вектор X</param>
        /// <param name="Y">Вектор Y</param>
        /// <param name="numberInitialNode">Индекс стартового узла</param>
        /// <param name="numberEndNode">Индекс конечного узла</param>
        /// <param name="PreviousSegment">Предыдущий сегмент</param>
        public Segment(double[] X, double[] Y, int numberInitialNode, int numberEndNode, Segment PreviousSegment = null)
        {
            this.numberInitialNode = numberInitialNode;
            this.numberEndNode = numberEndNode;

            this.T = new double[numberEndNode - numberInitialNode, 2];
            this.X = new double[numberEndNode - numberInitialNode];
            this.Y = new double[numberEndNode - numberInitialNode];

            for (int i = 0; i < numberEndNode - numberInitialNode; i++)
            {
                T[i, 0] = X[numberInitialNode + i];
                T[i, 1] = Y[numberInitialNode + i];
                this.X[i] = X[numberInitialNode + i];
                this.Y[i] = Y[numberInitialNode + i];
            }
        }
        /// <summary>
        /// Обновление матриц X, Y, T
        /// </summary>
        public void UpdatingMatrices(double[] X, double[] Y)
        {
            this.T = new double[numberEndNode - numberInitialNode, 2];
            this.X = new double[numberEndNode - numberInitialNode];
            this.Y = new double[numberEndNode - numberInitialNode];
            for (int i = 0; i < numberEndNode - numberInitialNode; i++)
            {
                T[i, 0] = X[numberInitialNode + i];
                T[i, 1] = Y[numberInitialNode + i];
                this.X[i] = X[numberInitialNode + i];
                this.Y[i] = Y[numberInitialNode + i];
            }
        }
        /// <summary>
        /// Вычисление коэффициента детерминации
        /// </summary>
        /// <returns></returns>
        public bool CalculationDetermination()
        {
            try
            {
                double Average = Y.Average();
                double Numerator = 0;
                double Denominator = 0;
                for (int i = 0; i < Y.Length; i++)
                {
                    Numerator += (Y[i] - YPractical[i]) * (Y[i] - YPractical[i]);
                    Denominator += (Y[i] - Average) * (Y[i] - Average);
                }
                Determination = 1 - Numerator / Denominator;
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Вычисление практического значения в методе наименьших квадратов
        /// </summary>
        /// <summary>
        /// Вычисление коэффициента детерминации
        /// </summary>
        /// <param name="Y">Вектор исходных значений</param>
        /// <param name="YT">Вектор значений, полученных из модели</param>
        /// <returns></returns>
        public void CalculatingPracticalValue()
        {
            YPractical = new double[X.Length];
            for (int i = 0; i < X.Length; i++)
            {
                YPractical[i] = A * T[i, 0] + B;
            }
        }
        /// <summary>
        /// Метод наименьших квадратов (линейная модель)
        /// </summary>
        public void LeastSquaresMethod()
        {
            int n = T.GetLength(0);
            A = (n * sumXY(T) - sumXsumY(T)) / (n * sumXX(T) - sumXsumX(T));
            B = (sumY(T) - this.A * sumX(T)) / n;
        }
        private double sumXY(double[,] inputMatrix)
        {
            double sumXY = 0;
            for (int i = 0; i < inputMatrix.GetLength(0); i++)
            {
                sumXY += inputMatrix[i, 0] * inputMatrix[i, 1];
            }
            return sumXY;
        }
        private double sumY(double[,] inputMatrix)
        {
            double sumY = 0;
            for (int i = 0; i < inputMatrix.GetLength(0); i++)
            {
                sumY += inputMatrix[i, 1];
            }
            return sumY;
        }
        private double sumX(double[,] inputMatrix)
        {
            double sumX = 0;
            for (int i = 0; i < inputMatrix.GetLength(0); i++)
            {
                sumX += inputMatrix[i, 0];
            }
            return sumX;
        }
        private double sumXX(double[,] inputMatrix)
        {
            double sumXX = 0;
            for (int i = 0; i < inputMatrix.GetLength(0); i++)
            {
                sumXX += inputMatrix[i, 0] * inputMatrix[i, 0];
            }
            return sumXX;
        }
        private double sumXsumX(double[,] inputMatrix)
        {
            double sumX = 0;
            for (int i = 0; i < inputMatrix.GetLength(0); i++)
            {
                sumX += inputMatrix[i, 0];
            }
            return Math.Pow(sumX, 2.0);
        }
        private double sumXsumY(double[,] inputMatrix)
        {
            double sumX = 0;
            double sumY = 0;
            for (int i = 0; i < inputMatrix.GetLength(0); i++)
            {
                sumX += inputMatrix[i, 0];
                sumY += inputMatrix[i, 1];
            }
            return sumX * sumY;
        }
    }
}

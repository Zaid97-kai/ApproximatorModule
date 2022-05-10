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
        public double[,] T;
        /// <summary>
        /// Вектор X
        /// </summary>
        public double[] X;
        /// <summary>
        /// Вектор Y
        /// </summary>
        public double[] Y;
        /// <summary>
        /// Номер участка
        /// </summary>
        public int Number;
        /// <summary>
        /// Коэффициент A линейной модели
        /// </summary>
        public double A;
        /// <summary>
        /// Коэффициент B линейной модели
        /// </summary>
        public double B;
        /// <summary>
        /// Практические значения Y
        /// </summary>
        public double[] YPractical;
        /// <summary>
        /// Коэффициент детерминации
        /// </summary>
        public double Determination;
        /// <summary>
        /// Текущая позиция
        /// </summary>
        public int currentState;
        /// <summary>
        /// Конструктор класса Участок
        /// </summary>
        /// <param name="X">Вектор X</param>
        /// <param name="Y">Вектор Y</param>
        public Segment(double[] X, double[] Y, int offset, bool flag = true)
        {
            this.T = new double[X.Length - offset, 2];
            this.X = new double[X.Length - offset];
            this.Y = new double[X.Length - offset];
            if (flag)
            {
                for (int i = 0; i < X.Length - offset; i++)
                {
                    T[i, 0] = X[i];
                    T[i, 1] = Y[i];
                    this.X[i] = X[i];
                    this.Y[i] = Y[i];
                }
            }
            else
            {
                for(int i = 0; i < X.Length - offset; i++)
                {
                    T[i, 0] = X[offset + i];
                    T[i, 1] = Y[offset + i];
                    this.X[i] = X[offset + i];
                    this.Y[i] = Y[offset + i];
                }
            }
        }
        /// <summary>
        /// Вычисление практического значения в методе наименьших квадратов
        /// </summary>
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

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
    }
}

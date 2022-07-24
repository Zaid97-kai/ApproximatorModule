using Microsoft.Win32;
using ProjectObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjectObjects.TemporaryClasses;
using ProjectObjects.AlgorithmClasses;
using Excel = Microsoft.Office.Interop.Excel;
namespace ImportExcel
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double[,] list;
        private int _rows = 0;
        private int _columns = 0;
        /// <summary>
        /// Коэффициент A линейной модели
        /// </summary>
        private double _a = 0;
        /// <summary>
        /// Коэффициент B линейной модели
        /// </summary>
        private double _b = 0;
        /// <summary>
        /// Конструктор главного окна
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            TbAcceptableAccuracy.Text = "0,996";
        }
        /// <summary>
        /// Обработка нажатия на кнопку Open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BnOpenExcel_Click(object sender, RoutedEventArgs e)
        {
            CalculatingComplexity.NullCalculatingComplexity();
            if(!Double.TryParse(TbAcceptableAccuracy.Text, out AuxiliaryTools.AcceptableAccuracyValue))
            {
                MessageBox.Show("Введено недопустимое значение точности вычислений!");
                return;
            }
            ExportExcel();
            
            string s;
            for (int i = 0; i < _rows; i++) // по всем строкам
            {
                s = "";
                for (int j = 0; j < _columns; j++) //по всем колонкам
                    s += " | " + list[i, j];
            }
            LeastSquaresMethod(this.list);
            CalculatingPracticalValue(this.list);

            AuxiliaryTools.X = new double[_rows];
            AuxiliaryTools.Y = new double[_rows];
            for (int i = 0; i < _rows; i++)
            {
                AuxiliaryTools.X[i] = list[i, 0];
                AuxiliaryTools.Y[i] = list[i, 1];
            }
            DevelopedMethod.Method();
            LbInputDataSecond.ItemsSource = SegmentContainer.segments;
            MessageBox.Show($"Количество сравнений = {CalculatingComplexity.CountComparisons}\n" +
                $"Количество сложений = {CalculatingComplexity.CountAdditions}\n" +
                $"Количество вычитаний = {CalculatingComplexity.CountSubtractions}\n" +
                $"Количество умножений = {CalculatingComplexity.CountMultiplications}\n" +
                $"Количество делений = {CalculatingComplexity.CountDivisions}");
        }
        /// <summary>
        /// Вычисление практического значения в методе наименьших квадратов
        /// </summary>
        /// <param name="inputMatrix">Входная матрица значений X-Y</param>
        private void CalculatingPracticalValue(double[,] inputMatrix)
        {
            AuxiliaryTools._vs = new double[_rows];
            for (int i = 0; i < _rows; i++)
            {
                AuxiliaryTools._vs[i] = this._a * inputMatrix[i, 0] + this._b;

                CalculatingComplexity.CountAdditions++;
                CalculatingComplexity.CountMultiplications++;            
            }
        }
        /// <summary>
        /// Метод наименьших квадратов (линейная модель)
        /// </summary>
        /// <param name="inputMatrix">Входная матрица значений X-Y</param>
        private void LeastSquaresMethod(double[,] inputMatrix)
        {
            int n = inputMatrix.GetLength(0);

            this._a = (n * SumXY(inputMatrix) - sumXsumY(inputMatrix)) / (n * SumXX(inputMatrix) - SumXsumX(inputMatrix));

            CalculatingComplexity.CountMultiplications++;
            CalculatingComplexity.CountSubtractions++;
            CalculatingComplexity.CountMultiplications++;
            CalculatingComplexity.CountSubtractions++;
            CalculatingComplexity.CountDivisions++;

            this._b = (SumY(inputMatrix) - this._a * SumX(inputMatrix)) / n;

            CalculatingComplexity.CountMultiplications++;
            CalculatingComplexity.CountMultiplications++;
            CalculatingComplexity.CountSubtractions++;
        }
        /// <summary>
        /// Вычисление суммы произведений XY для метода наименьших квадратов
        /// </summary>
        /// <param name="inputMatrix">Входная матрица значений X-Y</param>
        /// <returns>Сумма произведений XY</returns>
        public double SumXY(double[,] inputMatrix)
        {
            double sumXY = 0;
            for (int i = 0; i < inputMatrix.GetLength(0); i++)
            {
                sumXY += inputMatrix[i, 0] * inputMatrix[i, 1];

                CalculatingComplexity.CountAdditions++;
                CalculatingComplexity.CountMultiplications++;
            }
            return sumXY;
        }
        /// <summary>
        /// Вычисление суммы Y для метода наименьших квадратов
        /// </summary>
        /// <param name="inputMatrix">Входная матрица значений X-Y</param>
        /// <returns>Сумма Y</returns>
        public double SumY(double[,] inputMatrix)
        {
            double sumY = 0;
            for (int i = 0; i < inputMatrix.GetLength(0); i++)
            {
                sumY += inputMatrix[i, 1];

                CalculatingComplexity.CountAdditions++;
            }
            return sumY;
        }
        /// <summary>
        /// Вычисление суммы X для метода наименьших квадратов
        /// </summary>
        /// <param name="inputMatrix">Входная матрица значений X-Y</param>
        /// <returns>Сумма X</returns>
        public double SumX(double[,] inputMatrix)
        {
            double sumX = 0;
            for (int i = 0; i < inputMatrix.GetLength(0); i++)
            {
                sumX += inputMatrix[i, 0];

                CalculatingComplexity.CountAdditions++;
            }
            return sumX;
        }
        /// <summary>
        /// Вычисление суммы произведений XX для метода наименьших квадратов
        /// </summary>
        /// <param name="inputMatrix">Входная матрица значений X-Y</param>
        /// <returns>Сумма произведений XX</returns>
        public double SumXX(double[,] inputMatrix)
        {
            double sumXX = 0;
            for (int i = 0; i < inputMatrix.GetLength(0); i++)
            {
                sumXX += inputMatrix[i, 0] * inputMatrix[i, 0];

                CalculatingComplexity.CountAdditions++;
                CalculatingComplexity.CountMultiplications++;
            }
            return sumXX;
        }
        /// <summary>
        /// Вычисление произведения суммы квадратов X для метода наименьших квадратов
        /// </summary>
        /// <param name="inputMatrix">Входная матрица значений X-Y</param>
        /// <returns>Произведение суммы квадратов X</returns>
        public double SumXsumX(double[,] inputMatrix)
        {
            double sumX = 0;
            for (int i = 0; i < inputMatrix.GetLength(0); i++)
            {
                sumX += inputMatrix[i, 0];

                CalculatingComplexity.CountAdditions++;
            }

            CalculatingComplexity.CountMultiplications++;
            return Math.Pow(sumX, 2.0);
        }
        /// <summary>
        /// Вычисление произведения суммы X на сумму Y для метода наименьших квадратов
        /// </summary>
        /// <param name="inputMatrix">Входная матрица значений X-Y</param>
        /// <returns>Произведение суммы X на сумму Y</returns>
        public double sumXsumY(double[,] inputMatrix)
        {
            double sumX = 0;
            double sumY = 0;
            for (int i = 0; i < inputMatrix.GetLength(0); i++)
            {
                sumX += inputMatrix[i, 0];
                sumY += inputMatrix[i, 1];

                CalculatingComplexity.CountAdditions++;
                CalculatingComplexity.CountAdditions++;
            }

            CalculatingComplexity.CountMultiplications++;
            return sumX * sumY;
        }




        /// <summary>
        /// Импорт данных из Excel-файла (не более 5 столбцов и любое количество строк <= 50.
        /// </summary>
        /// <returns></returns>
        private void ExportExcel()
        {
            // Выбрать путь и имя файла в диалоговом окне
            OpenFileDialog ofd = new OpenFileDialog()
            {
                // Задаем расширение имени файла по умолчанию (открывается папка с программой)
                DefaultExt = "*.xls;*.xlsx",
                // Задаем строку фильтра имен файлов, которая определяет варианты
                Filter = "файл Excel (Spisok.xlsx)|*.xlsx",
                // Задаем заголовок диалогового окна
                Title = "Выберите файл базы данных"
            };
            if (!(ofd.ShowDialog() == true)) // если файл БД не выбран -> Выход
                return;

            Excel.Application ObjWorkExcel = new Excel.Application();
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(ofd.FileName);
            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1-й лист
            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//последнюю ячейку
                                                                                                // размеры базы
            _columns = (int)lastCell.Column;
            _rows = (int)lastCell.Row;

            this.list = new double[_rows, _columns];

            for (int j = 0; j < _columns; j++) //по всем колонкам
                for (int i = 0; i < _rows; i++) // по всем строкам
                    list[i, j] = Convert.ToDouble(ObjWorkSheet.Cells[i + 1, j + 1].Text.ToString()); //считываем данные
            ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
            ObjWorkExcel.Quit(); // выйти из Excel
            GC.Collect(); // убрать за собой
        }

        private void BnOpenExcelSecond_Click(object sender, RoutedEventArgs e)
        {
            if (!Double.TryParse(TbAcceptableAccuracy.Text, out AuxiliaryTools.AcceptableAccuracyValue))
            {
                MessageBox.Show("Введено недопустимое значение точности вычислений!");
                return;
            }
            ExportExcel();

            string s;
            for (int i = 0; i < _rows; i++) // по всем строкам
            {
                s = "";
                for (int j = 0; j < _columns; j++) //по всем колонкам
                    s += " | " + list[i, j];
            }
            LeastSquaresMethod(this.list);
            CalculatingPracticalValue(this.list);

            AuxiliaryTools.X = new double[_rows];
            AuxiliaryTools.Y = new double[_rows];
            for (int i = 0; i < _rows; i++)
            {
                AuxiliaryTools.X[i] = list[i, 0];
                AuxiliaryTools.Y[i] = list[i, 1];
            }

            AuxiliaryTools.CreateInitialSegment();
            do
            {
                GreedyAlgorithm.Method();
                AuxiliaryTools.AcceptableAccuracyValue = AuxiliaryTools.AcceptableAccuracyValue - 0.001;
                TbAcceptableAccuracy.Text = AuxiliaryTools.AcceptableAccuracyValue.ToString();
            }
            while (SegmentContainer.segments.Count == 1);
            LbInputDataSecond.ItemsSource = SegmentContainer.segments;
        }
    }
}

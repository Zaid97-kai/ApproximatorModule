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
using Excel = Microsoft.Office.Interop.Excel;
namespace ImportExcel
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
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CalculatingComplexity _calculatingComplexity;
        private double[,] list;
        private double[] X;
        private double[] Y;
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
        /// Вектор практических значений
        /// </summary>
        private double[] _vs;
        /// <summary>
        /// Номер рассматриваемого сегмента
        /// </summary>
        private int _numberSegment = 0;
        /// <summary>
        /// Множество сегментов
        /// </summary>
        public List<ProjectObjects.Segment> segments = new List<ProjectObjects.Segment>();
        /// <summary>
        /// Множество узлов
        /// </summary>
        private List<Node> nodesTable = new List<Node>();
        /// <summary>
        /// Допустимое значение точности
        /// </summary>
        private double AcceptableAccuracyValue = 0.0;
        /// <summary>
        /// Конструктор главного окна
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            TbAcceptableAccuracy.Text = "0,996";
            this._calculatingComplexity = new CalculatingComplexity();
        }
        /// <summary>
        /// Обработка нажатия на кнопку Open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BnOpenExcel_Click(object sender, RoutedEventArgs e)
        {
            _calculatingComplexity.NullCalculatingComplexity();
            if(!Double.TryParse(TbAcceptableAccuracy.Text, out AcceptableAccuracyValue))
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

            X = new double[_rows];
            Y = new double[_rows];
            for (int i = 0; i < _rows; i++)
            {
                X[i] = list[i, 0];
                Y[i] = list[i, 1];
            }
            Method();
            LbInputDataSecond.ItemsSource = segments;
            MessageBox.Show($"Количество сравнений = {_calculatingComplexity.CountComparisons}\n" +
                $"Количество сложений = {_calculatingComplexity.CountAdditions}\n" +
                $"Количество вычитаний = {_calculatingComplexity.CountSubtractions}\n" +
                $"Количество умножений = {_calculatingComplexity.CountMultiplications}\n" +
                $"Количество делений = {_calculatingComplexity.CountDivisions}");
        }
        /// <summary>
        /// Вычисление практического значения в методе наименьших квадратов
        /// </summary>
        /// <param name="inputMatrix">Входная матрица значений X-Y</param>
        private void CalculatingPracticalValue(double[,] inputMatrix)
        {
            this._vs = new double[_rows];
            for (int i = 0; i < _rows; i++)
            {
                this._vs[i] = this._a * inputMatrix[i, 0] + this._b;

                _calculatingComplexity.CountAdditions++;
                _calculatingComplexity.CountMultiplications++;            
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

            _calculatingComplexity.CountMultiplications++;
            _calculatingComplexity.CountSubtractions++;
            _calculatingComplexity.CountMultiplications++;
            _calculatingComplexity.CountSubtractions++;
            _calculatingComplexity.CountDivisions++;

            this._b = (SumY(inputMatrix) - this._a * SumX(inputMatrix)) / n;

            _calculatingComplexity.CountMultiplications++;
            _calculatingComplexity.CountMultiplications++;
            _calculatingComplexity.CountSubtractions++;
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

                _calculatingComplexity.CountAdditions++;
                _calculatingComplexity.CountMultiplications++;
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

                _calculatingComplexity.CountAdditions++;
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

                _calculatingComplexity.CountAdditions++;
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

                _calculatingComplexity.CountAdditions++;
                _calculatingComplexity.CountMultiplications++;
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

                _calculatingComplexity.CountAdditions++;
            }

            _calculatingComplexity.CountMultiplications++;
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

                _calculatingComplexity.CountAdditions++;
                _calculatingComplexity.CountAdditions++;
            }

            _calculatingComplexity.CountMultiplications++;
            return sumX * sumY;
        }
        /// <summary>
        /// Разрабатываемый метод аппроксимации
        /// </summary>
        private void Method()
        {
            CreateInitialSegment();
            Segment segment1;
            do
            {
                _numberSegment++;
                segment1 = new Segment(X, Y, this.segments[_numberSegment - 1])
                {
                    Number = _numberSegment
                };
                do
                {
                    if (segment1.numberEndNode == segment1.numberInitialNode)
                    {
                        return;
                    }
                    segment1.numberEndNode--;
                    segment1.UpdatingMatrices(X, Y);
                    segment1.LeastSquaresMethod();
                    segment1.CalculatingPracticalValue();
                    segment1.CalculationDetermination();

                    _calculatingComplexity.CountComparisons++;
                }
                while (segment1.Determination < AcceptableAccuracyValue);
                segment1.numberInitialNode++;
                this.segments.Add(segment1);
            }
            while (segment1.X.Length > 0);
        }
        /// <summary>
        /// Сравнение сегментов
        /// </summary>
        /// <param name="segments">Множество сегментов</param>
        /// <returns></returns>
        private bool SegmentComparison(List<Segment> segments)
        {
            bool flag = true;
            for (int i = 0; i < segments.Count; i++)
            {
                if(segments[i].Determination > AcceptableAccuracyValue)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            return flag;
        }
        /// <summary>
        /// Метод с постоянным шагом
        /// </summary>
        private void SecondMethod()
        {
            CreateInitialSegment();
            
            int CountSegments = 2;

            while (SegmentComparison(segments))
            {
                for (int i = 0; i < CountSegments; i++)
                {
                    segments.Add(new Segment(X, Y, segments[segments.Count - 1])
                    { 
                        numberInitialNode = segments[segments.Count - 1].numberInitialNode,
                        numberEndNode = segments[segments.Count - 1].numberEndNode / CountSegments + segments[segments.Count - 1].numberEndNode / CountSegments * i
                    });
                    segments[segments.Count - 1].LeastSquaresMethod();
                    segments[segments.Count - 1].CalculatingPracticalValue();
                    segments[segments.Count - 1].Determination = CalculationDetermination(segments[segments.Count].Y, _vs);
                }
            }
        }
        /// <summary>
        /// Создание нулевого сегмента
        /// </summary>
        private void CreateInitialSegment()
        {
            this.segments.Add(new Segment(X, Y) { Number = _numberSegment });
            this.nodesTable.Add(new Node() 
            { 
                A = this.segments[0].A, 
                B = this.segments[0].B, 
                Number = this.segments[0].Number, 
                numberEndNode = this.segments[0].numberEndNode, 
                numberInitialNode = this.segments[0].numberInitialNode 
            });
            this.segments[0].LeastSquaresMethod();
            this.segments[0].CalculatingPracticalValue();
            this.segments[0].Determination = this.CalculationDetermination(this.segments[0].Y, _vs); 
        }
        /// <summary>
        /// Вычисление коэффициента детерминации
        /// </summary>
        /// <param name="Y">Вектор исходных значений</param>
        /// <param name="YT">Вектор значений, полученных из модели</param>
        /// <returns></returns>
        private double CalculationDetermination(double[] Y, double[] YT)
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

        }
    }
}

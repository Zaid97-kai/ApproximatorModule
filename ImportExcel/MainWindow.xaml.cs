using Microsoft.Win32;
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
    /// Класс Участок
    /// </summary>
    public partial class Segment
    {
        public double[,] T;
        public double[] X;
        public double[] Y;
        public int Number;
        public double A;
        public double B;
        public double Determination;
        /// <summary>
        /// Конструктор класса Участок
        /// </summary>
        /// <param name="X">Вектор X</param>
        /// <param name="Y">Вектор Y</param>
        public Segment(double[] X, double[] Y, int offset)
        {
            this.T = new double[X.Length - offset, 2];
            this.X = new double[X.Length - offset];
            this.Y = new double[X.Length - offset];
            for (int i = 0; i < X.Length - offset; i++)
            {
                T[i, 0] = X[i];
                T[i, 1] = Y[i];
                this.X[i] = X[i];
                this.Y[i] = Y[i];
            }
        }
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double[,] list;
        private double[] X;
        private double[] Y;
        private int _rows = 0;
        private int _columns = 0;
        private double _sumX = 0;
        private double _sumXX = 0;
        private double _sumY = 0;
        private double _sumXY = 0;
        private double _a = 0;
        private double _b = 0;
        private double[] _vs;
        private static int _offset = 0;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void BnOpenExcel_Click(object sender, RoutedEventArgs e)
        {
            ExportExcel();
            LbInputData.Items.Clear();
            string s;
            for (int i = 0; i < _rows; i++) // по всем строкам
            {
                s = "";
                for (int j = 0; j < _columns; j++) //по всем колонкам
                    s += " | " + list[i, j];
                LbInputData.Items.Add(s);
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
        }
        /// <summary>
        /// Вычисление практического значения в методе наименьших квадратов
        /// </summary>
        private void CalculatingPracticalValue(double[,] inputMatrix)
        {
            this._vs = new double[_rows];
            for (int i = 0; i < _rows; i++)
            {
                this._vs[i] = _a * inputMatrix[i, 0] + _b;
                LbInputData.Items.Add(_vs[i]);
            }
        }
        /// <summary>
        /// Метод наименьших квадратов (линейная модель)
        /// </summary>
        /// <param name="inputMatrix">Входная матрица значений X-Y</param>
        private void LeastSquaresMethod(double[,] inputMatrix)
        {
            for (int i = 0; i < _rows; i++)
            {
                _sumX += list[i, 0];
                _sumXX += list[i, 0] * list[i, 0];
                _sumY += list[i, 1];
                _sumXY += list[i, 0] * list[i, 1];
            }
            LbInputData.Items.Add("SumX = " + _sumX);
            LbInputData.Items.Add("SumXX = " + _sumXX);
            LbInputData.Items.Add("SumY = " + _sumY);
            LbInputData.Items.Add("SumXY = " + _sumY);

            _a = Math.Sqrt(Math.Abs((_rows * _sumXY - _sumX * _sumY) / (_rows * _sumXX - _sumX * _sumX)));
            _b = Math.Sqrt(Math.Abs((_sumXX * _sumY - _sumX * _sumXY) / (_rows * _sumXX - _sumX * _sumX)));
            LbInputData.Items.Add("a = " + _a);
            LbInputData.Items.Add("b = " + _b);
        }
        /// <summary>
        /// Разрабатываемый метод аппроксимации
        /// </summary>
        private void Method()
        {
            List<Segment> segments = new List<Segment>();
            Segment segment = new Segment(X, Y, 0) { Number = 0 };
            segments.Add(segment);
            Segment segment1;
            do
            {
                _offset++;
                segment1 = new Segment(X, Y, _offset) { Number = 1 };
                LeastSquaresMethod(segment1.T);
                segment1.A = this._a;
                segment1.B = this._b;
                _rows -= 1;
                this.CalculatingPracticalValue(segment1.T);
                segment1.Determination = this.CalculationDetermination(segment1.Y, _vs);
            }
            while (segment1.Determination < 0.85);
            segments.Add(segment1);
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
    }
}

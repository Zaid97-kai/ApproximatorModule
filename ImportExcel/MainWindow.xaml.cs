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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double[,] list;
        private int _rows = 0;
        private int _columns = 0;
        private double _sumX = 0;
        private double _sumXX = 0;
        private double _sumY = 0;
        private double _sumXY = 0;
        private double _a = 0;
        private double _b = 0;
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
            LeastSquaresMethod();

            double[] vs = new double[_rows];
            for (int i = 0; i < _rows; i++)
            {
                vs[i] = _a * list[i, 0] + _b;
                LbInputData.Items.Add(vs[i]);
            }
        }
        /// <summary>
        /// Метод наименьших квадратов (линейная модель)
        /// </summary>
        private void LeastSquaresMethod()
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
            _b = Math.Sqrt((_sumXX * _sumY - _sumX * _sumXY) / (_rows * _sumXX - _sumX * _sumX));
            LbInputData.Items.Add("a = " + _a);
            LbInputData.Items.Add("b = " + _b);
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

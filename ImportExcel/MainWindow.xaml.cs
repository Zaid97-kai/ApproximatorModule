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
        string[,] list = new string[50, 5];
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BnOpenExcel_Click(object sender, RoutedEventArgs e)
        {
            int n = ExportExcel();
            LbInputData.Items.Clear();
            string s;
            for (int i = 0; i < n; i++) // по всем строкам
            {
                s = "";
                for (int j = 0; j < 5; j++) //по всем колонкам
                    s += " | " + list[i, j];
                LbInputData.Items.Add(s);
            }
        }
        /// <summary>
        /// Импорт данных из Excel-файла (не более 5 столбцов и любое количество строк <= 50.
        /// </summary>
        /// <returns></returns>
        private int ExportExcel()
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
                return 0;
            Excel.Application ObjWorkExcel = new Excel.Application();
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(ofd.FileName);
            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1-й лист
            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//последнюю ячейку
                                                                                                // размеры базы
            int lastColumn = (int)lastCell.Column;
            int lastRow = (int)lastCell.Row;

            for (int j = 0; j < 5; j++) //по всем колонкам
                for (int i = 0; i < lastRow; i++) // по всем строкам
                    list[i, j] = ObjWorkSheet.Cells[i + 1, j + 1].Text.ToString(); //считываем данные
            ObjWorkBook.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
            ObjWorkExcel.Quit(); // выйти из Excel
            GC.Collect(); // убрать за собой
            return lastRow;
        }
    }
}

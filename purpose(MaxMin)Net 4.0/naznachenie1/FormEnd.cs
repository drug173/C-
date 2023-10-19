using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace naznachenie1
{
    public partial class FormEnd : Form
    {
      
        


        public FormEnd(double[,] matrFirstCriterion, double[,] matrSecondCriterion)
        {
            InitializeComponent();
             
            List<int> markIndex = new List<int>();// Индекс помеченного столбца в строке
            gridSupport.RowCount = Static.rowEnd;
            gridSupport.ColumnCount = Static.colEnd;
            gridFinal.RowCount = Static.rowEnd;
            gridFinal.ColumnCount = Static.colEnd;
            if (Static.flagConvolution == true)
            {
                ///ищем полную вероятность (Произведение вероятностей)
                Hungarian hungarian = new Hungarian(matrFirstCriterion, matrSecondCriterion);

                SupportGrid(gridSupport, Static.matr3);
                SupportGrid(gridFinal, hungarian.HungarianMethod(ref markIndex));


            }
            else
            {
                //ищем мат ожидание  (Сумма вероятностей)
                HungarianAdd hungarian = new HungarianAdd(matrFirstCriterion, matrSecondCriterion);

                SupportGrid(gridSupport, Static.matr3);
                SupportGrid(gridFinal, hungarian.HungarianMethod(ref markIndex));

            }
            dataGridView1.ColumnCount = Static.colEnd;
            dataGridView1.RowCount = 1;
            Static.VectorToTable(dataGridView1, markIndex);
            Static.result = markIndex;
        }
        /// <summary>
        /// Получение таблицы из матрицы
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="matr"></param>
        private void SupportGrid(DataGridView grid, double[,] matr)   
        {
            Static.MatrToTable(grid, matr);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void gridSupport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gridFinal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        //сохранение результата  в файл
        private void button1_Click(object sender, EventArgs e)
        {
           int col = Static.colEnd;
           int row = Static.rowEnd;
            Excel.Application exApp = new Excel.Application();
            exApp.Workbooks.Add();
            Worksheet workSheet = (Worksheet)exApp.ActiveSheet;
            for (int i = 0; i < col; i++)
            {
                workSheet.Cells[1, i + 2] = "цель " + (i + 1).ToString();
            }
            for (int j = 0; j < row; j++)
            {
                 workSheet.Cells[j+2, 1] = "стрелок " + (j+1).ToString();
            }

            for(int i=0; i < row; i++)
            {
                for(int j=0; j < col; j++)
                {
                  double d  = Convert.ToDouble(gridFinal.Rows[i].Cells[j].Value);

                    workSheet.Cells[i + 2, j + 2] = String.Format("{0: 0.##}", d);
                }
            }
            
            string pathToXmlFile;
            pathToXmlFile = Environment.CurrentDirectory + "\\" + "result.xls";
            
            exApp.Application.DisplayAlerts = false;
            try
            {
                workSheet.SaveAs(pathToXmlFile);
               

            }
            catch
            {

            }
            exApp.Quit();
        }
    }
}

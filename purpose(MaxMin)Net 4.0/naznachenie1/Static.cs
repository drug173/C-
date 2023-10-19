using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace naznachenie1
{
    class Static
    {
        public static double[,] matr1 = new double[rowEnd, colEnd];
        public static double[,] matr2 = new double[rowEnd, colEnd];
        public static double[,] matr3;
        public static int colEnd, rowEnd;
        public static bool flagMatr = true;
        /// <summary>
        /// флаг свёртки (выбираем свёртку) если false,  ищем  мат ожидание
        /// </summary>
        public static bool flagConvolution = true;
        /// <summary>
        /// flagChecked1  если отмечен, решаем на мимнимум  первый  критерий
        /// </summary>
        public static bool flagChecked1 = false;
        /// <summary>
        /// flagChecked2  если отмечен, решаем на мимнимум  второй критерий
        /// </summary>
        public static bool flagChecked2 = false;
        /// <summary>
        /// результат  свертки
        /// </summary>
        public static List<int> result = new List<int>();
        public static double[,] Matr(DataGridView grid13, int N)  //Получение матрицы из таблицы
        {
            double[,] matr = new double[N, N];
            
            try
            {
                
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        if (String.IsNullOrEmpty(grid13[j, i].Value.ToString())==true)
                        {
                            matr[i, j] = 1;
                        }
                        else
                        {
                            matr[i, j] = Convert.ToDouble(grid13[j, i].Value);
                        }
                        if (matr[i, j]<0)
                        {
                            MessageBox.Show(
                            "Проверьте правильность ввода, отрицательное число!",
                            "Ошибка ввода",
                            MessageBoxButtons.RetryCancel,
                            MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                            flagMatr = false;

                        }
                    }
                }
                
            }
            catch
            {
             MessageBox.Show(
               "Проверьте правильность ввода, не во всех полях числа!",
               "Ошибка ввода",
               MessageBoxButtons.RetryCancel,
               MessageBoxIcon.Warning,
               MessageBoxDefaultButton.Button1,
               MessageBoxOptions.DefaultDesktopOnly);
                flagMatr = false;
            }
            
            return matr;
        }

        /// <summary>
        /// Копирование матрицы
        /// </summary>
        /// <param name="matr1"></param>
        /// <returns></returns>
        public static double[,] MatrCopy(double[,] matr1)
        {
            double[,] matr2 = new double[rowEnd, colEnd];

            for (int i=0; i<rowEnd; i++)
            {
                for(int j=0; j<colEnd; j++)
                {
                    matr2[i, j] = matr1[i, j];
                }
            }
            return matr2;
        }
   
        /// <summary>
        /// получение таблицы из вектора
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="markIndex"></param>
        public static void VectorToTable(DataGridView grid, List<int> markIndex)
        {
            for(int j= 0; j < colEnd; j++)
           {

                grid.Rows[0].Cells[j].Value = "Ц" + (markIndex[j] + 1).ToString();
            }

        }


        /// <summary>
        /// Получение таблицы из матрицы
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="matr"></param>
        public static void MatrToTable(DataGridView grid, double[,] matr) //Получение таблицы из матрицы
        {
            for (int i = 0; i < rowEnd; i++)
            {
                grid.Rows[i].HeaderCell.Value = "С" + (i + 1).ToString();
                for (int j = 0; j < colEnd; j++)
                {
                    grid.Rows[i].Cells[j].Value = matr[i, j].ToString();
                    grid.Columns[j].HeaderText = "Ц" + (j + 1).ToString();
                }
            }
        }
    }
}
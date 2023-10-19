using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace naznachenie1
{
    class HungarianAdd
    {
        double[,] matrHung;
        private int row = Static.rowEnd;
        private int col = Static.colEnd;
      


        public HungarianAdd(double[,] matr1, double[,] matr2)
        {
            ConvolutionMatr(matr1, matr2, ref Static.matr3);
            matrHung = Static.matr3;
        }

        /// <summary>
        /// .....Венгерский алгоритм .............///////////////////////
        /// </summary>
        /// <returns></returns>
        public double[,] HungarianMethod(ref List<int> markIndex)
        {
            for (int a = 0; a < col; a++)
                markIndex.Add(-1);
            Solve1(matrHung,  markIndex);//ищем решение
            return matrHung;
        }

        #region //ConvolutionMatr  // Свёртка критериев
        /// <summary>
        /// Свёртка критериев
        /// </summary>
        /// <param name="matr1"></param>
        /// <param name="matr2"></param>
        /// <param name="matr3"></param>
        public void ConvolutionMatr(double[,] matr1, double[,] matr2, ref double[,] matr3)
        {
            matr3 = new double[col, row];
            if (Static.flagChecked1!=true)
            {
                MinMax(ref matr1);
            }

            if (Static.flagChecked2 != true)
            {
                MinMax(ref matr2);
            }

            
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    matr3[i, j] = Math.Round(matr1[i, j] + matr2[i, j], 6);
                }
            }
        }
        #endregion

        #region //Solve1  //Поиск назначения (отмечаем нули)
        /// <summary>
        /// Поиск назначения (отмечаем нули)
        /// </summary>
        /// <param name="matr"></param>
        /// <param name="markIndex"></param>
        private void Solve1( double[,] matr, List<int> markIndex)
        {

            NullMatr(ref matrHung);//получаем матрицу с нулями  во всех строках и столбцах 
            MarkNullAll(ref matr, ref markIndex);

        }
        #endregion

        #region   //MinMax  //преобразует  матрицу  для решения на минимум
        /// <summary>
        /// преобразует  матрицу  для решения на минимум
        /// </summary>
        private void MinMax(ref double[,] matr)
        {

            double max = matr[0, 0];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (matr[i, j] > max)//ищем максимум 
                    {
                        max = matr[i, j];
                    }
                }
            }

            //преобразуем матрицу для решения на минимум
            if (max == 0)
            {
                return;
            }
            if (max <= 1)
            {
                max = 1;
            }       
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        matr[i, j] = max - matr[i, j];
                    }
                }          
        }
        #endregion

        #region//MarkNullAll  //отмечаем все возможные нули (ищем решение)
        /// <summary>
        /// отмечает все возможные нули
        /// </summary>
        /// <param name="matr"></param>
        /// <param name="markIndex">Список  индексов помеченных столбцов в строках</param>
        public void MarkNullAll(ref double[,] matr, ref List<int> markIndex)
        {
            List<int> iMarker = new List<int>();//список индексов строк отмеченных нулей
            List<int> jMarker = new List<int>();//список индексов столбцов отмеченных нулей
            bool flNull = true;//флаг-если хоть один нуль есть
            bool flag;//флаг решение не найдено
            flag = true;
            while (flag == true)//отмечаем нули 
            {
                flNull = false;
                MarkNull(matr, ref iMarker, ref jMarker, ref markIndex, ref flNull);//отмечаем  ноль



                flag = Find1(markIndex, -1);



                if (flag == false)//если все нули отмечены, выходим из цикла- решение найдено!
                {
                    return;
                }
                if (flNull == false)//Редукция матрицы, если нулей больше нет, а решение не найдено 

                {

                    AddNull(ref matr, iMarker, jMarker, markIndex);
                    iMarker.Clear();
                    jMarker.Clear();
                    markIndex.Clear();
                    for (int a = 0; a < col; a++)
                        markIndex.Add(-1);
                }
            }

        }
        #endregion

        #region//AddNull  //Редукция матрицы, добавляем нули
        /// <summary>
        /// Редукция матрицы, добавляем нули
        /// </summary>
        /// <param name="matr"></param>
        /// <param name="iMarker">список индексов строк отмеченных нулей</param>
        /// <param name="jMarker">список индексов столбцов отмеченных нулей</param>
        /// <param name="markIndex">Список  индексов помеченных столбцов в строках</param>
        private void AddNull(ref double[,] matr, List<int> iMarker, List<int> jMarker, List<int> markIndex)
        {
            List<int> markerRow = new List<int>();//список помеченных строк
            List<int> markerCol = new List<int>();//список помеченных столбцов
            List<int> listMarkerRowAdd = new List<int>();//Список добавленных строк к отмеченным
            List<int> listMarkerColAdd = new List<int>();//Список добавленных столбцов к отмеченным
            for (int i = 0; i < row; i++)
            {
                //ПЕРВЫЙ пункт
                if (Find1(iMarker, i) == false)//отмечаем строки в которых нет отмеченных нулей
                {
                    listMarkerRowAdd.Add(i);
                    markerRow.Add(i);//индексы не отмеченных строк
                }
            }

            //флаг пока есть что отмечать
            bool flag = true;

            while (flag)
            {
                flag = false;
                //ВТОРОЙ пункт
                //отмечаем столбцы, содержащие перечёркнутые нули в строках без отмеченых нулей
                foreach (int item in jMarker)//Проходим по столбцам с отмеченными нулями
                {
                    foreach (int item2 in listMarkerRowAdd)
                    {
                        if (matr[item2, item] == 0)
                        {
                            listMarkerColAdd.Add(item);
                            if (Find1(markerCol, item) == false)
                            {
                                markerCol.Add(item);//отмечаем столбцы с перечёркнутыми единицами в строке без отмеченных единиц
                            }
                            break;
                        }
                    }
                }

                listMarkerRowAdd.Clear();

                //ТРЕТИЙ пункт 
                //Отмечаем   все  строки,  содержащие  хотя  бы  один отмеченный  ноль  в  отмеченных  столбцах
                foreach (int item in iMarker)//проходим по строкам с отмеченными нулями
                {
                    foreach (int item2 in listMarkerColAdd)//проходим по отмеченным столбцам
                    {
                        if (Find2(markIndex, item2, item) == true)//Проверяем находится ли отмеченный ноль в столбце item2 и в строке item
                        {
                            listMarkerRowAdd.Add(item);
                            if (Find1(markerRow, item) == false)
                            {
                                markerRow.Add(item);//отмечаем строки 

                                flag = true;
                                break;//если отмеченная единица найдена, переходим к следующей строке
                            }
                        }

                    }
                }
                listMarkerColAdd.Clear();
            }


            ///////////////////
            double min = -1;
            int iii ;
            int jjj ;
            foreach (int i in markerRow)//ищем минимальный элемент из оставшихся
            {
                for (int j = 0; j < col; j++)
                {
                    if (Find1(markerCol, j) == false)
                    {
                        if (min == -1)
                        {
                            min = matr[i, j];
                        }
                        if (matr[i, j] < min)
                        {
                            min = matr[i, j];
                            if (min == 0)
                            {
                                 iii = i;
                                 jjj = j;
                            }
                        }
                    }
                }
            }

            foreach (int i in markerRow)//минимум отнимаем в отмеченный строках
            {
                for (int j = 0; j < col; j++)
                {
                    matr[i, j] -= min;
                }
            }
            foreach (int j in markerCol)//минимум прибавляем в отмеченных столбцах
            {
                for (int i = 0; i < row; i++)
                {
                    matr[i, j] += min;
                }
            }

            //округляем
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)

                {
                    matr[i, j] = Math.Round(matr[i, j], 6);
                }

            }
        }
        #endregion

        #region//Find2  //проверяет находится ли элемент в списке с индексом index
        private bool Find2(List<int> ts, int element, int index)
        {
            bool fl = false;
            if (IsNullOrEmpty(ts) == false)
            {
                if (element == ts[index])
                {

                    fl = true;
                }
            }
            return fl;
        }
        #endregion

        #region//IsNullOrEmpty  //проверяем пуст ли список или не существует
        public bool IsNullOrEmpty(List<int> items)
        {
            return items == null || !items.Any();
        }
        #endregion

        #region//Find1  //поиск элемента в списке
        private bool Find1(List<int> ts, int element)
        {
            bool fl = false;
            if (IsNullOrEmpty(ts) == false)
            {
                foreach (int item in ts)
                {
                    if (element == item)
                    {
                        fl = true;
                        break;
                    }
                }
                return fl;
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region //MarkNull  //отмечаем в матрице клетку с нулём 
        /// <summary>
        ///  отмечаем в матрице клетку с  нулём
        /// </summary>
        /// <param name="matr"></param>
        /// <param name="iMarker">список индексов строк отмеченных нулей</param>
        /// <param name="jMarker">список индексов столбцов отмеченных нулей</param>
        /// <param name="markIndex">Список  Индексов помеченных столбцов в строках</param>
        /// <param name="flNull">флаг-если хоть один нуль есть</param>
        private void MarkNull(double[,] matr, ref List<int> iMarker, ref List<int> jMarker, ref List<int> markIndex, ref bool flNull)
        {

            List<int> rowNum = NumRowMinNull(matr, iMarker, jMarker, ref flNull);//номера строк с минимальным количеством нулей
            List<int> colNum = NumColMinNull(matr, rowNum, iMarker, jMarker);//номера столбцов с мин. количеством нулей
            if (flNull == false)//если нулей больше нет(отмечать нечего)
            {
                return;
            }
            else
            {
                foreach (int item in rowNum)//проходим по строкам с минимумом нулей, ищем ноль
                {
                    if (matr[item, colNum[0]] == 0)
                    {
                        iMarker.Add(item);//отмечаем строку
                        jMarker.Add(colNum[0]);//отмечаем столбец
                        markIndex[item] = colNum[0];//вектор отмеченных нулей: где индексы-номера строк, а значения-номера столбцов
                        break;
                    }
                }
            }
        }
        #endregion

        #region//NullMatr  //преобразует матрицу с нулями в каждой строке и столбце
        /// <summary>
        /// преобразует матрицу с нулями в каждой строке и столбце
        /// </summary>
        /// <param name="matr"></param>
        private void NullMatr(ref double[,] matr)
        {

            List<double> u = new List<double>();
            List<double> v = new List<double>();
            for (int a = 0; a < row; a++)
                u.Add(0);
            for (int a = 0; a < col; a++)
                v.Add(0);
            double min;

            //ищем минимальные элементы по строкам и вычитаем
            for (int i = 0; i < row; i++)
            {
                min = matr[i, 0];
                for (int j = 1; j < col; j++)
                {
                    if (min > matr[i, j])
                    {
                        min = matr[i, j];
                    }
                }
                u[i] = min;
            }
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    matr[i, j] = matr[i, j] - u[i];
                }
            }

            //ищем минимальные элементы по столбцам и вычитаем
            for (int j = 0; j < col; j++)
            {
                min = matr[0, j];
                for (int i = 1; i < row; i++)
                {
                    if (min > matr[i, j])
                    {
                        min = matr[i, j];
                    }
                }
                u[j] = min;
            }
            for (int j = 0; j < row; j++)
            {
                for (int i = 0; i < col; i++)
                {
                    matr[i, j] = matr[i, j] - u[j];
                }
            }
        }//конец NullMatr
        #endregion


        #region//NumRowMinNull  //поиск номеров строк с минимальным количеством нулей
        private List<int> NumRowMinNull(double[,] matr, List<int> iMarker, List<int> jMarker, ref bool flNull)
        {
            List<int> numRow = new List<int>();//номера строк с минимумом нулей
            int[] countNullRow = new int[row];//количество нулей в строках
            for (int a = 0; a < row; a++)
                countNullRow[a] = -1;
            int min = -1;//минимальное количество нулей в строке
            int countNull;//количество нулей

            for (int i = 0; i < row; i++)//подсчёт количества нулей в каждой строке
            {

                if (Find1(iMarker, i) == true)//если строка отмечена то переходим к следующей
                {
                    continue;
                }
                countNull = 0;
                for (int j = 0; j < col; j++)
                {
                    if (Find1(jMarker, j) == true)//если столбец отмечен, то переходим к следующему
                    {
                        continue;
                    }
                    if (matr[i, j] == 0)
                    {
                        countNull++;//количество нулей в строке
                        flNull = true;//если нуль найден
                    }
                }
                if (countNull != 0)
                {
                    countNullRow[i] = countNull; //количество нулей в строках

                    if (min == -1)
                    {
                        min = countNull;
                    }
                    else
                    {
                        if (min > countNull)
                        {
                            min = countNull;     //минимальное количество нулей в строках                
                        }
                    }
                }
            }

            for (int item = 0; item < row; item++)//получаем список строк с минимальным количеством нулей
            {
                if (countNullRow[item] == min)
                {
                    numRow.Add(item);
                }
            }
            return numRow;
        }
        #endregion

        #region//NumColMinNull  //поиск номеров столбцов  с минимальным количеством нулей (в строках с минимальным количеством нулей)
        private List<int> NumColMinNull(double[,] matr, List<int> numRow, List<int> iMarker, List<int> jMarker)
        {
            List<int> numCol = new List<int>();//номера столбцов с минимальным количеством нулей
            int[] countNullCol = new int[col];//количество нулей в столбцах
            for (int a = 0; a < col; a++)
                countNullCol[a] = -1;
            int min = 0;//минимальное количество нулей в столбцах
            foreach (int item in numRow)//перебираем строки с минимальным кол-вом нулей
            {
                for (int j = 0; j < col; j++)//проходим по столбцам item строки
                {
                    if (Find1(jMarker, j) == true)//если столбец отмечен то переходим к следующему
                    {
                        continue;
                    }
                    if (matr[item, j] == 0)
                    {
                        int countNull = 0;
                        for (int i = 0; i < row; i++)//ищем нули в столбце j
                        {
                            if (Find1(iMarker, i) == true)//если строка отмечена то переходим к следующей
                            {
                                continue;
                            }
                            if (matr[i, j] == 0)
                            {
                                countNull++;

                            }

                        }
                        countNullCol[j] = countNull; //количество нулей в столбце j
                        if (min == 0)
                        {
                            min = countNull;
                        }
                        else
                        {
                            if (min > countNull)
                            {
                                min = countNull;     //минимальное количество нулей в столбцах                
                            }
                        }
                    }
                }
            }

            for (int j = 0; j < col; j++)//получаем список столбцов с минимальным количеством нулей
            {
                if (countNullCol[j] == min)
                {
                    numCol.Add(j);
                }

            }

            return numCol;
        }
        #endregion






    }
}


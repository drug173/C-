using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace naznachenie1
{
    class Hungarian
    {
        double[,] matrHung;//матрица назначений
        private int row = Static.rowEnd;
        private int col = Static.colEnd;
        ///Список  Индексов помеченных столбцов в строках         
       // List<int> markIndex = new List<int>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matr"></param>
        public Hungarian(double[,] matr1, double[,] matr2)
        {
            ConvolutionMatr(matr1, matr2, ref Static.matr3);
            matrHung = Static.matr3;
        }

        /// <summary>
        /// Модифицированный  венгерский алгоритм
        /// </summary>
        /// <param name="markIndex">//список индексов помеченых столбцов в строках</param>
        /// <returns></returns>
        public double[,] HungarianMethod(ref List<int> markIndex)
        {
            for (int a = 0; a < col; a++)
                markIndex.Add(-1);
            Solve1(matrHung, markIndex);//ищем решение        
            return matrHung;
        }

        #region   //MinMax  //преобразует  матрицу  для решения на минимум
        /// <summary>
        /// преобразует  матрицу  для решения на минимум
        /// </summary>
        private void MinMax(ref double[,] matr)
        {
            
            double max = matr[0, 0];
            for (int i = 1; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (matr[i, j] > max)//ищем максимум 
                    {
                        max = matr[i, j];
                    }
                }
            }
      
            
            if ((max < 1) || (max == 1))
            {                                   
                    max = 1;               
            }
            
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {

                    matr[i, j] = max / matr[i, j];

                }
            }


        }
        #endregion

        #region //ConvolutionMatr // свёртка критериев (мультипликативная)
        /// <summary>
        /// //Свёртка критериев
        /// </summary>
        /// <param name="matr1">Матрица первого критерия</param>
        /// <param name="matr2">Матрица второго критерия</param>
        /// <param name="matr3">Матрица свёртки критериев</param>
        private void ConvolutionMatr(double[,] matr1, double[,] matr2, ref double[,] matr3)
        {
           matr3 = new double[col, row];
           DelNul( ref matr1);
           DelNul(ref matr2);

            if (Static.flagChecked1 != true)
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
                        matr3[i, j] = Math.Round(matr1[i, j] * matr2[i, j], 6);
                    }
                }        
        }
        #endregion

        #region//DelNul // -замена нулей в матрице
        /// <summary>
        /// DelNul- избавляемся  от нулей  в матрице
        /// </summary>
        /// <param name="matr"></param>
        private void DelNul(ref double[,] matr)
        {
            double max = 0;
            double min = 0;
            for (int i = 0; i < row; i++)
            {

                for (int j = 0; j < col; j++)
                {
                    if (matr[i, j] == 0)
                    {
                        continue;
                    }
                    if (max == 0)
                    {
                        min = matr[i, j];
                        max = matr[i, j];
                        continue;
                    }

                    if (matr[i, j] > max)//ищем максимальный элемент в матрице
                    {
                        max = matr[i, j];
                    }
                    if (matr[i, j] < min)//ищем минимальный элемент  матрицы
                    {
                        min = matr[i, j];
                    }
                }
            }

            //если  все элементы ноль, меняем на 0.5
            if (max == 0)
            {
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        matr[i, j] = 0.5;

                    }
                }
            }
            else//меняем нули на заведомо маленькое значение
            {
                double x = Math.Pow(min, col) / (2 * Math.Pow(max, col - 1));//число заменяющее нули
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        if (matr[i, j] == 0)//если элемент ноль меняем на х
                        {
                            matr[i, j] = x;
                        }
                    }
                }
            }
        }
        #endregion

        #region// Solve1 // -поиск назначения
        /// <summary>
        ///Solve1 -  Поиск назначения 
        /// </summary>
        /// <param name="matr"></param>
        /// <param name="markIndex">Список  индексов помеченных столбцов в строках</param>
        private void Solve1(double[,] matr, List<int> markIndex)
        {
            SingleMatr(matr);//получаем матрицу с единицами  во всех строках и столбцах 
            MarkUnitAll(matr, markIndex);//делаем назначение
        }
        #endregion

        #region//MarkUnitAll // отмечает единицы
        /// <summary>
        ///MarkUnitAll - отмечает все возможные единицы
        /// </summary>
        /// <param name="matr"></param>
        /// <param name="markIndex">Список  индексов помеченных столбцов в строках</param>
        public void MarkUnitAll(double[,] matr, List<int> markIndex)
        {
            List<int> iMarker = new List<int>();//список индексов строк отмеченных единиц
            List<int> jMarker = new List<int>();//список индексов столбцов отмеченных единиц
            bool flNull = true;//флаг-если хоть одна единица есть
            bool flag;//флаг решение не найдено
            flag = true;


            while (flag == true)//отмечаем единицы
            {
                flNull = false;
                MarkUnit(matr, iMarker, jMarker, markIndex, ref flNull);//отмечаем  единицу

                flag = Find1(markIndex, -1);

                if (flag == false)//если все единицы отмечены, выходим из цикла- решение найдено!
                {
                    return;
                }
                if (flNull == false)//Редукция матрицы, если единиц больше нет, а решение не найдено 

                {

                    AddUnits(ref matr, iMarker, jMarker, markIndex);
                    iMarker.Clear();
                    jMarker.Clear();
                    markIndex.Clear();
                    for (int a = 0; a < col; a++)
                        markIndex.Add(-1);
                }
            }

        }
        #endregion

        #region//AddUnits // редукция матрицы (добавляем единицы)
        /// <summary>
        ///AddUnits - Редукция матрицы, получаем единицы
        /// </summary>
        /// <param name="matr"></param>
        /// <param name="iMarker">список индексов строк отмеченных единиц</param>
        /// <param name="jMarker">список индексов столбцов отмеченных единиц</param>
        /// <param name="markIndex">Список  Индексов помеченных столбцов в строках</param>
        private void AddUnits(ref double[,] matr, List<int> iMarker, List<int> jMarker, List<int> markIndex)
        {
            List<int> markerRow = new List<int>();//список помеченных строк           
            List<int> markerCol = new List<int>();//список помеченных столбцов
            List<int> listMarkerRowAdd = new List<int>();//Список добавленных строк к отмеченным
            List<int> listMarkerColAdd = new List<int>();//Список добавленных столбцов к отмеченным
            for (int i = 0; i < row; i++)
            {
                //ПЕРВЫЙ пункт поиска минимального набора строк и столбцов
                //отмечаем строки в которых нет отмеченных единиц
                if (Find1(iMarker, i) == false)
                {
                    listMarkerRowAdd.Add(i);
                    markerRow.Add(i);//отмечаем строки c неотмеченными  единицами
                }
            }

            //флаг пока есть что отмечать
            bool flag = true;

            while (flag)
            {
                flag = false;
                //ВТОРОЙ пункт 
                //отмечаем столбцы, содержащие перечёркнутые единицы в строках без отмеченых единиц
                foreach (int item in jMarker)//Проходим по столбцам с отмеченными единицами
                {
                    foreach (int item2 in listMarkerRowAdd)//проходим по новым помеченным строкам
                    {
                        if (matr[item2, item] == 1)
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
                //Отмечаем   все  строки,  содержащие  хотя  бы  одну отмеченную  единицу  в  отмеченных  столбцах
                foreach (int item in iMarker)//проходим по строкам с отмеченными единицами
                {
                    foreach (int item2 in listMarkerColAdd)//проходим по новым отмеченным столбцам
                    {
                        if (Find2(markIndex, item2, item) == true)//Проверяем находится ли отмеченная единица в столбце item2 и в строке item
                        {
                            listMarkerRowAdd.Add(item);
                            if (Find1(markerRow, item) == false)
                            {
                                markerRow.Add(item);//отмечаем строки 

                                flag = true;
                                break;//если отмеченный ноль найден, переходим к следующей строке
                            }
                        }
                    }                    
                }
                listMarkerColAdd.Clear();
            }

                //////
                double min = -1;
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
                            }
                        }
                    }
                }

                foreach (int i in markerRow)//делим  на минимум  в отмеченных строках
                {
                    for (int j = 0; j < col; j++)
                    {
                        matr[i, j] = matr[i, j] / min;
                    }
                }
                foreach (int j in markerCol)//умножаем  на минимум  в отмеченных столбцах
                {
                    for (int i = 0; i < row; i++)
                    {
                        matr[i, j] = matr[i, j] * min;

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

            #region//Find2 // проверяет находится ли элемент в списке с индексом index
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

            #region// IsNullOrEmpty // проверяем пуст ли список или не существует
            public bool IsNullOrEmpty(List<int> items)
            {
                return items == null || !items.Any();
            }
            #endregion

            #region//Find1 // поиск элемента в списке
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

            #region //MarkUnit // отмечает единицу
            /// <summary>
            /// MarkUnit - отмечаем в матрице клетку с  единицей
            /// </summary>
            /// <param name="matr"></param>
            /// <param name="iMarker">список индексов строк отмеченных единиц</param>
            /// <param name="jMarker">список индексов столбцов отмеченных единиц</param>
            /// <param name="markIndex">Список  Индексов помеченных столбцов в строках</param>
            /// <param name="flNull">флаг-если хоть одна единица есть</param>
            private void MarkUnit(double[,] matr, List<int> iMarker, List<int> jMarker, List<int> markIndex, ref bool flNull)
            {
                List<int> rowNum = NumRowMinUnit(matr, iMarker, jMarker, ref flNull);//номера строк с минимальным количеством единиц
                List<int> colNum = NumColMinUnit(matr, rowNum, iMarker, jMarker);//номера столбцов с мин. количеством единиц
                if (flNull == false)//если единиц больше нет(отмечать нечего)
                {
                    return;
                }
                else
                {
                    foreach (int item in rowNum)//проходим по строкам с минимумом единиц, ищем единицу
                    {
                        foreach (int item2 in colNum)
                        {
                            if (matr[item, item2] == 1)
                            {
                                iMarker.Add(item);//отмечаем строку
                                jMarker.Add(item2);//отмечаем столбец
                                markIndex[item] = item2;//вектор  отмеченных  единиц:  где  индексы-номера  строк,  а  значения-номера  столбцов
                                return;
                            }
                        }
                    }
                }
            }
        #endregion

        #region//SingleMatr // преобразует матрицу с единицами в каждой строке и столбце
        /// <summary>
        /// преобразует матрицу с единицами в каждой строке и столбце
        /// </summary>
        /// <param name="matr"></param>
        //ищем минимальные значения в каждой строке и столбце и делим на них соответствующие элементы строк и столбцов
        private void SingleMatr(double[,] matr)
            {
                List<double> u = new List<double>();
                List<double> v = new List<double>();
                for (int a = 0; a < row; a++)
                    u.Add(1);
                for (int a = 0; a < col; a++)
                    v.Add(1);
                double min;
                //ищем минимальные элементы по строкам и делим
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
                        matr[i, j] = matr[i, j] / u[i];

                    }
                }
                //ищем минимальные элементы по столбцам и делим
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
                    v[j] = min;
                }
                for (int j = 0; j < row; j++)
                {
                    for (int i = 0; i < col; i++)
                    {
                        matr[i, j] = matr[i, j] / v[j];
                    }
                }

                //округляем
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        matr[i, j] = Math.Round((matr[i, j]), 6);

                    }
                }
   
            }//конец SingleMatr
            #endregion

            #region//NumRowMinUnit // поиск номеров строк с минимальным количеством единиц
            private List<int> NumRowMinUnit(double[,] matr, List<int> iMarker, List<int> jMarker, ref bool flNull)
            {
                List<int> numRow = new List<int>();//номера строк с минимумом единиц
                int[] countNullRow = new int[row];//количество единиц в строках
                for (int a = 0; a < row; a++)
                    countNullRow[a] = -1;
                int min = -1;//минимальное количество единиц в строках
                int countNull;//количество единиц

                for (int i = 0; i < row; i++)//подсчёт количества единиц в каждой строке
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
                        if (matr[i, j] == 1)//единица найдена
                        {
                            countNull++;//количество единиц в строке
                            flNull = true;//если единица найдена
                        }
                    }
                    if (countNull != 0)
                    {
                        countNullRow[i] = countNull; //количество единиц в i-ой строке

                        if (min == -1)
                        {
                            min = countNull;
                        }
                        else
                        {
                            if (min > countNull)
                            {
                                min = countNull;     //минимальное количество единиц в строках                
                            }
                        }
                    }
                }
                for (int item = 0; item < row; item++)//получаем список строк с минимальным количеством единиц
                {
                    if (countNullRow[item] == min)
                    {
                        numRow.Add(item);
                    }
                }
                return numRow;
            }
            #endregion

            #region//NumColMinUnit // поиск номеров столбцов  с минимальным количеством единиц
            private List<int> NumColMinUnit(double[,] matr, List<int> numRow, List<int> iMarker, List<int> jMarker)
            {
                List<int> numCol = new List<int>();//номера столбцов с минимальным количеством единиц
                int[] countNullCol = new int[col];//количество единиц в столбцах
                for (int a = 0; a < col; a++)
                    countNullCol[a] = -1;
                int min = 0;//минимальное количество единиц в столбцах
                foreach (int item in numRow)//перебираем строки с минимальным кол-вом единиц
                {
                    for (int j = 0; j < col; j++)//проходим по столбцам item строки
                    {
                        if (Find1(jMarker, j) == true)//если столбец отмечен то переходим к следующему
                        {
                            continue;
                        }
                        if (matr[item, j] == 1)
                        {
                            int countNull = 0;
                            for (int i = 0; i < row; i++)//ищем единицы в столбце j
                            {
                                if (Find1(iMarker, i) == true)//если строка отмечена то переходим к следующей
                                {
                                    continue;
                                }
                                if (matr[i, j] == 1)
                                {
                                    countNull++;

                                }

                            }
                            countNullCol[j] = countNull; //количество единиц в столбце j
                            if (min == 0)
                            {
                                min = countNull;
                            }
                            else
                            {
                                if (min > countNull)
                                {
                                    min = countNull;     //минимальное количество единиц в столбцах                
                                }
                            }
                        }
                    }
                }

                for (int j = 0; j < col; j++)//получаем список столбцов с минимальным количеством единиц
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


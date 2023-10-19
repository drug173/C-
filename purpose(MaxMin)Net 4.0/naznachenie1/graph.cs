using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace naznachenie1
{
    public partial class graph : Form
    {
        Chart chart;
        //Левая граница графика
        double XMin = 0;
        //Правая граница графика
        double XMax = 9;
        //Шаг сетки графика
        double Step = 0.1;
        double[,] matr1;
        double[,] matr2;
        double[] x3 = { 0, 0 };
        double[] y3 = { 0, 0 };
        double[] x2 = { 0, 0 };
        double[] y2 = { 0, 0 };
        double Optim = 0;
        double OptimM = 0;
        public graph(double[,] matrFirst, double[,] matrSecond)
        {
            InitializeComponent();
            //вычисляем количество точек графика 
            int count = Factorial(Static.colEnd);
            // Массив значений X и Y
            double[] x = new double[count];
            double[] y = new double[count];
            chart = new Chart();
            matr1 = matrFirst;
            matr2 = matrSecond;
            ///////////////////////////////////////////////////
            //вычисление точек для назначений
            List<PairDouble> p = PointChart(matr1, matr2);
            int index = 0;
            double maxIndexX = 0;
            double maxIndexY = 0;
            double minIndexX = p[0].First;
            foreach (var item in p)
            {
                x[index] = item.First;
                if (x[index] > maxIndexX)
                {
                    maxIndexX = x[index];
                }
                if (x[index] < minIndexX)
                {
                    minIndexX = x[index];
                }
                y[index] = item.Second;
                if (y[index] > maxIndexY)
                {
                    maxIndexY = y[index];
                }
                index++;
            }

            if (maxIndexY < maxIndexX)
            {
                XMax = maxIndexX;///////////////////
            }
            else
            {
                XMax = maxIndexY;////////////////
            }


            double Step1;
            {
                //задаём шаг сетки графиков
                if (XMax <= 1)
                {
                    Step = XMax / 9;

                }
                else if (XMax < 5)
                {
                    Step = 0.5;
                }
                else if (XMax < 10)
                {
                    Step = 1;
                }
                else if (XMax < 50)
                {
                    Step = 5;
                }
                else if (XMax < 100)
                {
                    Step = 10;
                }
                else
                {
                    Step = XMax / 9;
                }
            }
            XMax += Step;
            if (XMax == 0)
            {
                XMax = 0.1;
            }


            ////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////
            ///  ищем  оптимальные  значения  назначения
            /////////////////////////////////////////////////////////////
            //Вычисление точек для графика Add свёртки

            //  для Мат ожидания
            if (Static.flagConvolution == false)
            {

                
                if ((Static.flagChecked1 != true && Static.flagChecked2 != true) || (Static.flagChecked1 == true && Static.flagChecked2 == true))
                {

                    x2[1] = Expectation(Static.result, matr1) + Expectation(Static.result, matr2);
                    y2[0] = x2[1];
                    
                }
                else if(Static.flagChecked1 != true && Static.flagChecked2 == true)
                {
                    Optim = Expectation(Static.result, matr1) + Static.colEnd - Expectation(Static.result, matr2);
                    y2[1] = 0;
                    x2[1] = y2[0] - Static.colEnd + Optim;
                    x2[0] = XMax;
                    y2[0] = Static.colEnd + x2[0] - Optim;
                   
                }
                else
                {
                    Optim = Expectation(Static.result, matr2) + Static.colEnd - Expectation(Static.result, matr1);
                    x2[1] = XMax; 
                    y2[0] = x2[0] + Optim - Static.colEnd;
                    y2[1] = x2[1] + Optim - Static.colEnd;
                }

            }
            else // для  полной  вероятности  поражения  всех  целей
            {
                Optim = OptimAdd(p);
                if ((Static.flagChecked1 != true && Static.flagChecked2 != true) || (Static.flagChecked1 == true && Static.flagChecked2 == true))
                {

                    x2[1] = Optim;
                    y2[0] = x2[1];

                }
                else if (Static.flagChecked1 != true && Static.flagChecked2 == true)
                {
                   
                    y2[1] = 0;
                    x2[1] = y2[0] - Static.colEnd + Optim;
                    x2[0] = XMax;
                    y2[0] = Static.colEnd + x2[0] - Optim;

                }
                else
                {
                   
                    x2[1] = XMax;
                    y2[0] = x2[0] + Optim - Static.colEnd;
                    y2[1] = x2[1] + Optim - Static.colEnd;
                }


            }

            //////////////////////////////////////////////////////////////
            //Вычисление точек для графика Mult свёртки
            //  для  полной  вероятности  поражения  всех  целей
            if (Static.flagConvolution != false)
            {
                if ((Static.flagChecked1 != true && Static.flagChecked2 != true) || (Static.flagChecked1 == true && Static.flagChecked2 == true))
                {
                    //Шаг точек графика Mult свёртки
                    int count1 = 1500;
                    double d = (int)XMax;
                    OptimM = OptimMult(p);
                    if (d > 1)
                    {
                        count1 = (int)(d * 12) + 15;
                    }

                    double[] xx3 = new double[count1 + 2];
                    double[] yy3 = new double[count1 + 2];
                    Step1 = XMax / count1;
                    XMax += Step;
                    xx3[0] = minIndexX / 20;
                    if (minIndexX > 0)
                    {
                        yy3[0] = OptimM / xx3[0];
                    }
                    else
                    {
                        yy3[0] = OptimM / (Step1 / 2);
                    }

                    for (int i = 1; i < count1 + 1; i++)
                    {
                        xx3[i] = minIndexX / 10 + Step1 * i;
                        yy3[i] = OptimM / xx3[i];
                    }
                    xx3[count1 + 1] = XMax;
                    yy3[count1 + 1] = OptimM / xx3[count1 + 1];
                    x3 = xx3;
                    y3 = yy3;
                }
                else if (Static.flagChecked1 != true && Static.flagChecked2 == true)
                {                   
                    y3[0] = 0;
                    x3[0] = 0;
                    x3[1] = XMax;
                    y3[1] = x3[1] / OptimMult(p);

                }
                else
                {
                    OptimM = OptimMult(p);
                    y3[0] = 0;
                    x3[0] = 0;
                    x3[1] = XMax;
                    y3[1] = x3[1] * OptimMult(p);
                }

            }
            else // Для Мат Ожидания
            {
               
                if ((Static.flagChecked1 != true && Static.flagChecked2 != true) || (Static.flagChecked1 == true && Static.flagChecked2 == true))
                {

                    //Шаг точек графика Mult свёртки

                    int count1 = 1500;
                    double d = (int)XMax;
                    if (d > 1)
                    {
                        count1 = (int)(d * 12) + 15;
                    }

                    double[] xx3 = new double[count1 + 2];
                    double[] yy3 = new double[count1 + 2];
                    Step1 = XMax / count1;
                    XMax += Step;
                    xx3[0] = minIndexX / 20;
                    if (minIndexX > 0)
                    {
                        yy3[0] = OptimMult(p) / xx3[0];
                    }
                    else
                    {
                        yy3[0] = OptimMult(p) / (Step1 / 2);
                    }

                    for (int i = 1; i < count1 + 1; i++)
                    {
                        xx3[i] = minIndexX / 10 + Step1 * i;
                        yy3[i] = OptimMult(p) / xx3[i];
                    }
                    xx3[count1 + 1] = XMax;
                    yy3[count1 + 1] = OptimMult(p) / xx3[count1 + 1];
                    x3 = xx3;
                    y3 = yy3;
                }
                else if (Static.flagChecked1 != true && Static.flagChecked2 == true)
                {

                    y3[0] = 0;
                    x3[0] = 0;
                    x3[1] = XMax;
                    y3[1] = x3[1] / OptimMult(p);

                }
                else
                {

                    OptimM = OptimMult(p);
                    y3[0] = 0;
                    x3[0] = 0;
                    x3[1] = XMax;
                    y3[1] = x3[1] * OptimMult(p);

                }


            }







            //Создаем график
            CreateChart();
            // Добавляем вычисленные значения в графики
            chart.Series[0].Points.DataBindXY(x, y);
            chart.Series[1].Points.DataBindXY(x2, y2);
            chart.Series[2].Points.DataBindXY(x3, y3);
        }
        //////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////

        #region //MaxMult  // находит максимальное значение свёртки Mult
        /// <summary>
        /// находит максимальное значение свёртки Mult
        /// </summary>
        /// <param name="pairDouble"></param>
        /// <returns></returns>
        private double MaxMult(List<PairDouble> pairDouble)
        {
            double Max = 0;
            foreach (var item in pairDouble)
            {
                if (Max < item.First * item.Second)
                {
                    Max = item.First * item.Second;
                }
            }
            return Max;
        }
        #endregion

        #region // находит минимальное значение свёртки Mult
        /// <summary>
        ///  находит минимальное значение свёртки Mult
        /// </summary>
        /// <param name="pairDouble"></param>
        /// <returns></returns>
        private double MinMult(List<PairDouble> pairDouble)
        {
            double Min = pairDouble[0].First * pairDouble[0].Second;
            foreach (var item in pairDouble)
            {
                if (Min > item.First * item.Second)
                {
                    Min = item.First * item.Second;
                }
            }
            return Min;
        }
        #endregion


        #region  //OptimMult
        private double OptimMult(List<PairDouble> pairDouble) {
            double OptimM1;
            if (Static.flagChecked1 != true && Static.flagChecked2 != true)
            {

                OptimM1 = pairDouble[0].First * pairDouble[0].Second;
                foreach (var item in pairDouble)
                {
                    if (OptimM1 < item.First * item.Second)
                    {
                        OptimM1 = item.First * item.Second;
                    }
                }
            }
            else if (Static.flagChecked1 == true && Static.flagChecked2 == true)
            {
                OptimM1 = pairDouble[0].First * pairDouble[0].Second;
                foreach (var item in pairDouble)
                {
                    if (OptimM1 > item.First * item.Second)
                    {
                        OptimM1 = item.First * item.Second;
                    }
                }
            }
            else if (Static.flagChecked1 != true && Static.flagChecked2 == true)
            {
                OptimM1 = pairDouble[0].First / pairDouble[0].Second;
                foreach (var item in pairDouble)
                {
                    if (OptimM1 < item.First / item.Second)
                    {
                        OptimM1 = item.First / item.Second;
                    }
                }
            }
            else 
            {
                OptimM1 = pairDouble[0].Second / pairDouble[0].First;
                foreach (var item in pairDouble)
                {
                    if (OptimM1 < item.Second / item.First)
                    {
                        OptimM1 = item.Second / item.First;
                    }
                }
            }

            return OptimM1;
        }
        #endregion

        #region // OptimAdd находит оптимальное значение свёртки Mult методом перебора
        /// <summary>
        /// OptimAdd находит оптимальное значение свёртки Mult
        /// </summary>
        /// <param name="pairDouble"></param>
        /// <returns></returns>
        private double OptimAdd(List<PairDouble> pairDouble)
        {
            

            if ( Static.flagChecked1 != true && Static.flagChecked2 != true)
            {

                Optim = pairDouble[0].First + pairDouble[0].Second;
                foreach (var item in pairDouble)
                {
                    if (Optim < item.First + item.Second)
                    {
                        Optim = item.First + item.Second;
                    }
                }
            }
            else if (Static.flagChecked1 == true && Static.flagChecked2 == true) 
            {
                Optim = pairDouble[0].First + pairDouble[0].Second;
                foreach (var item in pairDouble)
                {
                    if (Optim > item.First + item.Second)
                    {
                        Optim = item.First + item.Second;
                    }
                }
            }
            else if(Static.flagChecked1 != true && Static.flagChecked2 == true) 
            {
                Optim = pairDouble[0].First + Static.colEnd - pairDouble[0].Second;
                foreach (var item in pairDouble)
                {
                    if (Optim < item.First + Static.colEnd - item.Second)
                    {
                        Optim = item.First + Static.colEnd - item.Second;
                    }
                }
            }
            else if (Static.flagChecked1 == true && Static.flagChecked2 != true)
            {
                Optim = pairDouble[0].Second + Static.colEnd - pairDouble[0].First;
                foreach (var item in pairDouble)
                {
                    if (Optim < item.Second + Static.colEnd - item.First)
                    {
                        Optim = item.Second + Static.colEnd - item.First;
                    }
                }
            }
            return Optim;
        }
        #endregion

        #region// Expectation  значение  Мат ожидания
        /// <summary>
        /// значение  Мат ожидания
        /// </summary>
        /// <param name="result"></param>
        /// <param name="matr"></param>
        /// <returns></returns>
        private double Expectation(List<int> result, double[,] matr)
        {
            double d = 0;
            int i = 0;
            foreach (var item in result)
            {
                d += matr[i, item];
                i++;
            }
            return d;
        }
        #endregion

        #region//  Probability Значение  вероятности  поражения всех целей
        /// <summary>
        ///  Значение  вероятности  поражения всех целей
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private double Probability(List<int> result, double[,] matr)
        {
            double d = 1;
            int i = 0;
            foreach (var item in result)
            {
                d *= matr[i, item];
                i++;
            }
            return d;
        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region // PointChart // Вычисляет (точки графика)  массив стоимостей всех возможных назначений
        /// <summary>
        ///  Вычисляет (точки графика)  массив стоимостей всех возможных назначений
        /// </summary>
        /// <param name="matr1"></param>
        /// <param name="matr2"></param>
        /// <returns></returns>
        private List<PairDouble> PointChart(double[,] matr1, double[,] matr2)
        {           
            //массив точек графика
            List<PairDouble> masPoint = new List<PairDouble>();
            
            int n=Static.colEnd;
                int[]mas;
           
            mas = new int[n];
            for (int i = 0; i < n; i++)
                mas[i] = i + 1;
            masPoint.Add( MasToPoint(mas));
            while (NextSet(mas, n))
              masPoint.Add( MasToPoint(mas));



            return masPoint;
        }
        #endregion

        #region// NextSet //перебирает все возможные перестановки
        /// <summary>
        /// перебирает все возможные перестановки
        /// </summary>
        /// <param name="mas"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        bool NextSet(int[] mas, int n)
        {
            int j = n - 2;
            while (j != -1 && mas[j] >= mas[j + 1]) j--;
            if (j == -1)
                return false; // больше перестановок нет
            int k = n - 1;
            while (mas[j] >= mas[k]) k--;
            Swap(mas, j, k);
            int l = j + 1, r = n - 1; // сортируем оставшуюся часть последовательности
            while (l < r)
                Swap(mas, l++, r--);
            return true;
        }
        #endregion

        #region// Swap // меняет местами элементы массива
        /// <summary>
        /// меняет местами элементы массива
        /// </summary>
        /// <param name="a"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        void Swap(int[] mas, int i, int j)
        {
            int s = mas[i];
            mas[i] = mas[j];
            mas[j] = s;
        }
        #endregion

        #region// MasToPoint //Вычисляет координаты точки текущего назначения
        /// <summary>
        /// Вычисляет координаты точки текущего назначения
        /// </summary>
        /// <param name="mas">массив где значение номер столбца, а индекс номер строки </param>
        /// <returns></returns>
        private PairDouble MasToPoint(int[]mas)
        {
            PairDouble pair;
            if (Static.flagConvolution == true)
            {
                 pair = new PairDouble(1, 1);
                for (int i = 0; i < Static.colEnd; i++)
                {
                    pair = pair * (new PairDouble(matr1[i, mas[i] - 1], matr2[i, mas[i] - 1]));
                }
            }
            else
            {
                 pair = new PairDouble(0, 0);
                for (int i = 0; i < Static.colEnd; i++)
                {
                    pair = pair + (new PairDouble(matr1[i, mas[i] - 1], matr2[i, mas[i] - 1]));
                }

            }
            return pair;
        }
        #endregion
      
        #region//PairDouble // координаты точки графика
        /// <summary>
        /// 
        /// </summary>
        public class PairDouble
        {
            public PairDouble(double a1, double a2)
            {
                First = a1;
                Second = a2;
            }
            public PairDouble()
            {

            }

            //сравнение пар
            public bool Compare(PairDouble pair1)
            {
                bool fl = true;
                if (pair1.Second == this.Second && pair1.First == this.First)
                {
                    fl = true;
                }
                else
                {
                    fl = false;
                }
                return fl;
            }

            public static PairDouble operator *(PairDouble c1, PairDouble c2)
            {
                return new PairDouble { First = c1.First * c2.First, Second = c1.Second * c2.Second };
            }

            public static PairDouble operator +(PairDouble c1, PairDouble c2)
            {
                return new PairDouble { First = c1.First + c2.First, Second = c1.Second + c2.Second };
            }

            public void SetPair(double b1, double b2)
            {
                this.First = b1;
                this.Second = b2;             
            }
            public double First { get; set; }
            public double Second { get; set; }
        }
        #endregion

        #region // Factorial // Факториал 
        private int Factorial(int n)
        {
            var factorial = 1;
            for (int i = 1; i <= n; i++)
                factorial *= i;
            return factorial;
        }
        #endregion

        #region //CreateChart   // Создание графика   
        private void CreateChart()
        {
          
            chart.Parent = this;
            chart.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left);
            // Задаѐм размеры элемента
            chart.SetBounds(10, 10, ClientSize.Width - 20, ClientSize.Height - 20);
            ChartArea area = new ChartArea();
            area.Name = "GraphMulty";
            // Задаѐм левую и правую границы оси X и Y
            area.AxisX.Minimum = XMin;
            area.AxisY.Minimum = XMin;
            area.AxisY.Maximum = XMax;
            area.AxisX.Maximum = XMax;
            // Определяем шаг сетки
            area.AxisX.MajorGrid.Interval = Step;
            area.AxisY.MajorGrid.Interval = Step;
            // Добавляем область в диаграмму
            chart.ChartAreas.Add(area);

            // Создаѐм объект для первого графика
            Series series1 = new Series();
            // Ссылаемся на область для построения графика
            series1.ChartArea = "GraphMulty";
            // Задаѐм тип графика -сплайны
            series1.ChartType = SeriesChartType.Point;
            // Указываем ширину линии графика
            series1.BorderWidth = 4;
            // Название графика для отображения в легенде
            if (Static.flagConvolution == true)
            {
                series1.LegendText = "Значения критериев (вероятность поражения всех целей)";
            }
            else
            {
                series1.LegendText = "Значения критериев (мат. ожидание)";
            }
            // Добавляем в список графиков диаграммы
            chart.Series.Add(series1);

            // Создаѐм объект для  графика Add свёртки
            Series series2 = new Series();
            // Ссылаемся на область для построения графика
            series2.ChartArea = "GraphMulty";
            // Задаѐм тип графика -сплайны
            series2.ChartType = SeriesChartType.Spline;
            // Указываем ширину линии графика
            series2.BorderWidth = 1;
            // Название графика для отображения в легенде
            series2.LegendText = "Оптимальное значение Add-свёртки";
            // Добавляем в список графиков диаграммы
            chart.Series.Add(series2);


            // Создаѐм объект для  графика Mult свёртки
            Series series3 = new Series();
            // Ссылаемся на область для построения графика
            series3.ChartArea = "GraphMulty";
            // Задаѐм тип графика -сплайны
            series3.ChartType = SeriesChartType.Spline;
            // Указываем ширину линии графика
            series3.BorderWidth = 1;
            // Название графика для отображения в легенде
            series3.LegendText = "Оптимальное значение Mult-свёртки";
            // Добавляем в список графиков диаграммы
            chart.Series.Add(series3);

            // Создаѐм легенду, которая будет показывать названия
            Legend legend = new Legend();
            chart.Legends.Add(legend);
            


        }
        #endregion
       
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void graph_Load(object sender, EventArgs e)
        {

        }
    }
}

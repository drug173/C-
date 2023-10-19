using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace naznachenie1
{
    public partial class Form1 : Form
    {

        double[,] matrA;
        double[,] matrB;
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }
    

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
           int rowCount = Convert.ToInt32(numeric1.Value);
            if (rowCount < 1) { rowCount = 1; } numeric1.Value = rowCount;
            grid1.RowCount = rowCount;
            int colCount = rowCount;
            Random rnd = new Random();
            double num=1.0;
            grid1.ColumnCount = colCount;

            grid2.RowCount = rowCount;
         
            grid2.ColumnCount = colCount;

            grid1.Rows[rowCount - 1].HeaderCell.Value = "С" + rowCount.ToString();
            grid1.Columns[colCount - 1].HeaderCell.Value = "Ц" + colCount.ToString();



            grid2.Rows[rowCount - 1].HeaderCell.Value = "С" + rowCount.ToString();
            grid2.Columns[colCount - 1].HeaderCell.Value = "Ц" + colCount.ToString();

            for( int i=0; i<colCount; i++)
            {
                
              
                num = rnd.Next(4, 10)/10.0;
                grid2.Rows[i].Cells[rowCount - 1].Value = num;
                num = rnd.Next(3, 10)/10.0;
                grid2.Rows[colCount-1].Cells[i].Value = num;
                num = rnd.Next(3, 10)/10.0;
                grid1.Rows[i].Cells[rowCount - 1].Value = num;
                num = rnd.Next(4,10)/10.0;
                grid1.Rows[colCount - 1].Cells[i].Value = num;
            }

           

            Static.colEnd = colCount;
            Static.rowEnd = rowCount;
        }

       

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void grid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProg newForm = new FormProg();
            newForm.Show();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }


        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Static.flagMatr = true;
            try
            {                                    
                        if (Static.colEnd < 1)
                        {
                            MessageBox.Show(
                            "Проверьте правильность ввода, нет данных!",
                            "Ошибка ввода",
                            MessageBoxButtons.RetryCancel,
                            MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                            Static.flagMatr = false;
                        }              
            }
            catch
            {
                MessageBox.Show(
                  "Проверьте правильность ввода, нет данных!",
                  "Ошибка ввода",
                  MessageBoxButtons.RetryCancel,
                  MessageBoxIcon.Warning,
                  MessageBoxDefaultButton.Button1,
                  MessageBoxOptions.DefaultDesktopOnly);
                Static.flagMatr = false;
            }

            Static.matr1 = Static.Matr(grid1, Static.colEnd);
            if (Static.flagMatr == true)
            {
                matrA = Static.MatrCopy(Static.matr1);
                Static.matr2 = Static.Matr(grid2, Static.colEnd);
                if (Static.flagMatr == true)
                {
                    matrB = Static.MatrCopy(Static.matr2);
                  

                    FormEnd newForm = new FormEnd(matrA,matrB);                  
                    newForm.Show();

                    if (Static.colEnd < 9)
                    {
                        graph newGraf = new graph(Static.matr1, Static.matr2);
                       newGraf.Show();
                    }
                }
            }
        }
        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProg newForm = new FormProg();
            newForm.Show();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void grid2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                Static.flagConvolution = false;

            }
            else
            {
                Static.flagConvolution = true;
            }

        }

        private void minBox_CheckedChanged(object sender, EventArgs e)
        {


            CheckBox checkBox = (CheckBox)sender; // приводим отправителя к элементу типа CheckBox
            if (checkBox.Checked == true)
            {

                Static.flagChecked1 = true;
            }
            else
            {
                Static.flagChecked1 = false;

            }


        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

            CheckBox checkBox = (CheckBox)sender;  // приводим отправителя к элементу типа CheckBox
            if (checkBox.Checked == true)
            {

                Static.flagChecked2 = true;
            }
            else
            {
                Static.flagChecked2 = false;

            }


        }
    }
}

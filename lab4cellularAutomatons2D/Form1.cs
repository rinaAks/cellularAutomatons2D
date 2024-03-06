using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4cellularAutomatons2D
{
    public partial class Form1 : Form
    {
        private int[,] cells;
        const int numberHorizontalCells = 14;
        const int numberVerticalCells = 12;


        int[] rule(int it)
        {
            int[] ruleReturn = { 0, it, 0, it, 0, it, 0, it, 0 };
            return ruleReturn;
        }
        public Form1()
        {
            InitializeComponent();
            cells = new int[numberHorizontalCells, numberVerticalCells];

            var grid = new MatrixGrid()
            {
                Parent = panel1,
                Dock = DockStyle.Fill,
                GridSize = new Size(numberHorizontalCells, numberVerticalCells),
                BorderStyle = BorderStyle.FixedSingle,
                //Location = new Point(100, 100) 
            };
            grid.CellNeeded += grid_CellNeeded;
            grid.CellClick += grid_CellClick;
        }

        void grid_CellClick(object sender, MatrixGrid.CellClickEventArgs e)
        {
            cells[e.Cell.X, e.Cell.Y] = (1 - cells[e.Cell.X, e.Cell.Y]);
        }

        void grid_CellNeeded(object sender, MatrixGrid.CellNeededEventArgs e)
        {
            e.BackColor = cells[e.Cell.X, e.Cell.Y] > 0 ? Color.Green : Color.White;
        }

        Random rnd = new Random();

        int iterN = 1;
        private void btStart_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
        /*
        void applyRule(int iGot, int jGot)
        {
            int iRule = 0;
            for (int i = iGot - 1; i < iGot + 2; i++)
            {
                for (int j = jGot - 1; j < jGot + 2; j++)
                {
                    if (i >= 0 && j >= 0 && i < numberHorizontalCells && j < numberVerticalCells)
                    {
                        cells[i, j] = rule(iterN + 1)[iRule];
                        iRule++;
                    }
                }
            }
        }
        */
        private void timer1_Tick(object sender, EventArgs e)
        {
            newGeneration();
            tbDebug.Text = "Generation number " + iterN;
        }

        void newGeneration()
        {
            int neighbours;
            for (int j = 0; j < numberVerticalCells; j++)
            {
                for (int i = 0; i < cells.GetLength(0); i++)
                {
                    neighbours = neighboursCount(i, j);
                    //tbDebug.Text = "neigbours: " + neighbours;
                    if (cells[i, j] == iterN && neighbours < 2) cells[i, j] = 0; //смерть
                    else if (cells[i, j] == iterN && neighbours == 2) cells[i, j] = iterN + 1;
                    else if (cells[i, j] == iterN && neighbours == 3)
                    {
                        cells[i, j] = iterN + 1;
                        if(i - 1 >= 0) cells[i - 1, j] = iterN + 1;
                    }
                    else if (cells[i, j] == iterN && neighbours > 3) cells[i, j] = 0; //смерть
                    else if (cells[i, j] == 0 && neighbours == 3) cells[i, j] = iterN + 1;
                    else if (cells[i, j] == iterN) cells[i, j] = iterN + 1;

                    /*
                    if (cells[i, j] == iterN)
                    {
                        //applyRule(i, j);
                        //tbDebug.Text = tbDebug.Text + "i = " + i + ", ";
                        //tbDebug.Text = tbDebug.Text + "j = " + j + ", ";
                    }
                    */
                }

            }
            iterN++;
        }
        int neighboursCount(int iGot, int jGot)
        {
            int neighb = 0;
            for (int i = iGot - 1; i < iGot + 2; i++)
            {
                for (int j = jGot - 1; j < jGot + 2; j++)
                {
                    if (i >= 0 && j >= 0 && i < numberHorizontalCells && j < numberVerticalCells && (i!=iGot || j!=jGot))
                    {
                        if(cells[i, j] > 0) neighb++;
                    }
                }
            }

            return neighb;
        }
        private void btStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    cells[i, j] = 0;
                }
            }
            iterN = 1;
            tbDebug.Text = "Generation number " + iterN;
        }
    }
}

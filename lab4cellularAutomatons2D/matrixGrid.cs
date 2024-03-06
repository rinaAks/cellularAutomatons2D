using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4cellularAutomatons2D
{

    /// <summary>
    /// Кликабельный контрол для отображения матриц
    /// </summary>
    public class MatrixGrid : UserControl
    {
        public Size GridSize { get; set; }
        public Point HoveredCell = new Point(-1, -1);

        public event EventHandler<CellNeededEventArgs> CellNeeded;
        public event EventHandler<CellClickEventArgs> CellClick;

        public MatrixGrid()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var gr = e.Graphics;
            gr.SmoothingMode = SmoothingMode.HighQuality;

            if (CellNeeded == null)
                return;

            var cw = ClientSize.Width / GridSize.Width;
            var ch = ClientSize.Height / GridSize.Height;

            for (int j = 0; j < GridSize.Height; j++)
                for (int i = 0; i < GridSize.Width; i++)
                {
                    var cell = new Point(i, j);

                    //получаем значение ячейки от пользователя
                    var ea = new CellNeededEventArgs(cell);
                    CellNeeded(this, ea);

                    //рисуем ячейку
                    var rect = new Rectangle(cw * i, ch * j, cw, ch);
                    rect.Inflate(-1, -1);

                    if (cell == HoveredCell)
                        gr.DrawRectangle(Pens.Red, rect);

                    //фон
                    if (ea.BackColor != Color.Transparent)
                        using (var brush = new SolidBrush(ea.BackColor))
                            gr.FillRectangle(brush, rect);

                    //текст
                    if (!string.IsNullOrEmpty(ea.Value))
                        gr.DrawString(ea.Value, Font, Brushes.Black, rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var cell = PointToCell(e.Location);
            HoveredCell = cell;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                var cell = PointToCell(e.Location);
                OnCellClick(new CellClickEventArgs(cell));
                HoveredCell = cell;
            }
        }

        protected virtual void OnCellClick(CellClickEventArgs cellClickEventArgs)
        {
            if (CellClick != null)
                CellClick(this, cellClickEventArgs);
        }

        Point PointToCell(Point p)
        {
            var cw = ClientSize.Width / GridSize.Width;
            var ch = ClientSize.Height / GridSize.Height;
            return new Point(p.X / cw, p.Y / ch);
        }

        public class CellNeededEventArgs : EventArgs
        {
            public Point Cell { get; private set; }
            public string Value { get; set; }
            public Color BackColor { get; set; }

            public CellNeededEventArgs(Point cell)
            {
                Cell = cell;
            }
        }

        public class CellClickEventArgs : EventArgs
        {
            public Point Cell { get; private set; }

            public CellClickEventArgs(Point cell)
            {
                Cell = cell;
            }
        }
    }
}
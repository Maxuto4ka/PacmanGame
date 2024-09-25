using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace PacmanGame
{
    public enum Direction { Up, Down, Left, Right }
    internal class Pacman
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        private int cellSize = 20;
        public int startX { get; private set; }
        public int startY { get; private set; }
        public Pacman(int x, int y)
        {
            X = x;
            Y = y;
            startX = x;
            startY = y;
        }

        public void Move(Direction direction, Maze maze)
        {
            int newX = X, newY = Y;

            switch (direction)
            {
                case Direction.Up: newY--; break;
                case Direction.Down: newY++; break;
                case Direction.Left: newX--; break;
                case Direction.Right: newX++; break;
            }

            if (!maze.IsWall(newX, newY))
            {
                X = newX;
                Y = newY;
            }
        }
        // Метод для повернення Пакмена на стартову позицію
        public void ResetPosition()
        {
            X = startX;
            Y = startY;
        }
        public void Draw(Graphics g)
        {
            g.FillEllipse(Brushes.Yellow, X * cellSize, Y * cellSize, (cellSize * 5) / 6, (cellSize * 5) / 6);
        }
    }
}

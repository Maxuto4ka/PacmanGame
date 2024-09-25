using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PacmanGame
{
    internal class Ghost
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int StartX { get; private set; }  // Стартова позиція X
        public int StartY { get; private set; }  // Стартова позиція Y
        private int offsetX; // Зміщення по осі X для уникнення накладання привидів
        private int offsetY; // Зміщення по осі Y

        private Color color;
        private bool isActive = false; // Параметр активності привида
        private int cellSize = 20;
        private SearchAlgorithm algorithm;
        private List<Point> path = new List<Point>();
        private int pathIndex = 0;

        public bool HasSeenPacman { get; private set; } = false; // Чи бачив привид Пакмена
        public bool IsChasing { get; private set; } = false; // Чи привид переслідує Пакмена
        private Random rand = new Random();
        public Ghost(int x, int y, SearchAlgorithm algorithm, Color color, int offsetX = 0, int offsetY = 0)
        {
            X = x;
            Y = y;
            StartX = x;
            StartY = y;
            this.algorithm = algorithm;
            this.color = color;
            this.offsetX = offsetX; 
            this.offsetY = offsetY;
        }
        public void MoveTowardsPacman(Pacman pacman, Maze maze)
        {
            if (!isActive) return;
            // Отримуємо поточні координати Пакмена
            Point pacmanPosition = new Point(pacman.X, pacman.Y);
            // Якщо привид бачить Пакмена під час руху
            if (CanSeePacman(pacmanPosition, maze))
            {
                // Оновлюємо шлях до поточної позиції Пакмена
                path = algorithm.FindPath(new Point(X, Y), new Point(pacman.X, pacman.Y), maze);
                pathIndex = 0;  // Скидаємо індекс шляху
            }

            // Якщо шлях порожній або привид досяг цілі, шукаємо новий шлях
            if (path.Count == 0 || pathIndex >= path.Count)
            {
                if (HasReachedInitialPacmanPosition(pacman))  // Якщо досягли стартової позиції
                {
                    // Переходимо до випадкового пошуку Пакмена
                    path = FindRandomPath(maze);
                }
                else
                {
                    path = algorithm.FindPath(new Point(X, Y), new Point(pacman.startX, pacman.startY), maze);
                }
                //path = algorithm.FindPath(new Point(X, Y), new Point(pacman.X, pacman.Y), maze);
                pathIndex = 0;
            }

            // Рухаємо привида по шляху
            if (path.Count > 0 && pathIndex < path.Count)
            {
                Point next = path[pathIndex];
                X = next.X;
                Y = next.Y;
                pathIndex++;
            }

            // Пауза для уповільнення руху привида
            Thread.Sleep(200);
        }

        // Метод для перевірки, чи привид може бачити Пакмена
        private bool CanSeePacman(Point pacmanPosition, Maze maze)
        {
            if (X == pacmanPosition.X)
            {
                // Прямий погляд по вертикалі
                int minY = Math.Min(Y, pacmanPosition.Y);
                int maxY = Math.Max(Y, pacmanPosition.Y);
                for (int y = minY + 1; y < maxY; y++)
                {
                    if (maze.IsWall(X, y)) return false;
                }
                return true;
            }
            else if (Y == pacmanPosition.Y)
            {
                // Прямий погляд по горизонталі
                int minX = Math.Min(X, pacmanPosition.X);
                int maxX = Math.Max(X, pacmanPosition.X);
                for (int x = minX + 1; x < maxX; x++)
                {
                    if (maze.IsWall(x, Y)) return false;
                }
                return true;
            }
            return false;
        }

        // Метод для перевірки, чи привид досяг початкової позиції Пакмена
        private bool HasReachedInitialPacmanPosition(Pacman pacman)
        {
            return X == pacman.startX && Y == pacman.startY;
        }

        // Метод для випадкового пошуку Пакмена
        private List<Point> FindRandomPath(Maze maze)
        {
            Random rnd = new Random();
            // Генеруємо випадкову точку в лабіринті
            int randomX = rnd.Next(0, maze.Width);
            int randomY = rnd.Next(0, maze.Height);

            // Повертаємо шлях до цієї точки
            return algorithm.FindPath(new Point(X, Y), new Point(randomX, randomY), maze);
        }

        public void Activate()
        {
            isActive = true;
        }

        public void Sleep()
        {
            isActive = false;
        }
        public void ResetPosition()
        {
            X = StartX;
            Y = StartY;
            path.Clear(); // Очищаємо попередній шлях
            pathIndex = 0; // Скидаємо індекс шляху
            Sleep(); // Привид спить після відновлення
        }
        public void Draw(Graphics g)
        {
            using (Brush brush = new SolidBrush(color))
            {
                g.FillEllipse(brush, X * cellSize + offsetX, Y * cellSize + offsetY, (cellSize*5)/6, (cellSize * 5) / 6);
            }
        }
    }
}

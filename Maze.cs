using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace PacmanGame
{
    public class Maze
    {
        private int[,] maze;
        private int cellSize = 20;
        private Random random = new Random();

        public int Width { get; }
        public int Height { get; }

        public Maze(int width, int height)
        {
            Width = width;
            Height = height;
            maze = new int[width, height];
            GenerateMaze();
        }
        // Ініціалізація лабіринту зі стінами
        private void InitializeMazeWithWalls()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    maze[x, y] = 1; // 1 означає стіну
                }
            }
        }
        public void GenerateMaze()
        {
            InitializeMazeWithWalls();
            CarvePassage(1, 1);
        }

        // Рекурсивна функція для створення коридорів
        private void CarvePassage(int x, int y)
        {
            int[] dx = { 0, 0, 2, -2 };
            int[] dy = { 2, -2, 0, 0 };

            List<int> directions = new List<int> { 0, 1, 2, 3 };
            directions = directions.OrderBy(d => random.Next()).ToList();

            foreach (int dir in directions)
            {
                int nx = x + dx[dir];
                int ny = y + dy[dir];

                if (IsInsideMaze(nx, ny) && IsWall(nx, ny))
                {
                    RemoveWallBetween(x, y, nx, ny);
                    CarvePassage(nx, ny);
                }
            }
        }
        // Видалення стіни між двома клітинками
        private void RemoveWallBetween(int x1, int y1, int x2, int y2)
        {
            int wallX = (x1 + x2) / 2;
            int wallY = (y1 + y2) / 2;
            maze[wallX, wallY] = 0; // 0 означає прохід
            maze[x2, y2] = 0; // Видаляємо стіну з кінцевої точки
        }
        private bool IsInsideMaze(int x, int y)
        {
            return x > 0 && x < Width - 1 && y > 0 && y < Height - 1;
        }
        // Метод для перевірки, чи є клітинка вільною (не стіна)
        public bool IsCellFree(int x, int y)
        {
            // Перевірка, що координати не виходять за межі і це коридор (0)
            return x >= 0 && x < Width && y >= 0 && y < Height && maze[x, y] == 0;
        }

        public bool IsWall(int x, int y)
        {
            return maze[x, y] == 1; // 1 - це стіна
        }

        // Малювання лабіринту
        public void Draw(Graphics g)
        {
            int cellSize = 20;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (IsWall(x, y))
                    {
                        g.FillRectangle(Brushes.Black, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                    else
                    {
                        g.FillRectangle(Brushes.White, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                }
            }
        }

        // Спавн точок
        public Point GetRandomSpawnPoint(Pacman pacman, List<Ghost> ghosts)
        {
            Point spawnPoint;
            int x;
            int y;
            do
            {
                x = random.Next(1, Width - 1);
                y = random.Next(1, Height - 1);
                spawnPoint = new Point(x, y);
            } while (!IsValidSpawn(spawnPoint, pacman, ghosts) || !IsCellFree(x, y));

            return spawnPoint;
        }

        // Логіка для перевірки відстані між персонажами
        private bool IsValidSpawn(Point point, Pacman pacman, List<Ghost> ghosts)
        {
            if (IsWall(point.X, point.Y)) return false;
            if (pacman != null)
            {
                double pacmanDistance = GetDistance(pacman.X, pacman.Y, point.X, point.Y);
                if (pacmanDistance < 5) return false;
            }
            if (ghosts != null)
            {
                foreach (var ghost in ghosts)
                {
                    if (GetDistance(ghost.X, ghost.Y, point.X, point.Y) < 3) return false;
                }
            }
            return true;
        }

        // Метод для розрахунку відстані
        private double GetDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }


        /*private void GenerateMaze()
        {
            // Генерація зовнішніх стінок
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                    {
                        maze[x, y] = 1; // Зовнішні стіни
                    }
                    else
                    {
                        maze[x, y] = 0; // Порожні клітинки
                    }
                }
            }

            AddInternalWalls();
        }

        private void AddInternalWalls()
        {
            maze[10, 1] = 1;
            for (int x = 2; x <= 18; x++)
            {
                if (x == 5 || x == 9 || x == 11 || x == 15)
                {
                    continue; 
                }
                maze[x, 2] = 1;
            }
            for (int x = 2; x <= 18; x++)
            {
                if (x == 5 || x == 9 || x == 11 || x == 15)
                {
                    continue; 
                }
                maze[x, 3] = 1;
            }
            for (int x = 2; x <= 18; x++)
            {
                if (x == 5 || x == 7 || x == 13 || x == 15)
                {
                    continue; 
                }
                maze[x, 5] = 1;
            }
            maze[6, 6] = 1;
            maze[10, 6] = 1;
            maze[14, 6] = 1;
            for (int x = 1; x <= 19; x++)
            {
                if (x == 5 || x == 9 || x == 11 || x == 15)
                {
                    continue; 
                }
                maze[x, 7] = 1;
            }
            for (int x = 1; x <= 6; x++)
            {
                if (x == 5)
                {
                    continue;
                }
                maze[x, 8] = 1;
            }
            for (int x = 14; x <= 19; x++)
            {
                if (x == 15)
                {
                    continue; 
                }
                maze[x, 8] = 1;
            }
            for (int x = 1; x <= 19; x++)
            {
                if (x == 5 || x == 7 || x == 10 || x == 13 || x == 15)
                {
                    continue;
                }
                maze[x, 9] = 1;
            }
            for (int x = 1; x <= 4; x++)
            {
                maze[x, 10] = 1;
            }
            for (int x = 16; x <= 19; x++)
            {
                maze[x, 10] = 1;
            }
            maze[8, 10] = 1;
            maze[12, 10] = 1;
            for (int x = 1; x <= 19; x++)
            {
                if (x == 5 || x == 7 || x == 13 || x == 15)
                {
                    continue;
                }
                maze[x, 11] = 1;
            }
            for (int x = 1; x <= 6; x++)
            {
                if (x == 5)
                {
                    continue;
                }
                maze[x, 12] = 1;
            }
            for (int x = 14; x <= 19; x++)
            {
                if (x == 15)
                {
                    continue;
                }
                maze[x, 12] = 1;
            }
            for (int x = 1; x <= 19; x++)
            {
                if (x == 5 || x == 7 || x == 13 || x == 15)
                {
                    continue;
                }
                maze[x, 13] = 1;
            }
            maze[10, 14] = 1;
            for (int x = 2; x <= 18; x++)
            {
                if (x == 5 || x == 9 || x == 11 || x == 15)
                {
                    continue;
                }
                maze[x, 15] = 1;
            }
            maze[4, 16] = 1;
            maze[16, 16] = 1;
            for (int x = 1; x <= 19; x++)
            {
                if (x == 3 || x == 5 || x == 8 || x == 13 || x == 15 || x == 18)
                {
                    continue;
                }
                maze[x, 17] = 1;
            }
            maze[6, 18] = 1;
            maze[10, 18] = 1;
            maze[14, 18] = 1;
            for (int x = 2; x <= 18; x++)
            {
                if (x == 9 || x == 11)
                {
                    continue;
                }
                maze[x, 19] = 1;
            }
        }

        public bool IsWall(int x, int y)
        {
            return maze[x, y] == 1; // 1 - це стіна
        }

        public void Draw(Graphics g)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (IsWall(x, y))
                    {
                        g.FillRectangle(Brushes.Black, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                    else
                    {
                        g.FillRectangle(Brushes.White, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                }
            }
        }*/
    }
}

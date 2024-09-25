using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanGame
{
    public class Maze
    {
        private int[,] maze;
        private int cellSize = 20;

        public int Width { get; }
        public int Height { get; }

        public Maze(int width, int height)
        {
            Width = width;
            Height = height;
            maze = new int[width, height];
            GenerateMaze();
        }
        private void GenerateMaze()
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
        }
    }
}

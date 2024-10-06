using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacmanGame
{
    public partial class Form1 : Form
    {
        private Maze maze;
        private Pacman pacman;
        private Ghost ghost;
        private List<Ghost> ghosts;
        private Timer gameTimer;
        private int lives = 3; // Кількість життів Пакмена
        private bool isGameOver = false;
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            // Ініціалізація гри
            maze = new Maze(21, 23); // Лабіринт 21х22

            /*pacman = new Pacman(10, 16); // Позиція, з якої стартує пакмен
            ghosts = new List<Ghost>
            {
                new Ghost(1, 1, new AStarAlgorithm(), Color.Red, 2, 0),  // Привид з алгоритмом A*
                new Ghost(10, 10, new BFSAlgorithm(), Color.Blue, 0, -2)  // Привид з алгоритмом BFS
            };*/

            ghosts = new List<Ghost>();  // Ініціалізація списку
            var spawnPoints = maze.GetRandomSpawnPoint(pacman, ghosts);
            
            pacman = new Pacman(maze.GetRandomSpawnPoint(pacman, ghosts).X, maze.GetRandomSpawnPoint(pacman, ghosts).Y);

            ghosts.Add(new Ghost(1, 1, new AStarAlgorithm(), Color.Red, 2, 0));  // Привид з алгоритмом A*
            ghosts.Add(new Ghost(10, 10, new BFSAlgorithm(), Color.Blue, 0, -2)); // Привид з алгоритмом BFS
            foreach (var ghost in ghosts)
            {
                    Point spawnPoint = maze.GetRandomSpawnPoint(pacman, ghosts);
                    ghost.SetPosition(spawnPoint.X, spawnPoint.Y);
            }


            // Привиди сплять на початку
            if (ghosts != null)
            {
                foreach (var ghost in ghosts)
                {
                    ghost.Sleep();
                }
            }
            // Таймер для оновлення гри
            gameTimer = new Timer();
            gameTimer.Interval = 200; // Оновлення кожні 200 мс
            gameTimer.Tick += GameTick;
            gameTimer.Start();

            this.KeyDown += new KeyEventHandler(OnKeyDown); // Обробка клавіш
        }

        private void GameTick(object sender, EventArgs e)
        {
            if (isGameOver) return;
            if (ghosts != null)
            {
                foreach (var ghost in ghosts)
                {
                    ghost.MoveTowardsPacman(pacman, maze);
                    CheckCollisionWithGhosts();
                }
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // Малюємо лабіринт, пакмена та привидів
            maze.Draw(g);
            pacman.Draw(g);

            if (ghosts != null)
            {
                foreach (var ghost in ghosts)
                {
                    ghost.Draw(g);
                }
            }
            // Малюємо життя  пакмена за межами лабіринту
            for (int i = 0; i < lives; i++)
            {
                e.Graphics.FillEllipse(Brushes.Red, new Rectangle(30 + (i * 20), 500, 15, 15));
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (isGameOver) return;
            // Керування пакменом через клавіші
            switch (e.KeyCode)
            {
                case Keys.Up:
                    pacman.Move(Direction.Up, maze);
                    break;
                case Keys.Down:
                    pacman.Move(Direction.Down, maze);
                    break;
                case Keys.Left:
                    pacman.Move(Direction.Left, maze);
                    break;
                case Keys.Right:
                    pacman.Move(Direction.Right, maze);
                    break;
            }
            if (ghosts != null)
            {
                foreach (var ghost in ghosts)
                {
                    ghost.Activate();
                }
            }
            CheckCollisionWithGhosts();
        }
        private void GameLoop()
        {
            while (true)
            {
                if (isGameOver) break;
                foreach (var ghost in ghosts)
                {
                    ghost.MoveTowardsPacman(pacman, maze);
                    CheckCollisionWithGhosts();
                }
                Invalidate(); // Оновлення форми для відображення нових позицій
                CheckCollisionWithGhosts();
            }
        }
        // Перевірки зіткнення Пакмена з привидом
        private void CheckCollisionWithGhosts()
        {
            foreach (var ghost in ghosts)
            {
                if (ghost.X == pacman.X && ghost.Y == pacman.Y)
                {
                    LoseLife(); // Пакмен втрачає життя, якщо привид його спіймав
                }
            }
        }

        // Метод для втрати життя Пакмена
        private void LoseLife()
        {
            if (isGameOver) return;
            lives--;

            if (lives <= 0)
            {
                GameOver(); // Якщо життя закінчилися, гра закінчується
            }
            else
            {
                // Повертаємо Пакмена і привидів на рандомні стартові позиції
                var pacmanSpawn = maze.GetRandomSpawnPoint(null, ghosts);
                pacman.SetPosition(pacmanSpawn.X, pacmanSpawn.Y);

                foreach (var ghost in ghosts)
                {
                    var ghostSpawn = maze.GetRandomSpawnPoint(pacman, ghosts);
                    ghost.SetPosition(ghostSpawn.X, ghostSpawn.Y);
                }

                Invalidate(); // Оновлюємо екран після скидання позицій
            }
        }


        /*private void LoseLife()
        {
            if (isGameOver) return;
            lives--;

            if (lives <= 0)
            {
                GameOver(); // Якщо життя закінчилися, гра закінчується
            }
            else
            {
                // Повертаємо Пакмена і привидів на стартові позиції
                pacman.ResetPosition();
                foreach (var ghost in ghosts)
                {
                    ghost.ResetPosition(); 
                }

                Invalidate();
            }
        }*/

        // Метод для завершення гри
        private void GameOver()
        {
            if (isGameOver) return;
            isGameOver = true;
            MessageBox.Show("GAME OVER", "Pacman");
        }
        
    }
}

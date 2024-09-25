using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanGame
{
    public class AStarAlgorithm : SearchAlgorithm
    {
        public override List<Point> FindPath(Point start, Point target, Maze maze)
        {
            PriorityQueue<Point, int> frontier = new PriorityQueue<Point, int>();
            Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();
            Dictionary<Point, int> costSoFar = new Dictionary<Point, int>();

            frontier.Enqueue(start, 0);
            cameFrom[start] = start;
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                Point current = frontier.Dequeue();

                if (current == target)
                {
                    break;
                }

                foreach (var next in GetNeighbors(current, maze))
                {
                    int newCost = costSoFar[current] + 1; // Вартість переміщення

                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        int priority = newCost + Heuristic(next, target);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }

            return ReconstructPath(start, target, cameFrom);
        }

        private int Heuristic(Point a, Point b)
        {
            // Евристика: відстань Манхеттена
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private List<Point> GetNeighbors(Point point, Maze maze)
        {
            List<Point> neighbors = new List<Point>();

            Point[] directions = new Point[]
            {
                new Point(0, -1), // Вгору
                new Point(0, 1),  // Вниз
                new Point(-1, 0), // Вліво
                new Point(1, 0)   // Вправо
            };

            foreach (var dir in directions)
            {
                Point neighbor = new Point(point.X + dir.X, point.Y + dir.Y);
                if (neighbor.X >= 0 && neighbor.X < maze.Width &&
                    neighbor.Y >= 0 && neighbor.Y < maze.Height &&
                    !maze.IsWall(neighbor.X, neighbor.Y))
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        private List<Point> ReconstructPath(Point start, Point target, Dictionary<Point, Point> cameFrom)
        {
            List<Point> path = new List<Point>();
            if (!cameFrom.ContainsKey(target))
            {
                return path; // Шлях не знайдено
            }

            Point current = target;
            while (current != start)
            {
                path.Add(current);
                current = cameFrom[current];
            }

            path.Reverse();
            return path;
        }
    }
}

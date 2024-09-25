using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanGame
{
    public class BFSAlgorithm : SearchAlgorithm
    {
        public override List<Point> FindPath(Point start, Point target, Maze maze)
        {
            Queue<Point> frontier = new Queue<Point>();
            Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();

            frontier.Enqueue(start);
            cameFrom[start] = start;

            while (frontier.Count > 0)
            {
                Point current = frontier.Dequeue();

                if (current == target)
                {
                    break;
                }

                foreach (var next in GetNeighbors(current, maze))
                {
                    if (!cameFrom.ContainsKey(next))
                    {
                        frontier.Enqueue(next);
                        cameFrom[next] = current;
                    }
                }
            }

            return ReconstructPath(start, target, cameFrom);
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

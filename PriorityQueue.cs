using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanGame
{
    public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
    {
        private List<(TElement Element, TPriority Priority)> elements = new List<(TElement, TPriority)>();

        // Додає елемент у чергу з пріоритетом
        public void Enqueue(TElement element, TPriority priority)
        {
            elements.Add((element, priority));
        }

        // Витягує елемент з найвищим пріоритетом (мінімальний пріоритет)
        public TElement Dequeue()
        {
            int bestIndex = 0;

            for (int i = 1; i < elements.Count; i++)
            {
                if (elements[i].Priority.CompareTo(elements[bestIndex].Priority) < 0)
                {
                    bestIndex = i;
                }
            }

            TElement bestItem = elements[bestIndex].Element;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }

        public int Count => elements.Count;
    }
}

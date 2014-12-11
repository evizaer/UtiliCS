using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtiliCS
{
    public class UniqueQueue<T>
    {
        HashSet<T> ItemSet { get; set; }
        Queue<T> ItemQueue { get; set; }

        public UniqueQueue()
        {
            ItemSet = new HashSet<T>();
            ItemQueue = new Queue<T>();
        }

        public UniqueQueue(IEnumerable<T> items) : this()
        {
            EnqueueAll(items);
        }

        public void EnqueueAll(IEnumerable<T> items)
        {
            foreach (T item in items) Enqueue(item);
        }

        public void Enqueue(T item)
        {
            if (!ItemSet.Contains(item))
            {
                ItemQueue.Enqueue(item);
                ItemSet.Add(item);
            }
        }

        public T Dequeue()
        {
            T item = ItemQueue.Dequeue();
            ItemSet.Remove(item);
            return item;
        }

        public int Count { get { return ItemQueue.Count; } }

        public bool IsEmpty { get { return !ItemQueue.Any(); } }
    }
}

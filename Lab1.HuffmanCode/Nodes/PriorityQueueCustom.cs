namespace Lab1.HuffmanCode.Nodes;

public class PriorityQueueCustom<T> where T : IComparable<T>
{
    private List<T> heap = new List<T>();

    public int Count => heap.Count;

    public void Enqueue(T item)
    {
        heap.Add(item);
        int i = heap.Count - 1;
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (heap[i].CompareTo(heap[parent]) >= 0)
                break;
            T tmp = heap[i];
            heap[i] = heap[parent];
            heap[parent] = tmp;
            i = parent;
        }
    }

    public T Dequeue()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("The queue is empty.");
        T frontItem = heap[0];
        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);
        lastIndex--;
        int parent = 0;
        while (true)
        {
            int left = parent * 2 + 1;
            if (left > lastIndex)
                break;
            int right = left + 1;
            if (right <= lastIndex && heap[right].CompareTo(heap[left]) < 0)
                left = right;
            if (heap[parent].CompareTo(heap[left]) <= 0)
                break;
            T tmp = heap[parent];
            heap[parent] = heap[left];
            heap[left] = tmp;
            parent = left;
        }
        return frontItem;
    }
}
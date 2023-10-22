namespace Lab1.HuffmanCode.Nodes;

public class PriorityQueueCustom<HuffmanNode> where HuffmanNode : IComparable<HuffmanNode>
{
    private List<HuffmanNode> heap = new List<HuffmanNode>();

    public int Count => heap.Count;

    public void Enqueue(HuffmanNode item)
    {
        heap.Add(item);
        int i = heap.Count - 1;
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (heap[i].CompareTo(heap[parent]) >= 0)
                break;
            HuffmanNode tmp = heap[i];
            heap[i] = heap[parent];
            heap[parent] = tmp;
            i = parent;
        }
    }

    public HuffmanNode Dequeue()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("The queue is empty.");
        HuffmanNode frontItem = heap[0];
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
            HuffmanNode tmp = heap[parent];
            heap[parent] = heap[left];
            heap[left] = tmp;
            parent = left;
        }
        return frontItem;
    }
}
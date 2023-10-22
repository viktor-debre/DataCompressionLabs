using Lab1.HuffmanCode.Nodes;
using System.Text;

namespace Lab1.HuffmanCode.Huffman;

public class HuffmanEncoder
{
    public HuffmanNode BuildHuffmanTree(Dictionary<char, int> charFrequencies)
    {
        // Create a priority queue for Huffman nodes
        var priorityQueue = new PriorityQueueCustom<HuffmanNode>();

        // Initialize the priority queue with leaf nodes
        foreach (var entry in charFrequencies)
        {
            priorityQueue.Enqueue(new HuffmanNode { Character = entry.Key, Frequency = entry.Value });
        }

        // Build the Huffman tree
        while (priorityQueue.Count > 1)
        {
            HuffmanNode left = priorityQueue.Dequeue();
            HuffmanNode right = priorityQueue.Dequeue();
            HuffmanNode parent = new HuffmanNode
            {
                Character = '\0', // Internal node, no character
                Frequency = left.Frequency + right.Frequency,
                Left = left,
                Right = right,
            };
            priorityQueue.Enqueue(parent);
        }

        // The remaining node in the priority queue is the root of the Huffman tree
        return priorityQueue.Dequeue();
    }

    public Dictionary<char, string> BuildHuffmanCodes(HuffmanNode root)
    {
        var huffmanCodes = new Dictionary<char, string>();
        BuildHuffmanCodeRecursively(root, "", huffmanCodes);
        return huffmanCodes;
    }

    private void BuildHuffmanCodeRecursively(HuffmanNode node, string currentCode, Dictionary<char, string> huffmanCodes)
    {
        if (node == null)
            return;

        if (node.Character != '\0')
        {
            huffmanCodes[node.Character] = currentCode;
        }

        BuildHuffmanCodeRecursively(node.Left, currentCode + "0", huffmanCodes);
        BuildHuffmanCodeRecursively(node.Right, currentCode + "1", huffmanCodes);
    }

    public string Encode(string text, Dictionary<char, string> huffmanCodes)
    {
        StringBuilder encodedData = new StringBuilder();
        foreach (char c in text)
        {
            if (huffmanCodes.ContainsKey(c))
            {
                encodedData.Append(huffmanCodes[c]);
            }
        }
        return encodedData.ToString();
    }
}
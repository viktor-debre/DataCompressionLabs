using Lab1.HuffmanCode.Nodes;

namespace Lab1.HuffmanCode.Huffman;

public class HuffmanDecoder
{
    public static string Decode(HuffmanNode root, string encodedData)
    {
        string decodedData = "";
        HuffmanNode current = root;

        foreach (char bit in encodedData)
        {
            if (bit == '0')
            {
                current = current.Left;
            }
            else if (bit == '1')
            {
                current = current.Right;
            }

            if (current.Left == null && current.Right == null)
            {
                decodedData += current.Character;
                current = root;
            }
        }

        return decodedData;
    }
}
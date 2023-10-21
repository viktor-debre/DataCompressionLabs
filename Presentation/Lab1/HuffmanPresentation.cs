using Lab1.HuffmanCode.Huffman;
using Lab1.HuffmanCode.Nodes;

namespace DataCompressionLabs.Presentation.Lab1;

public class HuffmanPresentation : Presentation
{
    public void Execute()
    {
        var encoder = new HuffmanEncoder();
        var decoder = new HuffmanDecoder();

        string inputText = "this is an example for huffman encoding qwertyuiopwertyuioqwertyuiartyuiqwertywqertyuiqwertywq";

        Dictionary<char, int> charFrequencies = inputText
            .GroupBy(c => c)
            .ToDictionary(g => g.Key, g => g.Count());

        HuffmanNode huffmanTree = encoder.BuildHuffmanTree(charFrequencies);
        Dictionary<char, string> huffmanCodes = encoder.BuildHuffmanCodes(huffmanTree);

        string encodedData = encoder.Encode(inputText, huffmanCodes);

        Console.WriteLine("Original Text: " + inputText);
        Console.WriteLine("Encoded Data: " + encodedData);
        var compressionRatio = inputText.Length * 2 * 8 * 1.0f / encodedData.Length;
        Console.WriteLine("Compression ratio: " + compressionRatio);
        var decodedData = HuffmanDecoder.Decode(huffmanTree, encodedData);
        Console.WriteLine("Decoded Data: " + decodedData);

        
    }
}
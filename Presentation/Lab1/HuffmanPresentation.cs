using Lab1.HuffmanCode.Arithmetic;
using Lab1.HuffmanCode.Huffman;
using Lab1.HuffmanCode.Nodes;
using Lab1.HuffmanCode.ResultComparer;
using System.Numerics;

namespace DataCompressionLabs.Presentation.Lab1;

public class HuffmanPresentation : Presentation
{
    public void Execute()
    {
        //Huffman compression
        var encoder = new HuffmanEncoder();
        var decoder = new HuffmanDecoder();

        string inputText = "this is an example for huffman encoding";

        Dictionary<char, int> charFrequencies = inputText
            .GroupBy(c => c)
            .ToDictionary(g => g.Key, g => g.Count());

        HuffmanNode huffmanTree = encoder.BuildHuffmanTree(charFrequencies);
        Dictionary<char, string> huffmanCodes = encoder.BuildHuffmanCodes(huffmanTree);

        string encodedData = encoder.Encode(inputText, huffmanCodes);

        Console.WriteLine("Original Text: " + inputText);
        Console.WriteLine("Encoded Data: " + encodedData);
        var compressionRatio = CompressionComparer.CompressionRatio(inputText, encodedData);
        Console.WriteLine("Compression ratio: " + compressionRatio);
        var decodedData = HuffmanDecoder.Decode(huffmanTree, encodedData);
        Console.WriteLine("Decoded Data: " + decodedData);
        Console.WriteLine("Decoded text is similar: " + CompressionComparer.IsTextsSimilar(inputText, decodedData));

        //Arithmetic compression
        var arithmeticEncoder = new ArithmeticDataEncoder();
        var radix = 10;
        //Dictionary<char, double> symbolProbabilities = arithmeticEncoder.CalculateSymbolProbabilities(inputText);
        Console.WriteLine("Original Text: " + inputText);
        Tuple<BigInteger, int, Dictionary<char, long>> encoded = arithmeticEncoder.ArithmeticCoding(inputText, 10);
        //string compressedData = arithmeticEncoder.Encode(encoded);
        //Console.WriteLine("Compressed Data: " + compressedData);
        //compressionRatio = CompressionComparer.CompressionRatio(inputText, compressedData);
        Console.WriteLine("Compression ratio: " + compressionRatio);
        string decompressedData = arithmeticEncoder.ArithmeticDecoding(encoded.Item1, radix, encoded.Item2, encoded.Item3);
        Console.WriteLine("Decompressed Data: " + decompressedData);
        Console.WriteLine("Decoded text is similar: " + CompressionComparer.IsTextsSimilar(inputText, decompressedData));
    }
}
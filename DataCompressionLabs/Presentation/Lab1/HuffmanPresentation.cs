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

        string inputText = @"this is an example for huffman encoding this is an example for huffman encoding
        this is an example for huffman encodingthis is an example for huffman encodingthis is an example for huffman encodingthis is an example for huffman encoding
        this is an example for huffman encodingthis is an example for huffman encodingthis is an example for huffman encoding
        qwetSDFasbhnghbfxc";

        Dictionary<char, int> charFrequencies = inputText
            .GroupBy(c => c)
            .ToDictionary(g => g.Key, g => g.Count());

        HuffmanNode huffmanTree = encoder.BuildHuffmanTree(charFrequencies);
        Dictionary<char, string> huffmanCodes = encoder.BuildHuffmanCodes(huffmanTree);

        string encodedData = encoder.Encode(inputText, huffmanCodes);

        Console.WriteLine("Original Text: " + inputText);
        Console.WriteLine("Encoded Data: " + encodedData);
        var compressionRatio = CompressionComparer.CompressionRatio(inputText, encodedData);
        Console.WriteLine("Compression ratio: {0:F6}", compressionRatio);
        var decodedData = HuffmanDecoder.Decode(huffmanTree, encodedData);
        Console.WriteLine("Decoded Data: " + decodedData);
        Console.WriteLine("Decoded text is similar: " + CompressionComparer.IsTextsSimilar(inputText, decodedData));

        long radix = 10;
        //string inputText = "this is an example for huffman encoding";
        Console.WriteLine("Original Text: " + inputText);

        Tuple<BigInteger, int, Dictionary<char, long>> encoded = ArithmeticDataEncoder.ArithmeticCoding(inputText, radix);
        string dec = ArithmeticDataDecoder.ArithmeticDecoding(encoded.Item1, radix, encoded.Item2, encoded.Item3);
        Console.WriteLine("{0} * {1}^-{2}", encoded.Item1, radix, encoded.Item2);
        Console.WriteLine("Decoded text is similar: " + CompressionComparer.IsTextsSimilar(inputText, dec));

        var compressionRate = inputText.Length * 2 * 8 * 1.0f / (double)BigInteger.Log2(encoded.Item1);
        Console.WriteLine("Compression rate: {0:F6}", compressionRate);
    }
}
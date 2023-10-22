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
        string path = "D:\\repos\\DataCompressionLabs\\Lab1.HuffmanCode\\Files\\";
        var fileOfInputText = path + "input-text.txt";
        var inputText = File.ReadAllText(fileOfInputText);

        //Huffman compression
        var encoder = new HuffmanEncoder();
        var decoder = new HuffmanDecoder();

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


        int byteCount = (encodedData.Length + 7) / 8;
        byte[] data = new byte[byteCount];
        for (int i = 0; i < byteCount; i++)
        {
            string byteString = encodedData.Substring(i * 8, Math.Min(8, encodedData.Length - i * 8));
            if (byteString.Length < 8)
            {
                int zerosToAdd = 8 - byteString.Length;
                for (int j = 0; j < zerosToAdd; j++)
                {
                    byteString = byteString + "0";
                }
            }
            data[i] = Convert.ToByte(byteString, 2);
        }

        string filePath = path + "output-huffman-file.bat";

        File.WriteAllBytes(filePath, data);

        byte[] binaryData;
        using (FileStream fileStream = File.OpenRead(filePath))
        {
            binaryData = new byte[fileStream.Length];
            fileStream.Read(binaryData, 0, (int)fileStream.Length);
        }

        string binaryString = string.Concat(binaryData.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
        int bitsToRemove = binaryString.Length - encodedData.Length;
        binaryString = binaryString.Substring(0, binaryString.Length - bitsToRemove);


        var decodedData = HuffmanDecoder.Decode(huffmanTree, binaryString);
        Console.WriteLine("Decoded Data: " + decodedData);
        Console.WriteLine("Decoded text is similar: " + CompressionComparer.IsTextsSimilar(inputText, decodedData));

        //Arithmetic encoding
        long radix = 10;
        Console.WriteLine("Original Text: " + inputText);

        Tuple<BigInteger, int, Dictionary<char, long>> encoded = ArithmeticDataEncoder.ArithmeticEncoding(inputText, radix);

        var arithmeticEncodingFilePath = path + "output-arithmetic-file.bat";
        using (FileStream fileStream = new FileStream(arithmeticEncodingFilePath, FileMode.Create, FileAccess.Write))
        using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
        {
            // Write the BigInteger value as a byte array
            byte[] bytesToWrite = encoded.Item1.ToByteArray();
            binaryWriter.Write(bytesToWrite);
        }

        byte[] bytes;
        using (FileStream fileStream = new FileStream(arithmeticEncodingFilePath, FileMode.Open, FileAccess.Read))
        using (BinaryReader binaryReader = new BinaryReader(fileStream))
        {
            bytes = binaryReader.ReadBytes((int)fileStream.Length);
        }
        BigInteger readedNumber = new BigInteger(bytes);

        string dec = ArithmeticDataDecoder.ArithmeticDecoding(readedNumber, radix, encoded.Item2, encoded.Item3);
        Console.WriteLine("{0} * {1}^-{2}", encoded.Item1, radix, encoded.Item2);
        Console.WriteLine("Decoded text is similar: " + CompressionComparer.IsTextsSimilar(inputText, dec));

        var compressionRate = inputText.Length * 2 * 8 * 1.0f / (double)BigInteger.Log2(encoded.Item1);
        Console.WriteLine("Compression rate: {0:F6}", compressionRate);

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Compression rate for huffman: {0:F6}", compressionRatio);
        Console.WriteLine("Compression rate for arithmetic: {0:F6}", compressionRate);
        Console.WriteLine();
        Console.WriteLine("Decoded text is similar for huffman: " + CompressionComparer.IsTextsSimilar(inputText, decodedData));
        Console.WriteLine("Decoded text is similar for arithmetic: " + CompressionComparer.IsTextsSimilar(inputText, dec));
        Console.WriteLine();
    }
}
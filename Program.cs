//using DataCompressionLabs.Presentation.Lab1;

//while (true)
//{
//    Console.WriteLine(
//        @"Greetings, what lab are you want to run:
//        press: 1-huffman, 2-?, 3-?, 4-?");

//    var lab = Console.ReadLine();
//    var executeLab = lab switch
//    {
//        "1" => new HuffmanPresentation(),
//        "2" => new HuffmanPresentation(),
//        "3" => new HuffmanPresentation(),
//        "4" => new HuffmanPresentation(),
//        _ => null
//    };
//    if (executeLab != null)
//    {
//        executeLab.Execute();
//    }
//    Console.WriteLine(
//       @"Do you want to continue:
//       y-yes, anything else - no");
//    var programContinue = Console.ReadLine();
//    if (programContinue == "y")
//    {
//        Console.Clear();
//        continue;
//    }
//    break;
//}
using Lab1.HuffmanCode.ResultComparer;
using System.Numerics;
using System.Text;

namespace AruthmeticCoding
{
    using Freq = Dictionary<char, long>;
    using Triple = Tuple<BigInteger, int, Dictionary<char, long>>;

    class Program
    {
        static Freq CumulativeFreq(Freq freq)
        {
            long total = 0;
            Freq cf = new Freq();
            for (int i = 0; i < 256; i++)
            {
                char c = (char)i;
                if (freq.ContainsKey(c))
                {
                    long v = freq[c];
                    cf[c] = total;
                    total += v;
                }
            }
            return cf;
        }

        static Triple ArithmeticCoding(string str, long radix)
        {
            // The frequency of characters
            Freq freq = new Freq();
            foreach (char c in str)
            {
                if (freq.ContainsKey(c))
                {
                    freq[c] += 1;
                }
                else
                {
                    freq[c] = 1;
                }
            }

            // The cumulative frequency
            Freq cf = CumulativeFreq(freq);

            // Base
            BigInteger @base = str.Length;

            // Lower bound
            BigInteger lower = 0;

            // Product of all frequencies
            BigInteger pf = 1;

            // Each term is multiplied by the product of the
            // frequencies of all previously occuring symbols
            foreach (char c in str)
            {
                BigInteger x = cf[c];
                lower = lower * @base + x * pf;
                pf = pf * freq[c];
            }

            // Upper bound
            BigInteger upper = lower + pf;

            int powr = 0;
            BigInteger bigRadix = radix;

            while (true)
            {
                pf = pf / bigRadix;
                if (pf == 0) break;
                powr++;
            }

            BigInteger diff = (upper - 1) / (BigInteger.Pow(bigRadix, powr));
            return new Triple(diff, powr, freq);
        }

        static string ArithmeticDecoding(BigInteger num, long radix, int pwr, Freq freq)
        {
            BigInteger powr = radix;
            BigInteger enc = num * BigInteger.Pow(powr, pwr);
            long @base = freq.Values.Sum();

            // Create the cumulative frequency table
            Freq cf = CumulativeFreq(freq);

            // Create the dictionary
            Dictionary<long, char> dict = new Dictionary<long, char>();
            foreach (char key in cf.Keys)
            {
                long value = cf[key];
                dict[value] = key;
            }

            // Fill the gaps in the dictionary
            long lchar = -1;
            for (long i = 0; i < @base; i++)
            {
                if (dict.ContainsKey(i))
                {
                    lchar = dict[i];
                }
                else if (lchar != -1)
                {
                    dict[i] = (char)lchar;
                }
            }

            // Decode the input number
            StringBuilder decoded = new StringBuilder((int)@base);
            BigInteger bigBase = @base;
            for (long i = @base - 1; i >= 0; --i)
            {
                BigInteger pow = BigInteger.Pow(bigBase, (int)i);
                BigInteger div = enc / pow;
                char c = dict[(long)div];
                BigInteger fv = freq[c];
                BigInteger cv = cf[c];
                BigInteger diff = enc - pow * cv;
                enc = diff / fv;
                decoded.Append(c);
            }

            // Return the decoded output
            return decoded.ToString();
        }

        static void Main(string[] args)
        {
            long radix = 10;
            string inputText = "this is an example for huffman encoding";
            Console.WriteLine("Original Text: " + inputText);

            Triple encoded = ArithmeticCoding(inputText, radix);
            string dec = ArithmeticDecoding(encoded.Item1, radix, encoded.Item2, encoded.Item3);
            Console.WriteLine("{0,200} * {1}^{2}", encoded.Item1, radix, encoded.Item2);
            Console.WriteLine("Decoded text is similar: " + CompressionComparer.IsTextsSimilar(inputText, dec));

            var compressionRate = inputText.Length * 2 * 8 * 1.0f / (double)BigInteger.Log2(encoded.Item1);
            Console.WriteLine("Compression rate: {0:F6}", compressionRate);
        }
    }
}
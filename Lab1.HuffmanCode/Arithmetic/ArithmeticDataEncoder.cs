using System.Numerics;
using System.Text;

namespace Lab1.HuffmanCode.Arithmetic;

using Freq = Dictionary<char, long>;
using Triple = Tuple<BigInteger, int, Dictionary<char, long>>;

public class ArithmeticDataEncoder
{
    public static Triple ArithmeticCoding(string str, long radix)
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

    private static Freq CumulativeFreq(Freq freq)
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
}

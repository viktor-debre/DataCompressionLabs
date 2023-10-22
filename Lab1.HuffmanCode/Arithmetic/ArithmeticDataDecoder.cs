using System.Numerics;
using System.Text;

namespace Lab1.HuffmanCode.Arithmetic;

using Freq = Dictionary<char, long>;

public class ArithmeticDataDecoder
{
    public static string ArithmeticDecoding(BigInteger num, long radix, int pwr, Freq freq)
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
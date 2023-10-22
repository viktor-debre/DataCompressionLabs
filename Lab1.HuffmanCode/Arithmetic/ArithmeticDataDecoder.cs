using System.Numerics;
using System.Text;

namespace Lab1.HuffmanCode.Arithmetic;

using Frequency = Dictionary<char, long>;

public class ArithmeticDataDecoder
{
    public static string ArithmeticDecoding(BigInteger numeral, long radix, int exponent, Frequency frequency)
    {
        BigInteger power = radix;
        BigInteger normilizedNumber = numeral * BigInteger.Pow(power, exponent);
        long baseTextLength = frequency.Values.Sum();

        Frequency cumulativeFrequency = CumulativeFrequency(frequency);

        Dictionary<long, char> dictinaryOfCharacterCount = new Dictionary<long, char>();
        foreach (char key in cumulativeFrequency.Keys)
        {
            long value = cumulativeFrequency[key];
            dictinaryOfCharacterCount[value] = key;
        }

        long lchar = -1;
        for (long i = 0; i < baseTextLength; i++)
        {
            if (dictinaryOfCharacterCount.ContainsKey(i))
            {
                lchar = dictinaryOfCharacterCount[i];
            }
            else if (lchar != -1)
            {
                dictinaryOfCharacterCount[i] = (char)lchar;
            }
        }

        // Decode the input number
        StringBuilder decoded = new StringBuilder((int)baseTextLength);
        BigInteger bigBase = baseTextLength;
        for (long i = baseTextLength - 1; i >= 0; --i)
        {
            BigInteger pow = BigInteger.Pow(bigBase, (int)i);
            BigInteger div = normilizedNumber / pow;
            char character = dictinaryOfCharacterCount[(long)div];
            BigInteger frequencyOfCharacter = frequency[character];
            BigInteger cumulativeFrequencyOfCharacter = cumulativeFrequency[character];
            BigInteger difference = normilizedNumber - pow * cumulativeFrequencyOfCharacter;
            normilizedNumber = difference / frequencyOfCharacter;
            decoded.Append(character);
        }

        // Return the decoded output
        return decoded.ToString();
    }

    private static Frequency CumulativeFrequency(Frequency frequency)
    {
        long total = 0;
        Frequency cumulativeFrequency = new Frequency();
        for (int i = 0; i < 256; i++)
        {
            char character = (char)i;
            if (frequency.ContainsKey(character))
            {
                long appearingOfSymbol = frequency[character];
                cumulativeFrequency[character] = total;
                total += appearingOfSymbol;
            }
        }
        return cumulativeFrequency;
    }
}
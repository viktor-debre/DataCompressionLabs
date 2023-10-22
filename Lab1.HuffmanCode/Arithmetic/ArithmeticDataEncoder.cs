using System.Numerics;

namespace Lab1.HuffmanCode.Arithmetic;

using Frequency = Dictionary<char, long>;
using Triple = Tuple<BigInteger, int, Dictionary<char, long>>;

public class ArithmeticDataEncoder
{
    public static Triple ArithmeticEncoding(string inputText, long radix)
    {
        Frequency frequency = new Frequency();
        foreach (char character in inputText)
        {
            if (frequency.ContainsKey(character))
            {
                frequency[character] += 1;
            }
            else
            {
                frequency[character] = 1;
            }
        }

        Frequency cumulativeFrequency = CumulativeFrequency(frequency);

        BigInteger baseTextLength = inputText.Length;
        BigInteger lower = 0;
        BigInteger productOfFrequencies = 1;

        foreach (char character in inputText)
        {
            BigInteger countOfAppearingSpecificCharacter = cumulativeFrequency[character];
            lower = lower * baseTextLength + countOfAppearingSpecificCharacter * productOfFrequencies;
            productOfFrequencies = productOfFrequencies * frequency[character];
        }

        BigInteger upper = lower + productOfFrequencies;

        int power = 0;
        BigInteger bigRadix = radix;

        while (true)
        {
            productOfFrequencies = productOfFrequencies / bigRadix;
            if (productOfFrequencies == 0) break;
            power++;
        }

        BigInteger difference = (upper - 1) / (BigInteger.Pow(bigRadix, power));
        return new Triple(difference, power, frequency);
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

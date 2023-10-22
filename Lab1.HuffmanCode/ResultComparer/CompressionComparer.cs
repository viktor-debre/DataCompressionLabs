namespace Lab1.HuffmanCode.ResultComparer;

public static class CompressionComparer
{
    public static bool IsTextsSimilar(string text1, string text2)
    {
        if (text1.Length != text2.Length) return false;
        if (!text1.Contains(text2)) return false;

        return true;
    }

    public static double CompressionRatio(string inputText, string encodedText)
    {
        return 1.0f * inputText.Length * sizeof(char) * 8 / encodedText.Length;
    }
}
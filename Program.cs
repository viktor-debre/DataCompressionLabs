using DataCompressionLabs.Presentation.Lab1;

while (true)
{
    Console.WriteLine(
        @"Greetings, what lab are you want to run:
        press: 1-huffman, 2-?, 3-?, 4-?");

    var lab = Console.ReadLine();
    var executeLab = lab switch
    {
        "1" => new HuffmanPresentation(),
        "2" => new HuffmanPresentation(),
        "3" => new HuffmanPresentation(),
        "4" => new HuffmanPresentation(),
        _ => null
    };
    if (executeLab != null)
    {
        executeLab.Execute();
    }
    Console.WriteLine(
       @"Do you want to continue:
       y-yes, n-no");
    var programContinue = Console.ReadLine();
    if (programContinue == "n") break;
    Console.Clear();
}
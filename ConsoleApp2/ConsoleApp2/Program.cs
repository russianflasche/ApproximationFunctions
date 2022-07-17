using ConsoleApp2;

Approximationsfunktion approx = new Approximationsfunktion();
approx.calculate();

foreach (RegressionResult rg in approx.result)
{
    Console.WriteLine(rg.titel + ": " + rg.formel);
}
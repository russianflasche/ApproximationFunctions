namespace ConsoleApp2;

public class RegressionResult
{
    public double a;
    public double b;
    public double rr;
    public String titel;
    public String formel;
    ApproxFunction approxFunction;
}

interface ApproxFunction
{
    double execute( double a, double b, double x );
}

interface FunctionFromX
{
    double execute( double x, Object helpObject );
}
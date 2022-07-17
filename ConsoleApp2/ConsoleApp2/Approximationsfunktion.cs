namespace ConsoleApp2;

public class Approximationsfunktion
{
   private static int SP = 4;
   public List<RegressionResult> result = new List<RegressionResult>();
   private double[] xyArr = {5,100,6,99,7,96,8,97,9,95,10,94,11,92,12,95};
   public void calculate()
   {
      // Verschiedene Regressionen:
      result.Add(calculateLinearRegression(xyArr));
      result.Add(calculatePowerRegression(xyArr));
      result.Add(calculateLogarithmicRegression(xyArr));
      result.Add(calculateExponentialRegression(xyArr));
      // Lineare Regression
      // y = a + b * x
   }

   public RegressionResult calculateLinearRegression(double[] xyArr)
   {
      if (xyArr == null || xyArr.Length < 2 || xyArr.Length % 2 != 0) return null;

      int n = xyArr.Length / 2;
      double xs = 0;
      double ys = 0;
      double xqs = 0;
      double yqs = 0;
      double xys = 0;

      for (int i = 0; i < xyArr.Length; i += 2)
      {
         xs += xyArr[i];
         ys += xyArr[i + 1];
         xqs += xyArr[i] * xyArr[i];
         yqs += xyArr[i + 1] * xyArr[i + 1];
         xys += xyArr[i] * xyArr[i + 1];
      }

      RegressionResult abr = new RegressionResult();
      double xm = xs / n;
      double ym = ys / n;
      double xv = xqs / n - (xm * xm);
      double yv = yqs / n - (ym * ym);
      double kv = xys / n - (xm * ym);
      abr.rr = Math.Min((kv * kv) / (xv * yv), 1);
      abr.b = kv / xv;
      abr.a = ym - abr.b * xm;
      abr.titel = "Lin";
      abr.formel = "f(x) = " + roundSignificant(abr.a, SP) + " + " + roundSignificant(abr.b, SP) + " * x";
      return abr;
   }

   // Potenzielle Regression
   // y = a * x^b
   // Regression ueber: ln(y) = ln(a) + b * ln(x)
   public RegressionResult calculatePowerRegression( double[] xyArr )
   {
      if( xyArr == null || xyArr.Length < 2 || xyArr.Length % 2 != 0 ) return null;

      double[] xyArrConv = new double[xyArr.Length];

      for( int i=0; i < xyArr.Length; i+=2 ) {
         if( xyArr[i] <= 0 || xyArr[i+1] <= 0 ) return null;
         xyArrConv[i]   = Math.Log( xyArr[i]   );
         xyArrConv[i+1] = Math.Log( xyArr[i+1] );
      }

      RegressionResult abr = calculateLinearRegression( xyArrConv );
      if( abr == null ) return null;
      abr.a      = Math.Exp( abr.a );
      abr.titel  = "Pow";
      abr.formel = "f(x) = " + roundSignificant( abr.a, SP ) + " * x ^ " + roundSignificant( abr.b, SP );
      return abr;
   }

   // Logarithmische Regression
   // y = a + b * ln(x)
   public RegressionResult calculateLogarithmicRegression( double[] xyArr )
   {
      if( xyArr == null || xyArr.Length < 2 || xyArr.Length % 2 != 0 ) return null;

      double[] xyArrConv = new double[xyArr.Length];

      for( int i=0; i < xyArr.Length; i+=2 ) {
         if( xyArr[i] <= 0 ) return null;
         xyArrConv[i]   = Math.Log( xyArr[i] );
         xyArrConv[i+1] = xyArr[i+1];
      }

      RegressionResult abr = calculateLinearRegression( xyArrConv );
      if( abr == null ) return null;
      abr.titel  = "Log";
      abr.formel = "f(x) = " + roundSignificant( abr.a, SP ) + " + " + roundSignificant( abr.b, SP ) + " * ln(x)";
      return abr;
   }

   // Exponentielle Regression
   // y = a * e^(b * x)
   // Regression ueber: ln(y) = ln(a) + b * x
   public RegressionResult calculateExponentialRegression( double[] xyArr )
   {
      if( xyArr == null || xyArr.Length < 2 || xyArr.Length % 2 != 0 ) return null;

      double[] xyArrConv = new double[xyArr.Length];

      for( int i=0; i < xyArr.Length; i+=2 ) {
         if( xyArr[i+1] <= 0 ) return null;
         xyArrConv[i]   = xyArr[i];
         xyArrConv[i+1] = Math.Log( xyArr[i+1] );
      }

      RegressionResult abr = calculateLinearRegression( xyArrConv );
      if( abr == null ) return null;
      abr.a      = Math.Exp( abr.a );
      abr.titel  = "Exp";
      abr.formel = "f(x) = " + roundSignificant( abr.a, SP ) + " * e ^ (" + roundSignificant( abr.b, SP ) + " * x)";
      return abr;
   }

   // Gespiegelte und verschobene exponentielle Regression
   // y = a * ( 1 - e^(-b * x) )
   // Approximationsfunktion beginnt bei 0 und strebt gegen den Grenzwert "limit".
   // Falls "limit" nicht bekannt ist: Iterativ naehern.
   public RegressionResult calculateOneMinusExponentialRegression( double[] xyArr, double limit )
   {
      double[] xyArrTest = new double[xyArr.Length];

      for( int i = 0; i < xyArr.Length; i+=2 ) {
         xyArrTest[i]   = -xyArr[i];
         xyArrTest[i+1] = limit - xyArr[i+1];
      }

      RegressionResult abr = calculateExponentialRegression( xyArrTest );
      if( abr == null ) return null;
      abr.a = limit;
      return abr;
   }

   private double roundSignificant( double d, int significantPrecision )
   {
      if( d == 0 || significantPrecision < 1 || significantPrecision > 14 ) return d;
      double mul10 = 1;
      double minVal = Math.Pow( 10, significantPrecision - 1 );
      while( Math.Abs( d ) < minVal ) {
         mul10 *= 10;
         d *= 10;
      }
      return Math.Round( d ) / mul10;
   }
}
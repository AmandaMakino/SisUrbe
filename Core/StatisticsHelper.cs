using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    // ReSharper disable PossibleMultipleEnumeration
    public class StatisticsHelper
    {
        private Func<double, double> _LinearRegression;
        private double? _LinearRegressionIntercept;
        private double? _LinearRegressionSlope;

        private StatisticsHelper()
        {
        }

        public IEnumerable<double> XValues { get; set; }
        public IEnumerable<double> YValues { get; set; }

        public double LinearRegressionSlope
        {
            get
            {
                if (XValues == null)
                    throw new InvalidOperationException("Linear Regression cannot be calculated without X Values");

                if (YValues == null)
                    throw new InvalidOperationException("Linear Regression cannot be calculated without Y Values");

                if (_LinearRegressionSlope == null)
                    _LinearRegressionSlope = CalculateLinearRegressionSlope(XValues, YValues);

                return _LinearRegressionSlope.Value;
            }
        }

        public double LinearRegressionIntercept
        {
            get
            {
                if (XValues == null)
                    throw new InvalidOperationException("Linear Regression cannot be calculated without X Values");

                if (YValues == null)
                    throw new InvalidOperationException("Linear Regression cannot be calculated without Y Values");

                if (_LinearRegressionIntercept == null)
                    _LinearRegressionIntercept = CalculateLinearRegressionIntercept(XValues, YValues, LinearRegressionSlope);

                return _LinearRegressionIntercept.Value;
            }
        }

        public Func<double, double> LinearRegressionFunc
        {
            get
            {
                if (XValues == null)
                    throw new InvalidOperationException("Linear Regression cannot be calculated without X Values");

                if (YValues == null)
                    throw new InvalidOperationException("Linear Regression cannot be calculated without Y Values");

                return _LinearRegression ?? (_LinearRegression = (x => LinearRegressionSlope*x + LinearRegressionIntercept));
            }
        }

        public IEnumerable<Tuple<double, double>> LinearRegressionValues
        {
            get 
            {
                return XValues.Select(x => new Tuple<double, double>(x, LinearRegressionFunc(x)));
            }
        }

        public static double CalculateLinearRegressionSlope(IEnumerable<double> xValues, IEnumerable<double> yValues)
        {
            if (xValues == null)
                throw new ArgumentNullException("xValues");

            if (yValues == null)
                throw new ArgumentNullException("yValues");

            var n = xValues.Count();

            if (n <= 0)
                throw new ArgumentOutOfRangeException("xValues", "At least one x value must be provided");

            if (n != yValues.Count())
                throw new ArgumentException("The number of y values must match the number of x values", "yValues");

            // m = (n * Σ(xy) - Σx * Σy) / (n * Σ(x * x) - (Σx * Σx))
            var sumOfX = xValues.Sum();
            var sumOfY = yValues.Sum();
            var sumOfSquareOfX = xValues.Sum(val => Math.Pow(val, 2));
            var sumOfProducts = xValues.Zip(yValues, (x, y) => x*y).Sum();

            var numerator = n*sumOfProducts - sumOfX*sumOfY;
            var denominator = n*sumOfSquareOfX - Math.Pow(sumOfX, 2);
            var slope = numerator/denominator;

            return slope;
        }

        public static double CalculateLinearRegressionIntercept(IEnumerable<double> xValues, IEnumerable<double> yValues, double? slope = null)
        {
            if (slope == null)
                slope = CalculateLinearRegressionSlope(xValues, yValues);

            // b = (Σy - m * Σx) / n
            double n = xValues.Count();
            double sumOfX = xValues.Sum();
            double sumOfY = yValues.Sum();

            double numerator = sumOfY - (slope.Value*sumOfX);
            double intercept = numerator/n;

            return intercept;
        }

        public static Func<double, double> BuildLinearRegressionFunction(IEnumerable<double> xValues, IEnumerable<double> yValues)
        {
            double slope = CalculateLinearRegressionSlope(xValues, yValues);
            double intercept = CalculateLinearRegressionIntercept(xValues, yValues, slope);

            // y = mx + b
            return (x => slope*x + intercept);
        }

        public static StatisticsHelper Create(IEnumerable<double> xValues, IEnumerable<double> yValues)
        {
            return new StatisticsHelper {
                XValues = xValues,
                YValues = yValues
            };
        }
    }

    // ReSharper restore PossibleMultipleEnumeration
}
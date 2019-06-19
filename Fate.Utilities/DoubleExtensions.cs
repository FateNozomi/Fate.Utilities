using System;

namespace Fate.Utilities
{
    public static class DoubleExtensions
    {
        public static bool ApproximatelyEquals(this double x, double value, double tolerance)
        {
            x = Math.Abs(x);
            value = Math.Abs(value);
            double difference = Math.Abs(x - value);
            return difference < tolerance;
        }
    }
}

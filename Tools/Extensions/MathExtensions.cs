using System;
using System.Linq;

namespace Tools.Extensions
{
    public static class MathExtensions
    {
        public static long GCD(long a, long b)
        {
            while (b != 0)
            {
                var t = b;
                b = a % b;
                a = t;
            }

            return a;
        }

        public static long GCD(params long[] numbers)
        {
            return numbers.Aggregate(GCD);
        }

        public static bool AlmostEquals(this float value1, float value2, float precision = 0.0000001f)
        {
            return Math.Abs(value1 - value2) <= precision;
        }

        public static bool AlmostEquals(this double value1, double value2, double precision = 0.0000001d)
        {
            return Math.Abs(value1 - value2) <= precision;
        }


        public static bool AlmostEquals(this decimal value1, decimal value2, decimal precision = 0.0000001m)
        {
            return Math.Abs(value1 - value2) <= precision;
        }

        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static double Remap(this double value, double from1, double to1, double from2, double to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static decimal Remap(this decimal value, decimal from1, decimal to1, decimal from2, decimal to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0)
            {
                return min;
            }

            if (val.CompareTo(max) > 0)
            {
                return max;
            }

            return val;
        }

        /// <summary>
        ///     An equation of the form a⋅x^2 + b⋅x + c = 0 is a quadratic equation.
        /// </summary>
        /// <param name="a">real number a ≠ 0</param>
        /// <param name="b">real number</param>
        /// <param name="c">real numbers</param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns>false if no solution otherwise true</returns>
        public static bool QuadraticEquation(
            float a, float b, float c,
            out float x1, out float x2)
        {
            var discriminant = b * b - 4 * a * c; //Math.Pow(b, 2) - 4 * a * c;

            if (discriminant < 0)
            {
                x1 = -1;
                x2 = -1;

                return false;
            }

            if (discriminant == 0) //quadratic equation has two equal roots
            {
                x1 = -b / (2 * a);
                x2 = x1;
            }
            else //the equation has two different roots
            {
                x1 = (-b + MathF.Sqrt(discriminant)) / (2 * a);
                x2 = (-b - MathF.Sqrt(discriminant)) / (2 * a);
            }

            return true;
        }

        /// <summary>
        ///     An equation of the form a⋅x^2 + b⋅x + c = 0 is a quadratic equation.
        /// </summary>
        /// <param name="a">real number a ≠ 0</param>
        /// <param name="b">real number</param>
        /// <param name="c">real numbers</param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns>false if no solution otherwise true</returns>
        public static bool QuadraticEquation(
            double a, double b, double c,
            out double x1, out double x2)
        {
            var discriminant = b * b - 4 * a * c; //Math.Pow(b, 2) - 4 * a * c;

            if (discriminant < 0)
            {
                x1 = -1;
                x2 = -1;

                return false;
            }

            if (discriminant == 0) //quadratic equation has two equal roots
            {
                x1 = -b / (2 * a);
                x2 = x1;
            }
            else //the equation has two different roots
            {
                x1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                x2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
            }

            return true;
        }
    }
}
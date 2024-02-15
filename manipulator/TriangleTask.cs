using System;
using NUnit.Framework;

namespace Manipulation
{
    public class TriangleTask
    {
        /// <summary>
        /// Возвращает угол (в радианах) между сторонами a и b в треугольнике со сторонами a, b, c 
        /// </summary>
        public static double GetABAngle(double a, double b, double c)
        {
            if (!(a + b >= c) || !(a + c >= b) || !(b + c >= a))
                return double.NaN;
            if (!(a > 0) || !(b > 0))
                return double.NaN;
            
            var cosAngle = (a * a + b * b - c * c) / (2 * a * b);
            var acosAngle = Math.Acos(cosAngle);
            
            return acosAngle;

        }
    }

    [TestFixture]
    public class TriangleTaskTests
    {
        [TestCase(3, 4, 5, Math.PI / 2)]
        [TestCase(1, 1, 1, Math.PI / 3)]
        [TestCase(5, 12, 13, Math.PI / 2)]
        [TestCase(1, 2, 3, Math.PI)]
        [TestCase(1, 2, 4, double.NaN)]
        public void TestGetABAngle(double a, double b, double c, double expectedAngle)
        {
            var actual = TriangleTask.GetABAngle(a, b, c);

            Assert.AreEqual(expectedAngle, actual, 1e-5);
        }
    }
}
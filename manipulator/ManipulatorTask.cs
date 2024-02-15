using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Manipulation
{
    public static class ManipulatorTask
    {
        /// <summary>
        /// Возвращает массив углов (shoulder, elbow, wrist),
        /// необходимых для приведения эффектора манипулятора в точку x и y 
        /// с углом между последним суставом и горизонталью, равному alpha (в радианах)
        /// См. чертеж manipulator.png!
        /// </summary>
        public static double[] MoveManipulatorTo(double x, double y, double alpha)
        {
            var xWrist = x + Manipulator.Palm * Math.Cos(Math.PI - alpha);
            var yWrist = y + Manipulator.Palm * Math.Sin(Math.PI - alpha);

            var distanceToWrist = DistanceTo(
                new PointD(0.0, 0.0),
                new PointD(xWrist, yWrist));
            var elbow = TriangleTask.GetABAngle(Manipulator.UpperArm, Manipulator.Forearm, distanceToWrist);

            var firstHelpAngle = TriangleTask.GetABAngle(Manipulator.UpperArm, distanceToWrist, Manipulator.Forearm);
            var secondHelpAngle = Math.Atan2(yWrist, xWrist);

            var shoulder = firstHelpAngle + secondHelpAngle;
            var wrist = -alpha - shoulder - elbow;

            if (double.IsNaN(shoulder) ||
                double.IsNaN(elbow) ||
                double.IsNaN(wrist))
                return new[] { double.NaN, double.NaN, double.NaN };

            return new[] { shoulder, elbow, wrist };
        }

        private static double DistanceTo(PointD a, PointD b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }
    }

    public class PointD
    {
        public double X { get; }
        public double Y { get; }

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    [TestFixture]
    public class ManipulatorTaskTests
    {
        [Test]
        public void TestMoveManipulatorTo()
        {
            for (var i = 0; i < 1000; i++)
            {
                var rand = new Random();

                var randomPoint = GenerateRandomPoint(rand);
                var angle = GenerateRandomAngle(rand);
                var maxValue = GetMaxValue();

                var rmin = Math.Abs(Manipulator.UpperArm - Manipulator.Forearm - Manipulator.Palm);
                var rmax = Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm;

                var actualAngles = ManipulatorTask.MoveManipulatorTo(randomPoint.X, randomPoint.Y, angle);

                if (DistanceTo(new PointF(0f, 0f), randomPoint) > rmax ||
                    DistanceTo(new PointF(0f, 0f), randomPoint) < rmin)
                {
                    Assert.AreEqual(double.NaN, actualAngles[0]);
                    Assert.AreEqual(double.NaN, actualAngles[1]);
                    Assert.AreEqual(double.NaN, actualAngles[2]);
                }
                else
                {
                    var actualPoint = GetPointThroughAngles(actualAngles);
                    Assert.AreEqual(randomPoint.X, actualPoint.X, 1e-5);
                    Assert.AreEqual(randomPoint.Y, actualPoint.Y, 1e-5);
                }
            }
        }

        private static PointF GenerateRandomPoint(Random rand)
        {
            var x = rand.NextDouble() * 500;
            var y = rand.NextDouble() * 500;

            return new PointF((float)x, (float)y);
        }

        private static double GenerateRandomAngle(Random rand)
        {
            return rand.NextDouble() * Math.PI;
        }

        private static double DistanceTo(PointF a, PointF b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        private static float GetMaxValue() => 
            Manipulator.Palm + Manipulator.UpperArm + Manipulator.Forearm;
        

        private static PointF GetPointThroughAngles(IReadOnlyList<double> angles)
        {
            var coordinates = AnglesToCoordinatesTask.GetJointPositions(angles[0], angles[1], angles[2]);

            return new PointF(coordinates[2].X, coordinates[2].Y);
        }
    }
}
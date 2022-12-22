using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        /// <summary>
        /// По значению углов суставов возвращает массив координат суставов
        /// в порядке new []{elbow, wrist, palmEnd}
        /// </summary>
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            var elbowOX = shoulder + Math.PI + elbow;
            var wristOX = elbowOX + Math.PI + wrist;

            var elbowPos = CalculateAnglePos(shoulder, new PointF(0f, 0f), Manipulator.UpperArm);
            var wristPos = CalculateAnglePos(elbowOX, elbowPos, Manipulator.Forearm);
            var palmEndPos = CalculateAnglePos(wristOX, wristPos, Manipulator.Palm);

            return new[]
            {
                elbowPos,
                wristPos,
                palmEndPos
            };
        }

        private static PointF CalculateAnglePos(double angle, PointF anglePos, float partLength)
        {
            var cosAngle = (float)Math.Cos(angle);
            var sinAngle = (float)Math.Sin(angle);

            return new PointF(
                anglePos.X + partLength * cosAngle,
                anglePos.Y + partLength * sinAngle);
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTaskTests
    {
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(Math.PI / 2, Math.PI, Math.PI / 2, Manipulator.Palm, Manipulator.Forearm + Manipulator.UpperArm)]
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI / 2, Manipulator.Forearm, Manipulator.UpperArm - Manipulator.Palm)]
        [TestCase(Math.PI / 2, Math.PI, Math.PI, 0, Manipulator.Forearm + Manipulator.Palm + Manipulator.UpperArm)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);

            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
        }
    }
}
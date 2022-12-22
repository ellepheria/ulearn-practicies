using System;
using System.Drawing;
using System.Windows.Forms;

namespace Manipulation
{
    public static class VisualizerTask
    {
        public static readonly Brush UnreachableAreaBrush = new SolidBrush(Color.FromArgb(255, 255, 230, 230));
        
        private static readonly Brush ReachableAreaBrush = new SolidBrush(Color.FromArgb(255, 230, 255, 230));
        private static readonly Pen ManipulatorPen = new Pen(Color.Black, 3);
        
        private static double x = 220;
        private static double y = -100;
        private static double alpha = 0.05;
        private static double wrist = 2 * Math.PI / 3;
        private static double elbow = 3 * Math.PI / 4;
        private static double shoulder = Math.PI / 2;
        private static double delta = 0.1;

        private static float width = 5;
        private static float height = 5;

        public static void KeyDown(Form form, KeyEventArgs key)
        {
            switch (key.KeyCode)
            {
                case Keys.Q:
                    shoulder += delta;
                    break;
                case Keys.A:
                    shoulder -= delta;
                    break;
                case Keys.W:
                    elbow += delta;
                    break;
                case Keys.S:
                    elbow -= delta;
                    break;
            }
            wrist = -alpha - shoulder - elbow;
            UpdateManipulator();
            form.Invalidate();
        }


        public static void MouseMove(Form form, MouseEventArgs e)
        {
            var windowPoint = new PointF(e.X, e.Y);
            var mathPoint = ConvertWindowToMath(windowPoint, GetShoulderPos(form));

            x = mathPoint.X;
            y = mathPoint.Y;

            UpdateManipulator();
            form.Invalidate();
        }

        public static void MouseWheel(Form form, MouseEventArgs e)
        {
            alpha += e.Delta;

            UpdateManipulator();
            form.Invalidate();
        }

        private static void UpdateManipulator()
        {
            var angles = ManipulatorTask.MoveManipulatorTo(x, y, alpha);
            
            if (double.IsNaN(angles[0]))
                return;
            
            shoulder = angles[0];
            elbow = angles[1];
            wrist = angles[2];
        }

        public static void DrawManipulator(Graphics graphics, PointF shoulderPos)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);

            graphics.DrawString(
                $"X={x:0}, Y={y:0}, Alpha={alpha:0.00}",
                new Font(SystemFonts.DefaultFont.FontFamily, 12),
                Brushes.DarkRed,
                10,
                10);
            
            DrawReachableZone(graphics, ReachableAreaBrush, UnreachableAreaBrush, shoulderPos, joints);
            DrawManipulatorElements(graphics, shoulderPos, joints);
        }

        private static void DrawManipulatorElements(Graphics graphics, PointF shoulderPos, PointF[] joints)
        {
            var elbowPosWindow = ConvertMathToWindow(joints[0], shoulderPos);
            var wristPosWindow = ConvertMathToWindow(joints[1], shoulderPos);
            var palmEndPosWindow = ConvertMathToWindow(joints[2], shoulderPos);

            graphics.DrawLine(ManipulatorPen, shoulderPos, elbowPosWindow);
            graphics.DrawEllipse(ManipulatorPen, elbowPosWindow.X, elbowPosWindow.Y, width, height);

            graphics.DrawLine(ManipulatorPen, elbowPosWindow, wristPosWindow);
            graphics.DrawEllipse(ManipulatorPen, wristPosWindow.X, wristPosWindow.Y, width, height);

            graphics.DrawLine(ManipulatorPen, wristPosWindow, palmEndPosWindow);
            graphics.DrawEllipse(ManipulatorPen, palmEndPosWindow.X, palmEndPosWindow.Y, width, height);
        }

        private static void DrawReachableZone(
            Graphics graphics,
            Brush reachableBrush,
            Brush unreachableBrush,
            PointF shoulderPos,
            PointF[] joints)
        {
            var rmin = Math.Abs(Manipulator.UpperArm - Manipulator.Forearm);
            var rmax = Manipulator.UpperArm + Manipulator.Forearm;
            var mathCenter = new PointF(joints[2].X - joints[1].X, joints[2].Y - joints[1].Y);
            var windowCenter = ConvertMathToWindow(mathCenter, shoulderPos);
            graphics.FillEllipse(reachableBrush, windowCenter.X - rmax, windowCenter.Y - rmax, 2 * rmax, 2 * rmax);
            graphics.FillEllipse(unreachableBrush, windowCenter.X - rmin, windowCenter.Y - rmin, 2 * rmin, 2 * rmin);
        }

        private static PointF GetShoulderPos(Form form) =>
            new PointF(form.ClientSize.Width / 2f, form.ClientSize.Height / 2f);

        private static PointF ConvertMathToWindow(PointF mathPoint, PointF shoulderPos) =>
            new PointF(mathPoint.X + shoulderPos.X, shoulderPos.Y - mathPoint.Y);

        private static PointF ConvertWindowToMath(PointF windowPoint, PointF shoulderPos) =>
            new PointF(windowPoint.X - shoulderPos.X, shoulderPos.Y - windowPoint.Y);
    }
}
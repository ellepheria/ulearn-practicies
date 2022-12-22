namespace GeometryTasks;

public class Vector
{
    public readonly double X;
    public readonly double Y;

    public Vector()
    {
        X = 0;
        Y = 0;
    }

    public Vector(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double GetLength() =>
        Geometry.GetLength(this);

    public Vector Add(Vector vector) =>
        Geometry.Add(this, vector);

    public bool Belongs(Segment segment) =>
        Geometry.IsVectorInSegment(this, segment);
}
namespace GeometryTasks;

public static class Geometry
{
    public static double GetLength(Vector vector) =>
        Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);

    public static double GetLength(Segment segment)
    {
        var dx = segment.End.X - segment.Begin.X;
        var dy = segment.End.Y - segment.Begin.Y;

        return Math.Sqrt(dx * dx + dy * dy);
    }

    public static Vector Add(Vector vector1, Vector vector2) =>
        new Vector(vector1.X + vector2.X, vector1.Y + vector2.Y);

    public static bool IsVectorInSegment(Vector vector, Segment segment)
    {
        var distanceToBegin = GetLength(new Vector(segment.Begin.X - vector.X, segment.Begin.Y - vector.Y));
        var distanceToEnd = GetLength(new Vector(segment.End.X - vector.X, segment.End.Y - vector.Y));

        return distanceToBegin + distanceToEnd == GetLength(segment);
    }
}
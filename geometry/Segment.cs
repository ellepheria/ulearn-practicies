namespace GeometryTasks;

public class Segment
{
    public readonly Vector Begin;
    public readonly Vector End;

    public Segment()
    {
        Begin = new Vector();
        End = new Vector();
    }

    public Segment(Vector begin, Vector end)
    {
        Begin = begin;
        End = end;
    }

    public double GetLength() =>
        Geometry.GetLength(this);

    public bool Contains(Vector vector) =>
        Geometry.IsVectorInSegment(vector, this);
}
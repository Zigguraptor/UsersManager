namespace UsersManager.Application.Geometry;

public class Triangle : Shape
{
    public Triangle(float a, float b, float c)
    {
        if (!IsPossible(a, b, c))
            throw new ArgumentException("Impossible triangle");

        A = a;
        B = b;
        C = c;
    }

    public float A { get; }
    public float B { get; }
    public float C { get; }

    //Прямоугольный?
    public bool IsRight
    {
        get
        {
            var edges = new float[3];
            edges[0] = A;
            edges[1] = B;
            edges[2] = C;
            Array.Sort(edges);

            return FloatCompare(edges[2] * edges[2], edges[0] * edges[0] + edges[1] * edges[1]);
        }
    }

    public static bool IsPossible(float a, float b, float c)
    {
        if (a <= 0f && b <= 0f && c <= 0f) return false;

        var ab = a + b;
        var ac = a + c;
        var bc = b + c;

        return ab > c && ac > b && bc > a;
    }

    public static bool TryCreate(float a, float b, float c, out Triangle? triangle)
    {
        try
        {
            triangle = new Triangle(a, b, c);
        }
        catch
        {
            triangle = null;
            return false;
        }

        return true;
    }

    public override float Area
    {
        get
        {
            var p = (A + B + C) / 2;
            AreaCache ??= float.Sqrt(p * (p - A) * (p - B) * (p - C));
            return (float)AreaCache;
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is Triangle temp)
            return FloatCompare(A, temp.A) && FloatCompare(B, temp.B) && FloatCompare(C, temp.C);

        return false;
    }

    public override int GetHashCode()
    {
        var hash = 17;
        hash = 486187739 * hash + A.GetHashCode();
        hash = 486187739 * hash + B.GetHashCode();
        hash = 486187739 * hash + C.GetHashCode();

        return hash;
    }

    public override string ToString()
    {
        var isRight = IsRight ? "Right triangle." : "Not right triangle.";
        return $"A = {A}, B = {B}, C = {C}. {isRight}";
    }
}

using System.Collections.ObjectModel;
using UsersManager.Application.Geometry;

namespace UserManagerTests;

public class TriangleTests
{
    private ReadOnlyCollection<Triangle> _isoscelesTriangles;
    private ReadOnlyCollection<Triangle> _equilateralTriangles;
    private ReadOnlyCollection<Triangle> _rightTriangles;

    public TriangleTests()
    {
        _isoscelesTriangles = InitIsoscelesTriangles();
        _equilateralTriangles = InitEquilateralTriangles();
        _rightTriangles = InitRightTriangles();
    }

    #region Init

    private ReadOnlyCollection<Triangle> InitIsoscelesTriangles() =>
        new List<Triangle>
        {
            new(3f, 3f, 5f),
            new(4f, 3f, 3f),
            new(3f, 4f, 3f)
        }.AsReadOnly();

    private ReadOnlyCollection<Triangle> InitEquilateralTriangles() =>
        new List<Triangle>
        {
            new(3f, 3f, 3f),
            new(1000f, 1000f, 1000f),
            new(3.5f, 3.5f, 3.5f)
        }.AsReadOnly();

    private ReadOnlyCollection<Triangle> InitRightTriangles()
    {
        var a = 3.5f;
        var b = 6.5f;
        var c = float.Sqrt(a * a + b * b);
        return new List<Triangle>
        {
            new(a, b, c),
            new(3f, 4f, 5f),
            new(4f, 3f, 5f),
            new(5f, 4f, 3f),
            new(65f, 72f, 97f)
        }.AsReadOnly();
    }

    #endregion

    [Theory]
    [InlineData(0f, 4f, 5f)]
    [InlineData(4f, 0f, 5f)]
    [InlineData(5f, 4f, 0f)]
    [InlineData(0f, 4f, -9789f)]
    [InlineData(5f, -4f, 0f)]
    [InlineData(-65f, 72f, 97f)]
    [InlineData(1165f, 72f, 97f)]
    public void Constructor_ImpossibleTriangles_ThrowsArgumentException(float a, float b, float c)
    {
        // Assert
        Assert.Throws<ArgumentException>(() => new Triangle(a, b, c));
    }

    [Theory]
    [InlineData(2340f, 65f, 72f, 97f)]
    [InlineData(3.8971143f, 3f, 3f, 3f)]
    [InlineData(8.944272f, 6f, 7f, 3f)]
    public void Area_PossibleTriangles_ReturnsArea(float expectedArea, float a, float b, float c)
    {
        var triangle = new Triangle(a, b, c);

        // Assert
        Assert.Equal(expectedArea, triangle.Area);
    }

    [Fact]
    public void IsRight_RightTriangles_ReturnsTrue()
    {
        Assert.True(_rightTriangles.Select(triangle => triangle.IsRight).All(b => b));
    }

    [Fact]
    public void IsRight_NotRightTriangles_ReturnsFalse()
    {
        Assert.True(_isoscelesTriangles.Select(triangle => triangle.IsRight).All(b => !b));
        Assert.True(_equilateralTriangles.Select(triangle => triangle.IsRight).All(b => !b));
    }
}

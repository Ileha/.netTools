using Tools.Extensions;

namespace Test.Extensions;

[TestFixture]
public class MathExtensionsTests
{
    [TestCase(54, 24, 6)]
    [TestCase(48, 180, 12)]
    [TestCase(17, 5, 1)]
    public void GCD_TwoNumbers_ReturnsGreatestCommonDivisor(long num1, long num2, long result)
    {
        // Arrange & Act
        var result1 = MathExtensions.GCD(num1, num2);

        // Assert
        Assert.That(result1, Is.EqualTo(result));
    }

    [TestCase(12, 24, 36, 60)]
    public void GCD_MultipleNumbers_ReturnsGreatestCommonDivisor(long result, params long[] numbers)
    {
        // Arrange & Act
        var result1 = MathExtensions.GCD(numbers);

        // Assert
        Assert.That(result1, Is.EqualTo(result));
    }

    [Test]
    public void AlmostEquals_Float_WithDefaultPrecision_ReturnsTrueForCloseValues()
    {
        // Arrange
        var a = 1.23456718f;
        var b = 1.23456719f; // Difference within default precision

        // Act
        var result = a.AlmostEquals(b);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void AlmostEquals_Double_WithCustomPrecision_ReturnsCorrectResult()
    {
        // Arrange
        var a = 1.23456789012345;
        var b = 1.23456789012346;
        var precision = 1e-15; // Very small precision

        // Act
        var result1 = a.AlmostEquals(b, precision);
        var result2 = a.AlmostEquals(b, 1e-14); // Larger precision

        // Assert
        Assert.IsFalse(result1);
        Assert.IsTrue(result2);
    }

    [TestCase(5, 0, 10, 0, 100, 50)]
    public void Remap_Float_ReturnsCorrectlyRemappedValue(
        float value,
        float from1,
        float to1,
        float from2,
        float to2,
        float expected)
    {
        // Act
        var result = value.Remap(from1, to1, from2, to2);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Clamp_ValueWithinRange_ReturnsSameValue()
    {
        // Arrange
        var value = 5;
        var min = 0;
        var max = 10;

        // Act
        var result = value.Clamp(min, max);

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Clamp_ValueBelowMin_ReturnsMin()
    {
        // Arrange
        var value = -1;
        var min = 0;
        var max = 10;

        // Act
        var result = value.Clamp(min, max);

        // Assert
        Assert.That(result, Is.EqualTo(min));
    }

    [Test]
    public void Clamp_ValueAboveMax_ReturnsMax()
    {
        // Arrange
        var value = 15;
        var min = 0;
        var max = 10;

        // Act
        var result = value.Clamp(min, max);

        // Assert
        Assert.That(result, Is.EqualTo(max));
    }

    [Test]
    public void QuadraticEquation_Float_WithRealRoots_ReturnsCorrectRoots()
    {
        // Arrange
        // x² - 5x + 6 = 0 has roots at x=2 and x=3
        float a = 1, b = -5, c = 6;

        // Act
        var hasRealRoots = MathExtensions.QuadraticEquation(a, b, c, out var x1, out var x2);

        // Assert
        Assert.IsTrue(hasRealRoots);
        Assert.That(x1, Is.EqualTo(3f));
        Assert.That(x2, Is.EqualTo(2f));
    }

    [Test]
    public void QuadraticEquation_Float_WithNoRealRoots_ReturnsFalse()
    {
        // Arrange
        // x² + 1 = 0 has no real roots
        float a = 1, b = 0, c = 1;

        // Act
        var hasRealRoots = MathExtensions.QuadraticEquation(a, b, c, out _, out _);

        // Assert
        Assert.IsFalse(hasRealRoots);
    }

    [Test]
    public void QuadraticEquation_Double_WithDoubleRoot_ReturnsSameRootTwice()
    {
        // Arrange
        // x² - 4x + 4 = 0 has a double root at x=2
        double a = 1, b = -4, c = 4;

        // Act
        var hasRealRoots = MathExtensions.QuadraticEquation(a, b, c, out var x1, out var x2);

        // Assert
        Assert.IsTrue(hasRealRoots);
        Assert.That(x1, Is.EqualTo(2.0));
        Assert.That(x2, Is.EqualTo(2.0));
    }
}
using Tools.Extensions;

namespace Test.Extensions;

[TestFixture]
public class TimeSpanExtensionsTests
{
    public static readonly object[] NanoSeconds2SpanCases =
        // ReSharper disable once UseCollectionExpression
    {
        new object[] { 1_000_000_000L, TimeSpan.FromSeconds(1) },
        new object[] { 1_000_000L, TimeSpan.FromMilliseconds(1) },
        new object[] { 1_000L, TimeSpan.FromMicroseconds(1) },
        new object[] { 100L, TimeSpan.FromTicks(1) },
        new object[] { 500_000_000L, TimeSpan.FromMilliseconds(500) },
        new object[] { 86_400_000_000_000L, TimeSpan.FromDays(1) },// 1 day in nanoseconds
    };

    [TestCaseSource(nameof(NanoSeconds2SpanCases))]
    public void FromNanoseconds_WithValidValue_ReturnsCorrectTimeSpan(long nanoseconds, TimeSpan expected)
    {
        // Arrange & Act
        var result = TimeSpanExtensions.FromNanoseconds(nanoseconds);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void FromNanoseconds_WithZero_ReturnsZeroTimeSpan()
    {
        // Act
        var result = TimeSpanExtensions.FromNanoseconds(0);

        // Assert
        Assert.That(result, Is.EqualTo(TimeSpan.Zero));
    }

    [Test]
    public void FromNanoseconds_WithFractionalNanoseconds_RoundsToNearestTick()
    {
        // Arrange
        // 1.5 ticks = 150ns (since 1 tick = 100ns)
        // This should round to 2 ticks (200ns) since it's closer to 2 ticks than 1 tick
        double oneAndHalfTicksInNs = 150;

        // Act
        var result = TimeSpanExtensions.FromNanoseconds(oneAndHalfTicksInNs);

        // Assert
        // Should round to 2 ticks (200ns)
        Assert.That(result, Is.EqualTo(TimeSpan.FromTicks(2)));
    }
}
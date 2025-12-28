using Tools.Extensions;

namespace Test.Extensions;

[TestFixture]
public class DateTimeExtensionsTests
{
    [Test]
    public void ToUnixTime_ValidDateTime_ReturnsCorrectUnixTime()
    {
        // Arrange
        var date = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var expected = 1672531200; // Known Unix timestamp for 2023-01-01 00:00:00 UTC

        // Act
        var result = date.ToUnixTime();

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void FromUnixTime_ValidUnixTime_ReturnsCorrectDateTime()
    {
        // Arrange
        var unixTime = 1672531200L; // 2023-01-01 00:00:00 UTC
        var expected = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Act
        var result = unixTime.FromUnixTime();

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void ToUnixTime_And_FromUnixTime_AreInverseOperations()
    {
        // Arrange
        var originalDate = DateTime.UtcNow;

        // Act
        var unixTime = originalDate.ToUnixTime();
        var roundTrippedDate = unixTime.FromUnixTime();

        // Assert - Allow for 1 second difference due to second precision in Unix time
        var difference = (originalDate - roundTrippedDate).Duration();
        Assert.LessOrEqual(difference.TotalSeconds, 1);
    }
}
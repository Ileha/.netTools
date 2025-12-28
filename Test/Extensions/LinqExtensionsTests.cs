using Tools.Extensions;

namespace Test.Extensions;

[TestFixture]
public class LinqExtensionsTests
{
    [Test]
    public void CartesianProduct_TwoSequences_ReturnsAllCombinations()
    {
        // Arrange
        var sequences = new List<IEnumerable<string>>
        {
            new[] {"A", "B"},
            new[] {"1", "2"}
        };
        var expected = new[]
        {
            new[] {"A", "1"},
            new[] {"A", "2"},
            new[] {"B", "1"},
            new[] {"B", "2"}
        };

        // Act
        var result = sequences.CartesianProduct().Select(x => x.ToArray()).ToArray();

        // Assert
        Assert.That(result.Length, Is.EqualTo(expected.Length));

        for (var i = 0; i < expected.Length; i++)
        {
            CollectionAssert.AreEqual(expected[i], result[i]);
        }
    }

    [Test]
    public void Shuffle_WithRandomSeed_ReturnsShuffledCollection()
    {
        // Arrange
        var source = Enumerable.Range(1, 100).ToArray();
        var random = new Random(42); // Fixed seed for test determinism

        // Act
        var result = source.Shuffle(random).ToArray();

        // Assert
        CollectionAssert.AreNotEqual(source, result);
        CollectionAssert.AreEquivalent(source, result);
    }

    [Test]
    public void AddRange_ToHashSet_AddsAllItems()
    {
        // Arrange
        var set = new HashSet<int> {1, 2, 3};
        var newItems = new[] {4, 5, 6};

        // Act
        set.AddRange(newItems);

        // Assert
        CollectionAssert.AreEquivalent(new[] {1, 2, 3, 4, 5, 6}, set);
    }

    [Test]
    public void CustomMaxOrDefault_WithItems_ReturnsMax()
    {
        // Arrange
        var items = new[] {"a", "abc", "ab"};

        // Act
        var result = items.CustomMaxOrDefault(s => s.Length);

        // Assert
        Assert.That(result, Is.EqualTo("abc"));
    }

    [Test]
    public void CustomMaxOrDefault_EmptyCollection_ReturnsDefault()
    {
        // Arrange
        var items = Array.Empty<string>();

        // Act
        var result = items.CustomMaxOrDefault(s => s.Length);

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void CustomMinOrDefault_WithItems_ReturnsMin()
    {
        // Arrange
        var items = new[] {"a", "abc", "ab"};

        // Act
        var result = items.CustomMinOrDefault(s => s.Length);

        // Assert
        Assert.That(result, Is.EqualTo("a"));
    }

    [Test]
    public void FromParams_WithItems_ReturnsSameItems()
    {
        // Arrange
        var expected = new[] {1, 2, 3};

        // Act
        var result = LinqExtensions.FromParams(1, 2, 3);

        // Assert
        CollectionAssert.AreEqual(expected, result);
    }

    [Test]
    public void ForEach_WithAction_ExecutesForEachItem()
    {
        // Arrange
        var items = new[] {1, 2, 3};
        var sum = 0;

        // Act
        items.ForEach(x => sum += x);

        // Assert
        Assert.That(sum, Is.EqualTo(6));
    }

    [Test]
    public void ForEach_WithIndex_ExecutesForEachItemWithIndex()
    {
        // Arrange
        var items = new[] {"a", "b", "c"};
        var result = new List<(string, int)>();

        // Act
        items.ForEach((item, index) => result.Add((item, index)));

        // Assert
        var expected = new[] {("a", 0), ("b", 1), ("c", 2)};
        CollectionAssert.AreEqual(expected, result);
    }
}
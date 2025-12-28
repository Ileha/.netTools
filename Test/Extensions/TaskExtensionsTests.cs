using Tools.Extensions;

namespace Test.Extensions;

[TestFixture]
public class TaskExtensionsTests
{
    [Test]
    public async Task TrackExecutionTimeAsync_WithResult_ReturnsCorrectValueAndTracksTime()
    {
        // Arrange
        var task = Task.FromResult(42);

        // Act
        var result = await task.TrackExecutionTimeAsync(out var stopwatch);

        // Assert
        Assert.That(result, Is.EqualTo(42));
        Assert.IsTrue(stopwatch.ElapsedMilliseconds >= 0);
    }

    [Test]
    public async Task TrackExecutionTimeAsync_WithoutResult_TracksTime()
    {
        // Arrange
        var task = Task.Delay(100);

        // Act
        await task.TrackExecutionTimeAsync(out var stopwatch);

        // Assert
        Assert.IsTrue(stopwatch.ElapsedMilliseconds >= 90); // Account for timing variations
    }

    [Test]
    public void WithCancellationAsync_WithResult_CancelsWhenRequested()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        // ReSharper disable once MethodSupportsCancellation
        var longRunningTask = Task.Delay(5000)
            // ReSharper disable once MethodSupportsCancellation
            .ContinueWith(_ => 42);

        // Act & Assert
        cts.CancelAfter(100);

        Assert.ThrowsAsync<TaskCanceledException>(() =>
            longRunningTask.WithCancellationAsync(cts.Token));
    }

    [Test]
    public void WithCancellationAsync_WithoutResult_CancelsWhenRequested()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        // ReSharper disable once MethodSupportsCancellation
        var longRunningTask = Task.Delay(5000);

        // Act & Assert
        cts.CancelAfter(100);

        Assert.ThrowsAsync<TaskCanceledException>(() =>
            longRunningTask.WithCancellationAsync(cts.Token));
    }

    [Test]
    public async Task WithCancellationAsync_WithResult_CompletesNormallyIfNotCanceled()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var task = Task.FromResult(42);

        // Act
        var result = await task.WithCancellationAsync(cts.Token);

        // Assert
        Assert.That(result, Is.EqualTo(42));
    }

    [Test]
    public async Task WithCancellationAsync_WithoutResult_CompletesNormallyIfNotCanceled()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var task = Task.CompletedTask;

        // Act & Assert (should not throw)
        await task.WithCancellationAsync(cts.Token);
    }

    [Test]
    public async Task ForgetAsync_WithException_DoesNotThrow()
    {
        // Arrange
        var task = Task.FromException(new Exception("Test exception"));

        // Act & Assert (should not throw)
        await task.ForgetAsync();

        // The test passes if no exception is thrown
        Assert.Pass();
    }

    [Test]
    public async Task ForgetAsync_WithSuccessfulTask_CompletesNormally()
    {
        // Arrange
        var task = Task.Delay(100);

        // Act & Assert (should not throw)
        await task.ForgetAsync();

        // The test passes if no exception is thrown
        Assert.Pass();
    }
}
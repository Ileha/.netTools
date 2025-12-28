using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
#if !UNITY_5_3_OR_NEWER
using Microsoft.Extensions.Logging;
#endif

namespace Tools.Extensions
{
    public static class TaskExtensions
    {
        public static Task<T> TrackExecutionTimeAsync<T>(this Task<T> task, out Stopwatch stopwatch)
        {
            stopwatch = new Stopwatch();

            return TrackExecutionTimeInternalAsync(stopwatch);

            async Task<T> TrackExecutionTimeInternalAsync(Stopwatch stopwatch)
            {
                try
                {
                    stopwatch.Restart();
                    return await task;
                }
                finally
                {
                    stopwatch.Stop();
                }
            }
        }

        public static Task TrackExecutionTimeAsync(this Task task, out Stopwatch stopwatch)
        {
            stopwatch = new Stopwatch();

            return TrackExecutionTimeInternalAsync(stopwatch);

            async Task TrackExecutionTimeInternalAsync(Stopwatch stopwatch)
            {
                try
                {
                    stopwatch.Restart();
                    await task;
                }
                finally
                {
                    stopwatch.Stop();
                }
            }
        }

        public static Task<T> TrackExecutionTimeRefAsync<T>(this Task<T> task, ref Stopwatch stopwatch)
        {
            return TrackExecutionTimeInternalAsync(stopwatch);

            async Task<T> TrackExecutionTimeInternalAsync(Stopwatch stopwatch)
            {
                try
                {
                    stopwatch.Restart();
                    return await task;
                }
                finally
                {
                    stopwatch.Stop();
                }
            }
        }

        public static Task TrackExecutionTimeRefAsync(this Task task, ref Stopwatch stopwatch)
        {
            return TrackExecutionTimeInternalAsync(stopwatch);

            async Task TrackExecutionTimeInternalAsync(Stopwatch stopwatch)
            {
                try
                {
                    stopwatch.Restart();
                    await task;
                }
                finally
                {
                    stopwatch.Stop();
                }
            }
        }

        public static async ValueTask ForgetAsync(
            this Task task,
#if !UNITY_5_3_OR_NEWER
            ILogger? logger = null,
#endif
            [CallerFilePath] string? sourceFilePath = null,
            [CallerMemberName] string? method = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
#pragma warning disable VSTHRD003
                await task;
#pragma warning restore VSTHRD003
            }
            catch (Exception e)
            {
                var message = $"{nameof(ForgetAsync)}: exception {e.GetType()} happen\n" +
                              $"called at line {lineNumber}: {sourceFilePath}:{method}";

#if UNITY_5_3_OR_NEWER
                UnityEngine.Debug.LogError(message);
#else
                if (logger == null)
                {
                    Console.WriteLine(
                        $"{message}\n" +
                        $"{e}");
                }
                else
                {
                    logger.LogError(e, message);
                }
#endif
            }
        }

        /// <summary>
        ///     Attaches a CancellationToken to a Task, canceling it if the token is triggered.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the Task.</typeparam>
        /// <param name="task">The original Task.</param>
        /// <param name="cancellationToken">The CancellationToken to monitor.</param>
        /// <returns>A new Task that respects the provided CancellationToken.</returns>
        public static async Task<T> WithCancellationAsync<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var cancellationTask = CancellationTaskAsync<T>(cancellationToken);

            var completedTask = await Task.WhenAny(task, cancellationTask);

            if (completedTask == cancellationTask)
            {
                await cancellationTask;
            }

            // Otherwise, return the result of the original task
            return await task;
        }

        /// <summary>
        ///     Attaches a CancellationToken to a Task, canceling it if the token is triggered.
        /// </summary>
        /// <param name="task">The original Task.</param>
        /// <param name="cancellationToken">The CancellationToken to monitor.</param>
        /// <returns>A new Task that respects the provided CancellationToken.</returns>
        public static async Task WithCancellationAsync(this Task task, CancellationToken cancellationToken)
        {
            var cancellationTask = CancellationTaskAsync(cancellationToken);

            var completedTask = await Task.WhenAny(task, cancellationTask);

            if (completedTask == cancellationTask)
            {
                await cancellationTask;
            }

            await task;
        }

        public static async Task<T> CancellationTaskAsync<T>(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<T>();
            await using var _ = cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken));
            return await tcs.Task;
        }

        public static async Task CancellationTaskAsync(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            await using var _ = cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken));
            await tcs.Task;
        }
    }
}
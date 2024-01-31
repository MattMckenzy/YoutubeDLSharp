using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YoutubeDLSharp.Options;

namespace YoutubeDLSharp.Helpers
{
    /// <summary>
    /// Provides methods for throttled execution of processes.
    /// </summary>
    public class ProcessRunner(byte initialCount)
    {
        private const int MAX_COUNT = 100;
        private readonly SemaphoreSlim semaphore = new(initialCount, MAX_COUNT);

        public byte TotalCount { get; private set; } = initialCount;

        public int RemainingCount { get { return semaphore.CurrentCount; } }

        public async Task<(int, string[])> RunThrottled(YoutubeDLProcess process, string[] urls, OptionSet options,
                                       CancellationToken ct, IProgress<DownloadProgress> progress = null)
        {
            var errors = new List<string>();
            process.ErrorReceived += (o, e) => errors.Add(e.Data);
            await semaphore.WaitAsync(ct);
            try
            {
                var exitCode = await process.RunAsync(urls, options, ct, progress);
                return (exitCode, errors.ToArray());
            }
            finally
            {
                semaphore.Release();
            }
        }

        private void IncrementCount(byte incr)
        {
            semaphore.Release(incr);
            TotalCount += incr;
        }

        private async Task DecrementCount(byte decr)
        {
            Task[] decrs = new Task[decr];
            for (int i = 0; i < decr; i++)
                decrs[i] = semaphore.WaitAsync();
            TotalCount -= decr;
            await Task.WhenAll(decrs);
        }

        public async Task SetTotalCount(byte count)
        {
            if (count < 1 || count > MAX_COUNT)
                throw new ArgumentException($"Number of threads must be between 1 and {MAX_COUNT}.");
            if (count > TotalCount)
                IncrementCount((byte)(count - TotalCount));
            else if (count < TotalCount)
                await DecrementCount((byte)(TotalCount - count));
        }
    }
}

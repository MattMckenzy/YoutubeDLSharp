﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using YoutubeDLSharp.Helpers;
using YoutubeDLSharp.Options;

namespace YoutubeDLSharp
{
    /// <summary>
    /// A low-level wrapper for the yt-dlp executable.
    /// </summary>
    /// <remarks>
    /// Creates a new instance of the YoutubeDLProcess class.
    /// </remarks>
    /// <param name="executablePath">The path to the yt-dlp executable.</param>
    public partial class YoutubeDLProcess(string executablePath = "yt-dlp.exe") : IDisposable
    {

        [GeneratedRegex("Downloading video (\\d+) of (\\d+)", RegexOptions.Compiled)]
        private static partial Regex PlaylistRegex();
        // the regex used to match the currently downloaded video of a playlist.
        private static readonly Regex rgxPlaylist = PlaylistRegex();

        [GeneratedRegex("\\[download\\]\\s+(?:(?<percent>[\\d\\.]+)%(?:\\s+of\\s+\\~?\\s*(?<total>[\\d\\.\\w]+))?\\s+at\\s+(?:(?<speed>[\\d\\.\\w]+\\/s)|[\\w\\s]+)\\s+ETA\\s(?<eta>[\\d\\:]+))?", RegexOptions.Compiled)]
        private static partial Regex ProgressRegex();
        // the regex used for matching download progress information.
        private static readonly Regex rgxProgress = ProgressRegex();

        [GeneratedRegex("\\[(\\w+)\\]\\s+", RegexOptions.Compiled)]
        private static partial Regex PostProcessingRegex();
        // the regex used to match the beginning of post-processing.
        private static readonly Regex rgxPost = PostProcessingRegex();
        private bool disposedValue;

        /// <summary>
        /// The path to the Python interpreter.
        /// If this property is non-empty, yt-dlp will be run using the Python interpreter.
        /// In this case, ExecutablePath should point to a non-binary, Python version of yt-dlp.
        /// </summary>
        public string PythonPath { get; set; }

        /// <summary>
        /// The path to the yt-dlp executable.
        /// </summary>
        public string ExecutablePath { get; set; } = executablePath;

        /// <summary>
        /// Windows only. If set to true, start process via cmd.exe to support Unicode chars.
        /// </summary>
        public bool UseWindowsEncodingWorkaround { get; set; } = true;

        /// <summary>
        /// Occurs each time yt-dlp writes to the standard output.
        /// </summary>
        public event EventHandler<DataReceivedEventArgs> OutputReceived;
        /// <summary>
        /// Occurs each time yt-dlp writes to the error output.
        /// </summary>
        public event EventHandler<DataReceivedEventArgs> ErrorReceived;

        internal static string ConvertToArgs(string[] urls, OptionSet options)
            => options.ToString() + " -- " + (urls != null ? string.Join(" ", urls.Select(s => $"\"{s}\"")) : string.Empty);

        internal void RedirectToError(DataReceivedEventArgs e)
            => ErrorReceived?.Invoke(this, e);

        /// <summary>
        /// Invokes yt-dlp with the specified parameters and options.
        /// </summary>
        /// <param name="urls">The video URLs to be passed to yt-dlp.</param>
        /// <param name="options">An OptionSet specifying the options to be passed to yt-dlp.</param>
        /// <returns>The exit code of the yt-dlp process.</returns>
        public async Task<int> RunAsync(string[] urls, OptionSet options)
            => await RunAsync(urls, options, CancellationToken.None);

        /// <summary>
        /// Invokes yt-dlp with the specified parameters and options.
        /// </summary>
        /// <param name="urls">The video URLs to be passed to yt-dlp.</param>
        /// <param name="options">An OptionSet specifying the options to be passed to yt-dlp.</param>
        /// <param name="ct">A CancellationToken used to cancel the download.</param>
        /// <param name="progress">A progress provider used to get download progress information.</param>
        /// <returns>The exit code of the yt-dlp process.</returns>
        public async Task<int> RunAsync(string[] urls, OptionSet options,
            CancellationToken ct, IProgress<DownloadProgress> progress = null)
        {
            TaskCompletionSource<int> tcs = new ();
            using Process process = new ();
            ProcessStartInfo startInfo = new ()
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            };

            if (OSHelper.IsWindows && UseWindowsEncodingWorkaround)
            {
                startInfo.FileName = "cmd.exe";
                string runCommand;
                if (!string.IsNullOrEmpty(PythonPath))
                    runCommand = $"\"{PythonPath}\" \"{ExecutablePath}\" {ConvertToArgs(urls, options)}";
                else
                    runCommand = $"\"{ExecutablePath}\" {ConvertToArgs(urls, options)}";
                startInfo.Arguments = $"/C chcp 65001 >nul 2>&1 && {runCommand}";
            }
            else if (!string.IsNullOrEmpty(PythonPath))
            {
                startInfo.FileName = PythonPath;
                startInfo.Arguments = $"\"{ExecutablePath}\" {ConvertToArgs(urls, options)}";
            }
            else
            {
                startInfo.FileName = ExecutablePath;
                startInfo.Arguments = ConvertToArgs(urls, options);
            }

            process.EnableRaisingEvents = true;
            process.StartInfo = startInfo;
            TaskCompletionSource<bool> tcsOut = new();
            // this variable is used for tracking download states
            bool isDownloading = false;
            process.OutputDataReceived += (o, e) =>
            {
                if (e.Data == null)
                {
                    tcsOut.SetResult(true);
                    return;
                }

                if (progress != null)
                {
                    Match match;
                    if ((match = rgxProgress.Match(e.Data)).Success)
                    {
                        if (match.Groups.Count > 1 && match.Groups[1].Length > 0)
                        {
                            float progValue = float.Parse(match.Groups[1].ToString(), CultureInfo.InvariantCulture) / 100.0f;
                            Group totalGroup = match.Groups["total"];
                            string total = totalGroup.Success ? totalGroup.Value : null;
                            Group speedGroup = match.Groups["speed"];
                            string speed = speedGroup.Success ? speedGroup.Value : null;
                            Group etaGroup = match.Groups["eta"];
                            string eta = etaGroup.Success ? etaGroup.Value : null;
                            progress?.Report(
                                new DownloadProgress(
                                    DownloadState.Downloading, progress: progValue, totalDownloadSize: total, downloadSpeed: speed, eta: eta
                                )
                            );
                        }
                        else
                        {
                            progress?.Report(new DownloadProgress(DownloadState.Downloading));
                        }
                        isDownloading = true;
                    }
                    else if ((match = rgxPlaylist.Match(e.Data)).Success)
                    {
                        var index = int.Parse(match.Groups[1].Value);
                        progress?.Report(new DownloadProgress(DownloadState.PreProcessing, index: index));
                        isDownloading = false;
                    }
                    else if (isDownloading && (match = rgxPost.Match(e.Data)).Success)
                    {
                        progress?.Report(new DownloadProgress(DownloadState.PostProcessing, 1));
                        isDownloading = false;
                    }
                }
               
                Debug.WriteLine("[yt-dlp] " + e.Data);
                OutputReceived?.Invoke(this, e);
            };
            TaskCompletionSource<bool> tcsError = new();
            process.ErrorDataReceived += (o, e) =>
            {
                if (e.Data == null)
                {
                    tcsError.SetResult(true);
                    return;
                }
                Debug.WriteLine("[yt-dlp ERROR] " + e.Data);
                progress?.Report(new DownloadProgress(DownloadState.Error, data: e.Data));
                ErrorReceived?.Invoke(this, e);
            };
            process.Exited += async (sender, args) =>
            {
                // Wait for output and error streams to finish
                await tcsOut.Task;
                await tcsError.Task;
                tcs.TrySetResult(process.ExitCode);
                process.Dispose();
            };
            ct.Register(() =>
            {
                if (!tcs.Task.IsCompleted)
                    tcs.TrySetCanceled();
                try
                {
                    if (!process.HasExited) process.Kill(true);
                }
                catch { }
            });
            Debug.WriteLine("[yt-dlp] Arguments: " + process.StartInfo.Arguments);
            if (!await Task.Run(() => process.Start()))
                tcs.TrySetException(new InvalidOperationException("Failed to start yt-dlp process."));
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            progress?.Report(new DownloadProgress(DownloadState.PreProcessing));

            return await tcs.Task;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (OutputReceived != null)
                        foreach(Delegate delegateEvent in OutputReceived.GetInvocationList())
                            OutputReceived -= (EventHandler<DataReceivedEventArgs>)delegateEvent;
                    
                    if (ErrorReceived != null)
                        foreach(Delegate delegateEvent in ErrorReceived.GetInvocationList())
                            ErrorReceived -= (EventHandler<DataReceivedEventArgs>)delegateEvent;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

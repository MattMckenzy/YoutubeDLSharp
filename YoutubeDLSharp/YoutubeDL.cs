using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YoutubeDLSharp.Helpers;
using YoutubeDLSharp.Metadata;
using YoutubeDLSharp.Options;

namespace YoutubeDLSharp
{
    /// <summary>
    /// A class providing methods for downloading videos using yt-dlp.
    /// </summary>
    /// <remarks>
    /// Creates a new instance of the YoutubeDL class.
    /// </remarks>
    /// <param name="maxNumberOfProcesses">The maximum number of concurrent yt-dlp processes.</param>
    public partial class YoutubeDL(byte maxNumberOfProcesses = 4)
    {
        [GeneratedRegex("^outfile:\\s\\\"?(.*)\\\"?", RegexOptions.Compiled)]
        private static partial Regex OutfileRegex();
        private static readonly Regex rgxFile = OutfileRegex();

        [GeneratedRegex("\\[download\\] Destination: [a-zA-Z]:\\\\\\S+\\.\\S{3,}", RegexOptions.Compiled)]
        private static partial Regex DestinationRegex();
        private static readonly Regex rgxFilePostProc = DestinationRegex();

        protected ProcessRunner runner = new(maxNumberOfProcesses);

        /// <summary>
        /// Path to the yt-dlp executable.
        /// </summary>
        public string YoutubeDLPath { get; set; } = Utils.YtDlpBinaryName;
        /// <summary>
        /// Path to the FFmpeg executable.
        /// </summary>
        public string FFmpegPath { get; set; } = Utils.FfmpegBinaryName;
        /// <summary>
        /// Path of the folder where items will be downloaded to.
        /// </summary>
        public string OutputFolder { get; set; } = Environment.CurrentDirectory;
        /// <summary>
        /// Template of the name of the downloaded file on yt-dlp style.
        /// See https://github.com/yt-dlp/yt-dlp#output-template.
        /// </summary>
        public string OutputFileTemplate { get; set; } = "%(title)s [%(id)s].%(ext)s";
        /// <summary>
        /// If set to true, file names a re restricted to ASCII characters.
        /// </summary>
        public bool RestrictFilenames { get; set; } = false;
        /// <summary>
        /// If set to true, force overwriting of files.
        /// </summary>
        public bool OverwriteFiles { get; set; } = true;
        /// <summary>
        /// If set to true, download errors are ignored and downloading is continued.
        /// </summary>
        public bool IgnoreDownloadErrors { get; set; } = true;

        /// <summary>
        /// Optional. If set, will use python to run yt-dlp and <see cref="YoutubeDLPath"/> must be lead to the python script.
        /// </summary>
        public string PythonInterpreterPath { get; set; } = null;

        /// <summary>
        /// Gets the product version of the yt-dlp executable file.
        /// </summary>
        public string Version
            => FileVersionInfo.GetVersionInfo(Utils.GetFullPath(YoutubeDLPath)).FileVersion;

        /// <summary>
        /// Sets the maximal number of parallel download processes.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task SetMaxNumberOfProcesses(byte count) => await runner.SetTotalCount(count);

        public int RemainingProcessesCount { get { return runner.RemainingCount; } }

        #region Process methods

        /// <summary>
        /// Runs yt-dlp with the given option set.
        /// </summary>
        /// <param name="urls">The video URLs passed to yt-dlp.</param>
        /// <param name="options">The OptionSet of yt-dlp options.</param>
        /// <param name="ct">A CancellationToken used to cancel the process.</param>
        /// <returns>A RunResult object containing the output of yt-dlp as an array of string.</returns>
        public async Task<RunResult<string[]>> RunWithOptions(string[] urls, OptionSet options, CancellationToken ct)
        {
            List<string> output = [];

            using YoutubeDLProcess process = new (YoutubeDLPath)
            {
                PythonPath = PythonInterpreterPath,
            };

            process.OutputReceived += (o, e) => output.Add(e.Data);
            (int code, string[] errors) = await runner.RunThrottled(process, urls, options, ct);
            return new RunResult<string[]>(code == 0, errors, [.. output]);
        }

        /// <summary>
        /// Runs yt-dlp with the given option set and additional parameters.
        /// </summary>
        /// <param name="url">The video URL passed to yt-dlp.</param>
        /// <param name="options">The OptionSet of yt-dlp options.</param>
        /// <param name="ct">A CancellationToken used to cancel the process.</param>
        /// <param name="progress">A progress provider used to get download progress information.</param>
        /// <param name="output">A progress provider used to capture the standard output.</param>
        /// <param name="showArgs">When true, outputs full download arguments</param>
        /// <returns>A RunResult object containing the path to the downloaded and converted video.</returns>
        public async Task<RunResult<string>> RunWithOptions(string url, OptionSet options, IProgress<DownloadProgress> progress = null,
             IProgress<string> output = null, bool showArgs = true, CancellationToken ct = default)
        {
            string outFile = string.Empty;           
            
            using YoutubeDLProcess process = new (YoutubeDLPath)
            {
                PythonPath = PythonInterpreterPath,
            };

            if (showArgs)
                output?.Report($"Arguments: {YoutubeDLProcess.ConvertToArgs([url], options)}\n");
            else
                output?.Report($"Starting Download: {url}");

            process.OutputReceived += (o, e) =>
            {
                var match = rgxFilePostProc.Match(e.Data);
                if (match.Success)
                {
                    outFile = match.Groups[0].ToString().Replace("[download] Destination:", "").Replace(" ", "");
                    progress?.Report(new DownloadProgress(DownloadState.Success, data: outFile));
                }
                output?.Report(e.Data);
            };

            (int code, string[] errors) = await runner.RunThrottled(process, [url], options, ct, progress);
            return new RunResult<string>(code == 0, errors, outFile);
        }

        /// <summary>
        /// Runs an update of yt-dlp.
        /// </summary>
        /// <returns>The output of yt-dlp as string.</returns>
        public async Task<string> RunUpdate(string updateTo = null)
        {
            string output = string.Empty;           
            
            using YoutubeDLProcess process = new (YoutubeDLPath)
            {
                PythonPath = PythonInterpreterPath,
            };

            process.OutputReceived += (o, e) => output = e.Data;
            await process.RunAsync(null, string.IsNullOrWhiteSpace(updateTo) ? new OptionSet() { Update = true } : new OptionSet() { UpdateTo = updateTo });
            return output;
        }

        /// <summary>
        /// Runs a fetch of information for the given video without downloading the video.
        /// </summary>
        /// <param name="url">The URL of the video to fetch information for.</param>
        /// <param name="flatPlaylist">True to only retrieve metadata found on the playlist page, false to load the video page and extract more metadata.</param>        
        /// <param name="videoDataCallback">The function to callback when video data has been fetched.</param>
        /// <param name="errorDataCallback">The function to callback when error data was received.</param>
        /// <param name="overrideOptions">Override options of the default option set for this run.</param>
        /// <param name="ct">A CancellationToken used to cancel the process.</param>
        /// <returns>A RunResult object containing a VideoData object with the requested video information.</returns>
        public async Task<RunResult<IEnumerable<VideoData>>> RunPlaylistDataFetch(string url,
            bool flatPlaylist = false,
            Func<VideoData, Task> videoDataCallback = null,
            Func<string, Task> errorDataCallback = null,
            OptionSet overrideOptions = null,
            CancellationToken ct = default)
        {
            OptionSet opts = GetSearchOptions();
            opts.FlatPlaylist = flatPlaylist;
            opts.DumpJson = true;
            opts.Quiet = true;
            if (overrideOptions != null)
            {
                opts = opts.OverrideOptions(overrideOptions);
            }

            using YoutubeDLProcess process = new (YoutubeDLPath)
            {
                PythonPath = PythonInterpreterPath,
            };

            List<VideoData> videoDatas = [];

            int callbackCount = 0;
            if (videoDataCallback != null)
            {
                process.OutputReceived += async (o, e) =>
                {
                    Interlocked.Increment(ref callbackCount);
                    VideoData videoData = JsonConvert.DeserializeObject<VideoData>(e.Data);
                    videoDatas.Add(videoData);
                    await videoDataCallback(videoData);
                    Interlocked.Decrement(ref callbackCount);
                };
            }

            if (errorDataCallback != null)
            {
                process.ErrorReceived += async (o, e) =>
                {
                    Interlocked.Increment(ref callbackCount);
                    if (!string.IsNullOrWhiteSpace(e.Data))
                        await errorDataCallback(e.Data);
                    Interlocked.Decrement(ref callbackCount);
                };
            }

            (int code, string[] errors) = await runner.RunThrottled(process, [url], options: opts, ct);

            while (callbackCount > 0 && !ct.IsCancellationRequested)
                await Task.Delay(50, ct);

            return new RunResult<IEnumerable<VideoData>>(code == 0, errors, videoDatas);
        }

        /// <summary>
        /// Runs a fetch of information for the given video without downloading the video.
        /// </summary>
        /// <param name="url">The URL of the video to fetch information for.</param>       
        /// <param name="flatPlaylist">True to only retrieve metadata found on the playlist page, false to load the video page and extract more metadata.</param>        
        /// <param name="overrideOptions">Override options of the default option set for this run.</param>
        /// <param name="ct">A CancellationToken used to cancel the process.</param>
        /// <returns>A RunResult object containing a VideoData object with the requested video information.</returns>
        public async Task<RunResult<VideoData>> RunVideoDataFetch(string url,
            bool flatPlaylist = false,
            OptionSet overrideOptions = null,
            CancellationToken ct = default)
        {
            OptionSet opts = GetSearchOptions();
            opts.FlatPlaylist = flatPlaylist;
            if (overrideOptions != null)
            {
                opts = opts.OverrideOptions(overrideOptions);
            }

            VideoData videoData = null;

            using YoutubeDLProcess process = new (YoutubeDLPath)
            {
                PythonPath = PythonInterpreterPath,
            };

            process.OutputReceived += (o, e) =>
            {
                try
                {
                    videoData = JsonConvert.DeserializeObject<VideoData>(e.Data);
                }
                catch (JsonSerializationException)
                {
                    process.RedirectToError(e);
                }
            };
            (int code, string[] errors) = await runner.RunThrottled(process, new[] { url }, opts, ct);
            return new RunResult<VideoData>(code == 0, errors, videoData);
        }

        /// <summary>
        /// Runs a download of the specified video with an optional conversion afterwards.
        /// </summary>
        /// <param name="url">The URL of the video to be downloaded.</param>
        /// <param name="format">A format selection string in yt-dlp style.</param>
        /// <param name="mergeFormat">If a merge is required, the container format of the merged downloads.</param>
        /// <param name="recodeFormat">The video format the output will be recoded to after download.</param>
        /// <param name="ct">A CancellationToken used to cancel the download.</param>
        /// <param name="progress">A progress provider used to get download progress information.</param>
        /// <param name="output">A progress provider used to capture the standard output.</param>
        /// <param name="overrideOptions">Override options of the default option set for this run.</param>
        /// <returns>A RunResult object containing the path to the downloaded and converted video.</returns>
        public async Task<RunResult<string>> RunVideoDownload(string url,
            string format = "bestvideo+bestaudio/best",
            DownloadMergeFormat mergeFormat = DownloadMergeFormat.Unspecified,
            VideoRecodeFormat recodeFormat = VideoRecodeFormat.None,
            IProgress<string> output = null,
            OptionSet overrideOptions = null,
            IProgress<DownloadProgress> progress = null,
            CancellationToken ct = default)
        {
            OptionSet opts = GetDownloadOptions();
            opts.Format = format;
            opts.MergeOutputFormat = mergeFormat;
            opts.RecodeVideo = recodeFormat;
            if (overrideOptions != null)
            {
                opts = opts.OverrideOptions(overrideOptions);
            }

            string outputFile = string.Empty;

            using YoutubeDLProcess process = new (YoutubeDLPath)
            {
                PythonPath = PythonInterpreterPath,
            };

            // Report the used ytdl args
            output?.Report($"Arguments: {YoutubeDLProcess.ConvertToArgs([url], opts)}\n");
            process.OutputReceived += (o, e) =>
            {
                var match = rgxFile.Match(e.Data);
                if (match.Success)
                {
                    outputFile = match.Groups[1].ToString().Trim('"');
                    progress?.Report(new DownloadProgress(DownloadState.Success, data: outputFile));
                }
                output?.Report(e.Data);
            };

            (int code, string[] errors) = await runner.RunThrottled(process, [url], opts, ct, progress);
            return new RunResult<string>(code == 0, errors, outputFile);
        }

        /// <summary>
        /// Runs a download of the specified video playlist with an optional conversion afterwards.
        /// </summary>
        /// <param name="url">The URL of the playlist to be downloaded.</param>
        /// <param name="start">The index of the first playlist video to download (starting at 1).</param>
        /// <param name="end">The index of the last playlist video to dowload (if null, download to end).</param>
        /// <param name="items">An array of indices of playlist video to download.</param>
        /// <param name="format">A format selection string in yt-dlp style.</param>
        /// <param name="recodeFormat">The video format the output will be recoded to after download.</param>
        /// <param name="ct">A CancellationToken used to cancel the download.</param>
        /// <param name="progress">A progress provider used to get download progress information.</param>
        /// <param name="output">A progress provider used to capture the standard output.</param>
        /// <param name="overrideOptions">Override options of the default option set for this run.</param>
        /// <returns>A RunResult object containing the paths to the downloaded and converted videos.</returns>
        public async Task<RunResult<string[]>> RunVideoPlaylistDownload(string url,
            int? start = 1, int? end = null,
            int[] items = null,
            string format = "bestvideo+bestaudio/best",
            VideoRecodeFormat recodeFormat = VideoRecodeFormat.None,
            IProgress<string> output = null,
            OptionSet overrideOptions = null,
            IProgress<DownloadProgress> progress = null,
            CancellationToken ct = default)
        {
            OptionSet opts = GetDownloadOptions();
            opts.NoPlaylist = false;
            if (items != null)
                opts.PlaylistItems = string.Join(",", items);
            else
                opts.PlaylistItems = $"{start}:{end}";
            opts.Format = format;
            opts.RecodeVideo = recodeFormat;
            if (overrideOptions != null)
            {
                opts = opts.OverrideOptions(overrideOptions);
            }

            List<string> outputFiles = [];

            using YoutubeDLProcess process = new (YoutubeDLPath)
            {
                PythonPath = PythonInterpreterPath,
            };

            // Report the used ytdl args
            output?.Report($"Arguments: {YoutubeDLProcess.ConvertToArgs([url], opts)}\n");
            process.OutputReceived += (o, e) =>
            {
                var match = rgxFile.Match(e.Data);
                if (match.Success)
                {
                    var file = match.Groups[1].ToString().Trim('"');
                    outputFiles.Add(file);
                    progress?.Report(new DownloadProgress(DownloadState.Success, data: file));
                }
                output?.Report(e.Data);
            };

            (int code, string[] errors) = await runner.RunThrottled(process, [url], opts, ct, progress);
            return new RunResult<string[]>(code == 0, errors, [.. outputFiles]);
        }

        /// <summary>
        /// Runs a download of the specified video with and converts it to an audio format afterwards.
        /// </summary>
        /// <param name="url">The URL of the video to be downloaded.</param>
        /// <param name="format">The audio format the video will be converted to after downloaded.</param>
        /// <param name="ct">A CancellationToken used to cancel the download.</param>
        /// <param name="progress">A progress provider used to get download progress information.</param>
        /// <param name="output">A progress provider used to capture the standard output.</param>
        /// <param name="overrideOptions">Override options of the default option set for this run.</param>
        /// <returns>A RunResult object containing the path to the downloaded and converted video.</returns>
        public async Task<RunResult<string>> RunAudioDownload(string url, AudioConversionFormat format = AudioConversionFormat.Best,
            IProgress<string> output = null,
            OptionSet overrideOptions = null,
            IProgress<DownloadProgress> progress = null,
            CancellationToken ct = default)
        {
            OptionSet opts = GetDownloadOptions();
            opts.Format = "bestaudio/best";
            opts.ExtractAudio = true;
            opts.AudioFormat = format;
            if (overrideOptions != null)
            {
                opts = opts.OverrideOptions(overrideOptions);
            }

            string outputFile = string.Empty;
            List<string> error = [];

            using YoutubeDLProcess process = new (YoutubeDLPath)
            {
                PythonPath = PythonInterpreterPath,
            };

            // Report the used ytdl args
            output?.Report($"Arguments: {YoutubeDLProcess.ConvertToArgs([url], opts)}\n");
            process.OutputReceived += (o, e) =>
            {
                var match = rgxFile.Match(e.Data);
                if (match.Success)
                {
                    outputFile = match.Groups[1].ToString().Trim('"');
                    progress?.Report(new DownloadProgress(DownloadState.Success, data: outputFile));
                }
                output?.Report(e.Data);
            };

            (int code, string[] errors) = await runner.RunThrottled(process, [url], opts, ct, progress);
            return new RunResult<string>(code == 0, errors, outputFile);
        }

        /// <summary>
        /// Runs a download of the specified video playlist and converts all videos to an audio format afterwards.
        /// </summary>
        /// <param name="url">The URL of the playlist to be downloaded.</param>
        /// <param name="start">The index of the first playlist video to download (starting at 1).</param>
        /// <param name="end">The index of the last playlist video to dowload (if null, download to end).</param>
        /// <param name="items">An array of indices of playlist video to download.</param>
        /// <param name="format">The audio format the videos will be converted to after downloaded.</param>
        /// <param name="ct">A CancellationToken used to cancel the download.</param>
        /// <param name="progress">A progress provider used to get download progress information.</param>
        /// <param name="output">A progress provider used to capture the standard output.</param>
        /// <param name="overrideOptions">Override options of the default option set for this run.</param>
        /// <returns>A RunResult object containing the paths to the downloaded and converted videos.</returns>
        public async Task<RunResult<string[]>> RunAudioPlaylistDownload(string url,
            int? start = 1, int? end = null,
            int[] items = null, AudioConversionFormat format = AudioConversionFormat.Best,
            IProgress<string> output = null,
            OptionSet overrideOptions = null,
            IProgress<DownloadProgress> progress = null,
            CancellationToken ct = default)
        {
            List<string> outputFiles = [];
            OptionSet opts = GetDownloadOptions();

            opts.NoPlaylist = false;
            if (items != null)
                opts.PlaylistItems = string.Join(",", items);
            else
                opts.PlaylistItems = $"{start}:{end}";
            opts.Format = "bestaudio/best";
            opts.ExtractAudio = true;
            opts.AudioFormat = format;
            if (overrideOptions != null)
            {
                opts = opts.OverrideOptions(overrideOptions);
            }

            using YoutubeDLProcess process = new (YoutubeDLPath)
            {
                PythonPath = PythonInterpreterPath,
            };

            // Report the used ytdl args
            output?.Report($"Arguments: {YoutubeDLProcess.ConvertToArgs([url], opts)}\n");
            process.OutputReceived += (o, e) =>
            {
                Match match = rgxFile.Match(e.Data);
                if (match.Success)
                {
                    var file = match.Groups[1].ToString().Trim('"');
                    outputFiles.Add(file);
                    progress?.Report(new DownloadProgress(DownloadState.Success, data: file));
                }
                output?.Report(e.Data);
            };

            (int code, string[] errors) = await runner.RunThrottled(process, [url], opts, ct, progress);
            return new RunResult<string[]>(code == 0, errors, [.. outputFiles]);
        }

        /// <summary>
        /// Runs a YouToube search and slowly returns results as they come in.
        /// </summary>
        /// <param name="searchTerm">The URL of the playlist to be downloaded.</param>
        /// <param name="flatPlaylist">True to only retrieve metadata found on the search page, false to load the video page and extract more metadata.</param>     
        /// <param name="maxResultCount">The maximum amount of search results to return.</param>
        /// <param name="searchNewest">Whether or not to search and return the laatest videos matching the search term. True will return latest videos regardless of relevancy.</param>
        /// <param name="overrideOptions">An OptionsSet That can override the default options.</param>
        /// <param name="millisecondsResultWait">The amount of time to wait for any more incoming results before stopping.</param>
        /// <param name="cancellationToken">A CancellationToken used to cancel the download.</param>
        /// <returns>Yields returns search results.</returns>
        public async IAsyncEnumerable<VideoData> Search(string searchTerm,
            bool flatPlaylist = false,
            int maxResultCount = 10,
            bool searchNewest = false,
            OptionSet overrideOptions = null,
            int millisecondsResultWait = 5000,
            [EnumeratorCancellation]
            CancellationToken cancellationToken = default)
        {
            OptionSet optionSet = GetSearchOptions();
            optionSet.FlatPlaylist = flatPlaylist;
            if (overrideOptions != null)
                optionSet = optionSet.OverrideOptions(overrideOptions);

            CancellationTokenSource searchCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            using YoutubeDLProcess process = new (YoutubeDLPath)
            {
                PythonPath = PythonInterpreterPath,
            };

            BlockingCollection<VideoData> searchResults = [];
            process.OutputReceived += (_, dataReceivedEventArgs) =>
            {
                Debug.WriteLine($"Received data: {DateTime.Now}");
                VideoData videoData = JsonConvert.DeserializeObject<VideoData>(dataReceivedEventArgs.Data);

                searchCancellationTokenSource.CancelAfter(millisecondsResultWait);
                if (searchCancellationTokenSource.IsCancellationRequested)
                    return;

                searchResults.Add(videoData);
            };
            process.ErrorReceived += (_, _) => { searchCancellationTokenSource.CancelAfter(millisecondsResultWait); };

            string search = $"ytsearch{(searchNewest ? "date" : string.Empty)}{maxResultCount}:{searchTerm}";

            _ = process.RunAsync([search], optionSet, cancellationToken)
                .ContinueWith((_) => searchCancellationTokenSource.CancelAfter(millisecondsResultWait), cancellationToken);

            do
            {
                yield return await Task.Run(() => searchResults.Take(cancellationToken));
            }
            while (!searchCancellationTokenSource.IsCancellationRequested);
        }

        /// <summary>
        /// Returns an option set with default options used for most downloading operations.
        /// </summary>
        protected virtual OptionSet GetDownloadOptions()
        {
            return new OptionSet()
            {
                IgnoreErrors = IgnoreDownloadErrors,
                IgnoreConfig = true,
                NoPlaylist = true,
                Downloader = "m3u8:native",
                DownloaderArgs = "ffmpeg:-nostats -loglevel 0",
                Output = Path.Combine(OutputFolder, OutputFileTemplate),
                RestrictFilenames = RestrictFilenames,
                ForceOverwrites = OverwriteFiles,
                NoOverwrites = !OverwriteFiles,
                NoPart = true,
                FfmpegLocation = Utils.GetFullPath(FFmpegPath),
                Exec = "echo outfile: {}"
            };
        }

        /// <summary>
        /// Returns an option set with default options used for most downloading operations.
        /// </summary>
        protected virtual OptionSet GetSearchOptions()
        {
            return new OptionSet()
            {
                IgnoreErrors = IgnoreDownloadErrors,
                IgnoreConfig = true,
                DumpJson = true,
                Quiet = true
            };
        }

        #endregion
    }
}

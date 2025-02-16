﻿
namespace YoutubeDLSharp
{
    /// <summary>
    /// Specifies possible download states of a video.
    /// (None and Success are not used by YoutubeDLProcess.)
    /// </summary>
    public enum DownloadState
    {
        None = 0,
        PreProcessing = 1,
        Downloading = 2,
        PostProcessing = 3,
        Error = 4,
        Success = 5
    }

    /// <summary>
    /// Provides status information for a video download.
    /// </summary>
    /// <remarks>
    /// Creates a new instance of class DownloadProgress.
    /// </remarks>
    public class DownloadProgress(
        DownloadState status, float progress = 0,
        string totalDownloadSize = null, string downloadSpeed = null, string eta = null,
        int index = 1, string data = null
        )
    {
        /// <summary>
        /// Video download status.
        /// </summary>
        public DownloadState State { get; } = status;
        /// <summary>
        /// Download progress value between 0 and 1.
        /// </summary>
        public float Progress { get; } = progress;
        /// <summary>
        /// The total download size string as outputted by yt-dlp.
        /// </summary>
        public string TotalDownloadSize { get; } = totalDownloadSize;
        /// <summary>
        /// The download speed string as outputted by yt-dlp.
        /// </summary>
        public string DownloadSpeed { get; } = downloadSpeed;
        /// <summary>
        /// The estimated remaining time of the download as outputted by yt-dlp.
        /// </summary>
        public string ETA { get; } = eta;
        /// <summary>
        /// The current video index (starting at 1) if mutliple items are downloaded at once.
        /// </summary>
        public int VideoIndex { get; } = index;
        /// <summary>
        /// Additional optional progress information.
        /// </summary>
        public string Data { get; } = data;
    }
}

using System;

namespace YoutubeDLSharp
{
    /// <summary>
    /// Represents an individual result of a yt-dlp search operation.
    /// </summary>
    public class SearchResult
    {
        public string Id { get; set; }

        public string LocationUrl { get; set; }
        public string Container { get; set; }
        public string Title { get; set; }
        public double? Runtime { get; set; }

        public long? FileSize { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Channel { get; set; }
        public long? LikeCount { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}

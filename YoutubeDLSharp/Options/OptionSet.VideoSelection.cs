﻿// <auto-generated>
// This code was partially generated by a tool.
// </auto-generated>

using System;

namespace YoutubeDLSharp.Options
{
    public partial class OptionSet
    {
        private Option<string> playlistItems = new Option<string>("-I", "--playlist-items");
        private Option<string> minFilesize = new Option<string>("--min-filesize");
        private Option<string> maxFilesize = new Option<string>("--max-filesize");
        private Option<DateTime> date = new Option<DateTime>("--date");
        private Option<DateTime> dateBefore = new Option<DateTime>("--datebefore");
        private Option<DateTime> dateAfter = new Option<DateTime>("--dateafter");
        private MultiOption<string> matchFilters = new MultiOption<string>("--match-filters");
        private Option<bool> noMatchFilters = new Option<bool>("--no-match-filters");
        private Option<string> breakMatchFilters = new Option<string>("--break-match-filters");
        private Option<bool> noBreakMatchFilters = new Option<bool>("--no-break-match-filters");
        private Option<bool> noPlaylist = new Option<bool>("--no-playlist");
        private Option<bool> yesPlaylist = new Option<bool>("--yes-playlist");
        private Option<byte?> ageLimit = new Option<byte?>("--age-limit");
        private Option<string> downloadArchive = new Option<string>("--download-archive");
        private Option<bool> noDownloadArchive = new Option<bool>("--no-download-archive");
        private Option<int?> maxDownloads = new Option<int?>("--max-downloads");
        private Option<bool> breakOnExisting = new Option<bool>("--break-on-existing");
        private Option<bool> breakPerInput = new Option<bool>("--break-per-input");
        private Option<bool> noBreakPerInput = new Option<bool>("--no-break-per-input");
        private Option<int?> skipPlaylistAfterErrors = new Option<int?>("--skip-playlist-after-errors");

        /// <summary>
        /// Comma separated playlist_index of the items
        /// to download. You can specify a range using
        /// &quot;[START]:[STOP][:STEP]&quot;. For backward
        /// compatibility, START-STOP is also supported.
        /// Use negative indices to count from the right
        /// and negative STEP to download in reverse
        /// order. E.g. &quot;-I 1:3,7,-5::2&quot; used on a
        /// playlist of size 15 will download the items
        /// at index 1,2,3,7,11,13,15
        /// </summary>
        public string PlaylistItems { get => playlistItems.Value; set => playlistItems.Value = value; }
        /// <summary>
        /// Abort download if filesize is smaller than
        /// SIZE, e.g. 50k or 44.6M
        /// </summary>
        public string MinFilesize { get => minFilesize.Value; set => minFilesize.Value = value; }
        /// <summary>
        /// Abort download if filesize is larger than
        /// SIZE, e.g. 50k or 44.6M
        /// </summary>
        public string MaxFilesize { get => maxFilesize.Value; set => maxFilesize.Value = value; }
        /// <summary>
        /// Download only videos uploaded on this date.
        /// The date can be &quot;YYYYMMDD&quot; or in the format
        /// [now|today|yesterday][-N[day|week|month|year
        /// ]]. E.g. &quot;--date today-2weeks&quot; downloads
        /// only videos uploaded on the same day two
        /// weeks ago
        /// </summary>
        public DateTime Date { get => date.Value; set => date.Value = value; }
        /// <summary>
        /// Download only videos uploaded on or before
        /// this date. The date formats accepted is the
        /// same as --date
        /// </summary>
        public DateTime DateBefore { get => dateBefore.Value; set => dateBefore.Value = value; }
        /// <summary>
        /// Download only videos uploaded on or after
        /// this date. The date formats accepted is the
        /// same as --date
        /// </summary>
        public DateTime DateAfter { get => dateAfter.Value; set => dateAfter.Value = value; }
        /// <summary>
        /// Generic video filter. Any &quot;OUTPUT TEMPLATE&quot;
        /// field can be compared with a number or a
        /// string using the operators defined in
        /// &quot;Filtering Formats&quot;. You can also simply
        /// specify a field to match if the field is
        /// present, use &quot;!field&quot; to check if the field
        /// is not present, and &quot;&amp;&quot; to check multiple
        /// conditions. Use a &quot;\&quot; to escape &quot;&amp;&quot; or
        /// quotes if needed. If used multiple times,
        /// the filter matches if atleast one of the
        /// conditions are met. E.g. --match-filter
        /// !is_live --match-filter &quot;like_count&gt;?100 &amp;
        /// description~=&#x27;(?i)\bcats \&amp; dogs\b&#x27;&quot; matches
        /// only videos that are not live OR those that
        /// have a like count more than 100 (or the like
        /// field is not available) and also has a
        /// description that contains the phrase &quot;cats &amp;
        /// dogs&quot; (caseless). Use &quot;--match-filter -&quot; to
        /// interactively ask whether to download each
        /// video
        /// </summary>
        public MultiValue<string> MatchFilters { get => matchFilters.Value; set => matchFilters.Value = value; }
        /// <summary>
        /// Do not use any --match-filter (default)
        /// </summary>
        public bool NoMatchFilters { get => noMatchFilters.Value; set => noMatchFilters.Value = value; }
        /// <summary>
        /// Same as &quot;--match-filters&quot; but stops the
        /// download process when a video is rejected
        /// </summary>
        public string BreakMatchFilters { get => breakMatchFilters.Value; set => breakMatchFilters.Value = value; }
        /// <summary>
        /// Do not use any --break-match-filters
        /// (default)
        /// </summary>
        public bool NoBreakMatchFilters { get => noBreakMatchFilters.Value; set => noBreakMatchFilters.Value = value; }
        /// <summary>
        /// Download only the video, if the URL refers
        /// to a video and a playlist
        /// </summary>
        public bool NoPlaylist { get => noPlaylist.Value; set => noPlaylist.Value = value; }
        /// <summary>
        /// Download the playlist, if the URL refers to
        /// a video and a playlist
        /// </summary>
        public bool YesPlaylist { get => yesPlaylist.Value; set => yesPlaylist.Value = value; }
        /// <summary>
        /// Download only videos suitable for the given
        /// age
        /// </summary>
        public byte? AgeLimit { get => ageLimit.Value; set => ageLimit.Value = value; }
        /// <summary>
        /// Download only videos not listed in the
        /// archive file. Record the IDs of all
        /// downloaded videos in it
        /// </summary>
        public string DownloadArchive { get => downloadArchive.Value; set => downloadArchive.Value = value; }
        /// <summary>
        /// Do not use archive file (default)
        /// </summary>
        public bool NoDownloadArchive { get => noDownloadArchive.Value; set => noDownloadArchive.Value = value; }
        /// <summary>
        /// Abort after downloading NUMBER files
        /// </summary>
        public int? MaxDownloads { get => maxDownloads.Value; set => maxDownloads.Value = value; }
        /// <summary>
        /// Stop the download process when encountering
        /// a file that is in the archive
        /// </summary>
        public bool BreakOnExisting { get => breakOnExisting.Value; set => breakOnExisting.Value = value; }
        /// <summary>
        /// Alters --max-downloads, --break-on-existing,
        /// --break-match-filter, and autonumber to
        /// reset per input URL
        /// </summary>
        public bool BreakPerInput { get => breakPerInput.Value; set => breakPerInput.Value = value; }
        /// <summary>
        /// --break-on-existing and similar options
        /// terminates the entire download queue
        /// </summary>
        public bool NoBreakPerInput { get => noBreakPerInput.Value; set => noBreakPerInput.Value = value; }
        /// <summary>
        /// Number of allowed failures until the rest of
        /// the playlist is skipped
        /// </summary>
        public int? SkipPlaylistAfterErrors { get => skipPlaylistAfterErrors.Value; set => skipPlaylistAfterErrors.Value = value; }
    }
}

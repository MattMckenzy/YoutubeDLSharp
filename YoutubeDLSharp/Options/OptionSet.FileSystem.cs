﻿// <auto-generated>
// This code was partially generated by a tool.
// </auto-generated>

using System;

namespace YoutubeDLSharp.Options
{
    public partial class OptionSet
    {
        private Option<string> batchFile = new Option<string>("-a", "--batch-file");
        private Option<bool> noBatchFile = new Option<bool>("--no-batch-file");
        private Option<string> paths = new Option<string>("-P", "--paths");
        private Option<string> output = new Option<string>("-o", "--output");
        private Option<string> outputNaPlaceholder = new Option<string>("--output-na-placeholder");
        private Option<bool> restrictFilenames = new Option<bool>("--restrict-filenames");
        private Option<bool> noRestrictFilenames = new Option<bool>("--no-restrict-filenames");
        private Option<bool> windowsFilenames = new Option<bool>("--windows-filenames");
        private Option<bool> noWindowsFilenames = new Option<bool>("--no-windows-filenames");
        private Option<int?> trimFilenames = new Option<int?>("--trim-filenames");
        private Option<bool> noOverwrites = new Option<bool>("-w", "--no-overwrites");
        private Option<bool> forceOverwrites = new Option<bool>("--force-overwrites");
        private Option<bool> noForceOverwrites = new Option<bool>("--no-force-overwrites");
        private Option<bool> doContinue = new Option<bool>("-c", "--continue");
        private Option<bool> noContinue = new Option<bool>("--no-continue");
        private Option<bool> part = new Option<bool>("--part");
        private Option<bool> noPart = new Option<bool>("--no-part");
        private Option<bool> mtime = new Option<bool>("--mtime");
        private Option<bool> noMtime = new Option<bool>("--no-mtime");
        private Option<bool> writeDescription = new Option<bool>("--write-description");
        private Option<bool> noWriteDescription = new Option<bool>("--no-write-description");
        private Option<bool> writeInfoJson = new Option<bool>("--write-info-json");
        private Option<bool> noWriteInfoJson = new Option<bool>("--no-write-info-json");
        private Option<bool> writePlaylistMetafiles = new Option<bool>("--write-playlist-metafiles");
        private Option<bool> noWritePlaylistMetafiles = new Option<bool>("--no-write-playlist-metafiles");
        private Option<bool> cleanInfoJson = new Option<bool>("--clean-info-json");
        private Option<bool> noCleanInfoJson = new Option<bool>("--no-clean-info-json");
        private Option<bool> writeComments = new Option<bool>("--write-comments");
        private Option<bool> noWriteComments = new Option<bool>("--no-write-comments");
        private Option<string> loadInfoJson = new Option<string>("--load-info-json");
        private Option<string> cookies = new Option<string>("--cookies");
        private Option<bool> noCookies = new Option<bool>("--no-cookies");
        private Option<string> cookiesFromBrowser = new Option<string>("--cookies-from-browser");
        private Option<bool> noCookiesFromBrowser = new Option<bool>("--no-cookies-from-browser");
        private Option<string> cacheDir = new Option<string>("--cache-dir");
        private Option<bool> noCacheDir = new Option<bool>("--no-cache-dir");
        private Option<bool> removeCacheDir = new Option<bool>("--rm-cache-dir");

        /// <summary>
        /// File containing URLs to download (&quot;-&quot; for
        /// stdin), one URL per line. Lines starting
        /// with &quot;#&quot;, &quot;;&quot; or &quot;]&quot; are considered as
        /// comments and ignored
        /// </summary>
        public string BatchFile { get => batchFile.Value; set => batchFile.Value = value; }
        /// <summary>
        /// Do not read URLs from batch file (default)
        /// </summary>
        public bool NoBatchFile { get => noBatchFile.Value; set => noBatchFile.Value = value; }
        /// <summary>
        /// The paths where the files should be
        /// downloaded. Specify the type of file and the
        /// path separated by a colon &quot;:&quot;. All the same
        /// TYPES as --output are supported.
        /// Additionally, you can also provide &quot;home&quot;
        /// (default) and &quot;temp&quot; paths. All intermediary
        /// files are first downloaded to the temp path
        /// and then the final files are moved over to
        /// the home path after download is finished.
        /// This option is ignored if --output is an
        /// absolute path
        /// </summary>
        public string Paths { get => paths.Value; set => paths.Value = value; }
        /// <summary>
        /// Output filename template; see &quot;OUTPUT
        /// TEMPLATE&quot; for details
        /// </summary>
        public string Output { get => output.Value; set => output.Value = value; }
        /// <summary>
        /// Placeholder for unavailable fields in
        /// &quot;OUTPUT TEMPLATE&quot; (default: &quot;NA&quot;)
        /// </summary>
        public string OutputNaPlaceholder { get => outputNaPlaceholder.Value; set => outputNaPlaceholder.Value = value; }
        /// <summary>
        /// Restrict filenames to only ASCII characters,
        /// and avoid &quot;&amp;&quot; and spaces in filenames
        /// </summary>
        public bool RestrictFilenames { get => restrictFilenames.Value; set => restrictFilenames.Value = value; }
        /// <summary>
        /// Allow Unicode characters, &quot;&amp;&quot; and spaces in
        /// filenames (default)
        /// </summary>
        public bool NoRestrictFilenames { get => noRestrictFilenames.Value; set => noRestrictFilenames.Value = value; }
        /// <summary>
        /// Force filenames to be Windows-compatible
        /// </summary>
        public bool WindowsFilenames { get => windowsFilenames.Value; set => windowsFilenames.Value = value; }
        /// <summary>
        /// Make filenames Windows-compatible only if
        /// using Windows (default)
        /// </summary>
        public bool NoWindowsFilenames { get => noWindowsFilenames.Value; set => noWindowsFilenames.Value = value; }
        /// <summary>
        /// Limit the filename length (excluding
        /// extension) to the specified number of
        /// characters
        /// </summary>
        public int? TrimFilenames { get => trimFilenames.Value; set => trimFilenames.Value = value; }
        /// <summary>
        /// Do not overwrite any files
        /// </summary>
        public bool NoOverwrites { get => noOverwrites.Value; set => noOverwrites.Value = value; }
        /// <summary>
        /// Overwrite all video and metadata files. This
        /// option includes --no-continue
        /// </summary>
        public bool ForceOverwrites { get => forceOverwrites.Value; set => forceOverwrites.Value = value; }
        /// <summary>
        /// Do not overwrite the video, but overwrite
        /// related files (default)
        /// </summary>
        public bool NoForceOverwrites { get => noForceOverwrites.Value; set => noForceOverwrites.Value = value; }
        /// <summary>
        /// Resume partially downloaded files/fragments
        /// (default)
        /// </summary>
        public bool Continue { get => doContinue.Value; set => doContinue.Value = value; }
        /// <summary>
        /// Do not resume partially downloaded
        /// fragments. If the file is not fragmented,
        /// restart download of the entire file
        /// </summary>
        public bool NoContinue { get => noContinue.Value; set => noContinue.Value = value; }
        /// <summary>
        /// Use .part files instead of writing directly
        /// into output file (default)
        /// </summary>
        public bool Part { get => part.Value; set => part.Value = value; }
        /// <summary>
        /// Do not use .part files - write directly into
        /// output file
        /// </summary>
        public bool NoPart { get => noPart.Value; set => noPart.Value = value; }
        /// <summary>
        /// Use the Last-modified header to set the file
        /// modification time (default)
        /// </summary>
        public bool Mtime { get => mtime.Value; set => mtime.Value = value; }
        /// <summary>
        /// Do not use the Last-modified header to set
        /// the file modification time
        /// </summary>
        public bool NoMtime { get => noMtime.Value; set => noMtime.Value = value; }
        /// <summary>
        /// Write video description to a .description
        /// file
        /// </summary>
        public bool WriteDescription { get => writeDescription.Value; set => writeDescription.Value = value; }
        /// <summary>
        /// Do not write video description (default)
        /// </summary>
        public bool NoWriteDescription { get => noWriteDescription.Value; set => noWriteDescription.Value = value; }
        /// <summary>
        /// Write video metadata to a .info.json file
        /// (this may contain personal information)
        /// </summary>
        public bool WriteInfoJson { get => writeInfoJson.Value; set => writeInfoJson.Value = value; }
        /// <summary>
        /// Do not write video metadata (default)
        /// </summary>
        public bool NoWriteInfoJson { get => noWriteInfoJson.Value; set => noWriteInfoJson.Value = value; }
        /// <summary>
        /// Write playlist metadata in addition to the
        /// video metadata when using --write-info-json,
        /// --write-description etc. (default)
        /// </summary>
        public bool WritePlaylistMetafiles { get => writePlaylistMetafiles.Value; set => writePlaylistMetafiles.Value = value; }
        /// <summary>
        /// Do not write playlist metadata when using
        /// --write-info-json, --write-description etc.
        /// </summary>
        public bool NoWritePlaylistMetafiles { get => noWritePlaylistMetafiles.Value; set => noWritePlaylistMetafiles.Value = value; }
        /// <summary>
        /// Remove some internal metadata such as
        /// filenames from the infojson (default)
        /// </summary>
        public bool CleanInfoJson { get => cleanInfoJson.Value; set => cleanInfoJson.Value = value; }
        /// <summary>
        /// Write all fields to the infojson
        /// </summary>
        public bool NoCleanInfoJson { get => noCleanInfoJson.Value; set => noCleanInfoJson.Value = value; }
        /// <summary>
        /// Retrieve video comments to be placed in the
        /// infojson. The comments are fetched even
        /// without this option if the extraction is
        /// known to be quick (Alias: --get-comments)
        /// </summary>
        public bool WriteComments { get => writeComments.Value; set => writeComments.Value = value; }
        /// <summary>
        /// Do not retrieve video comments unless the
        /// extraction is known to be quick (Alias:
        /// --no-get-comments)
        /// </summary>
        public bool NoWriteComments { get => noWriteComments.Value; set => noWriteComments.Value = value; }
        /// <summary>
        /// JSON file containing the video information
        /// (created with the &quot;--write-info-json&quot;
        /// option)
        /// </summary>
        public string LoadInfoJson { get => loadInfoJson.Value; set => loadInfoJson.Value = value; }
        /// <summary>
        /// Netscape formatted file to read cookies from
        /// and dump cookie jar in
        /// </summary>
        public string Cookies { get => cookies.Value; set => cookies.Value = value; }
        /// <summary>
        /// Do not read/dump cookies from/to file
        /// (default)
        /// </summary>
        public bool NoCookies { get => noCookies.Value; set => noCookies.Value = value; }
        /// <summary>
        /// +KEYRING][:PROFILE][::CONTAINER]
        /// The name of the browser to load cookies
        /// from. Currently supported browsers are:
        /// brave, chrome, chromium, edge, firefox,
        /// opera, safari, vivaldi. Optionally, the
        /// KEYRING used for decrypting Chromium cookies
        /// on Linux, the name/path of the PROFILE to
        /// load cookies from, and the CONTAINER name
        /// (if Firefox) (&quot;none&quot; for no container) can
        /// be given with their respective seperators.
        /// By default, all containers of the most
        /// recently accessed profile are used.
        /// Currently supported keyrings are: basictext,
        /// gnomekeyring, kwallet, kwallet5, kwallet6
        /// </summary>
        public string CookiesFromBrowser { get => cookiesFromBrowser.Value; set => cookiesFromBrowser.Value = value; }
        /// <summary>
        /// Do not load cookies from browser (default)
        /// </summary>
        public bool NoCookiesFromBrowser { get => noCookiesFromBrowser.Value; set => noCookiesFromBrowser.Value = value; }
        /// <summary>
        /// Location in the filesystem where yt-dlp can
        /// store some downloaded information (such as
        /// client ids and signatures) permanently. By
        /// default ${XDG_CACHE_HOME}/yt-dlp
        /// </summary>
        public string CacheDir { get => cacheDir.Value; set => cacheDir.Value = value; }
        /// <summary>
        /// Disable filesystem caching
        /// </summary>
        public bool NoCacheDir { get => noCacheDir.Value; set => noCacheDir.Value = value; }
        /// <summary>
        /// Delete all filesystem cache files
        /// </summary>
        public bool RemoveCacheDir { get => removeCacheDir.Value; set => removeCacheDir.Value = value; }
    }
}

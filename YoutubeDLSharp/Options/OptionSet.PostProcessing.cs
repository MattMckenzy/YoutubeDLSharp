﻿// <auto-generated>
// This code was partially generated by a tool.
// </auto-generated>

using System;

namespace YoutubeDLSharp.Options
{
    public partial class OptionSet
    {
        private Option<bool> extractAudio = new Option<bool>("-x", "--extract-audio");
        private Option<AudioConversionFormat> audioFormat = new Option<AudioConversionFormat>("--audio-format");
        private Option<byte?> audioQuality = new Option<byte?>("--audio-quality");
        private Option<string> remuxVideo = new Option<string>("--remux-video");
        private Option<VideoRecodeFormat> recodeVideo = new Option<VideoRecodeFormat>("--recode-video");
        private MultiOption<string> postprocessorArgs = new MultiOption<string>("--postprocessor-args", "--ppa");
        private Option<bool> keepVideo = new Option<bool>("-k", "--keep-video");
        private Option<bool> noKeepVideo = new Option<bool>("--no-keep-video");
        private Option<bool> postOverwrites = new Option<bool>("--post-overwrites");
        private Option<bool> noPostOverwrites = new Option<bool>("--no-post-overwrites");
        private Option<bool> embedSubs = new Option<bool>("--embed-subs");
        private Option<bool> noEmbedSubs = new Option<bool>("--no-embed-subs");
        private Option<bool> embedThumbnail = new Option<bool>("--embed-thumbnail");
        private Option<bool> noEmbedThumbnail = new Option<bool>("--no-embed-thumbnail");
        private Option<bool> embedMetadata = new Option<bool>("--embed-metadata", "--add-metadata");
        private Option<bool> noEmbedMetadata = new Option<bool>("--no-embed-metadata", "--no-add-metadata");
        private Option<bool> embedChapters = new Option<bool>("--embed-chapters", "--add-chapters");
        private Option<bool> noEmbedChapters = new Option<bool>("--no-embed-chapters", "--no-add-chapters");
        private Option<bool> embedInfoJson = new Option<bool>("--embed-info-json");
        private Option<bool> noEmbedInfoJson = new Option<bool>("--no-embed-info-json");
        private Option<string> parseMetadata = new Option<string>("--parse-metadata");
        private MultiOption<string> replaceInMetadata = new MultiOption<string>("--replace-in-metadata");
        private Option<bool> xattrs = new Option<bool>("--xattrs");
        private Option<string> concatPlaylist = new Option<string>("--concat-playlist");
        private Option<string> fixup = new Option<string>("--fixup");
        private Option<string> ffmpegLocation = new Option<string>("--ffmpeg-location");
        private MultiOption<string> exec = new MultiOption<string>("--exec");
        private Option<bool> noExec = new Option<bool>("--no-exec");
        private Option<string> convertSubs = new Option<string>("--convert-subs", "--convert-subtitles");
        private Option<string> convertThumbnails = new Option<string>("--convert-thumbnails");
        private Option<bool> splitChapters = new Option<bool>("--split-chapters");
        private Option<bool> noSplitChapters = new Option<bool>("--no-split-chapters");
        private MultiOption<string> removeChapters = new MultiOption<string>("--remove-chapters");
        private Option<bool> noRemoveChapters = new Option<bool>("--no-remove-chapters");
        private Option<bool> forceKeyframesAtCuts = new Option<bool>("--force-keyframes-at-cuts");
        private Option<bool> noForceKeyframesAtCuts = new Option<bool>("--no-force-keyframes-at-cuts");
        private MultiOption<string> usePostprocessor = new MultiOption<string>("--use-postprocessor");

        /// <summary>
        /// Convert video files to audio-only files
        /// (requires ffmpeg and ffprobe)
        /// </summary>
        public bool ExtractAudio { get => extractAudio.Value; set => extractAudio.Value = value; }
        /// <summary>
        /// Format to convert the audio to when -x is
        /// used. (currently supported: best (default),
        /// aac, alac, flac, m4a, mp3, opus, vorbis,
        /// wav). You can specify multiple rules using
        /// similar syntax as --remux-video
        /// </summary>
        public AudioConversionFormat AudioFormat { get => audioFormat.Value; set => audioFormat.Value = value; }
        /// <summary>
        /// Specify ffmpeg audio quality to use when
        /// converting the audio with -x. Insert a value
        /// between 0 (best) and 10 (worst) for VBR or a
        /// specific bitrate like 128K (default 5)
        /// </summary>
        public byte? AudioQuality { get => audioQuality.Value; set => audioQuality.Value = value; }
        /// <summary>
        /// Remux the video into another container if
        /// necessary (currently supported: avi, flv,
        /// gif, mkv, mov, mp4, webm, aac, aiff, alac,
        /// flac, m4a, mka, mp3, ogg, opus, vorbis,
        /// wav). If target container does not support
        /// the video/audio codec, remuxing will fail.
        /// You can specify multiple rules; e.g.
        /// &quot;aac&gt;m4a/mov&gt;mp4/mkv&quot; will remux aac to m4a,
        /// mov to mp4 and anything else to mkv
        /// </summary>
        public string RemuxVideo { get => remuxVideo.Value; set => remuxVideo.Value = value; }
        /// <summary>
        /// Re-encode the video into another format if
        /// necessary. The syntax and supported formats
        /// are the same as --remux-video
        /// </summary>
        public VideoRecodeFormat RecodeVideo { get => recodeVideo.Value; set => recodeVideo.Value = value; }
        /// <summary>
        /// Give these arguments to the postprocessors.
        /// Specify the postprocessor/executable name
        /// and the arguments separated by a colon &quot;:&quot;
        /// to give the argument to the specified
        /// postprocessor/executable. Supported PP are:
        /// Merger, ModifyChapters, SplitChapters,
        /// ExtractAudio, VideoRemuxer, VideoConvertor,
        /// Metadata, EmbedSubtitle, EmbedThumbnail,
        /// SubtitlesConvertor, ThumbnailsConvertor,
        /// FixupStretched, FixupM4a, FixupM3u8,
        /// FixupTimestamp and FixupDuration. The
        /// supported executables are: AtomicParsley,
        /// FFmpeg and FFprobe. You can also specify
        /// &quot;PP+EXE:ARGS&quot; to give the arguments to the
        /// specified executable only when being used by
        /// the specified postprocessor. Additionally,
        /// for ffmpeg/ffprobe, &quot;_i&quot;/&quot;_o&quot; can be
        /// appended to the prefix optionally followed
        /// by a number to pass the argument before the
        /// specified input/output file, e.g. --ppa
        /// &quot;Merger+ffmpeg_i1:-v quiet&quot;. You can use
        /// this option multiple times to give different
        /// arguments to different postprocessors.
        /// (Alias: --ppa)
        /// </summary>
        public MultiValue<string> PostprocessorArgs { get => postprocessorArgs.Value; set => postprocessorArgs.Value = value; }
        /// <summary>
        /// Keep the intermediate video file on disk
        /// after post-processing
        /// </summary>
        public bool KeepVideo { get => keepVideo.Value; set => keepVideo.Value = value; }
        /// <summary>
        /// Delete the intermediate video file after
        /// post-processing (default)
        /// </summary>
        public bool NoKeepVideo { get => noKeepVideo.Value; set => noKeepVideo.Value = value; }
        /// <summary>
        /// Overwrite post-processed files (default)
        /// </summary>
        public bool PostOverwrites { get => postOverwrites.Value; set => postOverwrites.Value = value; }
        /// <summary>
        /// Do not overwrite post-processed files
        /// </summary>
        public bool NoPostOverwrites { get => noPostOverwrites.Value; set => noPostOverwrites.Value = value; }
        /// <summary>
        /// Embed subtitles in the video (only for mp4,
        /// webm and mkv videos)
        /// </summary>
        public bool EmbedSubs { get => embedSubs.Value; set => embedSubs.Value = value; }
        /// <summary>
        /// Do not embed subtitles (default)
        /// </summary>
        public bool NoEmbedSubs { get => noEmbedSubs.Value; set => noEmbedSubs.Value = value; }
        /// <summary>
        /// Embed thumbnail in the video as cover art
        /// </summary>
        public bool EmbedThumbnail { get => embedThumbnail.Value; set => embedThumbnail.Value = value; }
        /// <summary>
        /// Do not embed thumbnail (default)
        /// </summary>
        public bool NoEmbedThumbnail { get => noEmbedThumbnail.Value; set => noEmbedThumbnail.Value = value; }
        /// <summary>
        /// Embed metadata to the video file. Also
        /// embeds chapters/infojson if present unless
        /// --no-embed-chapters/--no-embed-info-json are
        /// used (Alias: --add-metadata)
        /// </summary>
        public bool EmbedMetadata { get => embedMetadata.Value; set => embedMetadata.Value = value; }
        /// <summary>
        /// Do not add metadata to file (default)
        /// (Alias: --no-add-metadata)
        /// </summary>
        public bool NoEmbedMetadata { get => noEmbedMetadata.Value; set => noEmbedMetadata.Value = value; }
        /// <summary>
        /// Add chapter markers to the video file
        /// (Alias: --add-chapters)
        /// </summary>
        public bool EmbedChapters { get => embedChapters.Value; set => embedChapters.Value = value; }
        /// <summary>
        /// Do not add chapter markers (default) (Alias:
        /// --no-add-chapters)
        /// </summary>
        public bool NoEmbedChapters { get => noEmbedChapters.Value; set => noEmbedChapters.Value = value; }
        /// <summary>
        /// Embed the infojson as an attachment to
        /// mkv/mka video files
        /// </summary>
        public bool EmbedInfoJson { get => embedInfoJson.Value; set => embedInfoJson.Value = value; }
        /// <summary>
        /// Do not embed the infojson as an attachment
        /// to the video file
        /// </summary>
        public bool NoEmbedInfoJson { get => noEmbedInfoJson.Value; set => noEmbedInfoJson.Value = value; }
        /// <summary>
        /// Parse additional metadata like title/artist
        /// from other fields; see &quot;MODIFYING METADATA&quot;
        /// for details. Supported values of &quot;WHEN&quot; are
        /// the same as that of --use-postprocessor
        /// (default: pre_process)
        /// </summary>
        public string ParseMetadata { get => parseMetadata.Value; set => parseMetadata.Value = value; }
        /// <summary>
        /// Replace text in a metadata field using the
        /// given regex. This option can be used
        /// multiple times. Supported values of &quot;WHEN&quot;
        /// are the same as that of --use-postprocessor
        /// (default: pre_process)
        /// </summary>
        public MultiValue<string> ReplaceInMetadata { get => replaceInMetadata.Value; set => replaceInMetadata.Value = value; }
        /// <summary>
        /// Write metadata to the video file&#x27;s xattrs
        /// (using dublin core and xdg standards)
        /// </summary>
        public bool Xattrs { get => xattrs.Value; set => xattrs.Value = value; }
        /// <summary>
        /// Concatenate videos in a playlist. One of
        /// &quot;never&quot;, &quot;always&quot;, or &quot;multi_video&quot;
        /// (default; only when the videos form a single
        /// show). All the video files must have same
        /// codecs and number of streams to be
        /// concatable. The &quot;pl_video:&quot; prefix can be
        /// used with &quot;--paths&quot; and &quot;--output&quot; to set
        /// the output filename for the concatenated
        /// files. See &quot;OUTPUT TEMPLATE&quot; for details
        /// </summary>
        public string ConcatPlaylist { get => concatPlaylist.Value; set => concatPlaylist.Value = value; }
        /// <summary>
        /// Automatically correct known faults of the
        /// file. One of never (do nothing), warn (only
        /// emit a warning), detect_or_warn (the
        /// default; fix file if we can, warn
        /// otherwise), force (try fixing even if file
        /// already exists)
        /// </summary>
        public string Fixup { get => fixup.Value; set => fixup.Value = value; }
        /// <summary>
        /// Location of the ffmpeg binary; either the
        /// path to the binary or its containing
        /// directory
        /// </summary>
        public string FfmpegLocation { get => ffmpegLocation.Value; set => ffmpegLocation.Value = value; }
        /// <summary>
        /// Execute a command, optionally prefixed with
        /// when to execute it, separated by a &quot;:&quot;.
        /// Supported values of &quot;WHEN&quot; are the same as
        /// that of --use-postprocessor (default:
        /// after_move). Same syntax as the output
        /// template can be used to pass any field as
        /// arguments to the command. If no fields are
        /// passed, %(filepath,_filename|)q is appended
        /// to the end of the command. This option can
        /// be used multiple times
        /// </summary>
        public MultiValue<string> Exec { get => exec.Value; set => exec.Value = value; }
        /// <summary>
        /// Remove any previously defined --exec
        /// </summary>
        public bool NoExec { get => noExec.Value; set => noExec.Value = value; }
        /// <summary>
        /// Convert the subtitles to another format
        /// (currently supported: ass, lrc, srt, vtt)
        /// (Alias: --convert-subtitles)
        /// </summary>
        public string ConvertSubs { get => convertSubs.Value; set => convertSubs.Value = value; }
        /// <summary>
        /// Convert the thumbnails to another format
        /// (currently supported: jpg, png, webp). You
        /// can specify multiple rules using similar
        /// syntax as --remux-video
        /// </summary>
        public string ConvertThumbnails { get => convertThumbnails.Value; set => convertThumbnails.Value = value; }
        /// <summary>
        /// Split video into multiple files based on
        /// internal chapters. The &quot;chapter:&quot; prefix can
        /// be used with &quot;--paths&quot; and &quot;--output&quot; to set
        /// the output filename for the split files. See
        /// &quot;OUTPUT TEMPLATE&quot; for details
        /// </summary>
        public bool SplitChapters { get => splitChapters.Value; set => splitChapters.Value = value; }
        /// <summary>
        /// Do not split video based on chapters
        /// (default)
        /// </summary>
        public bool NoSplitChapters { get => noSplitChapters.Value; set => noSplitChapters.Value = value; }
        /// <summary>
        /// Remove chapters whose title matches the
        /// given regular expression. The syntax is the
        /// same as --download-sections. This option can
        /// be used multiple times
        /// </summary>
        public MultiValue<string> RemoveChapters { get => removeChapters.Value; set => removeChapters.Value = value; }
        /// <summary>
        /// Do not remove any chapters from the file
        /// (default)
        /// </summary>
        public bool NoRemoveChapters { get => noRemoveChapters.Value; set => noRemoveChapters.Value = value; }
        /// <summary>
        /// Force keyframes at cuts when
        /// downloading/splitting/removing sections.
        /// This is slow due to needing a re-encode, but
        /// the resulting video may have fewer artifacts
        /// around the cuts
        /// </summary>
        public bool ForceKeyframesAtCuts { get => forceKeyframesAtCuts.Value; set => forceKeyframesAtCuts.Value = value; }
        /// <summary>
        /// Do not force keyframes around the chapters
        /// when cutting/splitting (default)
        /// </summary>
        public bool NoForceKeyframesAtCuts { get => noForceKeyframesAtCuts.Value; set => noForceKeyframesAtCuts.Value = value; }
        /// <summary>
        /// The (case sensitive) name of plugin
        /// postprocessors to be enabled, and
        /// (optionally) arguments to be passed to it,
        /// separated by a colon &quot;:&quot;. ARGS are a
        /// semicolon &quot;;&quot; delimited list of NAME=VALUE.
        /// The &quot;when&quot; argument determines when the
        /// postprocessor is invoked. It can be one of
        /// &quot;pre_process&quot; (after video extraction),
        /// &quot;after_filter&quot; (after video passes filter),
        /// &quot;video&quot; (after --format; before
        /// --print/--output), &quot;before_dl&quot; (before each
        /// video download), &quot;post_process&quot; (after each
        /// video download; default), &quot;after_move&quot;
        /// (after moving video file to it&#x27;s final
        /// locations), &quot;after_video&quot; (after downloading
        /// and processing all formats of a video), or
        /// &quot;playlist&quot; (at end of playlist). This option
        /// can be used multiple times to add different
        /// postprocessors
        /// </summary>
        public MultiValue<string> UsePostprocessor { get => usePostprocessor.Value; set => usePostprocessor.Value = value; }
    }
}

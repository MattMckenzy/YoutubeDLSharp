﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoutubeDLSharp.Options;

namespace YoutubeDLSharp.Tests
{
    [TestClass]
    public class OptionSetTests
    {
        [TestMethod]
        public void TestSimpleOptionFromString()
        {
            Option<string> stringOption = new("-s");
            Option<bool> boolOption = new("--bool");
            Option<int> intOption = new("--int", "-i");
            stringOption.SetFromString("-s someValue");
            Assert.AreEqual("someValue", stringOption.Value);
            boolOption.SetFromString("--bool");
            Assert.AreEqual(true, boolOption.Value);
            intOption.SetFromString("-i 42");
            Assert.AreEqual(42, intOption.Value);
        }

        [TestMethod]
        public void TestEnumOptionFromString()
        {
            Option<VideoRecodeFormat> videoOption = new("--vid");
            videoOption.SetFromString("--vid mp4");
            Assert.AreEqual(VideoRecodeFormat.Mp4, videoOption.Value);
        }

        [TestMethod]
        public void TestComplexOptionFromString()
        {
            Option<DateTime> dateOption = new("-d");
            dateOption.SetFromString("-d 20200322");
            Assert.AreEqual(new DateTime(2020, 03, 22), dateOption.Value);
        }

        [TestMethod]
        public void TestOptionSetFromString()
        {
            string[] lines = new[]
            {
                "-x",
                "# extract to mp3",
                "--audio-format mp3",
                "",
                "# Use this proxy",
                "--proxy 127.0.0.1:3128",
                "-o ~/Movies/%(title)s.%(ext)s",
                "--ffmpeg-location \"My Programs/ffmpeg.exe\""
            };
            OptionSet opts = OptionSet.FromString(lines);
            Assert.IsTrue(opts.ExtractAudio);
            Assert.AreEqual(AudioConversionFormat.Mp3, opts.AudioFormat);
            Assert.AreEqual("127.0.0.1:3128", opts.Proxy);
            Assert.AreEqual("~/Movies/%(title)s.%(ext)s", opts.Output);
            Assert.AreEqual("My Programs/ffmpeg.exe", opts.FfmpegLocation);
        }

        [TestMethod]
        public void TestOptionSetWithMultiOptionsFromString()
        {
            string[] lines = new[]
            {
                "--config-locations ~/config",
                "--config-locations /etc/yt-dlp/config",
                "--postprocessor-args ffmpeg:-vcodec h264_nvenc",
                "--no-part",
                "--verbose",
                "--postprocessor-args ffmpeg_i1:-hwaccel cuda -hwaccel_output_format cuda",
                "--retries 20"
            };
            OptionSet opts = OptionSet.FromString(lines);
            CollectionAssert.AreEquivalent(new[] { "~/config", "/etc/yt-dlp/config" }, (string[])opts.ConfigLocations);
            CollectionAssert.AreEquivalent(new[] { "ffmpeg:-vcodec h264_nvenc", "ffmpeg_i1:-hwaccel cuda -hwaccel_output_format cuda" }, (string[])opts.PostprocessorArgs);
            Assert.IsTrue(opts.NoPart);
            Assert.IsTrue(opts.Verbose);
            Assert.AreEqual(20, opts.Retries);
        }

        [TestMethod]
        public void TestOptionSetWithAliasFromString()
        {
            string[] lines = new[]
            {
                "-x",
                "--external-downloader ffmpeg",
                "--external-downloader-args ffmpeg:some_arg",
                "--min-sleep-interval 10",
                "--add-metadata"
            };
            OptionSet opts = OptionSet.FromString(lines);
            Assert.IsTrue(opts.ExtractAudio);
            CollectionAssert.AreEquivalent(new[] { "ffmpeg" }, (string[])opts.Downloader);
            CollectionAssert.AreEquivalent(new[] { "ffmpeg:some_arg" }, (string[])opts.DownloaderArgs);
            Assert.AreEqual(10, opts.SleepInterval);
            Assert.IsTrue(opts.EmbedMetadata);
        }

        [TestMethod]
        public void TestCustomOptionSetFromString()
        {
            static void AssertCustomOption(IOption option)
            {
                Assert.IsTrue(option.OptionStrings.Any());
                Assert.IsTrue(option.IsCustom);
                Assert.IsTrue(option.IsSet);
                Assert.IsNotNull(option.ToString());
            }

            const string firstOption = "--my-option";
            const string secondOption = "--my-valued-option";
            string[] lines = {
                firstOption,
                $"{secondOption} value"
            };
            
            // Assert custom options parsing from string
            OptionSet opts = OptionSet.FromString(lines);
            AssertCustomOption(opts.CustomOptions.First(s => s.DefaultOptionString == firstOption));
            AssertCustomOption(opts.CustomOptions.First(s => s.DefaultOptionString == secondOption));

            // Assert custom options cloning
            var cloned = opts.OverrideOptions(new OptionSet());
            AssertCustomOption(cloned.CustomOptions.First(s => s.DefaultOptionString == firstOption));
            AssertCustomOption(cloned.CustomOptions.First(s => s.DefaultOptionString == secondOption));
            
            // Assert custom options override
            var overrideOpts = opts.OverrideOptions(OptionSet.FromString(new[] { firstOption }));
            CollectionAssert.AllItemsAreUnique(overrideOpts.CustomOptions);
            
            // Assert custom options to string conversion
            Assert.IsFalse(string.IsNullOrWhiteSpace(opts.ToString()));
        }

        [TestMethod]
        public void TestCustomOptionSetByMethod()
        {
            // Can create custom options (also multiple with same name)
            OptionSet options = new();
            options.AddCustomOption("--custom-string-option", "hello");
            options.AddCustomOption("--custom-bool-option", true);
            options.AddCustomOption("--custom-string-option", "world");
            Assert.AreEqual("--custom-string-option \"hello\" --custom-bool-option --custom-string-option \"world\"", options.ToString().Trim());

            // Can set values of custom options
            options.SetCustomOption("--custom-string-option", "new");
            Assert.AreEqual("--custom-string-option \"new\" --custom-bool-option --custom-string-option \"new\"", options.ToString().Trim());

            // Can delete custom options
            options.DeleteCustomOption("--custom-string-option");
            Assert.AreEqual("--custom-bool-option", options.ToString().Trim());
        }

        [TestMethod]
        public void TestOptionSetOverrideOptions()
        {
            var originalOptions = new OptionSet()
            {
                MergeOutputFormat = DownloadMergeFormat.Mp4,
                ExtractAudio = true,
                AudioFormat = AudioConversionFormat.Wav,
                AudioQuality = 0,
                Username = "bob",
                Verbose = true
            };
            var overrideOptions = new OptionSet()
            {
                MergeOutputFormat = DownloadMergeFormat.Mkv,
                Password = "passw0rd",
                ForceKeyframesAtCuts = true
            };
            var newOptions = originalOptions.OverrideOptions(overrideOptions);
            Assert.AreEqual(AudioConversionFormat.Wav, newOptions.AudioFormat);
            Assert.AreEqual((byte)0, newOptions.AudioQuality);
            Assert.IsTrue(newOptions.Verbose);
            Assert.AreEqual(DownloadMergeFormat.Mkv, newOptions.MergeOutputFormat);
            Assert.AreEqual("bob", newOptions.Username);
            Assert.AreEqual("passw0rd", newOptions.Password);
            Assert.AreEqual(true, newOptions.ForceKeyframesAtCuts);
        }

        [TestMethod]
        public void TestOptionSetOverrideOptionsMulti()
        {
            var originalOptions = new OptionSet();
            var overrideOptions = new OptionSet()
            {
                DownloadSections = "*15-30",
                Downloader = new MultiValue<string>("aria2c", "dash,m3u8:native")
            };
            var newOptions = originalOptions.OverrideOptions(overrideOptions);
            Assert.AreEqual("*15-30", (string)newOptions.DownloadSections);
            CollectionAssert.AreEquivalent(new[] { "aria2c", "dash,m3u8:native" }, (string[])newOptions.Downloader);
        }

        [TestMethod]
        public void TestOptionSetOverrideOptionsForce()
        {
            var originalOptions = new OptionSet()
            {
                NoPlaylist = true,
                DumpSingleJson = true,
                Quiet = false,
            };
            var overrideOptions = (OptionSet)originalOptions.Clone();
            overrideOptions.NoPlaylist = false;
            overrideOptions.DumpSingleJson = false;
            overrideOptions.Quiet = true;
            var newOptions = originalOptions.OverrideOptions(overrideOptions, forceOverride: true);
            Assert.AreEqual(false, newOptions.NoPlaylist);
            Assert.AreEqual(false, newOptions.DumpSingleJson);
            Assert.AreEqual(true, newOptions.Quiet);
        }
    }
}

﻿// <auto-generated>
// This code was partially generated by a tool.
// </auto-generated>

namespace YoutubeDLSharp.Options
{
    public partial class OptionSet
    {
        private Option<string> apMso = new Option<string>("--ap-mso");
        private Option<string> apUsername = new Option<string>("--ap-username");
        private Option<string> apPassword = new Option<string>("--ap-password");
        private Option<bool> apListMso = new Option<bool>("--ap-list-mso");

        /// <summary>
        /// Adobe Pass multiple-system operator (TV
        /// provider) identifier, use --ap-list-mso for
        /// a list of available MSOs
        /// </summary>
        public string ApMso { get => apMso.Value; set => apMso.Value = value; }
        /// <summary>
        /// Multiple-system operator account login
        /// </summary>
        public string ApUsername { get => apUsername.Value; set => apUsername.Value = value; }
        /// <summary>
        /// Multiple-system operator account password.
        /// If this option is left out, youtube-dl will
        /// ask interactively.
        /// </summary>
        public string ApPassword { get => apPassword.Value; set => apPassword.Value = value; }
        /// <summary>
        /// List all supported multiple-system
        /// operators
        /// </summary>
        public bool ApListMso { get => apListMso.Value; set => apListMso.Value = value; }
    }
}

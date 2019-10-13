﻿// <auto-generated>
// This code was partially generated by a tool.
// </auto-generated>

namespace YoutubeDLSharp.Options
{
    public partial class OptionSet
    {
        private Option<string> geoVerificationProxy = new Option<string>("--geo-verification-proxy");
        private Option<bool> geoBypass = new Option<bool>("--geo-bypass");
        private Option<bool> noGeoBypass = new Option<bool>("--no-geo-bypass");
        private Option<string> geoBypassCountry = new Option<string>("--geo-bypass-country");
        private Option<string> geoBypassIpBlock = new Option<string>("--geo-bypass-ip-block");

        /// <summary>
        /// Use this proxy to verify the IP address for
        /// some geo-restricted sites. The default
        /// proxy specified by --proxy (or none, if the
        /// option is not present) is used for the
        /// actual downloading.
        /// </summary>
        public string GeoVerificationProxy { get => geoVerificationProxy.Value; set => geoVerificationProxy.Value = value; }
        /// <summary>
        /// Bypass geographic restriction via faking X
        /// -Forwarded-For HTTP header
        /// </summary>
        public bool GeoBypass { get => geoBypass.Value; set => geoBypass.Value = value; }
        /// <summary>
        /// Do not bypass geographic restriction via
        /// faking X-Forwarded-For HTTP header
        /// </summary>
        public bool NoGeoBypass { get => noGeoBypass.Value; set => noGeoBypass.Value = value; }
        /// <summary>
        /// Force bypass geographic restriction with
        /// explicitly provided two-letter ISO 3166-2
        /// country code
        /// </summary>
        public string GeoBypassCountry { get => geoBypassCountry.Value; set => geoBypassCountry.Value = value; }
        /// <summary>
        /// Force bypass geographic restriction with
        /// explicitly provided IP block in CIDR
        /// notation
        /// </summary>
        public string GeoBypassIpBlock { get => geoBypassIpBlock.Value; set => geoBypassIpBlock.Value = value; }
    }
}

using System.Text.RegularExpressions;

namespace Shared.Utilities
{
    public class UserAgentUtils
    {
        public static UserAgentInfo Parse(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return new UserAgentInfo();

            var info = new UserAgentInfo();

            var ua = userAgent.ToLower();

            info.IsMobile = ua.Contains("mobile") || ua.Contains("android") ||
                           ua.Contains("iphone") || ua.Contains("ipad") ||
                           ua.Contains("ipod") || ua.Contains("blackberry") ||
                           ua.Contains("windows phone");

            ParseBrowser(userAgent, info);

            ParseOperatingSystem(userAgent, info);

            ParseDevice(userAgent, info);

            return info;
        }

        private static void ParseBrowser(string userAgent, UserAgentInfo info)
        {
            // Chrome (check before Safari as Chrome contains Safari in UA)
            var chromeMatch = Regex.Match(userAgent, @"Chrome/(\d+\.\d+)", RegexOptions.IgnoreCase);
            if (chromeMatch.Success)
            {
                info.Browser = "Chrome";
                info.Version = chromeMatch.Groups [1].Value;
                return;
            }

            // Firefox
            var firefoxMatch = Regex.Match(userAgent, @"Firefox/(\d+\.\d+)", RegexOptions.IgnoreCase);
            if (firefoxMatch.Success)
            {
                info.Browser = "Firefox";
                info.Version = firefoxMatch.Groups [1].Value;
                return;
            }

            // Safari
            var safariMatch = Regex.Match(userAgent, @"Version/(\d+\.\d+).*Safari", RegexOptions.IgnoreCase);
            if (safariMatch.Success)
            {
                info.Browser = "Safari";
                info.Version = safariMatch.Groups [1].Value;
                return;
            }

            // Edge
            var edgeMatch = Regex.Match(userAgent, @"Edg/(\d+\.\d+)", RegexOptions.IgnoreCase);
            if (edgeMatch.Success)
            {
                info.Browser = "Edge";
                info.Version = edgeMatch.Groups [1].Value;
                return;
            }

            // Internet Explorer
            var ieMatch = Regex.Match(userAgent, @"MSIE (\d+\.\d+)", RegexOptions.IgnoreCase);
            if (ieMatch.Success)
            {
                info.Browser = "Internet Explorer";
                info.Version = ieMatch.Groups [1].Value;
                return;
            }

            // Trident (IE 11+)
            if (userAgent.Contains("Trident"))
            {
                var tridentMatch = Regex.Match(userAgent, @"rv:(\d+\.\d+)", RegexOptions.IgnoreCase);
                if (tridentMatch.Success)
                {
                    info.Browser = "Internet Explorer";
                    info.Version = tridentMatch.Groups [1].Value;
                    return;
                }
            }

            info.Browser = "Unknown";
            info.Version = "Unknown";
        }

        private static void ParseOperatingSystem(string userAgent, UserAgentInfo info)
        {
            if (userAgent.Contains("Windows NT 10.0"))
                info.OperatingSystem = "Windows 10";
            else if (userAgent.Contains("Windows NT 6.3"))
                info.OperatingSystem = "Windows 8.1";
            else if (userAgent.Contains("Windows NT 6.2"))
                info.OperatingSystem = "Windows 8";
            else if (userAgent.Contains("Windows NT 6.1"))
                info.OperatingSystem = "Windows 7";
            else if (userAgent.Contains("Windows NT 6.0"))
                info.OperatingSystem = "Windows Vista";
            else if (userAgent.Contains("Windows NT 5.1"))
                info.OperatingSystem = "Windows XP";
            else if (userAgent.Contains("Mac OS X"))
            {
                var macMatch = Regex.Match(userAgent, @"Mac OS X (\d+[._]\d+)", RegexOptions.IgnoreCase);
                if (macMatch.Success)
                {
                    var version = macMatch.Groups [1].Value.Replace('_', '.');
                    info.OperatingSystem = $"macOS {version}";
                }
                else
                    info.OperatingSystem = "macOS";
            }
            else if (userAgent.Contains("Android"))
            {
                var androidMatch = Regex.Match(userAgent, @"Android (\d+\.\d+)", RegexOptions.IgnoreCase);
                if (androidMatch.Success)
                    info.OperatingSystem = $"Android {androidMatch.Groups [1].Value}";
                else
                    info.OperatingSystem = "Android";
            }
            else if (userAgent.Contains("iPhone OS") || userAgent.Contains("iOS"))
            {
                var iosMatch = Regex.Match(userAgent, @"OS (\d+_\d+)", RegexOptions.IgnoreCase);
                if (iosMatch.Success)
                {
                    var version = iosMatch.Groups [1].Value.Replace('_', '.');
                    info.OperatingSystem = $"iOS {version}";
                }
                else
                    info.OperatingSystem = "iOS";
            }
            else if (userAgent.Contains("Linux"))
                info.OperatingSystem = "Linux";
            else
                info.OperatingSystem = "Unknown";
        }

        private static void ParseDevice(string userAgent, UserAgentInfo info)
        {
            if (userAgent.Contains("iPhone"))
                info.Device = "iPhone";
            else if (userAgent.Contains("iPad"))
                info.Device = "iPad";
            else if (userAgent.Contains("iPod"))
                info.Device = "iPod";
            else if (userAgent.Contains("Android"))
            {
                if (userAgent.Contains("Mobile"))
                    info.Device = "Android Phone";
                else
                    info.Device = "Android Tablet";
            }
            else if (userAgent.Contains("BlackBerry"))
                info.Device = "BlackBerry";
            else if (userAgent.Contains("Windows Phone"))
                info.Device = "Windows Phone";
            else if (info.IsMobile)
                info.Device = "Mobile Device";
            else
                info.Device = "Desktop";
        }
    }
    public class UserAgentInfo
    {
        public string Browser { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string OperatingSystem { get; set; } = string.Empty;
        public string Device { get; set; } = string.Empty;
        public bool IsMobile { get; set; }
    }
}

﻿namespace AssetManagement.Infrastructure.Settings.Options
{
    public class JwtOption
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; }
    }
}

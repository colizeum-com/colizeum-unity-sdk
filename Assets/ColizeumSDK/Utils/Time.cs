using System;

namespace ColizeumSDK.Utils
{
    internal static class Time
    {
        public static int Unix()
        {
            return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
using System;

namespace Sap.SmartAccounting.Core.Utility
{
    public class OSInfo
    {
        public static string GetOS()
        {
            return Environment.OSVersion.VersionString;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Sap.SmartAccounting.Core;

namespace Sap.SmartAccounting.Mvc.Entities
{
    public static class ConfigGlobal
    {
        static ConfigGlobal()
        {
            Init();
        }

        private static Dictionary<string, string> ConfigDictionary { get; set; }
        public static AssemblyInfo Assembly { get; private set; }

        public static void Refresh()
        {
            Init();
        }

        private static void Init()
        {
            Config.Cache.RefreshCache();
            ConfigDictionary = Config.Cache.GetDictionaryByConfigSystem(ConfigSystem.SmartAccounting);

            Assembly = new AssemblyInfo
            {
                Title = ConfigDictionary["AssemblyTitle"] ?? string.Empty,
                Description = ConfigDictionary["AssemblyDescription"] ?? string.Empty,
                Configuration = ConfigDictionary["AssemblyConfiguration"] ?? string.Empty,
                Company = ConfigDictionary["AssemblyCompany"] ?? string.Empty,
                Product = ConfigDictionary["AssemblyProduct"] ?? string.Empty,
                Copyright = ConfigDictionary["AssemblyCopyright"] ?? string.Empty,
                Trademark = ConfigDictionary["AssemblyTrademark"] ?? string.Empty,
                Culture = ConfigDictionary["AssemblyCulture"] ?? string.Empty,
                Version = ConfigDictionary["AssemblyVersion"] ?? string.Empty,
                FileVersion = ConfigDictionary["AssemblyFileVersion"] ?? string.Empty
            };
        }

        public static bool IsSystemAdmin(string userId)
        {
            if (!string.IsNullOrEmpty(userId) && SystemAdmin.Length > 0)
            {
                return SystemAdmin.Any(a => a.Equals(userId, StringComparison.OrdinalIgnoreCase));
            }

            return false;
        }

        #region Members and Properties

        public static string[] SystemAdmin
        {
            get
            {
                var admins = ConfigDictionary["SystemAdmin"];
                return admins.Split('|');
            }
        }

        public static bool SystemActive
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigDictionary["SystemActive"]);
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool SchedulerActive
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigDictionary["SchedulerActive"]);
                }
                catch
                {
                    return false;
                }
            }
        }

        #endregion
    }
}
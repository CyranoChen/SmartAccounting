using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Sap.SmartAccounting.Core.Dapper;

namespace Sap.SmartAccounting.Core
{
    [DbSchema("Core_Config", Sort = "ConfigSystem, ConfigKey")]
    public class Config : Dao
    {
        public Config() { }

        public Config(ConfigSystem cs, string key, object value)
        {
            ConfigSystemInfo = cs;
            ConfigSystem = cs.ToString();
            ConfigKey = key;
            ConfigValue = value?.ToString().Trim();
        }

        public override void Inital()
        {
            ConfigSystemInfo = (ConfigSystem)Enum.Parse(typeof(ConfigSystem), ConfigSystem);
        }

        public void Save()
        {
            IRepository repo = new Repository();

            if (string.IsNullOrEmpty(ConfigSystem))
            {
                ConfigSystem = ConfigSystemInfo.ToString();
            }

            repo.Save(this, x => x.ConfigSystem == ConfigSystem && x.ConfigKey == ConfigKey);
        }

        public static void UpdateAssemblyInfo(Assembly assembly, ConfigSystem configSystem)
        {
            if (assembly != null)
            {
                //[assembly: AssemblyTitle("Sap.SmartAccounting.Core")]
                //[assembly: AssemblyDescription("沪ICP备12045527号")]
                //[assembly: AssemblyConfiguration("webmaster@arsenalcn.com")]
                //[assembly: AssemblyCompany("Arsenal China Official Supporters Club")]
                //[assembly: AssemblyProduct("Arsenalcn.com")]
                //[assembly: AssemblyCopyright("© 2015")]
                //[assembly: AssemblyTrademark("ArsenalCN")]
                //[assembly: AssemblyCulture("")]
                //[assembly: AssemblyVersion("1.8.*")]
                //[assembly: AssemblyFileVersion("1.8.2")]

                //AssemblyTitle
                var c = new Config(configSystem, "AssemblyTitle", ((AssemblyTitleAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute)))?.Title);

                c.Save();

                //AssemblyDescription
                c = new Config(configSystem, "AssemblyDescription", ((AssemblyDescriptionAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof(AssemblyDescriptionAttribute)))?.Description);

                c.Save();

                //AssemblyConfiguration
                c = new Config(configSystem, "AssemblyConfiguration", ((AssemblyConfigurationAttribute)
                    Attribute.GetCustomAttribute(assembly, typeof(AssemblyConfigurationAttribute)))?.Configuration);

                c.Save();

                //AssemblyCompany
                c = new Config(configSystem, "AssemblyCompany", ((AssemblyCompanyAttribute)
                    Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute)))?.Company);

                c.Save();

                //AssemblyProduct
                c = new Config(configSystem, "AssemblyProduct", ((AssemblyProductAttribute)
                    Attribute.GetCustomAttribute(assembly, typeof(AssemblyProductAttribute)))?.Product);

                c.Save();

                //AssemblyCopyright
                c = new Config(configSystem, "AssemblyCopyright", ((AssemblyCopyrightAttribute)
                    Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute)))?.Copyright);

                c.Save();

                //AssemblyTrademark
                c = new Config(configSystem, "AssemblyTrademark", ((AssemblyTrademarkAttribute)
                    Attribute.GetCustomAttribute(assembly, typeof(AssemblyTrademarkAttribute)))?.Trademark);

                c.Save();

                //AssemblyCulture
                c = new Config(configSystem, "AssemblyCulture", ((AssemblyCultureAttribute)
                    Attribute.GetCustomAttribute(assembly, typeof(AssemblyCultureAttribute)))?.Culture);

                c.Save();

                //AssemblyVersion
                var assemblyName = assembly.GetName();
                var version = assemblyName.Version;

                c = new Config(configSystem, "AssemblyVersion", version?.ToString());

                c.Save();

                //AssemblyFileVersion
                c = new Config(configSystem, "AssemblyFileVersion", ((AssemblyFileVersionAttribute)
                    Attribute.GetCustomAttribute(assembly, typeof(AssemblyFileVersionAttribute)))?.Version);

                c.Save();
            }
        }

        public static class Cache
        {
            public static List<Config> ConfigList;

            static Cache()
            {
                InitCache();
            }

            public static void RefreshCache()
            {
                InitCache();
            }

            private static void InitCache()
            {
                IRepository repo = new Repository();

                ConfigList = repo.All<Config>();
            }

            public static Config Load(ConfigSystem cs, string key)
            {
                return ConfigList.Find(x => x.ConfigSystemInfo.Equals(cs) && x.ConfigKey.Equals(key));
            }

            public static string LoadDict(ConfigSystem cs, string key)
            {
                return GetDictionaryByConfigSystem(cs)[key];
            }

            public static Dictionary<string, string> GetDictionaryByConfigSystem(ConfigSystem cs)
            {
                var list = ConfigList.FindAll(x => x.ConfigSystemInfo.Equals(cs));

                if (list.Count > 0)
                {
                    var dict = new Dictionary<string, string>();

                    foreach (var c in list)
                    {
                        try
                        {
                            dict.Add(c.ConfigKey, c.ConfigValue);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    return dict;
                }
                return null;
            }
        }

        #region Members and Properties

        public ConfigSystem ConfigSystemInfo { get; set; }

        [DbColumn("ConfigSystem", IsKey = true)]
        public string ConfigSystem { get; set; }

        [DbColumn("ConfigKey", IsKey = true)]
        public string ConfigKey { get; set; }

        [DbColumn("ConfigValue")]
        public string ConfigValue { get; set; }

        #endregion
    }

    public enum ConfigSystem
    {
        SmartAccounting
    }
}
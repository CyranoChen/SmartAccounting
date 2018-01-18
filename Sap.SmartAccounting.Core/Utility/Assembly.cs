namespace Sap.SmartAccounting.Core
{
    public class AssemblyInfo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Configuration { get; set; }
        public string Company { get; set; }
        public string Product { get; set; }
        public string Copyright { get; set; }
        public string Trademark { get; set; }
        public string Culture { get; set; }
        public string Version { get; set; }
        public string FileVersion { get; set; }


        //public static string GenAssemblyVersionInfo(string version)
        //{
        //    if (version.LastIndexOf(".", StringComparison.Ordinal) <= 0) return version;

        //    var versionPrefix = version.Substring(0, version.LastIndexOf(".", StringComparison.Ordinal));

        //    long versionDateAdd;

        //    if (long.TryParse(version.Remove(0, version.LastIndexOf(".", StringComparison.Ordinal) + 1), out versionDateAdd))
        //    {
        //        //?
        //        var versionDate = DateTime.Now.AddDays(-versionDateAdd);

        //        return $"{versionPrefix} + {versionDate}";
        //    }
        //    else
        //    {
        //        return version;
        //    }
        //}
    }
}
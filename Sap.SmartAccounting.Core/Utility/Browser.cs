using System;
using System.Text;
using System.Web;

namespace Sap.SmartAccounting.Core.Utility
{
    public static class BrowserInfo
    {
        public static string GetBrowser()
        {
            if (HttpContext.Current == null)
            {
                return string.Empty;
            }

            var bc = HttpContext.Current.Request.Browser;

            var retValue = new StringBuilder();

            retValue.Append(bc.Type);

            if (!bc.MobileDeviceManufacturer.ToLower().Equals("unknown"))
            {
                retValue.AppendFormat(", {0}", bc.MobileDeviceManufacturer);
            }

            if (!bc.MobileDeviceModel.ToLower().Equals("unknown"))
            {
                retValue.AppendFormat(", {0}", bc.MobileDeviceModel);
            }

            return retValue.ToString();
        }

        public static bool IsWeChatClient()
        {
            if (HttpContext.Current == null)
            {
                return false;
            }

            var bc = HttpContext.Current.Request.Browser;

            return bc.Capabilities[""].ToString().IndexOf("MicroMessenger", StringComparison.OrdinalIgnoreCase) > 0;
        }
    }
}
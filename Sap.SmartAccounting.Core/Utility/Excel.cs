using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sap.SmartAccounting.Core.Utility
{
    public static class ExportUtl
    {
        public static void ExportToExcel(GridView gv, string fileName)
        {
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.Clear();

            // 设置编码和附件格式   
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding("gb2312");
            HttpContext.Current.Response.Charset = "gb2312";

            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);

            // 导出excel文件   
            var sw = new StringWriter();
            var hw = new HtmlTextWriter(sw);

            SetStyle(gv);
            gv.RenderControl(hw);

            HttpContext.Current.Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + sw);
            //HttpContext.Current.Response.Flush();
            //HttpContext.Current.Response.Close();
            HttpContext.Current.Response.End();
        }

        private static void SetStyle(GridView gv)
        {
            gv.Font.Name = "Tahoma";
            //gv.BorderStyle = BorderStyle.Solid;
            //gv.HeaderStyle.BackColor = Color.LightCyan;
            //gv.HeaderStyle.ForeColor = Color.Black;

            gv.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            gv.HeaderStyle.Wrap = false;
            gv.HeaderStyle.Font.Bold = true;
            gv.HeaderStyle.Font.Size = 10;

            gv.RowStyle.Font.Size = 10;
        }
    }
}
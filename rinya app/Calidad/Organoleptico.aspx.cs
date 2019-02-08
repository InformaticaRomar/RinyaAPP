using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Utiles;
using CustomControls;
using System.Data.SqlClient;
using OfficeOpenXml;


namespace rinya_app.Calidad
{
    public partial class Organoleptico : System.Web.UI.Page
    {
        [System.Web.Services.WebMethod()]

        [System.Web.Script.Services.ScriptMethod()]
        public static void Get_Excel()
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                // date columns
                List<string> dateColumns = new List<string>() {

                "WCPC_FECHAPED",
                "Fecha Pedido"
            };

                // hide columns
                List<string> hideColumns = new List<string>() {

                "RecordID",
                "CategoryID"
            };
              
                DataTable table = HttpContext.Current.Session["datos"] as DataTable;
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                ws.Cells["A1"].LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Medium14);

                FormatWorksheetData(dateColumns, hideColumns, table, ws);

                // make sure it is sent as a XLSX file
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                // make sure it is downloaded rather than viewed in the browser window
                HttpContext.Current.Response.AddHeader("Content-disposition", "attachment; filename=Pedidos_tablet.xlsx");
                HttpContext.Current.Response.BinaryWrite(pck.GetAsByteArray());
                HttpContext.Current.Response.End();
            }
        }
        private static void FormatWorksheetData(List<string> dateColumns, List<string> hideColumns, DataTable table, ExcelWorksheet ws)
        {
            int columnCount = table.Columns.Count;
            int rowCount = table.Rows.Count;

            ExcelRange r;

            // which columns have dates in
            for (int i = 1; i <= columnCount; i++)
            {
                // if cell header value matches a date column
                if (dateColumns.Contains(ws.Cells[1, i].Value.ToString()))
                {
                    r = ws.Cells[2, i, rowCount + 1, i];
                    r.AutoFitColumns();
                    r.Style.Numberformat.Format = @"dd MMM yyyy hh:mm";
                }
            }
            // get all data and autofit
            r = ws.Cells[1, 1, rowCount + 1, columnCount];
            r.AutoFitColumns();

            // which columns have columns that should be hidden
            for (int i = 1; i <= columnCount; i++)
            {
                // if cell header value matches a hidden column
                if (hideColumns.Contains(ws.Cells[1, i].Value.ToString()))
                {
                    ws.Column(i).Hidden = true;
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
         //   if (!IsPostBack)
          //  {
             
            //}
        }
    }
}
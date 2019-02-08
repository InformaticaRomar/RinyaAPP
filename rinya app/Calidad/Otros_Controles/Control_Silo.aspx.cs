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


namespace rinya_app.Calidad.Otros_Controles
{
    public partial class Control_Silo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private string sql()
        {

            string silo = comboseleccionado.Value;
            
            string F_desde = datepicker1.Value; // Request.Form["datepicker1"];

            string F_hasta = datepicker2.Value; //.Value; //this.Page.Request.Form["datepicker2"];//.Text;
            //F_desde = Request.QueryString("datepicker1");
            string sql = @"SELECT ID, SSCC,USUARIO,FECHA, PH, DORNIC, BRIX, INH_L, GRASA, PROTEINA, LACTOSA, SNF,TS,OLOR, TEST_R FROM DATOS_SILO_ORGANOLEPTICO where SSCC='" + silo + "' and FECHA between convert(datetime,'" + F_desde + "',103) and convert(datetime,'" + F_hasta + "',103)";
            return sql;
        }
        public void Get_Excel()
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
                Quality data = new Quality();
                DataTable table = data.Sql_Datatable(sql());
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                ws.Cells["A1"].LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Medium14);

                FormatWorksheetData(dateColumns, hideColumns, table, ws);

                // make sure it is sent as a XLSX file
                Response.ContentType = "application/vnd.ms-excel";
                // make sure it is downloaded rather than viewed in the browser window
                Response.AddHeader("Content-disposition", "attachment; filename=Control_Silo.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
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
     
        

        protected void btnexcel_Click(object sender, EventArgs e)
        {
            Get_Excel();
        }
    }
}
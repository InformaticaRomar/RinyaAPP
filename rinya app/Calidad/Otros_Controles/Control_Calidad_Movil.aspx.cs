using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utiles;
using System.Data;
using OfficeOpenXml;

namespace rinya_app.Calidad.Otros_Controles
{
    public partial class Control_Calidad_Movil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string desde = TextBox_Desde.Text;
            string hasta = TextBox_hasta.Text;
            if (desde.Length > 0 && desde.Length > 0)
            {
                string sql = "EXEC	 [dbo].[GET_DATOS_ORGANOLEPTICO_TABLET] N'" + desde + "', '" + hasta + "'";
                Quality con = new Quality();
                DataTable datos = con.Sql_Datatable(sql);
                if (datos.Rows.Count > 0)
                {
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                        ws.Cells["A1"].LoadFromDataTable(datos, true, OfficeOpenXml.Table.TableStyles.Medium16);
                       
                        List<string> dateColumns = new List<string>() { "F_Quality" };
                        FormatWorksheetData(dateColumns, datos, ws);
                        // make sure it is sent as a XLSX file
                        Response.ContentType = "application/vnd.ms-excel";
                        // make sure it is downloaded rather than viewed in the browser window
                        Response.AddHeader("Content-disposition", "attachment; filename=Control_Tablet.xlsx");
                        Response.BinaryWrite(pck.GetAsByteArray());
                        Response.End();
                    }
                }
                //Actualizo
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Actualizo Articulo " + DropDown_Articulo.Text + " con lote " + lote + " al Estado " + Estados.SelectedItem.Text + "')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tienes que rellenar todos los campos para obtener datos')", true);
            }
        }
        private static void FormatWorksheetData(List<string> dateColumns, DataTable table, ExcelWorksheet ws)
        {
            int columnCount = table.Columns.Count;
            int rowCount = table.Rows.Count;

            ExcelRange r;
           // ExcelRange r2;
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


        }
    }
}
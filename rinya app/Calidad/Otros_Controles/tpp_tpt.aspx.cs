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
    public partial class tpp_tpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        Quality con = new Quality();
        private static void FormatWorksheetData(List<string> dateColumns, DataTable table, ExcelWorksheet ws)
        {
            int columnCount = table.Columns.Count;
            int rowCount = table.Rows.Count;

            ExcelRange r;
            try { 
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
        }catch(Exception ex) { Console.WriteLine(ex.Message); }


        }
        DataTable datos_C_Tunel(string sscc)
        {
            string sql = @"select SSCC,	TUNEL, FECHA_INTRODUCIDA as	Hora_Pack from CONTROL_TUNEL where SSCC='" + sscc + "'";
            return con.Sql_Datatable(sql);

        }
        DataTable datos_L_Tunel(string sscc)
        {
            string sql = @"select Fecha,	Matricula,	Lector from LecturasTunel where Matricula='" + sscc + "'";
            return con.Sql_Datatable(sql);

        }
        DataTable datos_Temp(string sscc)
        {
            string sql = @"select SSCC,	Temperatura	,Fecha  from dbControlTemperatura where sscc='" + sscc + "'";
            return con.Sql_Datatable(sql);

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable datos_C_Tunel_ = datos_C_Tunel(SSCC_TextBox_.Text);
            DataTable datos_L_Tunel_ = datos_L_Tunel(SSCC_TextBox_.Text);
            DataTable datos_Temp_ = datos_Temp(SSCC_TextBox_.Text);
            using (ExcelPackage pck = new ExcelPackage())
            {
                datos_C_Tunel_.TableName = "C_Tunel";
                datos_L_Tunel_.TableName = "L_Tunel";
                datos_Temp_.TableName = "Temp";
                List<string> dateColumns1 = new List<string>() { "Fecha" };
                List<string> dateColumns2 = new List<string>() { "Hora_Pack" };
                
                ExcelWorksheet CT = pck.Workbook.Worksheets.Add("C_Tunel");
                ExcelWorksheet LT = pck.Workbook.Worksheets.Add("L_Tunel");
                ExcelWorksheet TE = pck.Workbook.Worksheets.Add("Temp");
                CT.Cells["A1"].LoadFromDataTable(datos_C_Tunel_, true, OfficeOpenXml.Table.TableStyles.Medium2);
                FormatWorksheetData(dateColumns2, datos_C_Tunel_, CT);
                LT.Cells["A1"].LoadFromDataTable(datos_L_Tunel_, true, OfficeOpenXml.Table.TableStyles.Medium6);
                FormatWorksheetData(dateColumns1, datos_L_Tunel_, LT);
                TE.Cells["A1"].LoadFromDataTable(datos_Temp_, true, OfficeOpenXml.Table.TableStyles.Medium12);
                FormatWorksheetData(dateColumns1, datos_Temp_, TE);
                Response.ContentType = "application/vnd.ms-excel";

                Response.AddHeader("Content-disposition", "attachment; filename=datos_tpp_tpt.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }
        }
    }
}

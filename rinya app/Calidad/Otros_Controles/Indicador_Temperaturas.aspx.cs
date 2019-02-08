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
    public partial class Indicador_Temperaturas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private string datos_temperaturas(string F_desde, string F_hasta)
        {

            string sql = @"select count (*) AS [Total temp analizadas]
,SUM(CASE WHEN  (DATOS_ORGANOLEPTICO.temperatura) BETWEEN 8 AND 15 THEN 1 ELSE 0 END) as [Total temperaturas Entre 8º y 15º]
,SUM(CASE WHEN  (DATOS_ORGANOLEPTICO.temperatura) > 15 THEN 1 ELSE 0 END) as [Total temperaturas mas de 15º]
,SUM(CASE WHEN  (DATOS_ORGANOLEPTICO.temperatura) >= 8  THEN 1 ELSE 0 END) as [Total temperaturas mas de 8]
,DATENAME(month, FECHA_SSCC_CON ) AS Mes 
,MONTH(FECHA_SSCC_CON) as orden
 from DATOS_ORGANOLEPTICO inner join dbControlTemperatura on dbControlTemperatura.sscc=DATOS_ORGANOLEPTICO.sscc
where FECHA_SSCC_CON BETWEEN convert (datetime,'"+F_desde+ @"',103) and convert (datetime,'" + F_hasta + @"',103)
group by DATENAME(month, FECHA_SSCC_CON ),MONTH(FECHA_SSCC_CON) 
order by MONTH(FECHA_SSCC_CON) asc";

            return sql;
        }
        private bool Get_Excel()
        {
            Quality con = new Quality();
            string F_desde = datepicker1.Value;
            string F_hasta = datepicker2.Value;
            string sql = datos_temperaturas(F_desde, F_hasta);
            List<string> hideColumns = new List<string>() {

                "orden"
            };
            using (ExcelPackage pck = new ExcelPackage())
            {
                

                DataTable table = con.Sql_Datatable(sql);
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                ws.Cells["A1"].LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Medium14);
                FormatWorksheetData( hideColumns, table, ws);
                
                // make sure it is sent as a XLSX file
                Response.ContentType = "application/vnd.ms-excel";
                // make sure it is downloaded rather than viewed in the browser window
                Response.AddHeader("Content-disposition", "attachment; filename=Indicador_Temperaturas.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }

            return true;
        }
        private static void FormatWorksheetData( List<string> hideColumns, DataTable table, ExcelWorksheet ws)
        {
            int columnCount = table.Columns.Count;
            int rowCount = table.Rows.Count;
            
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
        protected void Btexport_Click(object sender, EventArgs e)
        {
            Get_Excel();
        }
    }
}
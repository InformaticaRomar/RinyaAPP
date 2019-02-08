using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using System.Data;
using Utiles;

namespace rinya_app.Calidad.Otros_Controles
{
    public partial class Indicador_TTP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private string datos_temperaturas(string F_desde, string F_hasta)
        {

            /*string sql = @"SELECT  COUNT(*) AS TOTAL, SUM(CASE WHEN CONTROL_ENTRADA_TUNEL.TPP >= 7200 THEN 1 ELSE 0 END) AS [TPP+2H], SUM(CASE WHEN CONTROL_ENTRADA_TUNEL.TPRE_E >= 7200 THEN 1 ELSE 0 END) 
                         AS [TPRE+2H], DATENAME(month, DATOS_ORGANOLEPTICO.FECHA_SSCC_CON) AS Mes, AREA.Descripción as LINEA_PRODUCCION
						 ,MONTH(FECHA_SSCC_CON) as orden
FROM            AREA INNER JOIN
                         ARTICULO ON AREA.[Area Preparación] = ARTICULO.[Area Preparación] INNER JOIN
                         CONTROL_ENTRADA_TUNEL INNER JOIN
                         DATOS_ORGANOLEPTICO ON DATOS_ORGANOLEPTICO.SSCC = CONTROL_ENTRADA_TUNEL.SSCC ON ARTICULO.Artículo = DATOS_ORGANOLEPTICO.ARTICULO
where FECHA_SSCC_CON BETWEEN convert (datetime,'" + F_desde + @"',103) and convert (datetime,'" + F_hasta + @"',103)
GROUP BY DATENAME(month, DATOS_ORGANOLEPTICO.FECHA_SSCC_CON), MONTH(DATOS_ORGANOLEPTICO.FECHA_SSCC_CON), AREA.Descripción
ORDER BY orden";*/
            string sql = @"SELECT  COUNT(*) AS TOTAL, SUM(CASE WHEN CONTROL_ENTRADA_TUNEL.TPP >= 7200 THEN 1 ELSE 0 END) AS [TPP+2H], SUM(CASE WHEN CONTROL_ENTRADA_TUNEL.TPRE_E >= 7200 THEN 1 ELSE 0 END) 
                         AS [TPRE+2H], Sum(case when  CONTROL_ENTRADA_TUNEL.TPP is null then 1 else 0 end) as [No_TPP] , DATENAME(month, DATOS_ORGANOLEPTICO.FECHA_CREACION) AS Mes, AREA.Descripción as LINEA_PRODUCCION --,ARTICULO.[Area Preparación] 
						 ,MONTH(FECHA_CREACION) as orden
FROM            AREA inner  JOIN
                         ARTICULO ON AREA.[Area Preparación] = ARTICULO.[Area Preparación] INNER JOIN
                         DATOS_ORGANOLEPTICO ON ARTICULO.Artículo = DATOS_ORGANOLEPTICO.ARTICULO
						inner join FAMILIA on ARTICULO.Familia=FAMILIA.Familia
						left join CONTROL_ENTRADA_TUNEL  on DATOS_ORGANOLEPTICO.SSCC = CONTROL_ENTRADA_TUNEL.SSCC
where  AREA.[Area Preparación]<>106 and FECHA_CREACION BETWEEN convert (datetime,'" + F_desde + @"',103) and convert (datetime,'" + F_hasta + @"',103)
and (([DATOS_ORGANOLEPTICO].SSCC in (select ssccsilo from SILO) ) or ([DATOS_ORGANOLEPTICO].ID_LOTE in (select idlote from SSCC_CON inner join sscc on sscc.Id=sscc_con.IdPadre where sscc.sscc=DATOS_ORGANOLEPTICO.SSCC))) 
and ((ARTICULO.Discrim_4='VE'  and FAMILIA.Agrupación=2) OR (ARTICULO.Discrim_4 like '%P%') ) 
GROUP BY DATENAME(month, DATOS_ORGANOLEPTICO.FECHA_CREACION), MONTH(DATOS_ORGANOLEPTICO.FECHA_CREACION), AREA.Descripción";
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
                FormatWorksheetData(hideColumns, table, ws);

                // make sure it is sent as a XLSX file
                Response.ContentType = "application/vnd.ms-excel";
                // make sure it is downloaded rather than viewed in the browser window
                Response.AddHeader("Content-disposition", "attachment; filename=Indicador_TPP.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }

            return true;
        }
        private static void FormatWorksheetData(List<string> hideColumns, DataTable table, ExcelWorksheet ws)
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
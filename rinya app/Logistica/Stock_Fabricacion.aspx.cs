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

namespace rinya_app.Logistica
{
    public partial class Stock_Fabricacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private string consulta_sql(string fecha1, string fecha2)
        {

            string sql = @"SELECT [DESPIECE PARTIDAS].Año, [DESPIECE PARTIDAS].Empresa,
 [DESPIECE PARTIDAS].[Nº Despiece], [DESPIECE PARTIDAS].Despiece, 
 [DESPIECE PARTIDAS].Artículo, [DESPIECE PARTIDAS].[Año Lote Gen],
  [DESPIECE PARTIDAS].[Empresa Lote Gen], [DESPIECE PARTIDAS].[Serie Lote Gen], 
  [DESPIECE PARTIDAS].[Nº Lote Gen], [DESPIECE PARTIDAS].[Nº despieces] AS Piezas, [DESPIECE PARTIDAS].Fecha,
  Sum (case when (coalesce ([DESPIECE PARTIDAS].[Peso],0)=0) then [DESPIECE PARTIDAS].[Nº Despieces]*[ARTICULO].[Factor(Kg/Ud)] else [DESPIECE PARTIDAS].[Peso] end ) AS PesoDespiece,

    [DESPIECE PARTIDAS].[Precio Coste] AS PrecioDespiece, ARTICULO.Descripción, [DESPIECE PARTIDAS].SSCC, [STOCK PARTIDAS].[Unidades Iniciales], [STOCK PARTIDAS].[Unidades Actuales], [STOCK PARTIDAS].[Kg Iniciales], [STOCK PARTIDAS].[Kg Actuales], [STOCK PARTIDAS].FechaCaducidad, [STOCK PARTIDAS].LoteInterno, [STOCK PARTIDAS].Hora, ARTICULO.[Control exist], ARTICULO.ComboLista1, [STOCK PARTIDAS].Almacén, [DESPIECE PARTIDAS].NumPalet, [STOCK PARTIDAS].Hora AS FH, [STOCK PARTIDAS].[Fecha Creación], SSCC_CON.Estado, ARTICULO.[Area Preparación], [DESPIECE PARTIDAS].CampoAuxText4, ESTADO.Estado, [STOCK PARTIDAS].IDSSCC, ARTICULO.Familia, ARTICULO.ComboLista1
FROM (([DESPIECE PARTIDAS] INNER JOIN (ARTICULO INNER JOIN [STOCK PARTIDAS] ON ARTICULO.Artículo = [STOCK PARTIDAS].Artículo) ON ([DESPIECE PARTIDAS].[Nº Lote Gen] = [STOCK PARTIDAS].[Nº Partida]) AND ([DESPIECE PARTIDAS].[Serie Lote] = [STOCK PARTIDAS].Serie) AND ([DESPIECE PARTIDAS].[Empresa Lote] = [STOCK PARTIDAS].Empresa) AND ([DESPIECE PARTIDAS].[Año Lote] = [STOCK PARTIDAS].Año) AND ([DESPIECE PARTIDAS].IDSSCC = [STOCK PARTIDAS].IDSSCC)) LEFT JOIN SSCC_CON ON [STOCK PARTIDAS].IDSSCC = SSCC_CON.IdLote) LEFT JOIN ESTADO ON SSCC_CON.Estado = ESTADO.ID_Estado
GROUP BY [DESPIECE PARTIDAS].Año, [DESPIECE PARTIDAS].Empresa, [DESPIECE PARTIDAS].[Nº Despiece], [DESPIECE PARTIDAS].Despiece, [DESPIECE PARTIDAS].Artículo, [DESPIECE PARTIDAS].[Año Lote Gen], [DESPIECE PARTIDAS].[Empresa Lote Gen], [DESPIECE PARTIDAS].[Serie Lote Gen], [DESPIECE PARTIDAS].[Nº Lote Gen], [DESPIECE PARTIDAS].[Nº despieces], [DESPIECE PARTIDAS].Fecha, [DESPIECE PARTIDAS].[Precio Coste], ARTICULO.Descripción, [DESPIECE PARTIDAS].SSCC, [STOCK PARTIDAS].[Unidades Iniciales], [STOCK PARTIDAS].[Unidades Actuales], [STOCK PARTIDAS].[Kg Iniciales], [STOCK PARTIDAS].[Kg Actuales], [STOCK PARTIDAS].FechaCaducidad, [STOCK PARTIDAS].LoteInterno, [STOCK PARTIDAS].Hora, ARTICULO.[Control exist], ARTICULO.ComboLista1, [STOCK PARTIDAS].Almacén, [DESPIECE PARTIDAS].NumPalet, [STOCK PARTIDAS].Hora, [STOCK PARTIDAS].[Fecha Creación], SSCC_CON.Estado, ARTICULO.[Area Preparación], [DESPIECE PARTIDAS].CampoAuxText4, ESTADO.Estado, [STOCK PARTIDAS].IDSSCC, ARTICULO.Familia, ARTICULO.ComboLista1
HAVING ((([DESPIECE PARTIDAS].Año)>=2018) AND (([DESPIECE PARTIDAS].Empresa)=1) 
 AND (([STOCK PARTIDAS].Almacén)<>6 Or ([STOCK PARTIDAS].Almacén)<>7) 
 AND (([STOCK PARTIDAS].[Fecha Creación]) between convert(datetime,'" + fecha1 + "',103) and convert(datetime,'" + fecha2 + @"',103) )
 AND ((ARTICULO.[Area Preparación])<>200))
ORDER BY [DESPIECE PARTIDAS].[Año Lote Gen] DESC ";

            return sql;
        }

        private bool Get_Excel()
        {
            Quality con = new Quality();

            //string sql = consulta_sql(string agencia);
            List<string> hideColumns = new List<string>() {

                "orden"
            };
            using (ExcelPackage pck = new ExcelPackage())
            {

                List<string> dateColumns = new List<string>() {

                "Fecha",
                "FechaCaducidad",
                "Hora",
                "FH",
                "Fecha Creación"
                };
                DataTable table = con.Sql_Datatable(consulta_sql(datepicker_1.Text, datepicker_2.Text));
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                ws.Cells["A1"].LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Medium14);
                FormatWorksheetData(dateColumns,hideColumns, table, ws);

                // make sure it is sent as a XLSX file
                Response.ContentType = "application/vnd.ms-excel";
                // make sure it is downloaded rather than viewed in the browser window
                Response.AddHeader("Content-disposition", "attachment; filename=stock_fabricacion.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }

            return true;
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
        protected void BtBuscar_Click(object sender, EventArgs e)
        {
            //Buscar();
            Get_Excel();
        }


    }
}
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
    public partial class Control_liberado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private void obtener_datos(int tipo_select)
        {
           
            string strdocPath;
            strdocPath = HttpContext.Current.Server.MapPath(".") + @"\documentos\Control_Laboratorio.xlsx";

            string sql = @"SELECT  DATOS_ORGANOLEPTICO.articulo as ARTICULO,
REPLACE(REPLACE(ARTICULO.DESCRIPCIÓN,CHAR(10),''),CHAR(13),'') AS DESCRIPCION,
DATOS_ORGANOLEPTICO.SSCC as MATRICULA,DATOS_ORGANOLEPTICO.LOTE_INTERNO as [LOTE INTERNO], 
                         DATOS_ORGANOLEPTICO.NUMPALET as [NUMERO PALET],
						 DATOS_ORGANOLEPTICO.[FECHA_SSCC_CON] as [FECHA FABRICACION],[CONTROL_LIBERA_AUTO_ORGANO].ESTADO,
      case [RESULTADO] when 1 then 'LIBERAR'  when 0 then 'FUERA RANGO' else 'FALTAN PRUEBAS' end RESULTADO
      ,[LOG] as [EXPLICACION]
      ,[FECHA] as [FECHA EVALUACION]

  FROM [QC600].[dbo].[CONTROL_LIBERA_AUTO_ORGANO]
  inner join DATOS_ORGANOLEPTICO on id_lote=idsscc and DATOS_ORGANOLEPTICO.sscc=[CONTROL_LIBERA_AUTO_ORGANO].sscc

  inner join ARTICULO on artículo=DATOS_ORGANOLEPTICO.articulo ";
            switch (tipo_select)
            {
                case 1:
                    sql = sql + @" where DATOS_ORGANOLEPTICO.LOTE_INTERNO like '%" + Lote_textBoxs_.Text + "%'";
                    break;
                case 2:
                    sql = sql + @" where DATOS_ORGANOLEPTICO.LOTE_INTERNO like '%" + Lote_textBoxs_.Text + "%'  and DATOS_ORGANOLEPTICO.NUMPALET = '" + Palet_text_.Text + "'";
                    break;
                case 3:
                    sql = sql + @" where DATOS_ORGANOLEPTICO.SSCC like '%" + SSCC_TextBox_.Text + "%'";
                    break;
                case 4:
                    sql = sql + @" where DATOS_ORGANOLEPTICO.[FECHA_SSCC_CON] between convert(datetime,'" + TextBox_Desde.Text + "',103) and  convert(datetime,'" + TextBox_hasta.Text + "',103)";
                    break;
            }
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);
            if (datos.Rows.Count > 0)
            {
                SSCC_TextBox_.Text = "";
                Lote_textBoxs_.Text = "";
                Palet_text_.Text = "";
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                    List<string> dateColumns = new List<string>() { "FECHA FABRICACION", "FECHA EVALUACION" };

                    ws.Cells["A1"].LoadFromDataTable(datos, true, OfficeOpenXml.Table.TableStyles.Medium14);
                    FormatWorksheetData(dateColumns, datos, ws);
                    // make sure it is sent as a XLSX file
                    Response.ContentType = "application/vnd.ms-excel";
                    // make sure it is downloaded rather than viewed in the browser window
                    Response.AddHeader("Content-disposition", "attachment; filename=Control_Libera_Auto.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();
                }
            }
        }
        private static void FormatWorksheetData(List<string> dateColumns, DataTable table, ExcelWorksheet ws)
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

         
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            int tipo = 4;
            obtener_datos(tipo);

        }

        protected void BtBuscar_Click(object sender, EventArgs e)
        {

            int tipo = 1;
            Lote_textBoxs_.Text = Lote_textBoxs_.Text.Trim(' ');
            // Palet_text_.Text
            Palet_text_.Text = Palet_text_.Text.Trim(' ');
            if (Palet_text_.Text.Length > 0)
                tipo = 2;
            obtener_datos(tipo);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            int tipo = 3;
            SSCC_TextBox_.Text = SSCC_TextBox_.Text.TrimStart('0').Replace("91x", "");
            obtener_datos(tipo);
        }
        protected void SSCC_TextBox_TextChanged(object sender, EventArgs e)
        {

            SSCC_TextBox_.Text = SSCC_TextBox_.Text.TrimStart('0').Replace("91x", "");

        }
    }
}
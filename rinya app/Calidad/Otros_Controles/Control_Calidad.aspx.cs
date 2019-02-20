using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utiles;
using System.Data;
using OfficeOpenXml;
using System.IO;

namespace rinya_app.Calidad.Otros_Controles
{
    public partial class Control_Calidad : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack == true)
            {
                datos = Session["datos"] as DataTable;
            }
        }
        private DataTable datos { get; set; }

       // [WebMethod(EnableSession = true)]
        private void obtener_datos (int tipo_select)
        {
         //   bool resultado = false;
            string strdocPath;
            strdocPath = HttpContext.Current.Server.MapPath(".") + @"\documentos\Control_Laboratorio.xlsx";

            string sql = @"SELECT  CARACTERISTICAS_DATOS.ARTICULO as Articulo, ARTICULO.Descripción as [Descripcion articulo], CARACTERISTICAS_DATOS.SSCC as Matricula,DATOS_ORGANOLEPTICO.LOTE_INTERNO as [Lote Interno], 
                         DATOS_ORGANOLEPTICO.NUMPALET as [Numero Palet], CARACTERISTICAS_DATOS.CARACTERISTICA as [Cod. Caracteristica], 
                         Organolectico_Carac.Caracteristica AS [Descripcion Caracteristica], 
						 case when CARACTERISTICAS_DATOS.CARACTERISTICA=0 then (select [Estado] from Estado where [ID_Estado]= CARACTERISTICAS_DATOS.VALOR) else
						 Convert (varchar (30) ,CARACTERISTICAS_DATOS.VALOR) end as Valor,
						 CARACTERISTICAS_ARTICULO.V_Min as [V Min], 
						 CARACTERISTICAS_ARTICULO.V_Max as [V Max],
						  case when CARACTERISTICAS_DATOS.CARACTERISTICA=0 then '' else
						(case when CARACTERISTICAS_DATOS.VALOR  between CARACTERISTICAS_ARTICULO.V_Min and CARACTERISTICAS_ARTICULO.V_Max then 'Valor Entre Margenes' else 'Valor Fuera Margenes' end) end as Evaulacion,
						  CARACTERISTICAS_DATOS.USUARIO as Usuario, CARACTERISTICAS_DATOS.FECHA as Fecha,  
						    case when CARACTERISTICAS_DATOS.CARACTERISTICA=0 then '' else
						(                        
						  case when Organolectico_Carac.Tipo_Dato =2 then 'S/N' else 'Rango' end) end as [Tipo dato]
						 
FROM            CARACTERISTICAS_DATOS INNER JOIN
                         Organolectico_Carac ON CARACTERISTICAS_DATOS.CARACTERISTICA = Organolectico_Carac.Cod_Caracteristica left JOIN
                         CARACTERISTICAS_ARTICULO ON CARACTERISTICAS_DATOS.ARTICULO = CARACTERISTICAS_ARTICULO.Articulo AND 
                         CARACTERISTICAS_DATOS.CARACTERISTICA = CARACTERISTICAS_ARTICULO.Caracteristica INNER JOIN
                         ARTICULO ON CARACTERISTICAS_DATOS.ARTICULO = ARTICULO.Artículo INNER JOIN
                         DATOS_ORGANOLEPTICO ON CARACTERISTICAS_DATOS.SSCC = DATOS_ORGANOLEPTICO.SSCC AND CARACTERISTICAS_DATOS.ID_LOTE = DATOS_ORGANOLEPTICO.ID_LOTE ";
            switch (tipo_select) { 
                case 1:
                    sql = sql + @" where DATOS_ORGANOLEPTICO.LOTE_INTERNO like '%"+ Lote_textBoxs.Text+"%'";
                break;
                case 2:
                    sql = sql + @" where DATOS_ORGANOLEPTICO.LOTE_INTERNO like '%" + Lote_textBoxs.Text + "%'  and DATOS_ORGANOLEPTICO.NUMPALET = '"+ Palet_text.Text+"'";
                    break;
                case 3:
                    sql = sql + @" where CARACTERISTICAS_DATOS.SSCC like '%" + SSCC_TextBox.Text + "%'";
                    break;
            }
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);
            if (datos.Rows.Count > 0) { 
                SSCC_TextBox.Text = "";
            Lote_textBoxs.Text = "";
            Palet_text.Text = "";
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                List<string> dateColumns = new List<string>() {

                "Fecha"
                
            };

                // hide columns
                List<string> hideColumns = new List<string>() {

                "RecordID",
                "CategoryID"
            };
              
                ws.Cells["A1"].LoadFromDataTable(datos, true, OfficeOpenXml.Table.TableStyles.Medium14);
                FormatWorksheetData(dateColumns, hideColumns, datos, ws);

                /* byte[] a = pck.GetAsByteArray();

                 FileStream objfilestream = new FileStream(strdocPath, FileMode.Create, FileAccess.ReadWrite);
                 objfilestream.Write(a, 0, a.Length);
                 objfilestream.Close();
                 resultado = true;*/
                // make sure it is sent as a XLSX file
                Response.ContentType = "application/vnd.ms-excel";
                // make sure it is downloaded rather than viewed in the browser window
                Response.AddHeader("Content-disposition", "attachment; filename=Control_Laboratorio.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();

               
            }

                /* string filename = "Control_Laboratorio.xlsx";
                 System.IO.StringWriter tw = new System.IO.StringWriter();
                 System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                 System.IO.MemoryStream ms = DataTableToExcelXlsx(datos, "Datos");
                 ms.WriteTo(HttpContext.Current.Response.OutputStream);
                 HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                 HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                 HttpContext.Current.Response.StatusCode = 200;
                 HttpContext.Current.Response.End();
                 HttpContext.Current.Response.Write(tw.ToString());
                 HttpContext.Current.Response.End();*/
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
        protected void Button1_Click(object sender, EventArgs e)
        {
            int tipo = 3;
            SSCC_TextBox.Text = SSCC_TextBox.Text.TrimStart('0').Replace("91x", "");
            obtener_datos(tipo);
        }

        protected void BtBuscar_Click(object sender, EventArgs e)
        {
            int tipo = 1;
            Lote_textBoxs.Text = Lote_textBoxs.Text.Trim(' ');
            Palet_text.Text = Palet_text.Text.Trim(' ');
            if (Palet_text.Text.Length > 0)
                tipo = 2;
            obtener_datos(tipo);
        }

        protected void SSCC_TextBox_TextChanged(object sender, EventArgs e)
        {
            
            SSCC_TextBox.Text = SSCC_TextBox.Text.TrimStart('0').Replace("91x","");
            
        }
    }
}
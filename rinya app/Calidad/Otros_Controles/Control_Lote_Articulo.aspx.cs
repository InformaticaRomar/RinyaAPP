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
    public partial class Control_Lote_Articulo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string sql = "select distinct Articulo,  convert(varchar,Articulo) +' | ' + ARTICULO.Descripción as Descripcion  FROM [QC600].[dbo].[CARACTERISTICAS_ARTICULO] inner join ARTICULO on ARTICULO.Artículo=[CARACTERISTICAS_ARTICULO].[Articulo]";
                Quality con = new Quality();

                DataTable datos = con.Sql_Datatable(sql);
                DropDown_Articulo.DataSource = datos;
                DropDown_Articulo.DataTextField = "Descripcion";
                DropDown_Articulo.DataValueField = "Articulo";

                // Bind the data to the control.
                DropDown_Articulo.DataBind();
            }
         }
        private static void FormatWorksheetData(List<string> dateColumns, DataTable table, ExcelWorksheet ws)
        {
            int columnCount = table.Columns.Count;
            int rowCount = table.Rows.Count;

            ExcelRange r;
            ExcelRange r2;
            // which columns have dates in
            for (int i = 1; i <= columnCount; i++)
            {
                r2 = ws.Cells[7, i, rowCount + 6, i];
                r2.Style.Font.Size = 8;
                r2.AutoFitColumns();
                // if cell header value matches a date column
                if (dateColumns.Contains(ws.Cells[6, i].Value.ToString()))
                {
                    r = ws.Cells[6, i, rowCount + 1, i];
                    r.AutoFitColumns();
                    r.Style.Numberformat.Format = @"dd MMM yyyy hh:mm";
                }
            }
            // get all data and autofit
            r = ws.Cells[1, 1, rowCount + 1, columnCount];
            r.AutoFitColumns();


        }
        DataTable _Columnas_dato(DataTable dt) {
            // List<string> Ocultar_Columns = new List<string>();
            List<string> Ocultar_Columns = new List<string>() { "Articulo", "Descripcion", "Lote_Interno" };

           // DataTable result = new DataTable();
           /* foreach (DataColumn a in dt.AsDataView ())
            {
                if (!Ocultar_Columns.Contains(a.ColumnName))
                {
                    result.Columns.Add(a);
                }
            }*/
            
            string[] selectedColumns =(from p in dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList()
                                  where !(from ex in Ocultar_Columns select ex).Contains(p) select p).ToArray();

            DataTable result = new DataView(dt).ToTable(false, selectedColumns);

            return result;
        }

        DataTable _Cabecera_Excel(DataTable dt) {
            List<string> Mostrar_Columns = new List<string>() { "Articulo", "Descripcion", "Lote_Interno" };
            DataRow dat = new DataView(dt).ToTable(true, Mostrar_Columns.ToArray()).Select().First();
            DataTable result = new DataTable();
            result.Columns.Add("Nombre", typeof(String));
            result.Columns.Add("Valor", typeof(String));
            DataRow tmp_0 = result.NewRow();
            tmp_0[0] = "Articulo:";
            tmp_0[1] = dat[0].ToString();
            result.Rows.Add(tmp_0);
            DataRow tmp_1 = result.NewRow();
            tmp_1[0] = dat[1].ToString();
            tmp_1[1] = "";
            result.Rows.Add(tmp_1);
            DataRow tmp_2 = result.NewRow();
            tmp_2[0] = "Lote_Interno:";
            tmp_2[1] = dat[2].ToString();
            result.Rows.Add(tmp_2);

            return result;
        }

        protected void BtBuscar_Click(object sender, EventArgs e)
        {
            string articulo = DropDown_Articulo.Text;
            string lote = Lote_textBoxs.Text;
            if (articulo.Length > 0  && lote.Length > 0)
            {
                string sql = "EXEC	 [dbo].[GET_DATOS_ORGANOLEPTICO] N'"+ lote + "', "+ articulo ;
                Quality con = new Quality();
                DataTable datos = con.Sql_Datatable(sql);
                if (datos.Rows.Count > 0)
                {
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        DataTable dt = _Columnas_dato(datos);
                        DataTable dt2= _Cabecera_Excel(datos);

                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                        ws.Cells["A2"].LoadFromDataTable(dt2, false, OfficeOpenXml.Table.TableStyles.None);
                       // ws.Cells["D2"].Value = "Firma:";
                        //Add the textbox
                        var shape = ws.Drawings.AddShape("txtDesc", eShapeStyle.Rect);
                        shape.SetPosition(0, 2, 3, 3);
                        shape.SetSize(150, 90);
                        shape.Text = "Firma:";
                        shape.Fill.Style = eFillStyle.SolidFill;
                        shape.Fill.Color =System.Drawing.Color.DarkSlateGray;
                        shape.Fill.Transparancy = 20;
                        shape.Border.Fill.Style = eFillStyle.SolidFill;
                        shape.Border.LineStyle = OfficeOpenXml.Drawing.eLineStyle.LongDash;
                        shape.Border.Width = 1;
                        
                        shape.Border.Fill.Color = System.Drawing.Color.Black;
                        shape.Border.LineCap = OfficeOpenXml.Drawing.eLineCap.Round;
                        shape.TextAnchoring = OfficeOpenXml.Drawing.eTextAnchoringType.Top;
                        shape.TextVertical = OfficeOpenXml.Drawing.eTextVerticalType.Horizontal;
                        shape.TextAnchoringControl = false;

                        ws.Cells["A6"].LoadFromDataTable(dt, true, OfficeOpenXml.Table.TableStyles.Medium12);
                        List<string> dateColumns = new List<string>() { "FECHA" };
                        FormatWorksheetData(dateColumns, dt, ws);
                        // make sure it is sent as a XLSX file
                        Response.ContentType = "application/vnd.ms-excel";
                        // make sure it is downloaded rather than viewed in the browser window
                        Response.AddHeader("Content-disposition", "attachment; filename=Control_Lote_Articulo.xlsx");
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
    }
}
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
    public partial class Control_huevo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            string sql = "select Articulo, Descripcion from VW_ART_HUEVO";
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
            r2 = ws.Cells[1, i, rowCount , i];
            r2.Style.Font.Size = 8;
            r2.AutoFitColumns();
            // if cell header value matches a date column
            if (dateColumns.Contains(ws.Cells[1, i].Value.ToString()))
            {
                r = ws.Cells[1, i, rowCount + 1, i];
                r.AutoFitColumns();
                r.Style.Numberformat.Format = @"dd/MM/yyyy";
            }
        }
        // get all data and autofit
        r = ws.Cells[1, 1, rowCount + 1, columnCount];
        r.AutoFitColumns();


    }
   

    protected void BtBuscar_Click(object sender, EventArgs e)
    {
        string articulo = DropDown_Articulo.Text;
        string lote = Lote_textBoxs.Text;
            if (articulo.Length > 0 && lote.Length > 0)
            {
                string sql = "select * from VW_HUEVO where loteProveedor like'" + lote + "' and Articulo= " + articulo;
                Quality con = new Quality();
                DataTable datos = con.Sql_Datatable(sql);
                if (datos.Rows.Count > 0)
                {
                    //DropDown_Articulo.Text = "";
                    Lote_textBoxs.Text = "";

                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                        List<string> dateColumns = new List<string>() { "FechaCaducidadProveedor", "FechaCaducidad" };

                        ws.Cells["A1"].LoadFromDataTable(datos, true, OfficeOpenXml.Table.TableStyles.Medium14);
                        FormatWorksheetData(dateColumns, datos, ws);
                        // make sure it is sent as a XLSX file
                        Response.ContentType = "application/vnd.ms-excel";
                        // make sure it is downloaded rather than viewed in the browser window
                        Response.AddHeader("Content-disposition", "attachment; filename=Control_huevo.xlsx");
                        Response.BinaryWrite(pck.GetAsByteArray());
                        Response.End();
                    }
                }
            }
    }
}
}
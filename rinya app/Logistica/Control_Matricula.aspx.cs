using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Utiles;
using System.Data.SqlClient;
using OfficeOpenXml;

namespace rinya_app.Logistica
{
    public partial class Control_Matricula : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private string consulta_Creacion_sscc(string Matricula)
        {

            string sql = @"SELECT      distinct
            [STOCK PARTIDAS].Artículo,
             [QC600].[dbo].[ARTICULO].Descripción as [Nombre Articulo],
             cast([STOCK PARTIDAS].Año as varchar)+'/'+ cast([STOCK PARTIDAS].Empresa as varchar)+'/'+[STOCK PARTIDAS].Serie+'/'+cast([STOCK PARTIDAS].[Nº Partida] as varchar) as Partida,
             [STOCK PARTIDAS].[Nº linea Partida]
             ,[STOCK PARTIDAS].[Unidades Iniciales]
			 ,[STOCK PARTIDAS].[Kg Iniciales]
			 ,[STOCK PARTIDAS].[Unidades Actuales]
			,[STOCK PARTIDAS].[Kg Actuales],
            convert (varchar,SSCC_CON.Fecha, 113) as [Fecha Creacion], SSCC.Usuario as [Usuario Creacion]
FROM        SSCC_CON  INNER JOIN
                      SSCC ON  SSCC.Id =SSCC_CON.IdPadre
                      inner join 
                      [STOCK PARTIDAS] on [STOCK PARTIDAS].IDSSCC=SSCC_CON.IdLote
                      inner join 
                      [QC600].[dbo].[ARTICULO] on [QC600].[dbo].[ARTICULO].Artículo= [STOCK PARTIDAS].Artículo
                      inner join [QC600].[dbo].[Estado] on [QC600].[dbo].[Estado].ID_Estado=SSCC_CON.Estado
WHERE     (SSCC.SSCC ='" + Matricula + @"')";

            
            return sql;
        }

        private string consulta_Modificacion_sscc(string Matricula)
        {
            string sql = @"SELECT      
             SSCC_CON_AUDIT.UdInicial, 
             SSCC_CON_AUDIT.KgInicial, 
             SSCC_CON_AUDIT.UdActual, 
             SSCC_CON_AUDIT.KgActual,
             SSCC_CON_AUDIT.Temperatura, 
             SSCC_CON_AUDIT.Ph, 
             SSCC_CON_AUDIT.ObsEstado, 
             SSCC_CON_AUDIT.Color, 
             SSCC_CON_AUDIT.Sabor, 
             SSCC_CON_AUDIT.Textura, 
             SSCC_CON_AUDIT.Brix, 
             SSCC_CON_AUDIT.Corte, 
             SSCC_CON_AUDIT.Film, 
             SSCC_CON_AUDIT.Estado,
             [Estado].Estado as [Descr Estado], 
             SSCC_CON_AUDIT.Accion, 
             convert (varchar,SSCC_CON_AUDIT.DateChange, 113) as Fecha, 
             SSCC_CON_AUDIT.UserChange, 
             SSCC_CON_AUDIT.HostName
             
FROM        SSCC_CON_AUDIT  INNER JOIN
                      SSCC ON  SSCC.Id =SSCC_CON_AUDIT.IdPadre
                      inner join 
                      [STOCK PARTIDAS] on [STOCK PARTIDAS].IDSSCC=SSCC_CON_AUDIT.IdLote
                      inner join 
                      [QC600].[dbo].[ARTICULO] on [QC600].[dbo].[ARTICULO].Artículo= [STOCK PARTIDAS].Artículo
                      inner join [QC600].[dbo].[Estado] on [QC600].[dbo].[Estado].ID_Estado=SSCC_CON_AUDIT.Estado
WHERE     (SSCC.SSCC = '" + Matricula + @"')
ORDER BY SSCC_CON_AUDIT.DateChange asc";

            return sql;
        }

        private string consulta_Expedicion_sscc(string Matricula)
        {

            string sql = @"SELECT CAST(ALBARAN_PARTIDA.Año AS varchar) + '/' + CAST(ALBARAN_PARTIDA.Empresa AS varchar) 
                      + '/' + ALBARAN_PARTIDA.Serie + '/' + CAST(ALBARAN_PARTIDA.[Nº Albarán] AS varchar) AS [Albaran Expedido], ALBARAN_PARTIDA.[Nº linea Albarán], 
                      ALBARAN_PARTIDA.Unidades, ALBARAN_PARTIDA.Kgs, ALBARAN_CABE.[Código Cliente], CLIENTE.Nombre
FROM         ALBARAN_PARTIDA INNER JOIN
                      ALBARAN_CABE ON ALBARAN_PARTIDA.Año = ALBARAN_CABE.Año AND ALBARAN_PARTIDA.Empresa = ALBARAN_CABE.Empresa AND 
                      ALBARAN_PARTIDA.Serie = ALBARAN_CABE.Serie AND ALBARAN_PARTIDA.[Nº Albarán] = ALBARAN_CABE.[Nº Albarán] INNER JOIN
                      CLIENTE ON ALBARAN_CABE.[Código Cliente] = CLIENTE.Cliente
 where (SSCCLeido='" + Matricula + @"')";


            return sql;
        }
        protected void BtBuscar_Click(object sender, EventArgs e)
        {
            Quality con = new Quality();
            if (SSCC_textBoxs.Text.Length == 18) {
                string sscc = SSCC_textBoxs.Text;
                DataTable datos_creacion = con.Sql_Datatable(consulta_Creacion_sscc(sscc));
                DataTable datos_modificacion = con.Sql_Datatable(consulta_Modificacion_sscc(sscc));
                DataTable datos_expedicion = con.Sql_Datatable(consulta_Expedicion_sscc(sscc));

                datos_creacion.TableName = "Creacion";
                datos_modificacion.TableName = "Modificacion";
                datos_expedicion.TableName = "Expediciones";
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Creacion");
                    ws.Cells["A1"].LoadFromDataTable(datos_creacion, true, OfficeOpenXml.Table.TableStyles.Light13);
                    ExcelWorksheet ws2 = pck.Workbook.Worksheets.Add("Modificacion");
                    ws2.Cells["A1"].LoadFromDataTable(datos_modificacion, true, OfficeOpenXml.Table.TableStyles.Light12);
                    ExcelWorksheet ws3 = pck.Workbook.Worksheets.Add("Expedicion");
                    ws3.Cells["A1"].LoadFromDataTable(datos_expedicion, true, OfficeOpenXml.Table.TableStyles.Light14);
                    /*var dataRange = ws.Cells[ws.Dimension.Address.ToString()];
                    dataRange.AutoFitColumns();*/
                    // make sure it is sent as a XLSX file
                    Response.ContentType = "application/vnd.ms-excel";
                    // make sure it is downloaded rather than viewed in the browser window
                    Response.AddHeader("Content-disposition", "attachment; filename=Control_Matricula.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();


                }
                }
        }
    }
}
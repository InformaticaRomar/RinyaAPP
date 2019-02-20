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
    public partial class integracion_agencia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GridView1.ClearSearchFilters();
                FilterToNthPage(GridView1.SearchFilters, GridView1.CurrentSortExpression, (string)GridView1.CurrentSortDirection, 1);
            }

        }
        private void Buscar()
        {

            GridView1.Visible = false;

            
                GridView1.ClearSearchFilters();
                FilterToNthPage(GridView1.SearchFilters, GridView1.CurrentSortExpression, (string)GridView1.CurrentSortDirection, 1);
                //FilterToNthPage(dat,)
                /*  GridView1.DataSource = Fabricacion_stock (DropDownDesde.SelectedValue, DropDownHasta.SelectedValue);
                  GridView1.DataBind();*/
                GridView1.Visible = true;

            

        }

        private string consulta_sql(string agencia)
        {

            string sql = @"Select t.CLIENTE,t.[N.PEDIDO] as N_PEDIDO , CEILING (COALESCE(sum(t.Palet),0))as N_BUL, t.NOMBRE, t.DIRECCION,cast(t.[C.POS] as nvarchar) as [C_POS],t.POBLACION, sum (t.peso) as KILOS, convert (varchar, t.[FECHA ENTREGA],103) as [FECHA_ENTREGA] ,t.OBSERVACIONES    from (
SELECT       ALBARAN_CABE.[Código Cliente] as CLIENTE, ALBARAN_CABE.[Nº Albarán] as [N.PEDIDO] ,  CLIENTE.Nombre as NOMBRE, CLIENTE.Dirección as DIRECCION                     
, CLIENTE.[Código Postal] as [C.POS], CLIENTE.Población as POBLACION, ALBARAN_CABE.[Fecha Emisión],
                         ALBARAN_CABE.Ruta, dbo.OBTENER_FECHAS(PEDIDO_CABE.Observaciones) as [FECHA ENTREGA], COALESCE (ALBARAN_CABE.Observaciones, N'') AS OBSERVACIONES, ALBARAN_LIN.Artículo, ARTICULO.Descripción, ARTICULO.[Uds Venta], 
                         ARTICULO.[Factor(Kg/Ud)], ARTICULO.[Factor(Kg/Cj)], ARTICULO.UnidadesCaja, ARTICULO.CajasPalet,
						 case when  ARTICULO.[Uds Venta] = 'Kg' then SUM(ALBARAN_LIN.Cantidad) / Nullif( ARTICULO.[Factor(Kg/Cj)],0)
						 else (SUM(ALBARAN_LIN.Cantidad) /Nullif(ARTICULO.UnidadesCaja,0)) end as cajas,
						 (case when  ARTICULO.[Uds Venta] = 'Kg' then SUM(ALBARAN_LIN.Cantidad)  / Nullif( ARTICULO.[Factor(Kg/Cj)],0) else (SUM(ALBARAN_LIN.Cantidad) / Nullif(ARTICULO.UnidadesCaja,0)) end) /  nullif (ARTICULO.CajasPalet,0) as Palet,
						 case when  ARTICULO.[Uds Venta] = 'Kg' then SUM(ALBARAN_LIN.Cantidad)  else SUM(ALBARAN_LIN.Cantidad) * ARTICULO.[Factor(Kg/Ud)] end as peso
						 ,SUM(ALBARAN_LIN.Cantidad) as cantidad
						 ,PEDIDO_CABE.Observaciones as ped_obs
FROM            ALBARAN_CABE INNER JOIN
                         ALBARAN_LIN ON ALBARAN_CABE.Año = ALBARAN_LIN.Año AND ALBARAN_CABE.Empresa = ALBARAN_LIN.Empresa AND ALBARAN_CABE.Serie = ALBARAN_LIN.Serie AND 
                         ALBARAN_CABE.[Nº Albarán] = ALBARAN_LIN.[Nº Albarán] INNER JOIN
                         CLIENTE ON ALBARAN_CABE.[Código Cliente] = CLIENTE.Cliente INNER JOIN
                         PEDIDO_CABE ON ALBARAN_CABE.Empresa = PEDIDO_CABE.Empresa AND ALBARAN_LIN.[Nº Pedido] = PEDIDO_CABE.[Nº Pedido] AND 
                         PEDIDO_CABE.[C/P] = N'C' INNER JOIN
                         ARTICULO ON ALBARAN_LIN.Artículo = ARTICULO.Artículo
GROUP BY ALBARAN_CABE.[Código Cliente], ALBARAN_CABE.[Nº Albarán], CLIENTE.Nombre, CLIENTE.Dirección, CLIENTE.[Código Postal], CLIENTE.Población, ALBARAN_CABE.Ruta, PEDIDO_CABE.[Fecha Entrega], 
                         ALBARAN_CABE.Observaciones, ALBARAN_LIN.Artículo, ARTICULO.Descripción, ARTICULO.[Uds Venta], ARTICULO.[Factor(Kg/Ud)], ARTICULO.[Factor(Kg/Cj)], ARTICULO.UnidadesCaja, ARTICULO.CajasPalet,ALBARAN_CABE.[Fecha Emisión],PEDIDO_CABE.Observaciones,PEDIDO_CABE.[Nº Pedido] 
						 ) as t
						 where t.Ruta=" + agencia + @" and t.[Fecha Emisión]>= CAST(CURRENT_TIMESTAMP AS DATE)
						 group by  t.CLIENTE,t.[N.PEDIDO], t.NOMBRE, t.DIRECCION,t.[C.POS],t.POBLACION, t.[FECHA ENTREGA],t.OBSERVACIONES,t.ped_obs";

            return sql;
        }

        private void FilterToNthPage(DataTable SearchFilterValues, string SortExpression, string SortDirection, int PageIndex)
        {
            int PageSize = GridView1.PageSize;

            try
            {
                DataTable dt = GetSearchFilteredData("SearchForPagedData", "@SearchFilter", SearchFilterValues, consulta_sql(DropDownDesde.SelectedValue), SortExpression, SortDirection, PageIndex, PageSize);
                GridView1.CurrentSearchPageNo = PageIndex;
                if (dt.Rows.Count > 0)
                {
                    GridView1.TotalSearchRecords = (int)dt.Rows[0]["TotalRows"];
                }
                else
                {
                    GridView1.TotalSearchRecords = 0;
                }
                GridView1.DataSource = dt;

                GridView1.DataBind();
            }
            catch (System.Exception ex)
            {
                //  strMsg = ex.Message;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // lblMsg.Text = strMsg;
            }
        }
        public DataTable GetSearchFilteredData(string StoreProcedureName, string SQLTableVariableName, DataTable SearchFilterValues, string Select, string SortExpression, string SortDirection, int PageIndex, int PageSize)
        {

            // Assumes connection is an open SqlConnection object.
            SqlConnection conn = new SqlConnection(@"Data Source=192.168.1.195\sqlserver2008;Initial Catalog=QC600;Persist Security Info=True;User ID=dso;Password=dsodsodso");
            using (conn)
            {
                //Open the connection
                conn.Open();

                // Configure the SqlCommand and SqlParameter.
                SqlCommand cmd = new SqlCommand("SearchForPagedData", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                //creating parameter and assign values
                //Create SQL Table parameter and assign datatable to it @SqlSelect
                SqlParameter tvpParam = cmd.Parameters.AddWithValue(SQLTableVariableName, SearchFilterValues);
                tvpParam.SqlDbType = SqlDbType.Structured;

                SqlParameter stpPageIndex = cmd.Parameters.AddWithValue("@PageIndex", PageIndex);
                stpPageIndex.SqlDbType = SqlDbType.Int;

                SqlParameter stpPageSize = cmd.Parameters.AddWithValue("@PageSize", PageSize);
                stpPageSize.SqlDbType = SqlDbType.Int;

                SqlParameter stpSortExpression = cmd.Parameters.AddWithValue("@SortExpression", SortExpression);
                stpSortExpression.SqlDbType = SqlDbType.VarChar;

                SqlParameter stpSortDirection = cmd.Parameters.AddWithValue("@SortDirection", SortDirection);
                stpSortDirection.SqlDbType = SqlDbType.VarChar;

                SqlParameter stpselect = cmd.Parameters.AddWithValue("@SqlSelect", Select);
                stpselect.SqlDbType = SqlDbType.VarChar;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);

                //Close connection
                conn.Close();
                return ds.Tables[0];

            }
        }

        protected void BtBuscar_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        protected void GridView1_FilterButtonClick(object sender, SearchGridEventArgs e)
        {
            FilterToNthPage(e.SearchFilterValues, GridView1.CurrentSortExpression, GridView1.CurrentSortDirection, 1);
        }

        protected void GridView1_NavigationButtonClick(object sender, NavigationButtonEventArgs e)
        {
            if (e.NavigationButtonsType == NavigationButtonsTypes.GoFirst)
            {
                FilterToNthPage(GridView1.SearchFilters,
                    GridView1.CurrentSortExpression,
                    GridView1.CurrentSortDirection, 1);
            }
            else if (e.NavigationButtonsType == NavigationButtonsTypes.GoLast)
            {
                FilterToNthPage(GridView1.SearchFilters,
                    GridView1.CurrentSortExpression,
                    GridView1.CurrentSortDirection,
                    GridView1.TotalSearchPages);
            }
            else if (e.NavigationButtonsType == NavigationButtonsTypes.GoNext)
            {
                if (GridView1.CurrentSearchPageNo < GridView1.TotalSearchPages)
                {
                    FilterToNthPage(GridView1.SearchFilters,
                        GridView1.CurrentSortExpression,
                        GridView1.CurrentSortDirection,
                        (int)GridView1.CurrentSearchPageNo + 1);
                }
            }
            else if (e.NavigationButtonsType == NavigationButtonsTypes.GoPrevious)
            {
                if (GridView1.CurrentSearchPageNo > 1)
                {
                    FilterToNthPage(GridView1.SearchFilters,
                        GridView1.CurrentSortExpression,
                        GridView1.CurrentSortDirection,
                        (int)GridView1.CurrentSearchPageNo - 1);
                }
            }
            else if (e.NavigationButtonsType == NavigationButtonsTypes.GoToPage)
            {
                FilterToNthPage(GridView1.SearchFilters,
                    GridView1.CurrentSortExpression,
                    GridView1.CurrentSortDirection,
                    (int)e.PageIndex);
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            FilterToNthPage(GridView1.SearchFilters, GridView1.CurrentSortExpression, GridView1.CurrentSortDirection, (int)GridView1.CurrentSearchPageNo);
        }

        protected void GridView1_CancelFilterButtonClick(object sender, SearchGridEventArgs e)
        {
            FilterToNthPage(e.SearchFilterValues, GridView1.CurrentSortExpression, GridView1.CurrentSortDirection, 1);
        }

        protected void GridView1_ExcelButtonClick(object sender, SearchGridEventArgs e)
        {

            string filename = "Export.xlsx";
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            Quality data = new Quality();


            DataTable dt = new DataTable();
            dt = data.Sql_Datatable(consulta_sql(DropDownDesde.SelectedValue));

            System.IO.MemoryStream ms = DataTableToExcelXlsx(dt, "Sheet1");
            ms.WriteTo(HttpContext.Current.Response.OutputStream);
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            HttpContext.Current.Response.StatusCode = 200;
            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Write(tw.ToString());
            HttpContext.Current.Response.End();
        }
        public static System.IO.MemoryStream DataTableToExcelXlsx(DataTable table, string sheetName)
        {
            System.IO.MemoryStream Result = new System.IO.MemoryStream();
            ExcelPackage pack = new ExcelPackage();
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(sheetName);
            int col = 1;
            int row = 1;
            foreach (DataColumn column in table.Columns)
            {
                ws.Cells[row, col].Value = column.ColumnName.ToString();
                col++;
            }
            col = 1;
            row = 2;
            foreach (DataRow rw in table.Rows)
            {
                foreach (DataColumn cl in table.Columns)
                {
                    if (rw[cl.ColumnName] != DBNull.Value)
                        ws.Cells[row, col].Value = rw[cl.ColumnName].ToString();
                    col++;
                }
                row++;
                col = 1;
            }
            pack.SaveAs(Result);
            return Result;
        }

        protected void GridView1_PageSizeChanged(object sender, PageSizeChangeEventArgs e)
        {
            GridView1.PageSize = e.NewPageSize;
            GridView1.PageIndex = 0;
            FilterToNthPage(GridView1.SearchFilters, GridView1.CurrentSortExpression, GridView1.CurrentSortDirection, (int)GridView1.CurrentSearchPageNo);


        }
    }

}
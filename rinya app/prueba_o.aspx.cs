using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
//using Oracle.DataAccess.Client;
using Oracle.ManagedDataAccess.Client;
using Utiles;

namespace rinya_app
{
    public partial class prueba_o : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                datos = Session["datos"] as DataTable;
                DataView source = new DataView(datos);
                GridView1.DataSource = source;
                GridView1.DataBind();
            }
        }
        public DataTable Sql_Datatable(string sql)
        {
            string Con_E = @"Data Source=(DESCRIPTION =    (ADDRESS_LIST =      (ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.1.58)(PORT = 1521))    )    (CONNECT_DATA = (SID = dbgrinya))  );User Id=grinya_expert;Password=datadec;";
            DataTable dt = new DataTable("Lineas");

            using (OracleConnection cnn2 = new OracleConnection(Con_E))
            {
                try
                {
                    cnn2.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cnn2;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                    dt.Load(dr);

                    cnn2.Dispose();
                    return dt;
                }
                catch (Exception)
                {
                    throw;

                }
            }

        }
        private bool esnumero(string cadena)
        {

            int i = 0;
            string s = cadena;
            return int.TryParse(s, out i);

        }
        Expert con = new Expert();
        private DataTable datos { get; set; }
        protected void Button1_Click(object sender, EventArgs e)
        {
            GridView1.Visible = false;
            string articulo = TextBox1.Text;
            if (articulo.Length > 0 & esnumero(articulo))
            {
               
                
                string sql = @"SELECT PRO_CODIGO_PRODUCTO P_CODARTICULO,
       PRO_DESCRIPCION_CORTA P_DSCR,
       PRO_CLAVE_ESTAD_PRINCIPAL P_FAMILIA
 from PR_PRODUCTO where  PRO_CODIGO_PRODUCTO='" + articulo + @"' and PRO_EMPRESA=1";
               
                datos = con.Sql_Datatable(sql);
                DataView source = new DataView(datos);
                GridView1.DataSource = source;
                GridView1.DataBind();
                GridView1.Visible = true;
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            GridView1.Visible = false;
            string sql = @"SELECT PRO_CODIGO_PRODUCTO P_CODARTICULO,
       PRO_DESCRIPCION_CORTA P_DSCR,
       PRO_CLAVE_ESTAD_PRINCIPAL P_FAMILIA
 from PR_PRODUCTO where PRO_EMPRESA=1";
            datos = con.Sql_Datatable(sql);

            Session.Add("datos", datos);
            DataView source = new DataView(datos);
            GridView1.DataSource = source;
            GridView1.DataBind();
            GridView1.Visible = true;
            Button3.Enabled = true;
        }
        
        protected void Button3_Click(object sender, EventArgs e)
        {
            Export sacardatos = new Export();
            string sql = @"SELECT PRO_CODIGO_PRODUCTO P_CODARTICULO,
       PRO_DESCRIPCION_CORTA P_DSCR,
       PRO_CLAVE_ESTAD_PRINCIPAL P_FAMILIA
 from PR_PRODUCTO where PRO_EMPRESA=1";
            //datos = con.Sql_Datatable(sql);
            try { 
            if (datos!=null)
            sacardatos.ToExcelfile(@"C:\temp\prueba.xls", datos);
            } catch(Exception ex)
            {
              
            Response.Write("<script type=\"text/javascript\">alert('" + ex.Message.Replace("'", "") + "');</script>");
            }
            Button3.Enabled = false;
        }
    }
}
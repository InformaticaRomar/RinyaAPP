using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Data;

namespace rinya_app
{
    public partial class prueba : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private bool esnumero(string cadena)
        {

            int i = 0;
            string s = cadena;
            return int.TryParse(s, out i);

        }
        /*
        private DataTable Sql_Datatable(string sql)
        {
            DataTable dt = new DataTable("Lineas");
            try
            {
                string Con_Q = "Data Source=192.168.1.195\\sqlserver2008;Initial Catalog=QC600;Persist Security In" +
            "fo=True;User ID=dso;Password=dsodsodso";
                using (SqlConnection cnn = new SqlConnection(Con_Q))
                {


                    cnn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sql, cnn);
                    da.Fill(dt);
                    cnn.Close();
                    return dt;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        */
        protected void Button1_Click(object sender, EventArgs e)
        {
   //         Table1.Visible = false;
            string zona = TextBox1.Text;
            if (zona.Length > 0 & esnumero(zona))
            {


                string sql = @"SELECT TOP 1000 [Zona],[Nombre] FROM [QC600].[dbo].[ZONA] where [Zona]=" + zona;

                DataView source = null;// new DataView(Sql_Datatable(sql));
                GridView1.DataSource = source;
                GridView1.DataBind();
                GridView1.Visible = true;
               // Table1.DataSource = source;
                //Table1.DataBind();
       //         Table1.Visible = false;

            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            //Table1.Visible = false;
            string zona = TextBox1.Text;
           // string sql = @"SELECT TOP 1000 [Zona],[Nombre] FROM [QC600].[dbo].[ZONA]";

            //DataView source = new DataView(Sql_Datatable(sql));
           // GridView1.DataSource = source;
            //GridView1.DataBind();
            //GridView1.Visible = true;
            //SoftToRuleGridView1.DataSource = source;
            //SoftToRuleGridView1.DataBind();
            //SoftToRuleGridView1.Visible = true;
            //  Table1.DataSource = source;
            // Table1.DataBind();
            //Table1.Visible = true;
        }
    }
}

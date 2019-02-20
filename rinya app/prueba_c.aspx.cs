using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using Utiles;

namespace rinya_app
{
    public partial class prueba_c : System.Web.UI.Page
    {
        System.Collections.ArrayList prueba { get; set; }
        PlaceHolder PlaceHolder1 = new PlaceHolder();
        protected void Page_Load(object sender, EventArgs e)
        {
         /*   PlaceHolder1.Controls.Add(DataGridAutoFilter1);
            //PlaceHolder1.
            // this.RegisterRequiresRaiseEvent(DataGridAutoFilter1);
            if (Session["DataGridAutoFilter1"] as DataGridAutoFilter.DataGridAutoFilter != null)
            {
                DataGridAutoFilter1 = Session["DataGridAutoFilter1"] as DataGridAutoFilter.DataGridAutoFilter;
                prueba = DataGridAutoFilter1.list;
               // DataGridAutoFilter1.DataBind();
                pr(prueba);
            }
                if (!IsPostBack)
            {
                DataGridAutoFilter1.DataBind();
            }*/
            
         /*   if (IsPostBack)
            {
                if(Session["DataGridAutoFilter1"] as DataGridAutoFilter.DataGridAutoFilter != null) { 
                DataGridAutoFilter1 = Session["DataGridAutoFilter1"] as DataGridAutoFilter.DataGridAutoFilter;
                DataGridAutoFilter1.DataBind();
                    int a = 0;
                    a = 2;
                }*/
                /*
                                datos = Session["datos"] as DataTable;

                                DataView source = new DataView(datos);
                                DataSet ds = new DataSet(); ds.Tables.Add(datos);
                                DataGridAutoFilter1.DataSource = ds;
                                if (datos != null)
                                    DataGridAutoFilter1.DataBind();*/
          //  }
        }
        DataTable datos { get; set; }
        /*private void pr(System.Collections.ArrayList z)
        {
           
            // for each row;
            for (int i = 0; i < DataGridAutoFilter1.Items.Count; i++)
            {
                // for each column;
                for (int j = 0; j < DataGridAutoFilter1.Items[i].Cells.Count; j++)
                {
                    System.Web.UI.HtmlControls.HtmlSelect select =(System.Web.UI.HtmlControls.HtmlSelect)z[j];
                                             //  HtmlSelect select2 = Items[i].Cells[j];//(HtmlSelect)
                if (select.SelectedIndex > 0)      
                    {
                        // hide rows with a not selected value;
                        if (DataGridAutoFilter1.Items[i].Cells[j].Text != select.Items[select.SelectedIndex].Text)
                            DataGridAutoFilter1.Items[i].Visible = false;
                    }
                }
            }
        }
        */
        protected void Button1_Click(object sender, EventArgs e)
        {
            //Table1.Visible = false;
            string zona = TextBox1.Text;
            string sql = @"SELECT TOP 1000 [Zona],[Nombre] FROM [QC600].[dbo].[ZONA]";
            Quality con = new Quality();
            datos = con.Sql_Datatable(sql);
            DataView source = new DataView(datos);
            // GridView1.DataSource = source;
            //GridView1.DataBind();
            //GridView1.Visible = true;
           // datos = Session["datos"] as DataTable;

           // DataView source = new DataView(datos);
            DataSet ds = new DataSet(); ds.Tables.Add(datos);
            //SoftToRuleGridView1.DataSource = ds;
            // DataGridAutoFilter1.DataSource = source;
            //SoftToRuleGridView1.DataBind();
            //Session["DataGridAutoFilter1"] = SoftToRuleGridView1;
            //SoftToRuleGridView1.Visible = true;
        }

       
      
        protected void DataGridAutoFilter1_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            string valor = e.Item.FindControl("list").ToString();
        }

      
    }
}
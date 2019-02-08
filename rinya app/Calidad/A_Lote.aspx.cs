using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utiles;
using System.Data;


namespace rinya_app.Calidad
{
    public partial class A_Lote : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string sql = "select distinct Articulo,  convert(varchar,Articulo) +' | ' + ARTICULO.Descripción as Descripcion  FROM [QC600].[dbo].[CARACTERISTICAS_ARTICULO] inner join ARTICULO on ARTICULO.Artículo=[CARACTERISTICAS_ARTICULO].[Articulo]";
                Quality con = new Quality();
              
                DataTable datos= con.Sql_Datatable(sql);
                DropDown_Articulo.DataSource= datos;
                DropDown_Articulo.DataTextField = "Descripcion";
                DropDown_Articulo.DataValueField = "Articulo";

                // Bind the data to the control.
                DropDown_Articulo.DataBind();

                // Set the default selected item, if desired.
               // DropDown_Articulo.SelectedIndex = 0;
                sql = "select [ID_Estado] ,[Estado] FROM[QC600].[dbo].[Estado]";
                DataTable datos_2 = con.Sql_Datatable(sql);
                Estados.DataSource = datos_2;
                Estados.DataTextField = "Estado";
                Estados.DataValueField = "ID_Estado";
                Estados.DataBind();
                Estados.SelectedIndex = 0;
            }
        }

        protected void BtBuscar_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Si")
            { 
                string articulo = DropDown_Articulo.Text;
            string estado = Estados.SelectedValue;
            string lote = Lote_textBoxs.Text;
            if (articulo.Length > 0 && estado.Length > 0 && lote.Length > 0)
            {
                    string sql = "exec [PROPAGAR_ESTADO_LOTE]  "+estado+", "+ articulo +",'"+ lote+"'";
                    Quality con = new Quality();
                    con.sql_update(sql);
                   
                    sql = @"insert into [QC600].[dbo].[CONTROL_PROPAGAR_LOTE] ([FECHA] ,[ARTICULO],[LOTE_INTERNO],[ACCION],[OBSERVACIONES]) values ( getdate(),'" + articulo + @",'" + lote + @"','PROPAGAR ESTADO LOTE', 'Propago el estado '"+estado+@" a todos el lote)";
                    con.sql_update(sql);
                    //Actualizo
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Actualizo Articulo " + DropDown_Articulo.Text + " con lote "+lote +" al Estado " + Estados.SelectedItem.Text + "')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Tienes que rellenar todos los campos para actualizar el estado')", true);
            }
            }

        }
    }
}
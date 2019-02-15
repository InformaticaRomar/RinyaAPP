using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace rinya_app.Calidad.Otros_Controles
{
    public partial class Informe_Articulo_lote : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            String Desde = TextBox_Desde.Text.ToString();
            String Hasta = TextBox_hasta.Text.ToString();
            String turno = DropDownList1.SelectedValue.ToString();
            Response.Redirect("Informe_Articulo_lote_r.aspx?Desde=" + Desde + "&Hasta=" + Hasta + "&turno="+ turno);

        }
    }
}
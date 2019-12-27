using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace rinya_app.Calidad
{
    public partial class validacion_matricula : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtBuscar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Organoleptico_busqueda.aspx?sscc="+ SSCC_textBoxs.Text);
        }
    }
}
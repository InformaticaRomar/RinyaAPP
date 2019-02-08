using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace rinya_app.Tunel
{
    public partial class control_temperatura_fechas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Btexport_Click(object sender, EventArgs e)
        {
            
            String Desde = datepicker1.Value.ToString();
            String Hasta = datepicker2.Value.ToString();
            Response.Redirect("control_temperatura_fechas_r.aspx?Desde=" + Desde + "&Hasta=" + Hasta);
           
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace rinya_app.Calidad.Trazabilidad
{
    public partial class trazabilidad : System.Web.UI.Page
    {
        #region "Propiedades"

        // Estas propiedades se encargarán de encapsular los controles          
        //para que puedan ser accedidos desde otras páginas.
        /*
        public ListBox MiLista
        {
            get
            {
                return this.ListBox1;
            }
        }
        */
        public TextBox Lote_textBox
        {
            get
            {
                return this.Lote_textBoxs;
            }
        }
        public TextBox Palet_textBox
        {
            get
            {
                return this.Palet_text;
            }
        }
        

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        

        protected void BtBuscar_Click2(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Server.Transfer("traza.aspx");//PostBackUrl="~/Calidad/trazabilidad/traza.aspx" 
            }
        }
    }
}
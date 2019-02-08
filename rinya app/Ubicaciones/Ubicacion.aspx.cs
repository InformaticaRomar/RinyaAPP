using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Security;
using System.Data;
using Utiles;

namespace rinya_app.Ubicaciones
{
    public partial class Ubicacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.cmdUBICA.ServerClick += new System.EventHandler(this.cmdUBICA_serverClick);
            txtMatricula.Focus();

        }
        private bool GrabarMatriculaubicacion(string matricula, string ubicacion) {

            string ubica = string.Empty;
            string sql = string.Empty;
            bool exito = false;
            
            Quality quality = new Quality();
            try
            {
                sql = "select UBICACION FROM CONTROL_UBICACION where SSCC='" + matricula + "'";
                ubica = quality.sql_string(sql);
                sql = string.Empty;
                if (ubica.Length > 0)
                {
                    if (!ubica.Equals(ubicacion))
                        sql = "update CONTROL_UBICACION set  UBICACION=" + ubicacion + ", FECHA=getdate() where SSCC='" + matricula + "'";
                }
                else
                {
                    sql = "INSERT INTO CONTROL_UBICACION (UBICACION,SSCC,FECHA) VALUES (" + ubicacion + ",'" + matricula + "',getdate())";
                }
                if (sql.Length > 0)
                    quality.sql_update(sql);
                exito = true;
            }
            catch { exito = false; }
        
            return exito;
        }
        private void error()
        {
            lblMsg.Text = "Error en la matricula: " + txtMatricula.Text;
            txtMatricula.Text = "";
            txtUbicacion.Text = "";
            txtMatricula.Focus();
        }
        private void cmdUBICA_serverClick(object sender, System.EventArgs e)
        {
            if (txtMatricula.Text.Length > 0 && txtUbicacion.Text.Length > 0)
            {
                long ubicac = 0;
                long matri = 0;
                long.TryParse(txtMatricula.Text, out matri);
                long.TryParse(txtUbicacion.Text, out ubicac);
                int Lmatri = matri.ToString().Length;

                int Lubica = ubicac.ToString().Length;
                if (matri > 0 && Lmatri > 17 && Lmatri < 19 && Lubica < 11 && ubicac > 0)
                {
                    txtMatricula.Text = matri.ToString();

                    if (GrabarMatriculaubicacion(txtMatricula.Text, txtUbicacion.Text))
                    {
                        lblMsg.Text = "La Matricula " + txtMatricula.Text + " , se ha ubicado correctamente.";
                       // Page.ClientScript.RegisterStartupScript(GetType(), "hwa", "tempAlert('" + lblMsg.Text+ "', 5000);", true);
                        Page.ClientScript.RegisterStartupScript(GetType(), "hwa", "dialogo();", true);
                        txtMatricula.Text = "";
                        txtUbicacion.Text = "";
                        txtMatricula.Focus();
                    }
                    else
                     error();
                    
                }
                else
                    error();
                
            }
            else if (txtMatricula.Text.Length > 0 && txtUbicacion.Text.Length == 0)
            {
                txtUbicacion.Focus();
            }

        }
        protected void Guardar_click(object sender, EventArgs e)
        {
            this.cmdUBICA_serverClick(sender, e);

        }
    }
}
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace rinya_app.Calidad.Otros_Controles
{
    public partial class Informe_Articulo_lote_r : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)

            {
                WebService_SILO servicio_fechas = new WebService_SILO();
                string desde = Request.QueryString["Desde"];
                string hasta = Request.QueryString["Hasta"];
                string turno = Request.QueryString["turno"];
                if (desde != null && hasta != null && turno == "0")
                {
                    DataTable ddt = servicio_fechas.dt_fechas_articulo_lote(desde, hasta);
                    Generatereport(ddt);
                }
                else {
                    DataTable ddt = servicio_fechas.dt_fechas_articulo_lote(desde, hasta, turno);
                    Generatereport(ddt);
                }
            }
        }
        private void Generatereport(DataTable dt)

        {

            ReportViewer1.SizeToReportContent = true;



            ReportViewer1.LocalReport.DataSources.Clear();

            ReportDataSource _rsource = new ReportDataSource("DataSet2", dt);

            ReportViewer1.LocalReport.DataSources.Add(_rsource);

            ReportViewer1.LocalReport.Refresh();

        }
    }
}
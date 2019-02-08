using Microsoft.AspNet.Identity;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utiles;

namespace rinya_app.Tunel
{
    public partial class control_tunel_fechas_r : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)

            {
                Webservice_Tunel servicio_fechas = new Webservice_Tunel();
                string desde = Request.QueryString["Desde"];
                string hasta = Request.QueryString["Hasta"];
                if (desde != null && hasta != null)
                {
                    DataTable ddt = servicio_fechas.dt_fechas_tunel(desde, hasta);
                    Generatereport(ddt);
                }
            }

        }
        private void Generatereport(DataTable dt)

        {

            ReportViewer1.SizeToReportContent = true;



            ReportViewer1.LocalReport.DataSources.Clear();

            ReportDataSource _rsource = new ReportDataSource("DataSet1", dt);

            ReportViewer1.LocalReport.DataSources.Add(_rsource);

            ReportViewer1.LocalReport.Refresh();

        }
    }
}
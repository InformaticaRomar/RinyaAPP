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
    public partial class control_tunel_turno : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)

            {
                Webservice_Tunel servicio_turno = new Webservice_Tunel();

                string responsable = Context.User.Identity.GetUserName();
                if (responsable.Length > 0)
                {
                    //setResponsable(responsable);
                }
                else
                {
                    responsable = "No login";
                }
                DataTable ddt = servicio_turno.dt_Turno_entrada();

                // ddt = Getdata().AsEnumerable().Where(p => p["id"].ToString() == "1").CopyToDataTable();

                Generatereport(ddt);
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
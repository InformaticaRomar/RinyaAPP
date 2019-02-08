using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace rinya_app.Tunel
{
    public partial class control_tunel_4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)

            {
                Webservice_Tunel servicio_turno = new Webservice_Tunel();

                DataTable ddt = servicio_turno.dt_Turno_entrada_tunel4();

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
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
    public partial class control_temperatura_turno : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)

            {
                Webservice_Tunel servicio_turno = new Webservice_Tunel();

                string responsable =Context.User.Identity.GetUserName();
                if (responsable.Length > 0)
                {
                    //setResponsable(responsable);
                }
                else
                {
                    responsable = "No login";
                }
                DataTable ddt = servicio_turno.dt_Turno(responsable);

                // ddt = Getdata().AsEnumerable().Where(p => p["id"].ToString() == "1").CopyToDataTable();

                Generatereport(ddt);
            }

        }
        private void setResponsable(string responsable) {
            string sql = @"update dbControlTemperatura set dbControlTemperatura.Responsable='"+responsable+@"' where
datediff (hour,dbControlTemperatura.Fecha, getdate()) between 0 and 8 
						and

						 case when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '14:00:00', 108)  then 1 
						 when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '22:00:00', 108)  then 2
						   else 3 end
						   =
						   case when CONVERT(time,getdate(), 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '15:00:00', 108)  then 1 
						 when CONVERT(time, getdate(), 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '23:00:00', 108)  then 2
						   else 3 end";
            Quality con = new Quality();
            con.sql_update(sql);
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
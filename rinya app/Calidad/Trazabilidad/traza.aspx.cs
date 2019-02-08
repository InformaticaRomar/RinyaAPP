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


namespace rinya_app.Calidad.Trazabilidad
{
    public partial class traza : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
            if (Page.PreviousPage != null)
            {
                TextBox SourceTextBox =
                    (TextBox)Page.PreviousPage.FindControl("Lote_textBoxs");
                if (SourceTextBox != null)
                {
                    Label1.Text = SourceTextBox.Text;
                }
            }*/
            if (!IsPostBack) { 
                if (PreviousPage != null)
            {
                // PreviousPage.Lote_textBoxs.Text;
                // Label1.Text = PreviousPage.Lote_textBox.Text;
                // DataTable ddt = servicio_turno.dt_Turno(responsable);
                WebService_traza servicio_traza = new WebService_traza();
                DataTable ddt = servicio_traza.dt_Datos_traza("2016", "1", PreviousPage.Lote_textBox.Text);
                Generatereport(ddt);
            }
            else {
                WebService_traza servicio_traza = new WebService_traza();
                DataTable ddt = servicio_traza.dt_Datos_traza("2016", "1","245");
                Generatereport(ddt);
                //Label1.Text = "error";
            }
            }
        }
        private void Generatereport(DataTable dt)

        {

            ReportViewer1.SizeToReportContent = true;


            //DataSet1
            //ReportViewer1.Reset();
            ReportDataSource _rsource = new ReportDataSource();
            _rsource.Name = "DataSet1";
            _rsource.Value = dt;
            /*
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(_rsource);
            ReportViewer1.LocalReport.ReportPath = @"Calidad\Trazabilidad\Report1.rdlc";
            ReportViewer1.LocalReport.Refresh();
            ReportViewer1.LocalReport.DataSources.Clear();*/

            /*ReportDataSource _rsource = new ReportDataSource("DataSet1", dt);*/
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(_rsource);

            ReportViewer1.LocalReport.Refresh();

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using Utiles;
using Microsoft.Reporting.WebForms;

namespace rinya_app.Calidad.Otros_Controles
{
    public partial class Contol_lote_articulo : System.Web.UI.Page
    {
        private DataSet m_dataSet;
        private MemoryStream m_rdl;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)

            {
                Opendatabase();
            }
        }
        private List<string> GetAvailableFields()
        {
            DataTable dataTable = m_dataSet.Tables[0];
            List<string> availableFields = new List<string>();
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                availableFields.Add(dataTable.Columns[i].ColumnName);
            }
            return availableFields;
        }
        private MemoryStream GenerateRdl(List<string> allFields, List<string> selectedFields)
        {
            MemoryStream ms = new MemoryStream();

            RdlGenerator gen = new RdlGenerator();
            gen.AllFields = allFields;
            gen.SelectedFields = selectedFields;
            gen.WriteXml(ms);
            ms.Position = 0;
            return ms;
        }
        private void DumpRdl(MemoryStream rdl)
        {
#if DEBUG_RDLC
            using (FileStream fs = new FileStream(@"c:\test.rdlc", FileMode.Create))
            {
                rdl.WriteTo(fs);
            }
#endif
        }
        private void ShowReport()
        {
            ReportViewer1.SizeToReportContent = true;
            this.ReportViewer1.Reset();
            this.ReportViewer1.LocalReport.LoadReportDefinition(m_rdl);
            this.ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("MyData", m_dataSet.Tables[0]));
            this.ReportViewer1.LocalReport.Refresh();
        }
        private void Opendatabase()
        {
            try
            {
                m_dataSet = new DataSet();
                Quality con = new Quality();
                DataTable datos = con.Sql_Datatable("EXEC [dbo].[GET_DATOS_ORGANOLEPTICO] N'154071', 9720");
                m_dataSet.Tables.Add(datos);
                List<string> allFields = GetAvailableFields();
                List<string> selectedFields = GetAvailableFields();
                if (m_rdl != null)
                    m_rdl.Dispose();
                m_rdl = GenerateRdl(allFields, selectedFields);
                DumpRdl(m_rdl);

                 ShowReport();
                    

            }
            catch (Exception ex)
            {

            }
        }
    }
}
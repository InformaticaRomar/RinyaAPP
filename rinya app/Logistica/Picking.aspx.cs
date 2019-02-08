using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceProcess;

namespace rinya_app.Logistica
{
    public partial class Picking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
//            Button1.Enabled = true;
        }

        private void RestartTelnetService(string serviceName)
        {
            ServiceControllerPermission scp = new ServiceControllerPermission(ServiceControllerPermissionAccess.Control, Environment.MachineName, serviceName);
            scp.Assert();
            ServiceController serviceController = new ServiceController(serviceName);
            try
            {
                
                if ((serviceController.Status.Equals(ServiceControllerStatus.Running)) || (serviceController.Status.Equals(ServiceControllerStatus.StartPending)))
                {
                    serviceController.Stop();
                    serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "Parado");
                }
                if ((serviceController.Status.Equals(ServiceControllerStatus.Stopped)) || (serviceController.Status.Equals(ServiceControllerStatus.StopPending)))
                {
                    
                    serviceController.Start();
                    serviceController.WaitForStatus(ServiceControllerStatus.Running);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "Iniciado");
                }
            }
            catch (Exception e)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", e.Message);
               
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
            //RestartTelnetService("\"KpyM Telnet SSH Server v1.19c\"");
            System.Diagnostics.ProcessStartInfo psi =

new System.Diagnostics.ProcessStartInfo(@"C:\inetpub\wwwroot\script\restart.bat");

            // System.Diagnostics.Process.Start(@"C:\APPEYRON\restart.cmd");

            psi.RedirectStandardOutput = true;

            psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            psi.UseShellExecute = false;

            System.Diagnostics.Process listFiles;

            listFiles = System.Diagnostics.Process.Start(psi);

            System.IO.StreamReader myOutput = listFiles.StandardOutput;

            listFiles.WaitForExit(2000);

            if (listFiles.HasExited)

            {

                string output = myOutput.ReadToEnd();

                // TextBox1.Text = output;
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", output);

            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", ex.Message);

        }
    /*     ServiceController[] controllers = ServiceController.GetServices();
         GridView2.DataSource = controllers;
         GridView2.DataBind();*/

    
        }
    }
}   
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Utiles;
using System.Data;

namespace rinya_app.Calidad.Maestros
{
    /// <summary>
    /// Descripción breve de WebService_Organo_Comite
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]

    public class WebService_Organo_Comite : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetEstados()
        {
            string sql = "select [ID_Estado] ,[Estado] FROM[QC600].[dbo].[Estado]";
            Quality con = new Quality();
            DataTable datos_estados = con.Sql_Datatable(sql);
            return Newtonsoft.Json.JsonConvert.SerializeObject(datos_estados, Newtonsoft.Json.Formatting.Indented);
        }
        [WebMethod]
        public string GetMotivoMerma()
        {
            string sql = "select [NO_CONFORMIDADES].Cod_NC, [Nc_des] from [NO_CONFORMIDADES] order by [NO_CONFORMIDADES].Cod_NC asc";
            Quality con = new Quality();
            DataTable datos_estados = con.Sql_Datatable(sql);
            return Newtonsoft.Json.JsonConvert.SerializeObject(datos_estados, Newtonsoft.Json.Formatting.Indented);
        }
        [WebMethod(EnableSession = true)]
        public string GetCaracteristicas_articulo(Organoleptico datos)
        {
            string sql = @"SELECT  [CARACTERISTICAS_ARTICULO].[Caracteristica],
[Organolectico_Carac].Caracteristica + (case when [Organolectico_Carac].Tipo_Dato =1 then 
' ( ' + cast([V_Min] as varchar) +' | ' + cast( [V_Max] as varchar) +' ):' 
else (case when [V_Max]=0 then ' ( Incorrecto ) ' else ' ( Correcto ) ' end) end)
+case when [CARACTERISTICAS_ARTICULO].Obligatorio = 1 then '*' else '' end as lbl ,
 [Organolectico_Carac].Tipo_Dato FROM [QC600].[dbo].[CARACTERISTICAS_ARTICULO] inner join[QC600].[dbo].[Organolectico_Carac] on [Cod_Caracteristica]= [CARACTERISTICAS_ARTICULO].[Caracteristica] where Articulo = " + datos.ARTICULO;
            Quality con = new Quality();
            DataTable datos_estados = con.Sql_Datatable(sql);
            return Newtonsoft.Json.JsonConvert.SerializeObject(datos_estados, Newtonsoft.Json.Formatting.Indented);
        }
        private string get_parametters(object myData, string param)
        {
            string valor = string.Empty;
            Array a = (Array)myData;
            foreach (Dictionary<string, object> b in a)
            {
                string[] arr = { "sEcho" };
                string pair = b.SingleOrDefault(p => p.Key == "name").Value.ToString();

                if (pair == param)
                {
                    valor = b.SingleOrDefault(p => p.Key == "value").Value.ToString();
                    return valor;

                }
            }
            return valor;
        }
        [WebMethod(EnableSession = true)]
        public string GetCaracteristicas()
        {
            string sql = "select [Cod_Caracteristica] ,  convert(varchar,[Cod_Caracteristica]) +' | ' + [Caracteristica] as Caracteristica  FROM [QC600].[dbo].[Organolectico_Carac]";
            Quality con = new Quality();
            DataTable datos_estados = con.Sql_Datatable(sql);
            return Newtonsoft.Json.JsonConvert.SerializeObject(datos_estados, Newtonsoft.Json.Formatting.Indented);
        }

        [WebMethod(EnableSession = true)]
        public string GetArticulos()
        {
            string sql = "select distinct Articulo,  convert(varchar,Articulo) +' | ' + ARTICULO.Descripción as Descripcion  FROM [QC600].[dbo].[CARACTERISTICAS_ARTICULO] inner join ARTICULO on ARTICULO.Artículo=[CARACTERISTICAS_ARTICULO].[Articulo]";
            Quality con = new Quality();
            DataTable datos_estados = con.Sql_Datatable(sql);
            return Newtonsoft.Json.JsonConvert.SerializeObject(datos_estados, Newtonsoft.Json.Formatting.Indented);
        }

        [WebMethod(EnableSession = true)]
        public void New_Data(Caracteristicas datos)
        {
            Quality con = new Quality();

            string sql = "Insert INTO [CARACTERISTICAS_ARTICULO]  ([Articulo],[Caracteristica], [V_Min],[V_Max],[Obligatorio] ) values (" + datos.articulo + " , " + datos.Cod_Caracteristica
                + " , " + datos.V_Min + " , " + datos.V_Max + " , " + datos.Obligatorio + " )";
            con.sql_update(sql);

        }
        private bool cmp_datos_organoleptico(Organoleptico datos, Organoleptico datos_antiguo)
        {
            Quality con = new Quality();
            string sql = "";
            string usuario = "";
            if (User.Identity.IsAuthenticated)
                usuario = User.Identity.Name;
            double Datos_actuales = -999999;
            double Datos_anteriores = -999999;
            double.TryParse(datos.PH_CRUDO_AP, out Datos_actuales);
            double.TryParse(datos_antiguo.PH_CRUDO_AP, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",1," + datos.PH_CRUDO_AP + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.PH_CRUDO_DP, out Datos_actuales);
            double.TryParse(datos_antiguo.PH_CRUDO_DP, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",2," + datos.PH_CRUDO_DP + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.BRIX_CRUDO_AP, out Datos_actuales);
            double.TryParse(datos_antiguo.BRIX_CRUDO_AP, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",3," + datos.BRIX_CRUDO_AP + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.BRIX_CRUDO_DP, out Datos_actuales);
            double.TryParse(datos_antiguo.BRIX_CRUDO_DP, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",4," + datos.BRIX_CRUDO_DP + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos._HUMEDAD, out Datos_actuales);
            double.TryParse(datos_antiguo._HUMEDAD, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",5," + datos._HUMEDAD + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos._ES, out Datos_actuales);
            double.TryParse(datos_antiguo._ES, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",6," + datos._ES + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.HC, out Datos_actuales);
            double.TryParse(datos_antiguo.HC, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",7," + datos.HC + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.SACAROSA, out Datos_actuales);
            double.TryParse(datos_antiguo.SACAROSA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",8," + datos.SACAROSA + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.GRASA, out Datos_actuales);
            double.TryParse(datos_antiguo.GRASA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",9," + datos.GRASA + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.PROTEINA, out Datos_actuales);
            double.TryParse(datos_antiguo.PROTEINA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",10," + datos.PROTEINA + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.LACTOSA, out Datos_actuales);
            double.TryParse(datos_antiguo.LACTOSA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",11," + datos.LACTOSA + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.TEMPERATURA, out Datos_actuales);
            double.TryParse(datos_antiguo.TEMPERATURA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",12," + datos.TEMPERATURA + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.PH, out Datos_actuales);
            double.TryParse(datos_antiguo.PH, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",13," + datos.PH + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.COLOR, out Datos_actuales);
            double.TryParse(datos_antiguo.COLOR, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",14," + datos.COLOR + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.SABOR, out Datos_actuales);
            double.TryParse(datos_antiguo.SABOR, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",15," + datos.SABOR + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CORTE, out Datos_actuales);
            double.TryParse(datos_antiguo.CORTE, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",16," + datos.CORTE + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.FILM, out Datos_actuales);
            double.TryParse(datos_antiguo.FILM, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",17," + datos.FILM + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CATA, out Datos_actuales);
            double.TryParse(datos_antiguo.CATA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",18," + datos.CATA + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.GLUTEN, out Datos_actuales);
            double.TryParse(datos_antiguo.GLUTEN, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",19," + datos.GLUTEN + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CASEINA, out Datos_actuales);
            double.TryParse(datos_antiguo.CASEINA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",20," + datos.CASEINA + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.LISTERIA, out Datos_actuales);
            double.TryParse(datos_antiguo.LISTERIA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",21," + datos.LISTERIA + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.SALMONELLA, out Datos_actuales);
            double.TryParse(datos_antiguo.SALMONELLA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",22," + datos.SALMONELLA + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.PPC, out Datos_actuales);
            double.TryParse(datos_antiguo.PPC, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",23," + datos.PPC + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            //Estados
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.ID_ESTADO, out Datos_actuales);
            double.TryParse(datos_antiguo.ID_ESTADO, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",0," + datos.ID_ESTADO + ",'" + usuario + "',GETDATE(),'" + datos.OBS_ESTADO + "')";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.IMPUREZAS, out Datos_actuales);
            double.TryParse(datos_antiguo.IMPUREZAS, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",26," + datos.IMPUREZAS + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }


            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.NC, out Datos_actuales);
            double.TryParse(datos_antiguo.NC, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",0," + datos.NC + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CONSISTENCIA, out Datos_actuales);
            double.TryParse(datos_antiguo.CONSISTENCIA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",27," + datos.CONSISTENCIA + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.OLOR, out Datos_actuales);
            double.TryParse(datos_antiguo.OLOR, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",28," + datos.OLOR + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.EMULSION, out Datos_actuales);
            double.TryParse(datos_antiguo.EMULSION, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",29," + datos.EMULSION + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.HR, out Datos_actuales);
            double.TryParse(datos_antiguo.HR, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {

                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",30," + datos.HR + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }


            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.TS, out Datos_actuales);
            double.TryParse(datos_antiguo.TS, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",31," + datos.TS + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }


            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.SNF, out Datos_actuales);
            double.TryParse(datos_antiguo.SNF, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",32," + datos.SNF + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }


            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.ASPECTO, out Datos_actuales);
            double.TryParse(datos_antiguo.ASPECTO, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",33," + datos.ASPECTO + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.DORNIC, out Datos_actuales);
            double.TryParse(datos_antiguo.DORNIC, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",34," + datos.DORNIC + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.INHIBIDORES, out Datos_actuales);
            double.TryParse(datos_antiguo.INHIBIDORES, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",35," + datos.INHIBIDORES + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.BRIX, out Datos_actuales);
            double.TryParse(datos_antiguo.BRIX, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",25," + datos.BRIX + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.BOLETIN, out Datos_actuales);
            double.TryParse(datos_antiguo.BOLETIN, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",24," + datos.BOLETIN + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.SILO, out Datos_actuales);
            double.TryParse(datos_antiguo.SILO, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",35," + datos.SILO + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.TTRANSP, out Datos_actuales);
            double.TryParse(datos_antiguo.TTRANSP, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",36," + datos.TTRANSP + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.LAVADO, out Datos_actuales);
            double.TryParse(datos_antiguo.LAVADO, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",38," + datos.LAVADO + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.SILODEST, out Datos_actuales);
            double.TryParse(datos_antiguo.SILODEST, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",39," + datos.SILODEST + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.EDAD_LECHE, out Datos_actuales);
            double.TryParse(datos_antiguo.EDAD_LECHE, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",40," + datos.EDAD_LECHE + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.T_CISTERNA, out Datos_actuales);
            double.TryParse(datos_antiguo.T_CISTERNA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",41," + datos.T_CISTERNA + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.T_MUESTRA, out Datos_actuales);
            double.TryParse(datos_antiguo.T_MUESTRA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",42," + datos.T_MUESTRA + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.INH_R, out Datos_actuales);
            double.TryParse(datos_antiguo.INH_R, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",43," + datos.INH_R + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.INH_L, out Datos_actuales);
            double.TryParse(datos_antiguo.INH_L, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",44," + datos.INH_L + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.D, out Datos_actuales);
            double.TryParse(datos_antiguo.D, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",45," + datos.D + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.ALCOHOL, out Datos_actuales);
            double.TryParse(datos_antiguo.ALCOHOL, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",46," + datos.ALCOHOL + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.C_FILTRO, out Datos_actuales);
            double.TryParse(datos_antiguo.C_FILTRO, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",47," + datos.C_FILTRO + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.INH_BOB, out Datos_actuales);
            double.TryParse(datos_antiguo.INH_BOB, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",48," + datos.INH_BOB + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.MERMA, out Datos_actuales);
            double.TryParse(datos_antiguo.MERMA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",1," + datos.MERMA + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.FLATOXINA, out Datos_actuales);
            double.TryParse(datos_antiguo.FLATOXINA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",49," + datos.FLATOXINA + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CONTROL_PESO_CALIDAD, out Datos_actuales);
            double.TryParse(datos_antiguo.CONTROL_PESO_CALIDAD, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",50," + datos.CONTROL_PESO_CALIDAD + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CONTROL_PESO_LINEA, out Datos_actuales);
            double.TryParse(datos_antiguo.CONTROL_PESO_LINEA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",51," + datos.CONTROL_PESO_LINEA + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CURVA_PH, out Datos_actuales);
            double.TryParse(datos_antiguo.CONTROL_PESO_LINEA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",52," + datos.CURVA_PH + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.NUMPALET, out Datos_actuales);
            double.TryParse(datos_antiguo.NUMPALET, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA, [OBSERVACIONES]) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",0," + datos.NUMPALET + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            return false;
        }

        [WebMethod(EnableSession = true)]
        public void update_Data(Organoleptico datos)
        {
            Organoleptico fila_a = Obtener_fila_datos(datos.ID_LOTE).DefaultIfEmpty(new Organoleptico()).First();
            cmp_datos_organoleptico(datos, fila_a);

            Quality con = new Quality();
            string sql = "";
            string Estado = string.Empty;
            sql = " select Estado from [QC600].[dbo].[SSCC_CON]  where idLote=" + datos.ID_LOTE;
            Estado = con.sql_string(sql);
            int EstadoA = 0;
            int EstadoB = 0;
            if (datos.PH_CRUDO_AP.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [PH_CRUDO_AP]= " + datos.PH_CRUDO_AP.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.PH_CRUDO_DP.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [PH_CRUDO_DP]= " + datos.PH_CRUDO_DP.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.BRIX_CRUDO_AP.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [BRIX_CRUDO_AP]= " + datos.BRIX_CRUDO_AP.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.BRIX_CRUDO_DP.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [BRIX_CRUDO_DP]= " + datos.BRIX_CRUDO_DP.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos._HUMEDAD.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [%_HUMEDAD]= " + datos._HUMEDAD.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos._ES.Length > 0)
            {

                sql = "update [DATOS_ORGANOLEPTICO] set [%_ES]= " + datos._ES.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.HC.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [HC]= " + datos.HC.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.SACAROSA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [SACAROSA]= " + datos.SACAROSA.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.GRASA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [GRASA]= " + datos.GRASA.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.PROTEINA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [PROTEINA]= " + datos.PROTEINA.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.LACTOSA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [LACTOSA]= " + datos.LACTOSA.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.TEMPERATURA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [TEMPERATURA]= " + datos.TEMPERATURA.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.PH.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [PH]= " + datos.PH.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.COLOR.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [COLOR]= " + datos.COLOR.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.SABOR.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [SABOR]= " + datos.SABOR.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.CORTE.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CORTE]= " + datos.CORTE.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.FILM.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [FILM]= " + datos.FILM.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.CATA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CATA]= " + datos.CATA.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.GLUTEN.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [GLUTEN]= " + datos.GLUTEN.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.CASEINA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CASEINA]=" + datos.CASEINA.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.LISTERIA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [LISTERIA]= " + datos.LISTERIA.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.SALMONELLA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [SALMONELLA]= " + datos.SALMONELLA.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.PPC.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [PPC]= " + datos.PPC.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.OBS_ESTADO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [OBS_ESTADO]='" + datos.OBS_ESTADO + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.IMPUREZAS.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [IMPUREZAS]='" + datos.IMPUREZAS + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.NC.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [NC]='" + datos.NC + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.CONSISTENCIA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CONSISTENCIA]='" + datos.CONSISTENCIA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.OLOR.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [OLOR]='" + datos.OLOR + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.EMULSION.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [EMULSION]='" + datos.EMULSION + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.HR.Length > 0)
            {

                sql = "update [DATOS_ORGANOLEPTICO] set [HR]='" + datos.HR + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);

            }

            if (datos.TS.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [TS]='" + datos.TS + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.SNF.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [SNF]='" + datos.SNF + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.ASPECTO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ASPECTO]='" + datos.ASPECTO + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.DORNIC.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [DORNIC]='" + datos.DORNIC + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.INHIBIDORES.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [INHIBIDORES]='" + datos.INHIBIDORES + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.BRIX.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [BRIX]='" + datos.BRIX + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.BOLETIN.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [BOLETIN]='" + datos.BOLETIN + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.SILO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [SILO]='" + datos.SILO + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.TTRANSP.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [TTRANSP]='" + datos.TTRANSP + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.LAVADO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [LAVADO]='" + datos.LAVADO + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.SILODEST.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [SILODEST]='" + datos.SILODEST + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.EDAD_LECHE.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [EDAD_LECHE]='" + datos.EDAD_LECHE + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.T_CISTERNA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [T_CISTERNA]='" + datos.T_CISTERNA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.T_MUESTRA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [T_MUESTRA]='" + datos.T_MUESTRA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.INH_R.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [INH_R]='" + datos.INH_R + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.INH_L.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [INH_L]='" + datos.INH_L + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.D.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [D]='" + datos.D + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ALCOHOL.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ALCOHOL]='" + datos.ALCOHOL + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.C_FILTRO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [C_FILTRO]='" + datos.C_FILTRO + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.INH_BOB.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [INH_BOB]='" + datos.INH_BOB + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.MERMA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [MERMA]='" + datos.MERMA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.FLATOXINA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [FLATOXINA]='" + datos.FLATOXINA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.CONTROL_PESO_CALIDAD.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CONTROL_PESO_CALIDAD]='" + datos.CONTROL_PESO_CALIDAD + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.CONTROL_PESO_LINEA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CONTROL_PESO_LINEA]='" + datos.CONTROL_PESO_LINEA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.CURVA_PH.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CURVA_PH]='" + datos.CURVA_PH + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ESTADO_COMITE.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ESTADO_COMITE]='" + datos.ESTADO_COMITE + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.HORA_ARRIBA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [HORA_ARRIBA]='" + datos.HORA_ARRIBA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ESTADO_ARRIBA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ESTADO_ARRIBA]='" + datos.ESTADO_ARRIBA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.HORA_MEDIO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [HORA_MEDIO]='" + datos.HORA_MEDIO + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ESTADO_MEDIO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ESTADO_MEDIO]='" + datos.ESTADO_MEDIO + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ESTADO_ABAJO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ESTADO_ABAJO]='" + datos.ESTADO_ABAJO + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.HORA_ABAJO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [HORA_ABAJO]='" + datos.HORA_ABAJO + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.OBSEVACIONES_COMITE.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [OBSEVACIONES_COMITE]='" + datos.OBSEVACIONES_COMITE + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ID_ESTADO.Length > 0)
            {
                int.TryParse(Estado, out EstadoA);
                int.TryParse(datos.ID_ESTADO, out EstadoB);
                if (EstadoA != 0 && EstadoB != 0 && EstadoA != EstadoB)
                {
                    sql = "update [QC600].[dbo].[SSCC_CON] set [Estado]=" + datos.ID_ESTADO + " where idLote=" + datos.ID_LOTE;
                    con.sql_update(sql);
                }
            }

        }

        [WebMethod(EnableSession = true)]

        public void delete_Data(Caracteristicas datos)
        {

            Quality con = new Quality();

            string sql = "DELETE FROM [CARACTERISTICAS_ARTICULO] where id=" + datos.Id;
            con.sql_update(sql);

        }
        [WebMethod(EnableSession = true)]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        //  [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public string GetTableData(object myData)
        {

            var echo = int.Parse(get_parametters(myData, "sEcho"));
            var displayLength = int.Parse(get_parametters(myData, "iDisplayLength"));
            var displayStart = int.Parse(get_parametters(myData, "iDisplayStart"));
            var sortOrder = get_parametters(myData, "sSortDir_0").ToString(CultureInfo.CurrentCulture);
            //var roleId = HttpContext.Current.Request.Params["roleId"].ToString(CultureInfo.CurrentCulture);
            var fecha1 = get_parametters(myData, "fecha1").ToString(CultureInfo.CurrentCulture);
            var fecha2 = get_parametters(myData, "fecha2").ToString(CultureInfo.CurrentCulture);
            if (fecha2.Length <= 0 || fecha1.Length <= 0)
            { return string.Empty; }
            var search = get_parametters(myData, "sSearch").ToString(CultureInfo.CurrentCulture);
            var search1 = get_parametters(myData, "sSearch_1").ToString(CultureInfo.CurrentCulture);
            var search2 = get_parametters(myData, "sSearch_2").ToString(CultureInfo.CurrentCulture);
            var search3 = get_parametters(myData, "sSearch_3").ToString(CultureInfo.CurrentCulture);
            var search4 = get_parametters(myData, "sSearch_4").ToString(CultureInfo.CurrentCulture);
            var search5 = get_parametters(myData, "sSearch_5").ToString(CultureInfo.CurrentCulture);
            var search6 = get_parametters(myData, "sSearch_6").ToString(CultureInfo.CurrentCulture);
            var search7 = get_parametters(myData, "sSearch_7").ToString(CultureInfo.CurrentCulture);
            var search8 = get_parametters(myData, "sSearch_8").ToString(CultureInfo.CurrentCulture);
            var search9 = get_parametters(myData, "sSearch_9").ToString(CultureInfo.CurrentCulture);
            var search10 = get_parametters(myData, "sSearch_10").ToString(CultureInfo.CurrentCulture);
            var search11 = get_parametters(myData, "sSearch_11").ToString(CultureInfo.CurrentCulture);
            var search12 = get_parametters(myData, "sSearch_11").ToString(CultureInfo.CurrentCulture);
            // var records = ObtenerCaracteristicas(roleId).ToList();
            var records = Obtener_datos(fecha1, fecha2).ToList();
            if (!records.Any())
            {
                return string.Empty;
            }
            var filterRecords = records;
            if (search.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.ARTICULO.ToUpperInvariant().Contains(search.ToUpperInvariant()) || l.SSCC.Contains(search) || l.DESCRIPCION.ToUpperInvariant().Contains(search2.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search1.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.ARTICULO.ToUpperInvariant().Contains(search1.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search2.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.DESCRIPCION.ToUpperInvariant().Contains(search2.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search3.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.FECHA_SSCC_CON.ToUpperInvariant().Contains(search3.ToUpperInvariant())).ToList();  // prue.ToList();
            }

            if (search4.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.UD_ACTUAL.ToUpperInvariant().Contains(search4.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search5.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.KG_ACTUAL.ToUpperInvariant().Contains(search5.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search6.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.ESTADO.ToUpperInvariant().Contains(search6.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search7.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.SSCC.ToUpperInvariant().Contains(search7.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search8.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.LOTE_INTERNO.ToUpperInvariant().Contains(search8.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search9.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.ID_LOTE.ToUpperInvariant().Contains(search9.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search10.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.FECHA_CREACION.ToUpperInvariant().Contains(search10.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search11.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.HORA.ToUpperInvariant().Contains(search11.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search12.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.FECHACADUCIDAD.ToUpperInvariant().Contains(search12.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            var orderedResults = sortOrder == "asc"
                                ? filterRecords.OrderBy(o => o.ID)
                                : filterRecords.OrderByDescending(o => o.ID);
            var itemsToSkip = displayStart == 0
                              ? 0
                              : displayStart + 1;
            //  var filterRecords = (orderedResults);

            var pagedResults = orderedResults.Skip(itemsToSkip).ToList();

            if (displayLength >= 0)
            {
                pagedResults = orderedResults.Skip(itemsToSkip).Take(displayLength).ToList();
            }


            var hasMoreRecords = false;

            var sb = new System.Text.StringBuilder();
            sb.Append(@"{" + "\"sEcho\": " + echo + ",");
            sb.Append("\"recordsTotal\": " + filterRecords.Count + ",");
            sb.Append("\"recordsFiltered\": " + filterRecords.Count + ",");
            sb.Append("\"iTotalRecords\": " + filterRecords.Count + ",");
            sb.Append("\"iTotalDisplayRecords\": " + filterRecords.Count + ",");
            sb.Append("\"aaData\": [");

            foreach (var result in pagedResults)
            {
                if (hasMoreRecords)
                {
                    sb.Append(",");
                }

                sb.Append("[");
                sb.Append("\"" + result.ID.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ARTICULO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.DESCRIPCION.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.FECHA_SSCC_CON.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.UD_ACTUAL.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.KG_ACTUAL.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ESTADO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.SSCC.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.LOTE_INTERNO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ID_LOTE.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.FECHA_CREACION.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.HORA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.FECHACADUCIDAD.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.PH_CRUDO_AP.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.PH_CRUDO_DP.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.BRIX_CRUDO_AP.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.BRIX_CRUDO_DP.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result._HUMEDAD.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result._ES.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.HC.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.SACAROSA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.GRASA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.PROTEINA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.LACTOSA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.TEMPERATURA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.PH.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.COLOR.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.SABOR.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.CORTE.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.FILM.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.CATA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.GLUTEN.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.CASEINA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.LISTERIA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.SALMONELLA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.PPC.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.OBS_ESTADO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.BOLETIN.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.BRIX.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.IMPUREZAS.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.CONSISTENCIA + "\",");
                sb.Append("\"" + result.OLOR.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.EMULSION.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.HR.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.TS.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.SNF.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ASPECTO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.DORNIC.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.INHIBIDORES.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.NC.Replace('\r', ' ').Replace('\n', ' ') + "\",");

                sb.Append("\"" + result.SILO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.TTRANSP.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.LAVADO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.SILODEST.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.EDAD_LECHE.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.T_CISTERNA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.T_MUESTRA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.INH_R.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.INH_L.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.D.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ALCOHOL.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.C_FILTRO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.INH_BOB.Replace('\r', ' ').Replace('\n', ' ') + "\",");

                sb.Append("\"" + result.NUMPALET.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.FLATOXINA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.MERMA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.CONTROL_PESO_CALIDAD.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.CONTROL_PESO_LINEA.Replace('\r', ' ').Replace('\n', ' ') + "\",");

                sb.Append("\"" + result.CURVA_PH.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ESTADO_COMITE.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.HORA_ARRIBA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ESTADO_ARRIBA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.HORA_MEDIO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ESTADO_MEDIO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.HORA_ABAJO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ESTADO_ABAJO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.OBSEVACIONES_COMITE.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ID_ESTADO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"<img class='image-details' src='content/details_open.png' runat='server' height='16' width='16' alt='View Details'/>\"");
                sb.Append("]");
                hasMoreRecords = true;
            }
            sb.Append("]}");


            return sb.ToString();

        }
        public IEnumerable<Caracteristicas> ObtenerCaracteristicas(string articulo)
        {
            string sql = @"select [CARACTERISTICAS_ARTICULO].id, Articulo,ARTICULO.Descripción, [CARACTERISTICAS_ARTICULO].Caracteristica,[Organolectico_Carac].Caracteristica, V_Min, V_Max, Obligatorio 
  FROM [QC600].[dbo].[CARACTERISTICAS_ARTICULO] inner join ARTICULO on ARTICULO.Artículo=[CARACTERISTICAS_ARTICULO].[Articulo]
  inner join [Organolectico_Carac] on [CARACTERISTICAS_ARTICULO].Caracteristica= [Organolectico_Carac].Cod_Caracteristica   where [CARACTERISTICAS_ARTICULO].Articulo=" + articulo;
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);
            List<Caracteristicas> lista_datos = new List<Caracteristicas>();
            foreach (DataRow lin in datos.AsEnumerable())
            {
                Caracteristicas linea = new Caracteristicas { Id = lin[0].ToString(), articulo = lin[1].ToString(), art_descripcion = lin[2].ToString(), Cod_Caracteristica = lin[3].ToString(), Caracteristica = lin[4].ToString(), V_Min = lin[5].ToString(), V_Max = lin[6].ToString(), Obligatorio = lin[7].ToString() };
                lista_datos.Add(linea);
            }

            return lista_datos;
        }
        public class Caracteristicas
        {
            public string Id { get; set; }

            public string articulo { get; set; }
            public string art_descripcion { get; set; }
            public string V_Max { get; set; }
            public string V_Min { get; set; }
            public string Cod_Caracteristica { get; set; }
            public string Caracteristica { get; set; }
            public string Obligatorio { get; set; }

        }


        public IEnumerable<Organoleptico> Obtener_datos(string fecha1, string fecha2)
        {
            string sql = @"SELECT  [ID]
      ,[DATOS_ORGANOLEPTICO].[ARTICULO]
	  ,[ARTICULO].[Descr Abreviada] as [DESCRIPCION]
      ,[FECHA_SSCC_CON]
      ,ROUND ([UD_ACTUAL],2) as [UD_ACTUAL]
      ,ROUND ([KG_ACTUAL],2) as [KG_ACTUAL]
      ,Estado.Estado AS ESTADO
      ,[SSCC]
      ,[LOTE_INTERNO]
      ,[ID_LOTE]
      ,CONVERT (VARCHAR,[FECHA_CREACION],103) AS FECHA_CREACION
      ,CONVERT (VARCHAR,[HORA],108) AS [HORA]
      ,CONVERT (VARCHAR,[FECHACADUCIDAD],103) AS FECHACADUCIDAD
      ,[PH_CRUDO_AP]
      ,[PH_CRUDO_DP]
      ,[BRIX_CRUDO_AP]
      ,[BRIX_CRUDO_DP]
      ,[%_HUMEDAD]
      ,[%_ES]
      ,[HC]
      ,[SACAROSA]
      ,[GRASA]
      ,[PROTEINA]
      ,[LACTOSA]
      ,[TEMPERATURA]
      ,[PH]
      ,[COLOR]
      ,[SABOR]
      ,[CORTE]
      ,[FILM]
      ,[CATA]
      ,[GLUTEN]
      ,[CASEINA]
      ,[LISTERIA]
      ,[SALMONELLA]
      ,[PPC]
,[OBS_ESTADO]
,[BOLETIN]
,[BRIX]  

      ,[IMPUREZAS]
      ,[CONSISTENCIA]
      ,[OLOR]
      ,[EMULSION]
      ,[HR]
      ,[TS]
      ,[SNF]
      ,[ASPECTO]
      ,[DORNIC]
      ,[INHIBIDORES]
,[NC]

,[DATOS_ORGANOLEPTICO].[SILO]
      ,[TTRANSP]
      ,[LAVADO]
      ,[SILODEST]
      ,[EDAD_LECHE]
      ,[T_CISTERNA]
      ,[T_MUESTRA]
      ,[INH_R]
      ,[INH_L]
      ,[D]
      ,[ALCOHOL]
      ,[C_FILTRO]
      ,[INH_BOB],
[NUMPALET]
      ,[FLATOXINA]
,[MERMA]
,[CONTROL_PESO_CALIDAD]
,[CONTROL_PESO_LINEA]
,[CURVA_PH]
,[ESTADO_COMITE]
,[HORA_ARRIBA]
      ,[ESTADO_ARRIBA]
      ,[HORA_MEDIO]
      ,[ESTADO_MEDIO]
      ,[HORA_ABAJO]
      ,[ESTADO_ABAJO]
      ,[OBSEVACIONES_COMITE]
,[DATOS_ORGANOLEPTICO].[ESTADO] AS ID_ESTADO
  FROM [QC600].[dbo].[DATOS_ORGANOLEPTICO]
  INNER JOIN ARTICULO ON ARTICULO.Artículo=[DATOS_ORGANOLEPTICO].ARTICULO 
  INNER JOIN Estado ON Estado.ID_Estado=[DATOS_ORGANOLEPTICO].ESTADO  WHERE  FECHA_CREACION Between convert(datetime,'" + fecha1 + @"',103) and convert(datetime,'" + fecha2 + @"',103)";
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);
            List<Organoleptico> lista_datos = new List<Organoleptico>();
            if (datos != null)
            {
                foreach (DataRow lin in datos.AsEnumerable())
                {

                    Organoleptico linea = new Organoleptico
                    {
                        ID = lin[0].ToString(),
                        ARTICULO = lin[1].ToString(),
                        DESCRIPCION = lin[2].ToString(),
                        FECHA_SSCC_CON = lin[3].ToString(),
                        UD_ACTUAL = lin[4].ToString(),
                        KG_ACTUAL = lin[5].ToString(),
                        ESTADO = lin[6].ToString(),
                        SSCC = lin[7].ToString(),
                        LOTE_INTERNO = lin[8].ToString(),
                        ID_LOTE = lin[9].ToString(),
                        FECHA_CREACION = lin[10].ToString(),
                        HORA = lin[11].ToString(),
                        FECHACADUCIDAD = lin[12].ToString(),
                        PH_CRUDO_AP = lin[13].ToString(),
                        PH_CRUDO_DP = lin[14].ToString(),
                        BRIX_CRUDO_AP = lin[15].ToString(),
                        BRIX_CRUDO_DP = lin[16].ToString(),
                        _HUMEDAD = lin[17].ToString(),
                        _ES = lin[18].ToString(),
                        HC = lin[19].ToString(),
                        SACAROSA = lin[20].ToString(),
                        GRASA = lin[21].ToString(),
                        PROTEINA = lin[22].ToString(),
                        LACTOSA = lin[23].ToString(),
                        TEMPERATURA = lin[24].ToString(),
                        PH = lin[25].ToString(),
                        COLOR = lin[26].ToString(),
                        SABOR = lin[27].ToString(),
                        CORTE = lin[28].ToString(),
                        FILM = lin[29].ToString(),
                        CATA = lin[30].ToString(),
                        GLUTEN = lin[31].ToString(),
                        CASEINA = lin[32].ToString(),
                        LISTERIA = lin[33].ToString(),
                        SALMONELLA = lin[34].ToString(),
                        PPC = lin[35].ToString(),
                        OBS_ESTADO = lin[36].ToString(),
                        BOLETIN = lin[37].ToString(),
                        BRIX = lin[38].ToString(),
                        IMPUREZAS = lin[39].ToString(),
                        CONSISTENCIA = lin[40].ToString(),
                        OLOR = lin[41].ToString(),
                        EMULSION = lin[42].ToString(),
                        HR = lin[43].ToString(),
                        TS = lin[44].ToString(),
                        SNF = lin[45].ToString(),
                        ASPECTO = lin[46].ToString(),
                        DORNIC = lin[47].ToString(),
                        INHIBIDORES = lin[48].ToString(),
                        NC = lin[49].ToString(),
                        SILO = lin[50].ToString(),
                        TTRANSP = lin[51].ToString(),
                        LAVADO = lin[52].ToString(),
                        SILODEST = lin[53].ToString(),
                        EDAD_LECHE = lin[54].ToString(),
                        T_CISTERNA = lin[55].ToString(),
                        T_MUESTRA = lin[56].ToString(),
                        INH_R = lin[57].ToString(),
                        INH_L = lin[58].ToString(),
                        D = lin[59].ToString(),
                        ALCOHOL = lin[60].ToString(),
                        C_FILTRO = lin[61].ToString(),
                        INH_BOB = lin[62].ToString(),
                        NUMPALET = lin[63].ToString(),
                        FLATOXINA = lin[64].ToString(),
                        MERMA = lin[65].ToString(),
                        CONTROL_PESO_LINEA = lin[66].ToString(),
                        CONTROL_PESO_CALIDAD = lin[67].ToString(),
                        CURVA_PH = lin[68].ToString(),
                        ESTADO_COMITE = lin[69].ToString(),
                        HORA_ARRIBA = lin[70].ToString(),
                        ESTADO_ARRIBA = lin[71].ToString(),
                        HORA_MEDIO = lin[72].ToString(),
                        ESTADO_MEDIO = lin[73].ToString(),
                        HORA_ABAJO = lin[74].ToString(),
                        ESTADO_ABAJO = lin[75].ToString(),
                        OBSEVACIONES_COMITE = lin[76].ToString(),
                        ID_ESTADO = lin[77].ToString()



                    };
                    lista_datos.Add(linea);
                }
            }
            return lista_datos;
        }

        public IEnumerable<Organoleptico> Obtener_fila_datos(string IdLote)
        {
            string sql = @"SELECT  [ID]
      ,[DATOS_ORGANOLEPTICO].[ARTICULO]
	  ,[ARTICULO].[Descr Abreviada] as [DESCRIPCION]
      ,[FECHA_SSCC_CON]
      ,ROUND ([UD_ACTUAL],2) as [UD_ACTUAL]
      ,ROUND ([KG_ACTUAL],2) as [KG_ACTUAL]
      ,Estado.Estado AS ESTADO
      ,[SSCC]
      ,[LOTE_INTERNO]
      ,[ID_LOTE]
      ,CONVERT (VARCHAR,[FECHA_CREACION],103) AS FECHA_CREACION
      ,CONVERT (VARCHAR,[HORA],108) AS [HORA]
      ,CONVERT (VARCHAR,[FECHACADUCIDAD],103) AS FECHACADUCIDAD
      ,[PH_CRUDO_AP]
      ,[PH_CRUDO_DP]
      ,[BRIX_CRUDO_AP]
      ,[BRIX_CRUDO_DP]
      ,[%_HUMEDAD]
      ,[%_ES]
      ,[HC]
      ,[SACAROSA]
      ,[GRASA]
      ,[PROTEINA]
      ,[LACTOSA]
      ,[TEMPERATURA]
      ,[PH]
      ,[COLOR]
      ,[SABOR]
      ,[CORTE]
      ,[FILM]
      ,[CATA]
      ,[GLUTEN]
      ,[CASEINA]
      ,[LISTERIA]
      ,[SALMONELLA]
      ,[PPC]
,[OBS_ESTADO]
,[BOLETIN]
,[BRIX]  

      ,[IMPUREZAS]
      ,[CONSISTENCIA]
      ,[OLOR]
      ,[EMULSION]
      ,[HR]
      ,[TS]
      ,[SNF]
      ,[ASPECTO]
      ,[DORNIC]
      ,[INHIBIDORES]
,[NC]

,[DATOS_ORGANOLEPTICO].[SILO]
      ,[TTRANSP]
      ,[LAVADO]
      ,[SILODEST]
      ,[EDAD_LECHE]
      ,[T_CISTERNA]
      ,[T_MUESTRA]
      ,[INH_R]
      ,[INH_L]
      ,[D]
      ,[ALCOHOL]
      ,[C_FILTRO]
      ,[INH_BOB]
,[NUMPALET]
      ,[FLATOXINA]
,
[MERMA]
,[CONTROL_PESO_CALIDAD]
,[CONTROL_PESO_LINEA]
,[CURVA_PH]
,[ESTADO_COMITE]
,[HORA_ARRIBA]
      ,[ESTADO_ARRIBA]
      ,[HORA_MEDIO]
      ,[ESTADO_MEDIO]
      ,[HORA_ABAJO]
      ,[ESTADO_ABAJO]
      ,[OBSEVACIONES_COMITE]
,[DATOS_ORGANOLEPTICO].[ESTADO] AS ID_ESTADO
  FROM [QC600].[dbo].[DATOS_ORGANOLEPTICO]
  INNER JOIN ARTICULO ON ARTICULO.Artículo=[DATOS_ORGANOLEPTICO].ARTICULO 
  INNER JOIN Estado ON Estado.ID_Estado=[DATOS_ORGANOLEPTICO].ESTADO  
WHERE ID_LOTE=" + IdLote;
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);
            List<Organoleptico> lista_datos = new List<Organoleptico>();
            if (datos != null)
            {
                foreach (DataRow lin in datos.AsEnumerable())
                {
                    Organoleptico linea = new Organoleptico
                    {
                        ID = lin[0].ToString(),
                        ARTICULO = lin[1].ToString(),
                        DESCRIPCION = lin[2].ToString(),
                        FECHA_SSCC_CON = lin[3].ToString(),
                        UD_ACTUAL = lin[4].ToString(),
                        KG_ACTUAL = lin[5].ToString(),
                        ESTADO = lin[6].ToString(),
                        SSCC = lin[7].ToString(),
                        LOTE_INTERNO = lin[8].ToString(),
                        ID_LOTE = lin[9].ToString(),
                        FECHA_CREACION = lin[10].ToString(),
                        HORA = lin[11].ToString(),
                        FECHACADUCIDAD = lin[12].ToString(),
                        PH_CRUDO_AP = lin[13].ToString(),
                        PH_CRUDO_DP = lin[14].ToString(),
                        BRIX_CRUDO_AP = lin[15].ToString(),
                        BRIX_CRUDO_DP = lin[16].ToString(),
                        _HUMEDAD = lin[17].ToString(),
                        _ES = lin[18].ToString(),
                        HC = lin[19].ToString(),
                        SACAROSA = lin[20].ToString(),
                        GRASA = lin[21].ToString(),
                        PROTEINA = lin[22].ToString(),
                        LACTOSA = lin[23].ToString(),
                        TEMPERATURA = lin[24].ToString(),
                        PH = lin[25].ToString(),
                        COLOR = lin[26].ToString(),
                        SABOR = lin[27].ToString(),
                        CORTE = lin[28].ToString(),
                        FILM = lin[29].ToString(),
                        CATA = lin[30].ToString(),
                        GLUTEN = lin[31].ToString(),
                        CASEINA = lin[32].ToString(),
                        LISTERIA = lin[33].ToString(),
                        SALMONELLA = lin[34].ToString(),
                        PPC = lin[35].ToString(),

                        OBS_ESTADO = lin[36].ToString(),
                        BOLETIN = lin[37].ToString(),
                        BRIX = lin[38].ToString(),
                        IMPUREZAS = lin[39].ToString(),
                        CONSISTENCIA = lin[40].ToString(),
                        OLOR = lin[41].ToString(),
                        EMULSION = lin[42].ToString(),
                        HR = lin[43].ToString(),
                        TS = lin[44].ToString(),
                        SNF = lin[45].ToString(),
                        ASPECTO = lin[46].ToString(),
                        DORNIC = lin[47].ToString(),
                        INHIBIDORES = lin[48].ToString(),
                        NC = lin[49].ToString(),
                        SILO = lin[50].ToString(),
                        TTRANSP = lin[51].ToString(),
                        LAVADO = lin[52].ToString(),
                        SILODEST = lin[53].ToString(),
                        EDAD_LECHE = lin[54].ToString(),
                        T_CISTERNA = lin[55].ToString(),
                        T_MUESTRA = lin[56].ToString(),
                        INH_R = lin[57].ToString(),
                        INH_L = lin[58].ToString(),
                        D = lin[59].ToString(),
                        ALCOHOL = lin[60].ToString(),
                        C_FILTRO = lin[61].ToString(),
                        INH_BOB = lin[62].ToString(),
                        NUMPALET = lin[63].ToString(),
                        FLATOXINA = lin[64].ToString(),
                        MERMA = lin[65].ToString(),
                        CONTROL_PESO_LINEA = lin[66].ToString(),
                        CONTROL_PESO_CALIDAD = lin[67].ToString(),
                        CURVA_PH = lin[68].ToString(),
                        ESTADO_COMITE = lin[69].ToString(),
                        HORA_ARRIBA = lin[70].ToString(),
                        ESTADO_ARRIBA = lin[71].ToString(),
                        HORA_MEDIO = lin[72].ToString(),
                        ESTADO_MEDIO = lin[73].ToString(),
                        HORA_ABAJO = lin[74].ToString(),
                        ESTADO_ABAJO = lin[75].ToString(),
                        OBSEVACIONES_COMITE = lin[76].ToString(),
                        ID_ESTADO = lin[77].ToString()
                    };
                    lista_datos.Add(linea);
                }
            }
            return lista_datos;
        }
        public class Organoleptico
        {

            public string ID { get; set; }
            public string ARTICULO { get; set; }
            public string DESCRIPCION { get; set; }
            public string FECHA_SSCC_CON { get; set; }
            public string UD_ACTUAL { get; set; }
            public string KG_ACTUAL { get; set; }
            public string ESTADO { get; set; }
            public string SSCC { get; set; }
            public string LOTE_INTERNO { get; set; }
            public string ID_LOTE { get; set; }
            public string FECHA_CREACION { get; set; }
            public string HORA { get; set; }
            public string FECHACADUCIDAD { get; set; }
            public string PH_CRUDO_AP { get; set; }
            public string PH_CRUDO_DP { get; set; }
            public string BRIX_CRUDO_AP { get; set; }
            public string BRIX_CRUDO_DP { get; set; }
            public string _HUMEDAD { get; set; }
            public string _ES { get; set; }
            public string HC { get; set; }
            public string SACAROSA { get; set; }
            public string GRASA { get; set; }
            public string PROTEINA { get; set; }
            public string LACTOSA { get; set; }
            public string TEMPERATURA { get; set; }
            public string PH { get; set; }
            public string COLOR { get; set; }
            public string SABOR { get; set; }
            public string CORTE { get; set; }
            public string FILM { get; set; }
            public string CATA { get; set; }
            public string GLUTEN { get; set; }
            public string CASEINA { get; set; }
            public string LISTERIA { get; set; }
            public string SALMONELLA { get; set; }
            public string PPC { get; set; }
            public string ID_ESTADO { get; set; }
            public string OBS_ESTADO { get; set; }
            public string IMPUREZAS { get; set; }
            public string BRIX { get; set; }
            public string NC { get; set; }
            public string CONSISTENCIA { get; set; }
            public string OLOR { get; set; }
            public string EMULSION { get; set; }
            public string HR { get; set; }
            public string TS { get; set; }
            public string SNF { get; set; }
            public string ASPECTO { get; set; }
            public string DORNIC { get; set; }
            public string INHIBIDORES { get; set; }
            public string BOLETIN { get; set; }

            public string SILO { get; set; }
            public string TTRANSP { get; set; }
            public string LAVADO { get; set; }
            public string SILODEST { get; set; }
            public string EDAD_LECHE { get; set; }
            public string T_CISTERNA { get; set; }
            public string T_MUESTRA { get; set; }
            public string INH_R { get; set; }
            public string INH_L { get; set; }
            public string D { get; set; }
            public string ALCOHOL { get; set; }
            public string C_FILTRO { get; set; }
            public string INH_BOB { get; set; }
            public string MERMA { get; set; }
            public string NUMPALET { get; set; }
            public string FLATOXINA { get; set; }
            public string CONTROL_PESO_LINEA { get; set; }
            public string CONTROL_PESO_CALIDAD { get; set; }
            public string CURVA_PH { get; set; }
            public string ESTADO_COMITE { get; set; }
            public string HORA_ARRIBA { get; set; }
            public string ESTADO_ARRIBA { get; set; }
            public string HORA_MEDIO { get; set; }
            public string ESTADO_MEDIO { get; set; }
            
            public string HORA_ABAJO { get; set; }
            public string ESTADO_ABAJO { get; set; }
            public string OBSEVACIONES_COMITE { get; set; }
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hola a todos";
        }
    }
}

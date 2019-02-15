using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Utiles;
using System.Data;

namespace rinya_app.Calidad.Otros_Controles
{
    /// <summary>
    /// Descripción breve de WebService_SILO
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class WebService_SILO : System.Web.Services.WebService
    {
        public DataTable dt_fechas_articulo_lote(string desde, string hasta)
        {
            string sql = @" SELECT * FROM V_Articulo_lote_Turno_fechas  where Fecha BETWEEN CONVERT(DATETIME,'" + desde + @"',103) AND CONVERT(DATETIME,'" + hasta + @"',103)";
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);


            return datos;
        }
        public DataTable dt_fechas_articulo_lote(string desde, string hasta, string turno)
        {
            string sql = @" SELECT * FROM V_Articulo_lote_Turno_fechas  where Fecha BETWEEN CONVERT(DATETIME,'" + desde + @"',103) AND CONVERT(DATETIME,'" + hasta + @"',103) and turno="+turno;
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);


            return datos;
        }

        [WebMethod(EnableSession = true)]
        public void New_Data(SILO datos)
        {
            string usuario = "pruebas";
            if (User.Identity.IsAuthenticated)
                usuario = User.Identity.Name;
            Quality con = new Quality();

           /* string sql =@"INSERT INTO [QC600].[dbo].[DATOS_SILO_ORGANOLEPTICO] ([SSCC],[USUARIO],[FECHA],[PH],[DORNIC],[BRIX],[INH_L],[GRASA],[PROTEINA],[LACTOSA],[SNF],[TS],[OLOR],[TEST_R]) 
  values('"+datos.SSCC + "' , '" +usuario + "' , " + "GETDATE()" + " , " +datos.PH + " , " +datos.DORNIC + " , " +datos.BRIX + " , " +datos.INH_L + " , " +datos.GRASA + " , " +datos.PROTEINA+ " , " +datos.LACTOSA + " , " +datos.SNF + " , " +datos.TS + " , " +datos.OLOR+ " , "+datos.TEST_R + " )";
  */
            string sql = @"INSERT INTO [QC600].[dbo].[DATOS_SILO_ORGANOLEPTICO] ([SSCC],[USUARIO],[FECHA]) 
  values('" + datos.SSCC + "' , '" + usuario + "' , " + "GETDATE()" +  " )";
            con.sql_update(sql);
            update_Data(datos);
        }
        [WebMethod(EnableSession = true)]
        public void Delete_Data(SILO datos)
        {
            string sql = @"Delete from [QC600].[dbo].[DATOS_SILO_ORGANOLEPTICO] where ID="+datos.ID;
            Quality con = new Quality();
            con.sql_update(sql);
        }
        [WebMethod(EnableSession = true)]
        public string Get_all_Silos()
        {
            string sql = "SELECT [SSCCSilo] as SSCC,[DescripcionSilo] as DESCRIPCION FROM [QC600].[dbo].[SILO] where TipoSilo=0 and [DescripcionSilo] like '%LECHE%'";
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
       
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string GetTableData(object myData)
        {

            var echo = int.Parse(get_parametters(myData, "sEcho"));
            var displayLength = int.Parse(get_parametters(myData, "iDisplayLength"));
            var displayStart = int.Parse(get_parametters(myData, "iDisplayStart"));
            var sortOrder = get_parametters(myData, "sSortDir_0").ToString(CultureInfo.CurrentCulture);
            
            var roleId = get_parametters(myData, "roleId").ToString(CultureInfo.CurrentCulture);
            if (roleId.Length <= 0)
            { return string.Empty; }
            var fecha1 = get_parametters(myData, "fecha1").ToString(CultureInfo.CurrentCulture);
            var fecha2 = get_parametters(myData, "fecha2").ToString(CultureInfo.CurrentCulture);
            if (fecha2.Length <= 0 || fecha1.Length <= 0)
            { return string.Empty; }
            var search = get_parametters(myData, "sSearch").ToString(CultureInfo.CurrentCulture);
            var records = ObtenerDatosSilos(roleId,fecha1,fecha2).ToList();
            if (!records.Any())
            {
                return string.Empty;
            }
            var filterRecords = records;
            if (search.Length > 0)
            {
             //   filterRecords = filterRecords.Where(l => l.Cod_Caracteristica.Contains(search) || l.Caracteristica.Contains(search)).ToList();  // prue.ToList();
            }
            var orderedResults = sortOrder == "desc"
                                ? filterRecords.OrderBy(o => o.ID)
                                : filterRecords.OrderByDescending(o => o.ID);
            var itemsToSkip = displayStart == 0
                              ? 0
                              : displayStart + 1;
            //  var filterRecords = (orderedResults);




            var pagedResults = orderedResults.Skip(itemsToSkip).Take(displayLength).ToList();
            var hasMoreRecords = false;

            var sb = new System.Text.StringBuilder();
            sb.Append(@"{" + "\"sEcho\": " + echo + ",");
            sb.Append("\"recordsTotal\": " + records.Count + ",");
            sb.Append("\"recordsFiltered\": " + records.Count + ",");
            sb.Append("\"iTotalRecords\": " + records.Count + ",");
            sb.Append("\"iTotalDisplayRecords\": " + records.Count + ",");
            sb.Append("\"aaData\": [");

            foreach (var result in pagedResults)
            {
                if (hasMoreRecords)
                {
                    sb.Append(",");
                }

                sb.Append("[");
                sb.Append("\"" + result.ID + "\",");
                sb.Append("\"" + result.SSCC + "\",");
                sb.Append("\"" + result.USUARIO + "\",");
                sb.Append("\"" + result.FECHA + "\",");
                sb.Append("\"" + result.PH + "\",");
                sb.Append("\"" + result.DORNIC + "\",");
                sb.Append("\"" + result.BRIX + "\",");
                sb.Append("\"" + result.INH_L + "\",");
                sb.Append("\"" + result.GRASA + "\",");
                sb.Append("\"" + result.PROTEINA + "\",");
                sb.Append("\"" + result.LACTOSA + "\",");
                sb.Append("\"" + result.SNF + "\",");
                sb.Append("\"" + result.TS + "\",");
                sb.Append("\"" + result.OLOR + "\",");
                sb.Append("\"" + result.TEST_R + "\",");
                sb.Append("\"<img class='image-details' src='content/details_open.png' runat='server' height='16' width='16' alt='View Details'/>\"");
                sb.Append("]");
                hasMoreRecords = true;
            }
            sb.Append("]}");
            return sb.ToString();

        }
        [WebMethod(EnableSession = true)]
        public void update_Data(SILO datos)
        {

            string usuario = "pruebas";
            if (User.Identity.IsAuthenticated)
                usuario = User.Identity.Name;
            Quality con = new Quality();
            string sql = "";
            if (datos.PH.Length>0)
            {
                sql = @"UPDATE QC600.dbo.DATOS_SILO_ORGANOLEPTICO set USUARIO='" + usuario + @"' ,FECHA=GETDATE() , PH= " + datos.PH.Replace(',', '.') + "where ID=" + datos.ID;
                con.sql_update(sql);
            }
            if (datos.DORNIC.Length > 0)
            {
                sql = @"UPDATE QC600.dbo.DATOS_SILO_ORGANOLEPTICO set USUARIO='" + usuario + @"' ,FECHA=GETDATE() , DORNIC=" + datos.DORNIC.Replace(',', '.') + "where ID=" + datos.ID;
                con.sql_update(sql);
            }
            if (datos.BRIX.Length > 0)
            {
                sql = @"UPDATE QC600.dbo.DATOS_SILO_ORGANOLEPTICO set USUARIO='" + usuario + @"' ,FECHA=GETDATE() , BRIX =" + datos.BRIX.Replace(',', '.') + "where ID=" + datos.ID;
                con.sql_update(sql);
            }
            if (datos.INH_L.Length > 0)
            {
                sql = @"UPDATE QC600.dbo.DATOS_SILO_ORGANOLEPTICO set USUARIO='" + usuario + @"' ,FECHA=GETDATE() , INH_L =" + datos.INH_L.Replace(',', '.') + "where ID=" + datos.ID;
                con.sql_update(sql);
            }
            if (datos.GRASA.Length > 0)
            {
                sql = @"UPDATE QC600.dbo.DATOS_SILO_ORGANOLEPTICO set USUARIO='" + usuario + @"' ,FECHA=GETDATE() , GRASA =" + datos.GRASA.Replace(',', '.') + "where ID=" + datos.ID;
                con.sql_update(sql);
            }
            if (datos.PROTEINA.Length > 0)
            {
                sql = @"UPDATE QC600.dbo.DATOS_SILO_ORGANOLEPTICO set USUARIO='" + usuario + @"' ,FECHA=GETDATE() , PROTEINA =" + datos.PROTEINA.Replace(',', '.') + "where ID=" + datos.ID;
                con.sql_update(sql);
            }
            if (datos.LACTOSA.Length > 0)
            {
                sql = @"UPDATE QC600.dbo.DATOS_SILO_ORGANOLEPTICO set USUARIO='" + usuario + @"' ,FECHA=GETDATE() , LACTOSA =" + datos.LACTOSA.Replace(',', '.') + "where ID=" + datos.ID;
                con.sql_update(sql);
            }
            if (datos.SNF.Length > 0)
            {
                sql = @"UPDATE QC600.dbo.DATOS_SILO_ORGANOLEPTICO set USUARIO='" + usuario + @"' ,FECHA=GETDATE() , SNF =" + datos.SNF.Replace(',', '.') + "where ID=" + datos.ID;
                con.sql_update(sql);
            }
            if (datos.TS.Length > 0)
            {
                sql = @"UPDATE QC600.dbo.DATOS_SILO_ORGANOLEPTICO set USUARIO='" + usuario + @"' ,FECHA=GETDATE() , TS = " + datos.TS.Replace(',', '.') + "where ID=" + datos.ID;
                con.sql_update(sql);
            }
            if (datos.OLOR.Length > 0)
            {
                sql = @"UPDATE QC600.dbo.DATOS_SILO_ORGANOLEPTICO set USUARIO='" + usuario + @"' ,FECHA=GETDATE() , OLOR = " + datos.OLOR.Replace(',', '.') + "where ID=" + datos.ID;
                con.sql_update(sql);
            }
            if (datos.TEST_R.Length > 0)
            {
                sql = @"UPDATE QC600.dbo.DATOS_SILO_ORGANOLEPTICO set USUARIO='" + usuario + @"' ,FECHA=GETDATE() , TEST_R = " + datos.TEST_R.Replace(',', '.') + "where ID=" + datos.ID;
                con.sql_update(sql);
            }

        }

        public IEnumerable<SILO> ObtenerDatosSilos(string SSCC, string fecha1, string fecha2)
        {
            string sql = @"SELECT ID, SSCC,USUARIO,FECHA, PH, DORNIC, BRIX, INH_L, GRASA, PROTEINA, LACTOSA, SNF,TS,OLOR, TEST_R FROM DATOS_SILO_ORGANOLEPTICO where SSCC='" + SSCC+ "' AND FECHA Between convert(datetime,'" + fecha1 + @"',103) and convert(datetime,'" + fecha2 + @"',103)+1";
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);
            List<SILO> lista_datos = new List<SILO>();
            foreach (DataRow lin in datos.AsEnumerable())
            {
                SILO linea = new SILO {
                    ID = lin[0].ToString(),
                    SSCC = lin[1].ToString(),
                    USUARIO = lin[2].ToString(),
                    FECHA = lin[3].ToString(),
                    PH = lin[4].ToString(),
                    DORNIC = lin[5].ToString(),
                    BRIX = lin[6].ToString(),
                    INH_L = lin[7].ToString(),
                    GRASA = lin[8].ToString(),
                    PROTEINA = lin[9].ToString(),
                    LACTOSA = lin[10].ToString(),
                    SNF = lin[11].ToString(),
                    TS = lin[12].ToString(),
                    OLOR = lin[13].ToString(),
                    TEST_R = lin[14].ToString()

                };
                lista_datos.Add(linea);
            }

            return lista_datos;
        }

        public class SILO
        {
            public string ID { get; set; }
            public string SSCC { get; set; }
            public string USUARIO { get; set; }
            public string FECHA { get; set; }
            public string PH { get; set; }
            public string DORNIC { get; set; }
            public string BRIX { get; set; }
            public string INH_L { get; set; }
            public string GRASA { get; set; }
            public string PROTEINA { get; set; }
            public string LACTOSA { get; set; }
            public string SNF { get; set; }
            public string TS { get; set; }

            public string OLOR
            {
                get; set;
            }
            public string TEST_R
            {
                get; set;
            }
            

        }
        [WebMethod]
        public string HelloWorld()
        {
            return "Hola a todos";
        }
    }
}

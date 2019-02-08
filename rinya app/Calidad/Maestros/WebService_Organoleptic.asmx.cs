using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Utiles;
using System.Data;
using System.Web.Script.Serialization;

namespace rinya_app.Calidad.Maestros
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string value, StringComparison compareMode)
        {
            if (string.IsNullOrEmpty(source))
                return false;

            return source.IndexOf(value, compareMode) >= 0;
        }
    }
    /// <summary>
    /// Descripción breve de WebService_articulo_caracteristicas
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class WebService_Organoleptic : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetEstados()
        {
            string sql = "select [ID_Estado] ,[Estado] FROM[QC600].[dbo].[Estado]";
            Quality con = new Quality();
            DataTable datos_estados = con.Sql_Datatable(sql);
            return Newtonsoft.Json.JsonConvert.SerializeObject(datos_estados, Newtonsoft.Json.Formatting.Indented);
        }
        [WebMethod(EnableSession = true)]
        public string GetCaracteristicas_articulo(Organoleptico datos)
        {
            string sql = "SELECT  [CARACTERISTICAS_ARTICULO].[Caracteristica],[Organolectico_Carac].Caracteristica + ' ( ' + cast([V_Min] as varchar) +' | ' + cast( [V_Max] as varchar) +' ):' as lbl FROM[QC600].[dbo].[CARACTERISTICAS_ARTICULO] inner join[QC600].[dbo].[Organolectico_Carac] on [Cod_Caracteristica]= [CARACTERISTICAS_ARTICULO].[Caracteristica] where Articulo = " + datos.ARTICULO;
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
        [WebMethod(EnableSession = true)]
        public void update_Data(Organoleptico datos)
        {

            Quality con = new Quality();
            if (datos.PH_CRUDO_AP.Length <= 0)
                datos.PH_CRUDO_AP = "0";
            if (datos.PH_CRUDO_DP.Length <= 0)
                datos.PH_CRUDO_DP = "0";
            if (datos.BRIX_CRUDO_AP.Length <= 0)
                datos.BRIX_CRUDO_AP = "0";
            if (datos.BRIX_CRUDO_DP.Length <= 0)
                datos.BRIX_CRUDO_DP = "0";
            if (datos._HUMEDAD.Length <= 0)
                datos._HUMEDAD = "0";
            if (datos._ES.Length <= 0)
                datos._ES = "0";
            if (datos.HC.Length <= 0)
                datos.HC = "0";
            if (datos.SACAROSA.Length <= 0)
                datos.SACAROSA = "0";
            if (datos.GRASA.Length <= 0)
                datos.GRASA = "0";
            if (datos.PROTEINA.Length <= 0)
                datos.PROTEINA = "0";
            if (datos.LACTOSA.Length <= 0)
                datos.LACTOSA = "0";
            if (datos.TEMPERATURA.Length <= 0)
                datos.TEMPERATURA = "0";
            if (datos.PH.Length <= 0)
                datos.PH = "0";
            if (datos.COLOR.Length <= 0)
                datos.COLOR = "0";
            if (datos.SABOR.Length <= 0)
                datos.SABOR = "0";
            if (datos.CORTE.Length <= 0)
                datos.CORTE = "0";
            if (datos.FILM.Length <= 0)
                datos.FILM = "0";
            if (datos.CATA.Length <= 0)
                datos.CATA = "0";
            if (datos.GLUTEN.Length <= 0)
                datos.GLUTEN = "0";
            if (datos.CASEINA.Length <= 0)
                datos.CASEINA = "0";
            if (datos.LISTERIA.Length <= 0)
                datos.LISTERIA = "0";
            if (datos.SALMONELLA.Length <= 0)
                datos.SALMONELLA = "0";
            if (datos.PPC.Length <= 0)
                datos.PPC = "0";
            if (datos.OBS_ESTADO.Length <= 0)
                datos.OBS_ESTADO = " ";
            string sql = "update [DATOS_ORGANOLEPTICO] set [PH_CRUDO_AP]= "+datos.PH_CRUDO_AP.Replace(',', '.') + " ,[PH_CRUDO_DP]= "+datos.PH_CRUDO_DP.Replace(',', '.') + " ,[BRIX_CRUDO_AP]= "+datos.BRIX_CRUDO_AP.Replace(',', '.') + " ,[BRIX_CRUDO_DP]= "+datos.BRIX_CRUDO_DP.Replace(',', '.') + " ,[%_HUMEDAD]= "+datos._HUMEDAD.Replace(',', '.') + " ,[%_ES]= "+datos._ES.Replace(',', '.') + " ,[HC]= "+datos.HC.Replace(',', '.') + " ,[SACAROSA]= "+datos.SACAROSA.Replace(',', '.') + " ,[GRASA]= "+datos.GRASA.Replace(',', '.') + " ,[PROTEINA]= "+datos.PROTEINA.Replace(',', '.') + " ,[LACTOSA]= "+datos.LACTOSA.Replace(',', '.') + " ,[TEMPERATURA]= "+datos.TEMPERATURA.Replace(',', '.') + " ,[PH]= "+datos.PH.Replace(',', '.') + " ,[COLOR]= "+datos.COLOR.Replace(',', '.') + " ,[SABOR]= "+datos.SABOR.Replace(',', '.') + " ,[CORTE]= "+datos.CORTE.Replace(',', '.') + " ,[FILM]= "+ datos.FILM.Replace(',', '.') + " ,[CATA]= "+datos.CATA.Replace(',', '.') + " ,[GLUTEN]= "+datos.GLUTEN.Replace(',', '.') + " ,[CASEINA]="+ datos.CASEINA.Replace(',', '.') + " ,[LISTERIA]= "+datos.LISTERIA.Replace(',', '.') + " ,[SALMONELLA]= "+datos.SALMONELLA.Replace(',', '.') + " ,[PPC]= " +datos.PPC.Replace(',', '.') + ", [OBS_ESTADO]='"+ datos.OBS_ESTADO + "' where [ID_LOTE]=" + datos.ID_LOTE;
            con.sql_update(sql);

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
        public string GetTableData( object myData)
        {

            var echo = int.Parse(get_parametters(myData, "sEcho"));
            var displayLength = int.Parse(get_parametters(myData, "iDisplayLength"));
            var displayStart = int.Parse(get_parametters(myData, "iDisplayStart"));
            var sortOrder = get_parametters(myData, "sSortDir_0").ToString(CultureInfo.CurrentCulture);
            //var roleId = HttpContext.Current.Request.Params["roleId"].ToString(CultureInfo.CurrentCulture);
            var fecha1 = get_parametters(myData, "fecha1").ToString(CultureInfo.CurrentCulture);
            var fecha2 = get_parametters(myData, "fecha2").ToString(CultureInfo.CurrentCulture);
            if (fecha2.Length <= 0 || fecha1.Length<=0)
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
                filterRecords = filterRecords.Where(l => l.ARTICULO.ToUpperInvariant().Contains(search.ToUpperInvariant()) || l.SSCC.Contains(search) || l.DESCRIPCION.ToUpperInvariant().Contains(search2.ToUpperInvariant())).ToList() ;  // prue.ToList();
            }
            if (search1.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.ARTICULO.ToUpperInvariant().Contains(search1.ToUpperInvariant()) ).ToList();  // prue.ToList();
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
                sb.Append("\"" + result.ARTICULO + "\",");
                sb.Append("\"" + result.DESCRIPCION + "\",");
                sb.Append("\"" + result.FECHA_SSCC_CON + "\",");
                sb.Append("\"" + result.UD_ACTUAL + "\",");
                sb.Append("\"" + result.KG_ACTUAL + "\",");
                sb.Append("\"" + result.ESTADO + "\",");
                sb.Append("\"" + result.SSCC + "\",");
                sb.Append("\"" + result.LOTE_INTERNO + "\",");
                sb.Append("\"" + result.ID_LOTE + "\",");
                sb.Append("\"" + result.FECHA_CREACION + "\",");
                sb.Append("\"" + result.HORA + "\",");
                sb.Append("\"" + result.FECHACADUCIDAD + "\",");
                sb.Append("\"" + result.PH_CRUDO_AP + "\",");
                sb.Append("\"" + result.PH_CRUDO_DP + "\",");
                sb.Append("\"" + result.BRIX_CRUDO_AP + "\",");
                sb.Append("\"" + result.BRIX_CRUDO_DP + "\",");
                sb.Append("\"" + result._HUMEDAD + "\",");
                sb.Append("\"" + result._ES + "\",");
                sb.Append("\"" + result.HC + "\",");
                sb.Append("\"" + result.SACAROSA + "\",");
                sb.Append("\"" + result.GRASA + "\",");
                sb.Append("\"" + result.PROTEINA + "\",");
                sb.Append("\"" + result.LACTOSA + "\",");
                sb.Append("\"" + result.TEMPERATURA + "\",");
                sb.Append("\"" + result.PH + "\",");
                sb.Append("\"" + result.COLOR + "\",");
                sb.Append("\"" + result.SABOR + "\",");
                sb.Append("\"" + result.CORTE + "\",");
                sb.Append("\"" + result.FILM + "\",");
                sb.Append("\"" + result.CATA + "\",");
                sb.Append("\"" + result.GLUTEN + "\",");
                sb.Append("\"" + result.CASEINA + "\",");
                sb.Append("\"" + result.LISTERIA + "\",");
                sb.Append("\"" + result.SALMONELLA + "\",");
                sb.Append("\"" + result.PPC + "\",");
                sb.Append("\"" + result.ID_ESTADO + "\",");
                sb.Append("\"" + result.OBS_ESTADO + "\",");
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
                        ID_ESTADO = lin[36].ToString(),
                        OBS_ESTADO=lin[37].ToString()
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

        }
        [WebMethod]
        public string HelloWorld()
        {
            return "Hola a todos";
        }
    }
}

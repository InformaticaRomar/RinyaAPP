using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

using Utiles;
using System.Data;


namespace rinya_app.Calidad
{
    /// <summary>
    /// Descripción breve de WebService_calidad
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class WebService_calidad : WebService
    {
        [WebMethod(EnableSession = true)]

        public void update_Data(C_Calidad datos)
        {
            /*string sql = "update [QC600].[dbo].[ZONA] set [Nombre]='" + datos.Nombre + "' where [Zona]=" + datos.Zona;
            Utiles.Quality lanza = new Quality();
            lanza.sql_update(sql);*/
            string sql1 = "select SSCC.Id from SSCC where SSCC.SSCC='"+datos.SSCC+"'";
            Quality con = new Quality();
             string sscc_id= con.sql_string(sql1);
            string sql2 = "update SSCC_CON set Brix=" + datos.Brix.Replace(',', '.') + " , Color=" + datos.Color.Replace(',', '.') + " , Corte= " + datos.Corte.Replace(',', '.') + " , Film=" + datos.Film.Replace(',', '.') + " , Ph= " + datos.Ph.Replace(',', '.') +
            " , Sabor = " + datos.Sabor.Replace(',', '.') + " , Temperatura = "+datos.Temperatura.Replace(',', '.') + " , Textura = "+ datos.Textura.Replace(',', '.')+ " WHERE IdPadre ="+ sscc_id;
            con.sql_update(sql2);
            int a = 0;
        }
        public class Estados
        {
            string ID_Estado { get; set; }
            string Estado { get; set; }
        }
        [WebMethod]
        public  string GetEstados()
        {
            string sql = "select [ID_Estado] ,[Estado] FROM[QC600].[dbo].[Estado]";
            Quality con = new Quality();
            DataTable datos_estados = con.Sql_Datatable(sql);
           
           

            return Newtonsoft.Json.JsonConvert.SerializeObject(datos_estados, Newtonsoft.Json.Formatting.Indented);
        }
        [WebMethod]
        public string HelloWorld()
        {
            return "Hola a todos";
        }
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string GetTableData()
        {
            //var txt3 = Request.Form["date_input"];
            var txt3 = HttpContext.Current.Request.Form["datepicker1"];
            var txt1 = HttpContext.Current.Request["datepicker1"];
            var echo = int.Parse(HttpContext.Current.Request.Params["sEcho"]);
            var displayLength = int.Parse(HttpContext.Current.Request.Params["iDisplayLength"]);
            var displayStart = int.Parse(HttpContext.Current.Request.Params["iDisplayStart"]);
            var sortOrder = HttpContext.Current.Request.Params["sSortDir_0"].ToString(CultureInfo.CurrentCulture);
            var fecha1 = HttpContext.Current.Request.Params["fecha1"].ToString(CultureInfo.CurrentCulture);
            var fecha2 = HttpContext.Current.Request.Params["fecha2"].ToString(CultureInfo.CurrentCulture);
            var search = HttpContext.Current.Request.Params["sSearch"].ToString(CultureInfo.CurrentCulture);
            // var records = Obtener_Datos_Calidad().ToList();
            var records =  Obtener_Datos_Calidad(fecha1, fecha2);
            //Añadido Filtro columnas
           string [] filtros = HttpContext.Current.Request.QueryString.AllKeys;
            
            
            if (!records.Any())
            {
                return string.Empty;
            }
            var filterRecords = records;
            if (search.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.Fecha.ToLower().Contains(search.ToLower()) || l.Nalbaran.ToLower().Contains(search.ToLower()) || l.NomEs.ToLower().Contains(search.ToLower()) || l.aRDescrip.ToLower().Contains(search.ToLower()) || l.Cliente.ToLower().Contains(search.ToLower()) || l.loteprov.ToLower().Contains(search.ToLower()) || l.SSCC.ToLower().Contains(search.ToLower()) || l.FechaCaducidad.ToLower().Contains(search.ToLower()) || l.Hora.ToLower().Contains(search.ToLower()) || l.Color.ToLower().Contains(search.ToLower()) || l.Sabor.ToLower().Contains(search.ToLower()) || l.Textura.ToLower().Contains(search.ToLower()) || l.Corte.ToLower().Contains(search.ToLower()) || l.Film.ToLower().Contains(search.ToLower()) || l.Temperatura.ToLower().Contains(search.ToLower()) || l.Ph.ToLower().Contains(search.ToLower()) || l.Brix.ToLower().Contains(search.ToLower()) || l.ObsEstado.ToLower().Contains(search.ToLower()) || l.estado.ToLower().Contains(search.ToLower())).ToList();
            }
            foreach (string a in filtros)
            {
                if (a.Contains("sSearch_"))
                {
                    if (HttpContext.Current.Request.QueryString[a].Length > 0)
                    {
                        string columna_Buscar=System.Text.RegularExpressions.Regex.Replace(a, @"[^\d]", "");
                        var dato_buscar = HttpContext.Current.Request.Params[a].ToString(CultureInfo.CurrentCulture) ;
                        int c = -1;
                        int.TryParse(columna_Buscar, out c);
                        if (c >= 0)
                        {
                            switch (c)
                            {
                                case 0:
                                    filterRecords = filterRecords.Where(l => l.Fecha.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 1:
                                    filterRecords = filterRecords.Where(l => l.Nalbaran.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 2:
                                    filterRecords = filterRecords.Where(l => l.NomEs.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 3:
                                    filterRecords = filterRecords.Where(l => l.aRDescrip.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 4:
                                    filterRecords = filterRecords.Where(l => l.Cliente.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 5:
                                    filterRecords = filterRecords.Where(l => l.loteprov.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 6:
                                    filterRecords = filterRecords.Where(l => l.SSCC.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 7:
                                    filterRecords = filterRecords.Where(l => l.FechaCaducidad.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 8:
                                    filterRecords = filterRecords.Where(l => l.Hora.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 9:
                                    filterRecords = filterRecords.Where(l => l.Color.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 10:
                                    filterRecords = filterRecords.Where(l => l.Sabor.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 11:
                                    filterRecords = filterRecords.Where(l => l.Textura.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 12:
                                    filterRecords = filterRecords.Where(l => l.Corte.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 13:
                                    filterRecords = filterRecords.Where(l => l.Film.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 14:
                                    filterRecords = filterRecords.Where(l => l.Temperatura.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 15:
                                    filterRecords = filterRecords.Where(l => l.Ph.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 16:
                                    filterRecords = filterRecords.Where(l => l.Brix.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 17:
                                    filterRecords = filterRecords.Where(l => l.ObsEstado.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                                case 18:
                                    filterRecords = filterRecords.Where(l => l.estado.ToLower().Contains(dato_buscar.ToLower()));
                                    break;
                            }
                        }

                    }
                }
            }

            var orderedResults = sortOrder == "asc"
                                 ? filterRecords.OrderBy(o => o.Fecha).ThenBy(n => n.Hora)
                                 : filterRecords.OrderByDescending(o => o.Fecha).ThenBy(n => n.Hora);
            var itemsToSkip = displayStart == 0
                              ? 0
                              : displayStart + 1;
          


            var pagedResults = orderedResults.Skip(itemsToSkip).Take(displayLength).ToList();
            var hasMoreRecords = false;
            
            var sb = new StringBuilder();
            sb.Append(@"{" + "\"sEcho\": " + echo + ",");
            sb.Append("\"recordsTotal\": " + records.Count() + ",");
            sb.Append("\"recordsFiltered\": " + records.Count() + ",");
            sb.Append("\"iTotalRecords\": " + records.Count() + ",");
            sb.Append("\"iTotalDisplayRecords\": " + records.Count() + ",");
            sb.Append("\"aaData\": [");
            foreach (var result in pagedResults)
            {
                if (hasMoreRecords)
                {
                    sb.Append(",");
                }

                sb.Append("[");
                sb.Append("\"" + result.Fecha +"\",");
                sb.Append("\"" + result.Nalbaran +"\",");
                sb.Append("\"" + result.NomEs +"\",");
                sb.Append("\"" + result.aRDescrip +"\",");
                sb.Append("\"" + result.Cliente + "\",");
                sb.Append("\"" + result.loteprov + "\",");
                sb.Append("\"" + result.SSCC +"\",");
                sb.Append("\"" + result.FechaCaducidad +"\",");
                sb.Append("\"" + result.Hora +"\",");
                sb.Append("\"" + result.Color +"\",");
                sb.Append("\"" + result.Sabor +"\",");
                sb.Append("\"" + result.Textura +"\",");
                sb.Append("\"" + result.Corte +"\",");
                sb.Append("\"" + result.Film +"\",");
                sb.Append("\"" + result.Temperatura +"\",");
                sb.Append("\"" + result.Ph +"\",");
                sb.Append("\"" + result.Brix +"\",");
                sb.Append("\"" + result.ObsEstado +"\",");
                sb.Append("\"" + result.estado + "\",");
                sb.Append("\"<img class='image-details' src='content/details_open.png' runat='server'alt='View Details'/>\"");
                sb.Append("]");
                hasMoreRecords = true;
            }
            sb.Append("]}");
            return sb.ToString();
           
        }
        public IEnumerable<C_Calidad> Obtener_Datos_Calidad( string fecha1, string fecha2)
        {
            string sql = @"select CONVERT(varchar,SSCC_CON.Fecha,103) as Fecha,
                    [STOCK PARTIDAS].[Nº Partida] AS [Nº Albarán], 
                    estado.Estado AS NomEs, 
                    ARTICULO.Descripción, 
                    case when ([STOCK PARTIDAS].Serie='D')then ''else  RECEPCION_LIN.Temperatura  end AS Cliente,
                    case when Len([STOCK PARTIDAS].LoteInterno)>0 then [STOCK PARTIDAS].LoteInterno else RECEPCION_LIN.CampoAuxText4 end AS LoteProv,
                    SSCC.SSCC, 
                    CONVERT(varchar,[STOCK PARTIDAS].FechaCaducidad,103) as FechaCaducidad,
                    CONVERT(varchar,SSCC_CON.Fecha,108) as Hora,
                    coalesce(SSCC_CON.Color,'') as Color ,
                    coalesce(SSCC_CON.Sabor,'') as Sabor,
                    coalesce(SSCC_CON.Textura,'') as Textura,
                    coalesce(SSCC_CON.Corte,'') as Corte,
                    coalesce(SSCC_CON.Film,'') as Film,
                    coalesce(SSCC_CON.Temperatura,'') as Temperatura,
                    coalesce(SSCC_CON.Ph,'') as Ph, 
                    coalesce(SSCC_CON.Brix,'') as Brix,
                    coalesce(SSCC_CON.ObsEstado,'') as ObsEstado,
                    SSCC_CON.Estado	 
                    FROM ((SSCC INNER JOIN SSCC_CON ON SSCC.Id = SSCC_CON.IdPadre) INNER JOIN (ARTICULO INNER JOIN [STOCK PARTIDAS] ON ARTICULO.Artículo = [STOCK PARTIDAS].Artículo) ON SSCC_CON.IdLote = [STOCK PARTIDAS].IDSSCC) INNER JOIN estado ON SSCC_CON.Estado = estado.ID_Estado
					 LEFT OUTER JOIN 
					  RECEPCION_LIN on  RECEPCION_LIN.SSCCGenerado=SSCC.SSCC and RECEPCION_LIN.Año=[STOCK PARTIDAS].Año
					  and RECEPCION_LIN.[Nº Albarán]=[STOCK PARTIDAS].[Nº Partida] and RECEPCION_LIN.Empresa=[STOCK PARTIDAS].Empresa and RECEPCION_LIN.serie=[STOCK PARTIDAS].Serie

                    WHERE SSCC_CON.Fecha Between convert(datetime,'" + fecha1+ @"',103) and convert(datetime,'"+fecha2+@"',103)";
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);
            List<C_Calidad> lista_datos = new List<C_Calidad>();
            if (datos != null) { 
            foreach (DataRow lin in datos.AsEnumerable())
            {
                C_Calidad linea = new C_Calidad
                {
                    Fecha = lin[0].ToString(),
                    Nalbaran = lin[1].ToString(),
                    NomEs = lin[2].ToString(),
                    aRDescrip = lin[3].ToString(),
                    Cliente= lin[4].ToString(),
                    loteprov= lin[5].ToString(),
                    SSCC = lin[6].ToString(),
                    FechaCaducidad = lin[7].ToString(),
                    Hora = lin[8].ToString(),
                    Color = lin[9].ToString(),
                    Sabor = lin[10].ToString(),
                    Textura = lin[11].ToString(),
                    Corte = lin[12].ToString(),
                    Film = lin[13].ToString(),
                    Temperatura = lin[14].ToString(),
                    Ph = lin[15].ToString(),
                    Brix = lin[16].ToString(),
                    ObsEstado = lin[17].ToString(),
                    estado = lin[18].ToString()
                };
                lista_datos.Add(linea);
            }
            }

            return lista_datos;
        }
        public class C_Calidad
        {
            public string Fecha { get; set; }
            public string Nalbaran { get; set; }
            public string NomEs { get; set; }
            public string aRDescrip { get; set; }
            public string Cliente { get; set; }
            public string loteprov { get; set; }
            public string SSCC { get; set; }
            public string FechaCaducidad { get; set; }
            public string Hora { get; set; }
            public string Color { get; set; }
            public string Sabor { get; set; }
            public string Textura { get; set; }
            public string Corte { get; set; }
            public string Film { get; set; }
            public string Temperatura { get; set; }
            public string Ph { get; set; }

            public string Brix { get; set; }
            public string ObsEstado { get; set; }
            public string estado { get; set; }




        }
    }
}

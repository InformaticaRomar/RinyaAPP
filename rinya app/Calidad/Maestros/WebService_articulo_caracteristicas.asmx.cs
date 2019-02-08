using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Utiles;
using System.Data;
using OfficeOpenXml;

namespace rinya_app.Calidad.Maestros
{
    /// <summary>
    /// Descripción breve de WebService_articulo_caracteristicas
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
     [System.Web.Script.Services.ScriptService]
    public class WebService_articulo_caracteristicas : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        public string GetCaracteristicas()
        {
            string sql = "select [Cod_Caracteristica] ,  convert(varchar,[Cod_Caracteristica]) +' | ' + [Caracteristica] as Caracteristica, Tipo_Dato  FROM [QC600].[dbo].[Organolectico_Carac] where [Cod_Caracteristica]>0";
            Quality con = new Quality();
            DataTable datos_estados = con.Sql_Datatable(sql);
            return Newtonsoft.Json.JsonConvert.SerializeObject(datos_estados, Newtonsoft.Json.Formatting.Indented);
        }
        [WebMethod(EnableSession = true)]
        public bool copia_Articulo(CopiarARticulos datos)
        {
            bool result = false;
            string sql = @"SELECT * FROM  [CARACTERISTICAS_ARTICULO] where [Articulo]=" + datos.articuloA;
            Quality con = new Quality();
            DataTable datos_estados = con.Sql_Datatable(sql);
            foreach(DataRow a in datos_estados.AsEnumerable())
            {
                sql = "";
              string Caracteristica=  a[2].ToString();
                string V_MIN = a[3].ToString();
                string V_MAX = a[4].ToString();
                string obliga = a[5].ToString();
                string tipo = a[6].ToString();
                string a_lote = a[7].ToString();
                string sql2 = @"SELECT COUNT(*) FROM  [CARACTERISTICAS_ARTICULO] where [Articulo]=" + datos.articuloB + " and [Caracteristica]=" + Caracteristica;
                string existe = con.sql_string(sql2);
                int exist = 0;
                int.TryParse(existe, out exist);

                if (exist > 0)
                {
                    sql = "UPDATE [CARACTERISTICAS_ARTICULO] set [V_Min]="+ V_MIN + @",[V_Max]="+ V_MAX + ",[Obligatorio]="+obliga + ",[A_Lote]="+ a_lote + @"WHERE  [Articulo]=" + datos.articuloB + " [Caracteristica]=" + Caracteristica ;
                }
                else
                {
                    sql = "Insert INTO [CARACTERISTICAS_ARTICULO]  ([Articulo],[Caracteristica], [V_Min],[V_Max],[Obligatorio],[A_Lote] ) values (" + datos.articuloB + " , " + Caracteristica
                       + " , " + V_MIN + " , " + V_MAX + " , " + obliga+ " , " + a_lote+ " )";
                  
                }
                if(sql.Length>0)
                    con.sql_update(sql);
                result = true;
            }
            return result;
            // return Newtonsoft.Json.JsonConvert.SerializeObject(datos_estados, Newtonsoft.Json.Formatting.Indented);
        }

        [WebMethod(EnableSession = true)]
        public string GetCaracteristicas2(Caracteristicas datos)
        {
            string sql = "select  Tipo_Dato  FROM [QC600].[dbo].[Organolectico_Carac] where Cod_Caracteristica="+datos.Cod_Caracteristica;
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
        public string Get_all_Articulos()
        {
            string sql = "select [Artículo] as Articulo, cast ([Artículo] as varchar) +' | '+[Descripción] as Descripcion FROM [QC600].[dbo].[ARTICULO]    where Activo<>0 and [Artículo] not in (SELECT distinct[Articulo] FROM[QC600].[dbo].[CARACTERISTICAS_ARTICULO])";
            Quality con = new Quality();
            DataTable datos_estados = con.Sql_Datatable(sql);
            return Newtonsoft.Json.JsonConvert.SerializeObject(datos_estados, Newtonsoft.Json.Formatting.Indented);
        }
        [WebMethod(EnableSession = true)]
        public void New_Data(Caracteristicas datos)
        {
            Quality con = new Quality();
            string existe = con.sql_string(@"SELECT COUNT(*) FROM  [CARACTERISTICAS_ARTICULO] where [Articulo]=" + datos.articulo + " , [Caracteristica]=" + datos.Cod_Caracteristica);
            int exist = 0;
            int.TryParse(existe, out exist);

            if (exist > 0)
            {

            }
            else { 
            string sql = "Insert INTO [CARACTERISTICAS_ARTICULO]  ([Articulo],[Caracteristica], [V_Min],[V_Max],[Obligatorio],[A_Lote] ) values (" + datos.articulo + " , " + datos.Cod_Caracteristica
                + " , " + datos.V_Min + " , " + datos.V_Max + " , " + datos.Obligatorio + " , " + datos.A_lote + " )";
            con.sql_update(sql);
        }

        }
        [WebMethod(EnableSession = true)]
        public void update_Data(Caracteristicas datos)
        {

            Quality con = new Quality();
            if (datos.V_Min.Contains(","))
            {
                datos.V_Min = datos.V_Min.Replace(',', '.');
            }
            if (datos.V_Max.Contains(","))
            {
                datos.V_Max = datos.V_Max.Replace(',', '.');
            }
            string sql = "update [CARACTERISTICAS_ARTICULO] set [V_Min]=" + datos.V_Min + " , [V_Max]=" + datos.V_Max + " , [Obligatorio]=" + datos.Obligatorio  + " , [A_Lote]="+datos.A_lote + " where id=" + datos.Id;
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
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public string GetTableData()
        {
            var echo = int.Parse(HttpContext.Current.Request.Params["sEcho"]);
            var displayLength = int.Parse(HttpContext.Current.Request.Params["iDisplayLength"]);
            var displayStart = int.Parse(HttpContext.Current.Request.Params["iDisplayStart"]);
            var sortOrder = HttpContext.Current.Request.Params["sSortDir_0"].ToString(CultureInfo.CurrentCulture);
            var roleId = HttpContext.Current.Request.Params["roleId"].ToString(CultureInfo.CurrentCulture);
            if (roleId.Length<=0)
            { return string.Empty;  }
            var search = HttpContext.Current.Request.Params["sSearch"].ToString(CultureInfo.CurrentCulture);
            var records = ObtenerCaracteristicas(roleId).ToList();
            if (!records.Any())
            {
                return string.Empty;
            }
            var filterRecords = records;
            if (search.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.Cod_Caracteristica.Contains(search) || l.Caracteristica.Contains(search)).ToList();  // prue.ToList();
            }
            var orderedResults = sortOrder == "asc"
                                ? filterRecords.OrderBy(o => o.Cod_Caracteristica)
                                : filterRecords.OrderByDescending(o => o.Cod_Caracteristica);
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
                sb.Append("\"" + result.Id + "\",");
                sb.Append("\"" + result.articulo + "\",");
                sb.Append("\"" + result.art_descripcion + "\",");
                sb.Append("\"" + result.Cod_Caracteristica + "\",");
                sb.Append("\"" + result.Caracteristica + "\",");
                sb.Append("\"" + result.V_Min + "\",");
                sb.Append("\"" + result.V_Max + "\",");
                sb.Append("\"" + result.Obligatorio + "\",");
                sb.Append("\"" + result.Tipo_Dato + "\",");
                sb.Append("\"" + result.A_lote + "\",");
                sb.Append("\"<img class='image-details' src='content/details_open.png' runat='server' height='16' width='16' alt='View Details'/>\"");
                sb.Append("]");
                hasMoreRecords = true;
            }
            sb.Append("]}");
            return sb.ToString();

        }
        public IEnumerable<Caracteristicas> ObtenerCaracteristicas(string articulo)
        {
            string sql = @"  select [CARACTERISTICAS_ARTICULO].id, Articulo,REPLACE(REPLACE(ARTICULO.Descripción,CHAR(10),''),CHAR(13),'') as Descripción, [CARACTERISTICAS_ARTICULO].Caracteristica,[Organolectico_Carac].Caracteristica, V_Min, V_Max, Obligatorio ,[Organolectico_Carac].Tipo_Dato,[CARACTERISTICAS_ARTICULO].A_Lote
  FROM [QC600].[dbo].[CARACTERISTICAS_ARTICULO] inner join ARTICULO on ARTICULO.Artículo=[CARACTERISTICAS_ARTICULO].[Articulo]
  inner join [Organolectico_Carac] on [CARACTERISTICAS_ARTICULO].Caracteristica= [Organolectico_Carac].Cod_Caracteristica   where [CARACTERISTICAS_ARTICULO].Articulo=" + articulo;
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);
            List<Caracteristicas> lista_datos = new List<Caracteristicas>();
            foreach (DataRow lin in datos.AsEnumerable())
            {
                Caracteristicas linea = new Caracteristicas { Id = lin[0].ToString(), articulo = lin[1].ToString(), art_descripcion = lin[2].ToString(), Cod_Caracteristica = lin[3].ToString(), Caracteristica = lin[4].ToString(), V_Min = lin[5].ToString(), V_Max = lin[6].ToString(), Obligatorio = lin[7].ToString(), Tipo_Dato = lin[8].ToString(), A_lote = lin[9].ToString() };
                lista_datos.Add(linea);
            }

            return lista_datos;
        }
        public class CopiarARticulos
        {
            public string articuloA { get; set; }
            public string articuloB { get; set; }
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
            public string Tipo_Dato { get; set; }
            public string A_lote { get; set; }
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hola a todos";
        }
    }
}

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
    /// Descripción breve de WebService_maestro_caracteristicas
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
     [System.Web.Script.Services.ScriptService]
    public class WebService_maestro_caracteristicas : System.Web.Services.WebService
    {
        
        [WebMethod(EnableSession = true)]
            public void New_Data(Caracteristicas datos)
        {
            Quality con = new Quality();
            string sql1 = "select Max([Cod_Caracteristica])+1 FROM [QC600].[dbo].[Organolectico_Carac]";
            string Cod_carac = con.sql_string(sql1);
            string sql = "Insert INTO [Organolectico_Carac]  ([Cod_Caracteristica],[Caracteristica]) values (" + Cod_carac + " , '" + datos.Caracteristica + "' )";
            con.sql_update(sql);

        }
        [WebMethod(EnableSession = true)]
        public void update_Data(Caracteristicas datos)
        {
          
            Quality con = new Quality();
            
            string sql = "update [Organolectico_Carac] set [Cod_Caracteristica]="+datos.Cod_Caracteristica + " , [Caracteristica]='"+datos.Caracteristica+ "' , [Tipo_Dato]= "+datos.Tipo_Dato +", [Zona]="+ datos.Visible_Tablet+" where Id=" + datos.Id;
            con.sql_update(sql);
           
        }

        [WebMethod(EnableSession = true)]

        public void delete_Data(Caracteristicas datos)
        {
           
            Quality con = new Quality();

            string sql = "DELETE FROM [Organolectico_Carac] where Id=" + datos.Id;
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
            var search = HttpContext.Current.Request.Params["sSearch"].ToString(CultureInfo.CurrentCulture);
            var records = ObtenerCaracteristicas().ToList();
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
                sb.Append("\"" + result.Cod_Caracteristica + "\",");
                sb.Append("\"" + result.Caracteristica + "\",");
                sb.Append("\"" + result.Tipo_Dato + "\",");
                sb.Append("\"" + result.Visible_Tablet + "\",");
                sb.Append("\"<img class='image-details' src='content/details_open.png' runat='server' height='16' width='16' alt='View Details'/>\"");
                sb.Append("]");
                hasMoreRecords = true;
            }
            sb.Append("]}");
            return sb.ToString();
            
        }
        public IEnumerable<Caracteristicas> ObtenerCaracteristicas()
        {
            string sql = @"select [Id],[Cod_Caracteristica] ,[Caracteristica],(case Tipo_Dato 
  when 1 then 'Rango'
  when 2 then 'S/N'
  when 3 then 'Info'
else 'Error' 
end ) AS Tipo_Dato,(case [Zona] 
  when 1 then 'Si'
  when 0 then 'No'
else 'Error' 
end ) AS Visible_Tablet  FROM [QC600].[dbo].[Organolectico_Carac] where [Cod_Caracteristica]>0 order by cast([Cod_Caracteristica] as int )";
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);
            List<Caracteristicas> lista_datos = new List<Caracteristicas>();
            foreach (DataRow lin in datos.AsEnumerable())
            {
                Caracteristicas linea = new Caracteristicas { Id = lin[0].ToString(), Cod_Caracteristica = lin[1].ToString(), Caracteristica = lin[2].ToString(), Tipo_Dato=lin[3].ToString() ,Visible_Tablet= lin[4].ToString() };
                lista_datos.Add(linea);
            }

            return lista_datos;
        }
        public class Caracteristicas
        {
            public string Id { get; set; }
            public string Cod_Caracteristica { get; set; }
            public string Caracteristica { get; set; }
            public string Tipo_Dato { get; set; }
            public string Visible_Tablet { get; set; }


        }
        [WebMethod(EnableSession = true)]
        public string HelloWorld()
        {
            return "Hola a todos";
        }
    }
}

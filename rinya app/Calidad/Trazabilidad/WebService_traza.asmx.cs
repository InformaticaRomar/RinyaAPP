using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Utiles;
using System.Data;

namespace rinya_app.Calidad.Trazabilidad
{
    /// <summary>
    /// Summary description for WebService_traza
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService_traza : System.Web.Services.WebService
    {

        public IEnumerable<Datos_traza> ObtenerShema_traza()
        {
            List<Datos_traza> lista_datos = new List<Datos_traza>();
            DataTable dt = dt_Datos_traza("2016","1","246");
            
           Datos_traza a = new Datos_traza();
           /* foreach (DataRow lin in dt.AsEnumerable())
            {
                Datos_traza linea = new Datos_traza { Anyo = (int) lin[0], Empresa = (int)lin[1], NDespiece = (int)lin[2], nLote = (int)lin[3], nLote_o = (int)lin[4], producto = (int)lin[5], articulo = (int)lin[6], UD = (int)lin[7], KG = (float)lin[8], Tipo_articulo = lin[9].ToString(), nivel = (int)lin[10], Familia = (int)lin[11], Linea = (int)lin[12], SSCC = lin[13].ToString() };
                lista_datos.Add(linea);
            }*/
            
            lista_datos.Add(a);
            return lista_datos;
        }
        public IEnumerable<Datos_traza2> ObtenerShema_traza2()
        {
            List<Datos_traza2> lista_datos = new List<Datos_traza2>();
            DataTable dt = dt_Datos_traza("2016", "1", "246");

            Datos_traza2 a = new Datos_traza2();
         /*   foreach (DataRow lin in dt.AsEnumerable())
            {
                Datos_traza2 linea = new Datos_traza2 { Anyo = (int)lin[0], Empresa = (int)lin[1], NDespiece = (int)lin[2], nLote = (int)lin[3], nLote_o = (int)lin[4], producto = (int)lin[5], articulo = (int)lin[6], UD = (int)lin[7], KG = (float)lin[8], Tipo_articulo = lin[9].ToString(), nivel = (int)lin[10], Familia = (int)lin[11], Linea = (int)lin[12], SSCC = lin[13].ToString() };
                lista_datos.Add(linea);
            }
            */
            lista_datos.Add(a);
            return lista_datos;
        }
        public DataTable dt_Datos_traza(string anyo,string empresa,string lote)
        {
            string sql = @"SELECT Año as Anyo, Empresa, [Nº Despiece] as NDespiece , Lote_Interno, numpalet as Npalet, [nº Lote] as nLote, nLote as nLote_o, producto,Descripcion, articulo, UD, KG, Tipo_articulo, nivel, Familia, Linea, SSCC FROM dbo.GetTraza(" + anyo+", "+ empresa+", '"+ lote+"')"; 
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);
            return datos;
        }
        public class Datos_traza
        {
            public Datos_traza() {
              /*  Anyo = 0;
                Empresa = 0;
                NDespiece = 0;
                nLote = 0;
                nLote_o = 0;
                producto = 0;
                articulo = 0;
                UD = 0;
                KG = 0;
                Tipo_articulo = "";
                nivel = 0;
                Familia = 0;
                Linea = 0;
                SSCC = "";*/
            }
           public int Anyo { get; set; }
            public int Empresa { get; set; }
            public int NDespiece { get; set; }
            public string Lote_Interno { get; set; }
            public string Npalet { get; set; }
            
            public int nLote { get; set; }
            public int nLote_o { get; set; }
            public int producto { get; set; }
            public string Descripcion { get; set; }
            public int articulo { get; set; }
            public int UD { get; set; }
            public float KG { get; set; }
            public string Tipo_articulo { get; set; }
            public int nivel { get; set; }
            public int Familia { get; set; }
            public int Linea { get; set; }
            public string SSCC { get; set; }

            
        }
        public class Datos_traza2
        {
            public Datos_traza2()
            {
                /*  Anyo = 0;
                  Empresa = 0;
                  NDespiece = 0;
                  nLote = 0;
                  nLote_o = 0;
                  producto = 0;
                  articulo = 0;
                  UD = 0;
                  KG = 0;
                  Tipo_articulo = "";
                  nivel = 0;
                  Familia = 0;
                  Linea = 0;
                  SSCC = "";*/
            }
            public string Anyo { get; set; }
            public string Empresa { get; set; }
            public string NDespiece { get; set; }
            public string Lote_Interno { get; set; }
            public string Npalet { get; set; }
            public string Descripcion { get; set; }
            public string nLote { get; set; }
            public string nLote_o { get; set; }
            public string producto { get; set; }
            public string articulo { get; set; }
            public string UD { get; set; }
            public string KG { get; set; }
            public string Tipo_articulo { get; set; }
            public string nivel { get; set; }
            public string Familia { get; set; }
            public string Linea { get; set; }
            public string SSCC { get; set; }


        }
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}

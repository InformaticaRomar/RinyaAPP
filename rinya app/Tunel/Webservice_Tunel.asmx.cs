using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Utiles;
using System.Data;

namespace rinya_app.Tunel
{
    /// <summary>
    /// Summary description for Webservice_Tunel
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Webservice_Tunel : System.Web.Services.WebService
    {
        public IEnumerable<Control_Turno> ObtenerDatosTurno()
        {
            string sql = @"SELECT CONVERT(varchar, dbControlTemperatura.Fecha, 103), CONVERT(varchar, dbControlTemperatura.Fecha, 108) 
                         AS [Hora Toma Temperatura], 
						 dbControlTemperatura.SSCC AS Matricula,
						 DATOS_ORGANOLEPTICO.LOTE_INTERNO as [Lote], 
						 DATOS_ORGANOLEPTICO.NUMPALET as [N Palet], 
						 DATOS_ORGANOLEPTICO.ARTICULO as Articulo, 
						 ARTICULO.Descripción,
						 dbControlTemperatura.Temperatura
						 ,case when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '14:00:00', 108)  then 1 
						 when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '22:00:00', 108)  then 2
						   else 3 end as Turno
,[dbControlTemperatura].TUNEL as Tunel
,'' as Media
,'' as [Media_tunel_1]
,'' as [Media_tunel_2]
,'' as [Max_tunel_1]
,'' as [Max_tunel_2]
FROM            dbControlTemperatura INNER JOIN
                         DATOS_ORGANOLEPTICO ON DATOS_ORGANOLEPTICO.SSCC = dbControlTemperatura.SSCC INNER JOIN
                        
                         ARTICULO ON DATOS_ORGANOLEPTICO.ARTICULO = ARTICULO.Artículo
WHERE datediff (hour,dbControlTemperatura.Fecha, getdate()) between 0 and 8 
						and

						 case when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '14:00:00', 108)  then 1 
						 when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '22:00:00', 108)  then 2
						   else 3 end
						   =
						   case when CONVERT(time,getdate(), 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '15:00:00', 108)  then 1 
						 when CONVERT(time, getdate(), 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '23:00:00', 108)  then 2
						   else 3 end
order by dbControlTemperatura.Fecha  ";
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);
            List<Control_Turno> lista_datos = new List<Control_Turno>();
            foreach (DataRow lin in datos.AsEnumerable())
            {
                Control_Turno linea = new Control_Turno { Fecha = lin[0].ToString(), Hora = lin[1].ToString(), Matricula = lin[2].ToString(), Lote = lin[3].ToString(), Numpalet = lin[4].ToString(), articulo = lin[5].ToString(), descripcion = lin[6].ToString(), Temperatura = lin[7].ToString(), Turno = lin[8].ToString() , Tunel = lin[9].ToString() };
                lista_datos.Add(linea);
            }

            return lista_datos;
        }
        public IEnumerable<Control_Turno_Entrada_4> Enumeracion_4()
        {

            return new List<Control_Turno_Entrada_4>();

        }
        public IEnumerable<Control_Turno_Entrada> Enumeracion() {
            
            return new List<Control_Turno_Entrada>();


        }

        public IEnumerable<Control_Turno_Entrada> Enumeracion2()
        {

            return new List<Control_Turno_Entrada>();

        }
        public DataTable dt_Turno_entrada() { /*string sql = @"with pr1 as (SELECT CONVERT(varchar, CONTROL_TUNEL.FECHA_CREACION, 103)  AS [Fecha],  CONVERT(varchar, DATOS_ORGANOLEPTICO.FECHA_SSCC_CON, 108) as [Hora],
                       
						 DATOS_ORGANOLEPTICO.NUMPALET as [Numpalet],
						  DATOS_ORGANOLEPTICO.ARTICULO as Articulo, 
						REPLACE(REPLACE(LEFT (ARTICULO.Descripción , 45),CHAR(10),''),CHAR(13),'') as Descripcion,
						 CONTROL_TUNEL.SSCC AS Matricula,
						  DATOS_ORGANOLEPTICO.LOTE_INTERNO as [Lote], 
						  CONVERT(varchar, CONTROL_TUNEL.FECHA_CREACION, 108) as [Hora_entrada],
						 CONTROL_TUNEL.Tunel
						 ,case when CONVERT(time, CONTROL_TUNEL.FECHA_CREACION, 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '14:00:00', 108)  then 1 
						 when CONVERT(time, CONTROL_TUNEL.FECHA_CREACION, 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '22:00:00', 108)  then 2
						   else 3 end as Turno
						   , row_number() over (partition by CONTROL_TUNEL.sscc order by CONTROL_TUNEL.FECHA_CREACION) as Vuelta
FROM            CONTROL_TUNEL INNER JOIN
                         DATOS_ORGANOLEPTICO ON DATOS_ORGANOLEPTICO.SSCC = CONTROL_TUNEL.SSCC INNER JOIN
                        
                         ARTICULO ON DATOS_ORGANOLEPTICO.ARTICULO = ARTICULO.Artículo)
select pr1.* from pr1 where datediff (hour,pr1.[Hora_entrada], CONVERT(time, getdate(), 108)) between 0 and 8 
and datediff (hour,CONVERT(datetime,pr1.Fecha, 103), CONVERT(date, getdate(), 103) )<=1
	and

						 case when CONVERT(time,pr1.[Hora_entrada],108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '14:00:00', 108)  then 1 
						 when CONVERT(time,pr1.[Hora_entrada],108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '22:00:00', 108)  then 2
						   else 3 end
						   =
						   case when CONVERT(time,getdate(), 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '15:00:00', 108)  then 1 
						 when CONVERT(time, getdate(), 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '23:00:00', 108)  then 2
						   else 3 end";*/
            string sql = @"with pr1 as (SELECT CONVERT(varchar, CONTROL_TUNEL.FECHA_CREACION, 103)  AS [Fecha], 
             CONVERT(varchar, DATOS_ORGANOLEPTICO.FECHA_SSCC_CON, 108) as [Hora],
                       
						 DATOS_ORGANOLEPTICO.NUMPALET as [Numpalet],
						  DATOS_ORGANOLEPTICO.ARTICULO as Articulo, 
						REPLACE(REPLACE(LEFT (ARTICULO.Descripción , 45),CHAR(10),''),CHAR(13),'') as Descripcion,
						 CONTROL_TUNEL.SSCC AS Matricula,
						  DATOS_ORGANOLEPTICO.LOTE_INTERNO as [Lote], 
						  CONTROL_TUNEL.FECHA_CREACION,
						 CONTROL_TUNEL.Tunel
						 ,case when CONVERT(time, CONTROL_TUNEL.FECHA_CREACION, 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '14:00:00', 108)  then 1 
						 when CONVERT(time, CONTROL_TUNEL.FECHA_CREACION, 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '22:00:00', 108)  then 2
						   else 3 end as Turno
						   , row_number() over (partition by CONTROL_TUNEL.sscc order by CONTROL_TUNEL.FECHA_CREACION) as Vuelta
FROM            CONTROL_TUNEL INNER JOIN
                         DATOS_ORGANOLEPTICO ON DATOS_ORGANOLEPTICO.SSCC = CONTROL_TUNEL.SSCC INNER JOIN
                        
                         ARTICULO ON DATOS_ORGANOLEPTICO.ARTICULO = ARTICULO.Artículo)
select pr1.[Fecha],pr1.Hora,pr1.Numpalet,pr1.Articulo, pr1.Descripcion , pr1.Matricula,[Lote], CONVERT(varchar,FECHA_CREACION,108) as [Hora_entrada],Tunel ,Turno,Vuelta from pr1 
where datediff (hour,pr1.FECHA_CREACION,  getdate()) between 0 and 8 

	and

						 case when CONVERT(time,pr1.FECHA_CREACION,108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '14:00:00', 108)  then 1 
						 when CONVERT(time,pr1.FECHA_CREACION,108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '22:00:00', 108)  then 2
						   else 3 end
						   =
						   case when CONVERT(time,getdate(), 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '15:00:00', 108)  then 1 
						 when CONVERT(time, getdate(), 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '23:00:00', 108)  then 2
						   else 3 end
						   order by pr1.FECHA_CREACION";
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);


            return datos;
        }

        public DataTable dt_Turno_entrada_tunel4()
        {
            string sql = @"with pr1 as (SELECT CONVERT(varchar, CONTROL_TUNEL.FECHA_CREACION, 103)  AS [Fecha],  CONVERT(varchar, DATOS_ORGANOLEPTICO.FECHA_SSCC_CON, 108) as [Hora],
                       
						 DATOS_ORGANOLEPTICO.NUMPALET as [Numpalet],
						  DATOS_ORGANOLEPTICO.ARTICULO as Articulo, 
						 ARTICULO.Descripción as Descripcion,
						 CONTROL_TUNEL.SSCC AS Matricula,
						  DATOS_ORGANOLEPTICO.LOTE_INTERNO as [Lote], 
						  CONVERT(varchar, CONTROL_TUNEL.FECHA_CREACION, 108) as [Hora_entrada],
						 CONTROL_TUNEL.Tunel,
						 (select t1.Temperatura from dbControlTemperatura t1 where Fecha= (select max(t2.fecha) from dbControlTemperatura t2 where t1.sscc=t2.SSCC group by t2.SSCC)  and t1.SSCC=CONTROL_TUNEL.SSCC) as Temperatura
						 ,case when CONVERT(time, CONTROL_TUNEL.FECHA_CREACION, 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '14:00:00', 108)  then 1 
						 when CONVERT(time, CONTROL_TUNEL.FECHA_CREACION, 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '22:00:00', 108)  then 2
						   else 3 end as Turno
						   , row_number() over (partition by CONTROL_TUNEL.sscc order by CONTROL_TUNEL.FECHA_CREACION) as Vuelta
FROM            CONTROL_TUNEL INNER JOIN
                         DATOS_ORGANOLEPTICO ON DATOS_ORGANOLEPTICO.SSCC = CONTROL_TUNEL.SSCC INNER JOIN
                        
                         ARTICULO ON DATOS_ORGANOLEPTICO.ARTICULO = ARTICULO.Artículo
						
						)
select pr1.*  from pr1 where tunel=4 and Temperatura>8 ";
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);


            return datos;
        }
        public DataTable dt_Turno(string responsable)
        {
            //,coalesce (dbControlTemperatura.Responsable, '') as Responsable ,'"+ responsable+ @"' as Responsable
            string sql = @"SELECT CONVERT(varchar, dbControlTemperatura.Fecha, 103) as Fecha, CONVERT(varchar, dbControlTemperatura.Fecha, 108) 
                         AS Hora, 
						 dbControlTemperatura.SSCC AS Matricula,
						 DATOS_ORGANOLEPTICO.LOTE_INTERNO as [Lote], 
						 DATOS_ORGANOLEPTICO.NUMPALET as Numpalet, 
						 DATOS_ORGANOLEPTICO.ARTICULO as articulo, 
						 ARTICULO.Descripción as descripcion,
						 dbControlTemperatura.Temperatura as Temperatura
						,case when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '14:00:00', 108)  then 1 
						 when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '22:00:00', 108)  then 2
						   else 3 end as Turno
,[dbControlTemperatura].TUNEL as Tunel
,'" + Media() + @"' as Media
,'" + Media_tunel_1() + @"' as [Media_tunel_1]
,'" + Media_tunel_2() + @"' as [Media_tunel_2]
,'" + Max_tunel_1() + @"' as [Max_tunel_1]
,'" + Max_tunel_2() + @"' as [Max_tunel_2]
FROM            dbControlTemperatura INNER JOIN
                         DATOS_ORGANOLEPTICO ON DATOS_ORGANOLEPTICO.SSCC = dbControlTemperatura.SSCC INNER JOIN
                        
                         ARTICULO ON DATOS_ORGANOLEPTICO.ARTICULO = ARTICULO.Artículo
WHERE datediff (hour,dbControlTemperatura.Fecha, getdate()) between 0 and 8 
						and

						 case when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '14:00:00', 108)  then 1 
						 when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '22:00:00', 108)  then 2
						   else 3 end
						   =
						   case when CONVERT(time,getdate(), 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '15:00:00', 108)  then 1 
						 when CONVERT(time, getdate(), 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '23:00:00', 108)  then 2
						   else 3 end
order by dbControlTemperatura.Fecha  ";
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);
            

            return datos;
        }
        public DataTable dt_fechas_tunel(string desde, string hasta)
        { 
            string sql = @"with pr1 as (SELECT CONVERT(varchar, CONTROL_TUNEL.FECHA_CREACION, 103)  AS [Fecha], 
             CONVERT(varchar, DATOS_ORGANOLEPTICO.FECHA_SSCC_CON, 108) as [Hora],
                       
						 DATOS_ORGANOLEPTICO.NUMPALET as [Numpalet],
						  DATOS_ORGANOLEPTICO.ARTICULO as Articulo, 
						REPLACE(REPLACE(LEFT (ARTICULO.Descripción , 45),CHAR(10),''),CHAR(13),'') as Descripcion,
						 CONTROL_TUNEL.SSCC AS Matricula,
						  DATOS_ORGANOLEPTICO.LOTE_INTERNO as [Lote], 
						  CONTROL_TUNEL.FECHA_CREACION,
						 CONTROL_TUNEL.Tunel
						 ,case when CONVERT(time, CONTROL_TUNEL.FECHA_CREACION, 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '14:00:00', 108)  then 1 
						 when CONVERT(time, CONTROL_TUNEL.FECHA_CREACION, 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '22:00:00', 108)  then 2
						   else 3 end as Turno
						   , row_number() over (partition by CONTROL_TUNEL.sscc order by CONTROL_TUNEL.FECHA_CREACION) as Vuelta
FROM            CONTROL_TUNEL INNER JOIN
                         DATOS_ORGANOLEPTICO ON DATOS_ORGANOLEPTICO.SSCC = CONTROL_TUNEL.SSCC INNER JOIN
                        
                         ARTICULO ON DATOS_ORGANOLEPTICO.ARTICULO = ARTICULO.Artículo)
select pr1.[Fecha],pr1.Hora,pr1.Numpalet,pr1.Articulo, pr1.Descripcion , pr1.Matricula,[Lote], CONVERT(varchar,FECHA_CREACION,108) as [Hora_entrada],Tunel ,Turno,Vuelta from pr1 
where  pr1.FECHA_CREACION BETWEEN CONVERT(DATETIME,'" + desde + @"',103) AND CONVERT(DATETIME,'" + hasta + @"',103)

						   order by pr1.FECHA_CREACION";
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);


            return datos;
        }


        public DataTable dt_fechas(string desde, string hasta)
        {
            //,coalesce (dbControlTemperatura.Responsable, '') as Responsable ,'"+ responsable+ @"' as Responsable
            string sql = @"SELECT CONVERT(varchar, dbControlTemperatura.Fecha, 103) as Fecha, CONVERT(varchar, dbControlTemperatura.Fecha, 108) 
                         AS Hora, 
						 dbControlTemperatura.SSCC AS Matricula,
						 DATOS_ORGANOLEPTICO.LOTE_INTERNO as [Lote], 
						 DATOS_ORGANOLEPTICO.NUMPALET as Numpalet, 
						 DATOS_ORGANOLEPTICO.ARTICULO as articulo, 
						 ARTICULO.Descripción as descripcion,
						 dbControlTemperatura.Temperatura as Temperatura
						,case when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '14:00:00', 108)  then 1 
						 when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '22:00:00', 108)  then 2
						   else 3 end as Turno
,[dbControlTemperatura].TUNEL as Tunel
,'" + Media() + @"' as Media
,'" + Media_tunel_1() + @"' as [Media_tunel_1]
,'" + Media_tunel_2() + @"' as [Media_tunel_2]
,'" + Max_tunel_1() + @"' as [Max_tunel_1]
,'" + Max_tunel_2() + @"' as [Max_tunel_2]
FROM            dbControlTemperatura INNER JOIN
                         DATOS_ORGANOLEPTICO ON DATOS_ORGANOLEPTICO.SSCC = dbControlTemperatura.SSCC INNER JOIN
                        
                         ARTICULO ON DATOS_ORGANOLEPTICO.ARTICULO = ARTICULO.Artículo
WHERE dbControlTemperatura.Fecha BETWEEN CONVERT(DATETIME,'"+desde+ @"',103) AND CONVERT(DATETIME,'" + hasta + @"',103)
order by dbControlTemperatura.Fecha  ";
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);


            return datos;
        }
        private string Media() {
            string sql = @"select avg (dbControlTemperatura.Temperatura) 
                         FROM  dbControlTemperatura INNER JOIN
                         DATOS_ORGANOLEPTICO ON DATOS_ORGANOLEPTICO.SSCC = dbControlTemperatura.SSCC INNER JOIN
                        
                         ARTICULO ON DATOS_ORGANOLEPTICO.ARTICULO = ARTICULO.Artículo
WHERE datediff (hour,dbControlTemperatura.Fecha, getdate()) between 0 and 8 
						and

						 case when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '14:00:00', 108)  then 1 
						 when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '22:00:00', 108)  then 2
						   else 3 end
						   =
						   case when CONVERT(time,getdate(), 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '15:00:00', 108)  then 1 
						 when CONVERT(time, getdate(), 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '23:00:00', 108)  then 2
						   else 3 end";
Quality con = new Quality();
            return con.sql_string(sql);
        }
        private string Media_tunel_1()
        {
            string sql = @"select  avg(case when [dbControlTemperatura].TUNEL=1 then dbControlTemperatura.Temperatura else null end) 
                         FROM  dbControlTemperatura INNER JOIN
                         DATOS_ORGANOLEPTICO ON DATOS_ORGANOLEPTICO.SSCC = dbControlTemperatura.SSCC INNER JOIN
                        
                         ARTICULO ON DATOS_ORGANOLEPTICO.ARTICULO = ARTICULO.Artículo
WHERE datediff (hour,dbControlTemperatura.Fecha, getdate()) between 0 and 8 
						and

						 case when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '14:00:00', 108)  then 1 
						 when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '22:00:00', 108)  then 2
						   else 3 end
						   =
						   case when CONVERT(time,getdate(), 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '15:00:00', 108)  then 1 
						 when CONVERT(time, getdate(), 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '23:00:00', 108)  then 2
						   else 3 end";
            Quality con = new Quality();
            return con.sql_string(sql);
        }
        private string Media_tunel_2()
        {
            string sql = @"select  avg(case when [dbControlTemperatura].TUNEL=2 then dbControlTemperatura.Temperatura else null end) 
                         FROM  dbControlTemperatura INNER JOIN
                         DATOS_ORGANOLEPTICO ON DATOS_ORGANOLEPTICO.SSCC = dbControlTemperatura.SSCC INNER JOIN
                        
                         ARTICULO ON DATOS_ORGANOLEPTICO.ARTICULO = ARTICULO.Artículo
WHERE datediff (hour,dbControlTemperatura.Fecha, getdate()) between 0 and 8 
						and

						 case when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '14:00:00', 108)  then 1 
						 when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '22:00:00', 108)  then 2
						   else 3 end
						   =
						   case when CONVERT(time,getdate(), 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '15:00:00', 108)  then 1 
						 when CONVERT(time, getdate(), 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '23:00:00', 108)  then 2
						   else 3 end";
            Quality con = new Quality();
            return con.sql_string(sql);
        }
        private string Max_tunel_1()
        {
            string sql = @"select max(case when [dbControlTemperatura].TUNEL=1 then dbControlTemperatura.Temperatura else null end) 
                         FROM  dbControlTemperatura INNER JOIN
                         DATOS_ORGANOLEPTICO ON DATOS_ORGANOLEPTICO.SSCC = dbControlTemperatura.SSCC INNER JOIN
                        
                         ARTICULO ON DATOS_ORGANOLEPTICO.ARTICULO = ARTICULO.Artículo
WHERE datediff (hour,dbControlTemperatura.Fecha, getdate()) between 0 and 8 
						and

						 case when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '14:00:00', 108)  then 1 
						 when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '22:00:00', 108)  then 2
						   else 3 end
						   =
						   case when CONVERT(time,getdate(), 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '15:00:00', 108)  then 1 
						 when CONVERT(time, getdate(), 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '23:00:00', 108)  then 2
						   else 3 end";
            Quality con = new Quality();
            return con.sql_string(sql);
        }
        private string Max_tunel_2()
        {
            string sql = @"select max(case when [dbControlTemperatura].TUNEL=2 then dbControlTemperatura.Temperatura else null end) 
                         FROM  dbControlTemperatura INNER JOIN
                         DATOS_ORGANOLEPTICO ON DATOS_ORGANOLEPTICO.SSCC = dbControlTemperatura.SSCC INNER JOIN
                        
                         ARTICULO ON DATOS_ORGANOLEPTICO.ARTICULO = ARTICULO.Artículo
WHERE datediff (hour,dbControlTemperatura.Fecha, getdate()) between 0 and 8 
						and

						 case when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '14:00:00', 108)  then 1 
						 when CONVERT(time, dbControlTemperatura.Fecha, 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '22:00:00', 108)  then 2
						   else 3 end
						   =
						   case when CONVERT(time,getdate(), 108) between CONVERT(time, '06:00:00', 108)  and CONVERT(time, '15:00:00', 108)  then 1 
						 when CONVERT(time, getdate(), 108) between CONVERT(time, '14:01:00', 108)  and CONVERT(time, '23:00:00', 108)  then 2
						   else 3 end";
            Quality con = new Quality();
            return con.sql_string(sql);
        }
        public class Control_Turno_Entrada
        {
            public Control_Turno_Entrada() { }
            public string Fecha { get; set; }
            public string Hora { get; set; }
            public string Numpalet { get; set; }
            public string articulo { get; set; }
            public string Descripcion { get; set; }
            public string Matricula { get; set; }
            public string Hora_entrada { get; set; }

            public string Lote { get; set; }
            public string Turno { get; set; }
            public string Tunel { get; set; } //Max_tunel_2
            public string Vuelta { get; set; }
        }
        public class Control_Turno_Entrada_4
        {
            public Control_Turno_Entrada_4() { }
            public string Fecha { get; set; }
            public string Hora { get; set; }
            public string Numpalet { get; set; }
            public string articulo { get; set; }
            public string Descripcion { get; set; }
            public string Matricula { get; set; }
            public string Hora_entrada { get; set; }
            public string Lote { get; set; }
            public string temperatura { get; set; }
            public string Turno { get; set; }
            public string Tunel { get; set; } //Max_tunel_2
            public string Vuelta { get; set; }
        }
        public class Control_Turno
        {
            public string Fecha { get; set; }
            public string Hora { get; set; }
            public string Matricula { get; set; }
            public string Lote { get; set; }
            public string Numpalet { get; set; }
            public string articulo { get; set; }
            public string descripcion { get; set; }
            public string Temperatura { get; set; }
            public string Turno { get; set; }
            public string Tunel { get; set; } //Max_tunel_2
            public string Media { get; set; }
            public string Media_tunel_1 { get; set; }
            public string Media_tunel_2 { get; set; }
            public string Max_tunel_1 { get; set; }
            public string Max_tunel_2 { get; set; }
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}

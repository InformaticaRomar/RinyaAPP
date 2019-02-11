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
using System.IO;

namespace rinya_app.Calidad
{
    public static class Extenders

    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection, string tableName)

        {

            DataTable tbl = ToDataTable(collection);

            tbl.TableName = tableName;

            return tbl;

        }



        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)

        {

            DataTable dt = new DataTable();

            Type t = typeof(T);

            System.Reflection.PropertyInfo[] pia = t.GetProperties();

            //Create the columns in the DataTable

            foreach (System.Reflection.PropertyInfo pi in pia)

            {

                dt.Columns.Add(pi.Name, pi.PropertyType);

            }

            //Populate the table

            foreach (T item in collection)

            {

                DataRow dr = dt.NewRow();

                dr.BeginEdit();

                foreach (System.Reflection.PropertyInfo pi in pia)

                {

                    dr[pi.Name] = pi.GetValue(item, null);

                }

                dr.EndEdit();

                dt.Rows.Add(dr);

            }

            return dt;

        }

    }
    /// <summary>
    /// Descripción breve de WebService_calidad
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]

    /*
    select CARACTERISTICAS_DATOS.ARTICULO as Articulo,CARACTERISTICAS_DATOS.SSCC as Matricula, [STOCK PARTIDAS].LoteInterno as [L.Interno-Orden], [Organolectico_Carac].Caracteristica, 
case when CARACTERISTICAS_DATOS.CARACTERISTICA =0 then (SELECT [Estado] FROM [QC600].[dbo].[Estado]where[Estado].ID_Estado= CARACTERISTICAS_DATOS.VALOR) else cast (CARACTERISTICAS_DATOS.VALOR as varchar ) end Valor, CARACTERISTICAS_DATOS.usuario as Usuario
,CARACTERISTICAS_DATOS.fecha as Fecha
 from CARACTERISTICAS_DATOS 
inner join [Organolectico_Carac] on CARACTERISTICAS_DATOS.CARACTERISTICA=[Organolectico_Carac].[Cod_Caracteristica]
inner join [STOCK PARTIDAS] on [STOCK PARTIDAS].IDSSCC =CARACTERISTICAS_DATOS.ID_LOTE
order by fecha desc
    */
    public class WebService_Organoleptico : System.Web.Services.WebService
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
        private DataTable datos_Pre_excel(DataTable dat)
        {
            string sql = @"select Artículo as ART,ARTICULO.Descripción,ARTICULO.Familia, AGRUPACION.Agrupación,AGRUPACION.Descripción, ARTICULO.Discrim_4, ARTICULO.[Area Preparación] from ARTICULO
inner join FAMILIA on FAMILIA.Familia = ARTICULO.Familia inner join AGRUPACION on AGRUPACION.Agrupación = FAMILIA.Agrupación
where ARTICULO.Activo <> 0";
            Quality con = new Quality();
            DataTable table2 = con.Sql_Datatable(sql);
            DataTable table = new DataTable("DATOS");
            DataColumn[] newcolumns = new DataColumn[dat.Columns.Count];
            DataColumn[] newcolumns2 = new DataColumn[table2.Columns.Count];

            for (int i = 0; i < dat.Columns.Count; i++)

            {

                newcolumns[i] = new DataColumn(dat.Columns[i].ColumnName, dat.Columns[i].DataType);

            }
            for (int i = 0; i < table2.Columns.Count; i++)
            {
                newcolumns2[i] = new DataColumn(table2.Columns[i].ColumnName, table2.Columns[i].DataType);
            }
            table.Columns.AddRange(newcolumns);
            table.Columns.AddRange(newcolumns2);
            table.BeginLoadData();
            foreach (DataRow row in dat.Rows)
            {
                DataRow fila = table.NewRow();
               
                for (int i =0; i <= table.Columns.Count;i++)
                {
                    if ( i< dat.Columns.Count)
                    {
                        fila[i] = row[i];

                    }
                    else
                    {
                        DataRow resultado = table2.Select("ART=" + row["ARTICULO"].ToString()).FirstOrDefault();
                        if (resultado != null)
                        {
                            int j = i - (dat.Columns.Count) ;
                            if (j< table2.Columns.Count)
                            fila[i] = resultado[j];
                        }
                    }

                }
                table.LoadDataRow(fila.ItemArray, true);
              

            }
            
           
          /*  for (int r = 0; r < table2.Rows.Count; r++)
            {
                for (int c = 0; c < table2.Columns.Count; c++)
                {
                    string cn = table2.Columns[c].ColumnName.ToString();
                    table.Rows[r][cn] = table2.Rows[r][cn];
                }
            }*/

            table.EndLoadData();
            return table;
        }
        private DataTable datos_Pre_excel2(DataTable dat)
        {
          /*  string sql = @"select Convert (varchar,Artículo) as ARTICULO,ARTICULO.Descripción,ARTICULO.Familia, AGRUPACION.Agrupación,AGRUPACION.Descripción, ARTICULO.Discrim_4, ARTICULO.[Area Preparación] from ARTICULO
inner join FAMILIA on FAMILIA.Familia = ARTICULO.Familia inner join AGRUPACION on AGRUPACION.Agrupación = FAMILIA.Agrupación
where ARTICULO.Activo <> 0";
            Quality con = new Quality();
            DataTable table2 = con.Sql_Datatable(sql);
            DataTable dtResult = new DataTable();
            var result =
from dataRows1 in dat.AsEnumerable()
join dataRows2 in table2.AsEnumerable() on dataRows1.Field<string>("ARTICULO") equals dataRows2.Field<string>("ARTICULO") into ps
from r in ps.DefaultIfEmpty()
select new { C = dataRows1, r == null ? "0" : dataRows2.Field<string>("ARTICULO") };*/

          /*  var result = from dataRows1 in dat.AsEnumerable()
                         join dataRows2 in table2.AsEnumerable()
                         on dataRows1.Field<string>("ARTICULO") equals dataRows2.Field<string>("ARTICULO")

                         select dtResult.LoadDataRow(new object[]
                         {
                dataRows1,
               
                dataRows2,
                          }, false);*/
            //result.CopyToDataTable();

            /* var result = from dataRows1 in dat.AsEnumerable()
                          join dataRows2 in table2.AsEnumerable()
                          on dataRows1.Field<string>("ARTICULO") equals dataRows2.Field<string>("ARTICULO") into rows
                          from row in rows.DefaultIfEmpty().se;*/
            /*  select dtResult.LoadDataRow(new object[]
              {
 dataRows1.Field<string>("ID"),
 dataRows1.Field<string>("name"),
 row==null? null : row.Field<int>("stock"),
               }, false);*/

            return null; 
        }


        [WebMethod(EnableSession = true)]
        public bool GetDocument()
        {
            bool resultado = false;
            string strdocPath;
            strdocPath = HttpContext.Current.Server.MapPath(".")+ @"\documentos\Organoleptico.xlsx";
            using (ExcelPackage pck = new ExcelPackage())
            {
                // date columns
                List<string> dateColumns = new List<string>() {

                    "WCPC_FECHAPED",
                    "Fecha Pedido"
                };

                // hide columns
                List<string> hideColumns = new List<string>() {

                    "RecordID",
                    "CategoryID"
                };
                List<string> visibleColumns = new List<string>() {

                    "ARTICULO",
                    "DESCRIPCION",
                    "LOTE_INTERNO",
                    "NUMPALET",
                    "FECHA_SSCC_CON",
                    "ESTADO",
                    "SSCC",
                    "FECHACADUCIDAD"
                };

                DataTable table = datos_Pre_excel(Session["datos"] as DataTable);
                //DataTable table2 =new DataTable();
                int j = 0;
                foreach (DataColumn col in table.Columns)
                {
                   
                    string nombre = Get_caracteristicas_nombre(col.ColumnName);
                    if (nombre.Length > 0)
                        table.Columns[col.ColumnName].ColumnName = nombre;
                    else {
                        int a = 0;
                            }
                }
                    List<string> articulos = new List<string>();
                var dat = table.AsEnumerable().Select(r => r["ARTICULO"]).Distinct();
                foreach (var d in dat)
                {
                    articulos.Add(d.ToString());
                }
                if (articulos.Count() > 0)
                {
                    hideColumns.Clear();
                    hideColumns.Add("ID");
                    hideColumns.Add("UD_ACTUAL");
                    hideColumns.Add("FECHA_CREACION");
                    hideColumns.Add("KG_ACTUAL");
                    hideColumns.Add("ID_LOTE");
                    hideColumns.Add("HORA");
                    hideColumns.Add("ID_ESTADO");
                    hideColumns.Add("NC");
                    hideColumns.Add("D");
                    hideColumns.Add("MERMA");
                    hideColumns.Add("ESTADO_COMITE");
                    hideColumns.Add("HORA_ARRIBA");
                    hideColumns.Add("ESTADO_ARRIBA");
                    hideColumns.Add("HORA_MEDIO");
                    hideColumns.Add("ESTADO_MEDIO");
                    hideColumns.Add("HORA_ABAJO");
                    hideColumns.Add("ESTADO_ABAJO");
                    hideColumns.Add("OBSEVACIONES_COMITE");
                    hideColumns.Add("OBSERVA_ARRIBA");
                    hideColumns.Add("OBSERVA_MEDIO");
                    hideColumns.Add("OBSERVA_ABAJO");
                    string sql = @"SELECT DISTINCT [Caracteristica] FROM[QC600].[dbo].[Organolectico_Carac] where[Cod_Caracteristica] not in (select[Caracteristica] FROM[QC600].[dbo].[CARACTERISTICAS_ARTICULO]  where";
                    for (int i = 0; i < articulos.Count(); i++)
                    {
                        if (i == 0)
                        {
                            sql = sql + " [Articulo] = " + articulos[i];
                        }
                        else
                        {
                            sql = sql + " AND [Articulo] = " + articulos[i];
                        }

                    }
                    sql = sql + ")";

                    Quality con = new Quality();
                    DataTable datos_caracteristicas = con.Sql_Datatable(sql);
                    foreach (DataRow c in datos_caracteristicas.AsEnumerable())
                    {
                       // string column = nombre_columna_caracteristica(c[0].ToString());
                        if (c[0].ToString().Length > 0)
                            hideColumns.Add(c[0].ToString());

                    }

                     sql = @"select distinct [Organolectico_Carac].Caracteristica FROM [QC600].[dbo].[CARACTERISTICAS_ARTICULO]  inner join [QC600].[dbo].[Organolectico_Carac] on [QC600].[dbo].[Organolectico_Carac].Cod_Caracteristica=[QC600].[dbo].[CARACTERISTICAS_ARTICULO].[Caracteristica] where";
                    for (int i = 0; i < articulos.Count(); i++)
                    {
                        if (i == 0)
                        {
                            sql = sql + " [Articulo] = " + articulos[i];
                        }
                        else
                        {
                            sql = sql + " OR [Articulo] = " + articulos[i];
                        }

                    }
                    

                   
                    DataTable datos_caracteristicas2 = con.Sql_Datatable(sql);

                    //Getcaracteristicas
                    foreach (DataRow c in datos_caracteristicas2.AsEnumerable())
                    {
                        //string column = nombre_columna_caracteristica(c[0].ToString());
                        string column = c[0].ToString();
                        if (column.Length > 0)
                            visibleColumns.Add(column);

                    }
                    visibleColumns.Add("OBS_ESTADO");
                    int columnIndex = 0;
                    foreach (string columnName in visibleColumns)
                    {
                        if (table.Columns.Contains(columnName)) {
                            table.Columns[columnName].SetOrdinal(columnIndex);
                            columnIndex++; }
                    }
                    // SetColumnsOrder(table, visibleColumns);
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                    ws.Cells["A1"].LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Medium14);

                    FormatWorksheetData(dateColumns, hideColumns, table, ws);
                    byte[] a = pck.GetAsByteArray();

                    FileStream objfilestream = new FileStream(strdocPath, FileMode.Create, FileAccess.ReadWrite);
                    objfilestream.Write(a, 0, a.Length);
                    objfilestream.Close();
                    resultado = true;
                }
            }

            return resultado;
        }
       
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void datos_Excel()
        {

           
                   // datos = Session["datos"] as DataTable;
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        // date columns
                        List<string> dateColumns = new List<string>() {

                    "WCPC_FECHAPED",
                    "Fecha Pedido"
                };

                        // hide columns
                        List<string> hideColumns = new List<string>() {

                    "RecordID",
                    "CategoryID"
                };
                DataTable table =Session["datos"] as DataTable;
               

                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");
                        ws.Cells["A1"].LoadFromDataTable(datos, true, OfficeOpenXml.Table.TableStyles.Medium14);

                        FormatWorksheetData(dateColumns, hideColumns, datos, ws);
                System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
                // make sure it is sent as a XLSX file
                Response.ContentType = "application/vnd.ms-excel";
                // make sure it is downloaded rather than viewed in the browser window
                 Response.AddHeader("Content-disposition", "attachment; filename=Pedidos_tablet.xlsx");
                // Response.BinaryWrite(pck.GetAsByteArray());
                byte[] a = pck.GetAsByteArray();
                Response.OutputStream.Write(a,0, a.Length);
                // WebOperationContext.Current.OutgoingResponse.ContentType
                //Response.OutputStream.Write(file.Bytes, 0, file.Bytes.Length);
                //Response.Close();
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                //   Response.End();
                // Response.BufferOutput = true;
                //  Response.Flush(); // Sends all currently buffered output to the client.
                // Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                // HttpContext.Current.ApplicationInstance.CompleteRequest();
                /*System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(URL);
                using (var webResponse = request.GetResponse())
                using (var webStream = webResponse.GetResponseStream())
                {
                    if (webStream != null)
                    {
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Disposition", "attachment; filename=\"test.pdf\"");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        webStream.CopyTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }*/
                /*  System.IO.BinaryReader binReader = new
                  System.IO.BinaryReader(System.IO.File.Open(Server.MapPath("Pedidos_tablet.xlsx"), System.IO.FileMode.Open,
      System.IO.FileAccess.Read));*/
                // byte[] binFile = pck.GetAsByteArray();


                // return binFile;

            }
        }
       
        [WebMethod]
        public byte[] GetFile(string filename)
        {
            System.IO.BinaryReader binReader = new
      System.IO.BinaryReader(System.IO.File.Open(Server.MapPath(filename), System.IO.FileMode.Open,
     System.IO.FileAccess.Read));
            binReader.BaseStream.Position = 0;
            byte[] binFile =
     binReader.ReadBytes(Convert.ToInt32(binReader.BaseStream.Length));
            binReader.Close();
            return binFile;
        }

        [WebMethod]
        public void PutFile(byte[] buffer, string filename)
        {
            System.IO.BinaryWriter binWriter = new System.IO.BinaryWriter(System.IO.File.Open(Server.MapPath(filename), System.IO.FileMode.CreateNew,
     System.IO.FileAccess.ReadWrite));
            binWriter.Write(buffer);
            binWriter.Close();
        }
        private string nombre_caracteristica_columna(string columna)
        {
            string caracteristica = "";
            switch (columna)
            {
                case "PH_CRUDO_AP":
                    caracteristica = "1";
                    break;
                case "PH_CRUDO_DP":
                    caracteristica = "2";
                    break;
                case "BRIX_CRUDO_AP":
                    caracteristica = "3";
                    break;
                case "BRIX_CRUDO_DP":
                    caracteristica = "4";
                    break;
                case "_HUMEDAD":
                    caracteristica = "5";
                    break;
                case "_ES":
                    caracteristica = "6";
                    break;
                case "HC":
                    caracteristica = "7";
                    break;
                case "SACAROSA":
                    caracteristica = "8";
                    break;
                case "GRASA":
                    caracteristica = "9";
                    break;
                case "PROTEINA":
                    caracteristica = "10";
                    break;
                case "LACTOSA":
                    caracteristica = "11";
                    break;
                case "TEMPERATURA":
                    caracteristica = "12";
                    break;
                case "PH":
                    caracteristica = "13";
                    break;
                case "COLOR":
                    caracteristica = "14";
                    break;
                case "SABOR":
                    caracteristica = "15";
                    break;
                case "CORTE":
                    caracteristica = "16";
                    break;
                case "FILM":
                    caracteristica = "17";
                    break;
                case "CATA":
                    caracteristica = "18";
                    break;
                case "GLUTEN":
                    caracteristica = "19";
                    break;
                case "CASEINA":
                    caracteristica = "20";
                    break;
                case "LISTERIA":
                    caracteristica = "21";
                    break;
                case "SALMONELLA":
                    caracteristica = "22";
                    break;
                case "PPC":
                    caracteristica = "23";
                    break;
                case "BOLETIN":
                    caracteristica = "24";
                    break;
                case "BRIX":
                    caracteristica = "25";
                    break;
                case "IMPUREZAS":
                    caracteristica = "26";
                    break;
                case "CONSISTENCIA":
                    caracteristica = "27";
                    break;
                case "OLOR":
                    caracteristica = "28";
                    break;
                case "EMULSION":
                    caracteristica = "29";
                    break;
                case "HR":
                    caracteristica = "30";
                    break;
                case "TS":
                    caracteristica = "31";
                    break;
                case "SNF":
                    caracteristica = "32";
                    break;
                case "ASPECTO":
                    caracteristica = "33";
                    break;
                case "DORNIC":
                    caracteristica = "34";
                    break;
                case "INHIBIDORES":
                    caracteristica = "35";
                    break;
                case "SILO":
                    caracteristica = "36";
                    break;
                case "TTRANSP":
                    caracteristica = "37";
                    break;
                case "FLATOXINA":
                    caracteristica = "49";
                    break;
                case "CONTROL_PESO_CALIDAD":
                    caracteristica = "50";
                    break;
                case "CONTROL_PESO_LINEA":
                    caracteristica = "51";
                    break;
                case "ABSORVANCIA":
                    caracteristica = "54";
                    break;
                case "WEB":
                    caracteristica = "58";
                    break;
                case "CAG":
                    caracteristica = "59";
                    break;
                case "LAVADO":
                    caracteristica = "38";
                    break;
                case "SILODEST":
                    caracteristica = "39";
                    break;
                case "EDAD_LECHE":
                    caracteristica = "40";
                    break;
                case "T_CISTERNA":
                    caracteristica = "41";
                    break;
                case "T_MUESTRA":
                    caracteristica = "42";
                    break;
                case "INH_R":
                    caracteristica = "43";
                    break;
                case "INH_L":
                    caracteristica = "44";
                    break;
                case "ALCOHOL":
                    caracteristica = "46";
                    break;
                case "C_FILTRO":
                    caracteristica = "47";
                    break;
                case "INH_BOB":
                    caracteristica = "48";
                    break;
                case "CURVA_PH":
                    caracteristica = "52";
                    break;
                case "TEXTURA":
                    caracteristica = "53";
                    break;
                case "SILO_DESTINO":
                    caracteristica = "60";
                    break;
                case "CROMATOGRAMAS_FILM":
                    caracteristica = "61";
                    break;
                case "PROTEINA_DILUCION":
                    caracteristica = "62";
                    break;
                case "GRASA_DILUCION":
                    caracteristica = "63";
                    break;
                case "TTP":
                    caracteristica = "64";
                    break;
                case "TPP":
                    caracteristica = "65";
                    break;
                case "SABOR_MERENGUE":
                    caracteristica = "70";
                    break;
                case "OLOR_TORTILLA":
                    caracteristica = "71";
                    break;
                case "SABOR_TORTILLA":
                    caracteristica = "72";
                    break;
                case "Espectofotometro":
                    caracteristica = "73";
                    break;
                case "BOLETIN_GRASA":
                    caracteristica = "74";
                    break;
                case "BOLETIN_PROTEINA":
                    caracteristica = "75";
                    break;
                case "BRIX_PASTE_2500":
                    caracteristica = "76";
                    break;
                case "BRIX_PASTE_6000":
                    caracteristica = "77";
                    break;
                case "GLUTEN_BOLETIN":
                    caracteristica = "78";
                    break;
                case "BOLETIN_LISTERIA":
                    caracteristica = "79";
                    break;
                case "BOLETIN_SALMONELLA":
                    caracteristica = "80";
                    break;
                case "PRE_BOLETIN_FQ":
                    caracteristica = "81";
                    break;
                case "BOLETIN_SOJA":
                    caracteristica = "82";
                    break;

                case "BOLETIN_CARAMELO":
                    caracteristica = "83";
                    break;


                case "ETIQUETA_CORRECTA":
                    caracteristica = "84";
                    break;

                case "CURVA_FERMENTACION":
                    caracteristica = "85";
                    break;
                case "ATP_APERTURA":
                    caracteristica = "86";
                    break;
                case "CATA_COMITE":
                    caracteristica = "87";
                    break;
                case "TEXTUROMETRO":
                    caracteristica = "88";
                    break;
                case "MUESTROTECA":
                    caracteristica = "89";
                    break;
                case "CURVA_ENFRIAMIENTO":
                    caracteristica = "90";
                    break;
                case "CURVA_FERMENTACION_":
                    caracteristica = "91";
                    break;
                case "tp_tq_mix_crudo":
                    caracteristica = "92";
                    break;
                case "tp_tq_mix_paste":
                    caracteristica = "93";
                    break;
                case "tp_tq_enfr_dosif":
                    caracteristica = "94";
                    break;
                case "ph_box":
                    caracteristica = "95";
                    break;
                case "col_box":
                    caracteristica = "96";
                    break;
                case "olor_box":
                    caracteristica = "97";
                    break;
                case "sab_box":
                    caracteristica = "98";
                    break;
                case "ºB_box":
                    caracteristica = "99";
                    break;
                case "Purez":
                    caracteristica = "100";
                    break;
                case "Org_antes_envio":
                    caracteristica = "101";
                    break;
                case "ph_(50%)":
                    caracteristica = "102";
                    break;
                case "Consistencia_20º":
                    caracteristica = "103";
                    break;
                case "Sacarasa_bol":
                    caracteristica = "104";
                    break;
                case "(%)sal_bol":
                    caracteristica = "105";
                    break;
                case "ATP_Tq_formulacion":
                    caracteristica = "106";
                    break;
                case "ATP_tq_destino":
                    caracteristica = "107";
                    break;
                case "ATP_linea":
                    caracteristica = "108";
                    break;
                case "ph_(6.67%)":
                    caracteristica = "109";
                    break;
                case "olor_tort_box":
                    caracteristica = "110";
                    break;
                case "Sabor_tort_box":
                    caracteristica = "111";
                    break;
                case "ATP_manguera":
                    caracteristica = "112";
                    break;
                case "Mohos_levad":
                    caracteristica = "113";
                    break;
                case "Presencia_ojos":
                    caracteristica = "114";
                    break;
                case "Recirculacion":
                    caracteristica = "115";
                    break;
                case "b_tras_paste":
                    caracteristica = "116";
                    break;
                case "bloom":
                    caracteristica = "117";
                    break;
                case "CC_obli":
                    caracteristica = "118";
                    break;
                case "textu_tras_tunel":
                    caracteristica = "119";
                    break;
                case "textu_revision":
                    caracteristica = "120";
                    break;
                case "HR_bol":
                    caracteristica = "121";
                    break;
                case "t_frabricacion":
                    caracteristica = "122";
                    break;
                case "Enterobac":
                    caracteristica = "123";
                    break;
                case "Den":
                    caracteristica = "124";
                    break;
                case "Contrast":
                    caracteristica = "125";
                    break;
                case "Rev_Filtros":
                    caracteristica = "126";
                    break;
                case "Gras_Box":
                    caracteristica = "127";
                    break;
                case "Prot_box":
                    caracteristica = "128";
                    break;
                case "V1":
                    caracteristica = "129";
                    break;
                case "V2":
                    caracteristica = "130";
                    break;
                case "V3":
                    caracteristica = "131";
                    break;

                default:
                    caracteristica = columna;
                    break;
            }
            return caracteristica;
        }
        private string nombre_columna_caracteristica (string caracteristica)
        {
            string columna = "";
            switch (caracteristica) { 
                case "1":
                    columna = "PH_CRUDO_AP";
                break;
                case "2":
                    columna = "PH_CRUDO_DP";
                    break;
                case "3":
                    columna = "BRIX_CRUDO_AP";
                    break;
                case "4":
                    columna = "BRIX_CRUDO_DP";
                    break;
                case "5":
                    columna = "_HUMEDAD";
                    break;
                case "6":
                    columna = "_ES";
                    break;
                case "7":
                    columna = "HC";
                    break;
                case "8":
                    columna = "SACAROSA";
                    break;
                case "9":
                    columna = "GRASA";
                    break;
                case "10":
                    columna = "PROTEINA";
                    break;
                case "11":
                    columna = "LACTOSA";
                    break;
                case "12":
                    columna = "TEMPERATURA";
                    break;
                case "13":
                    columna = "PH";
                    break;
                case "14":
                    columna = "COLOR";
                    break;
                case "15":
                    columna = "SABOR";
                    break;
                case "16":
                    columna = "CORTE";
                    break;
                case "17":
                    columna = "FILM";
                    break;
                case "18":
                    columna = "CATA";
                    break;
                case "19":
                    columna = "GLUTEN";
                    break;
                case "20":
                    columna = "CASEINA";
                    break;
                case "21":
                    columna = "LISTERIA";
                    break;
                case "22":
                    columna = "SALMONELLA";
                    break;
                case "23":
                    columna = "PPC";
                    break;
                case "24":
                    columna = "BOLETIN";
                    break;
                case "25":
                    columna = "BRIX";
                    break;
                case "26":
                    columna = "IMPUREZAS";
                    break;
                case "27":
                    columna = "CONSISTENCIA";
                    break;
                case "28":
                    columna = "OLOR";
                    break;
                case "29":
                    columna = "EMULSION";
                    break;
                case "30":
                    columna = "HR";
                    break;
                case "31":
                    columna = "TS";
                    break;
                case "32":
                    columna = "SNF";
                    break;
                case "33":
                    columna = "ASPECTO";
                    break;
                case "34":
                    columna = "DORNIC";
                    break;
                case "35":
                    columna = "INHIBIDORES";
                    break;
                case "36":
                    columna = "SILO";
                    break;
                case "37":
                    columna = "TTRANSP";
                    break;
                case "49":
                    columna = "FLATOXINA";
                    break;
                case "50":
                    columna = "CONTROL_PESO_CALIDAD";
                    break;
                case "51":
                    columna = "CONTROL_PESO_LINEA";
                    break;
                case "54":
                    columna = "ABSORVANCIA";
                    break;
                case "58":
                    columna = "WEB";
                    break;
                case "59":
                    columna = "CAG";
                    break;
                case "38":
                    columna = "LAVADO";
                    break;
                case "39":
                    columna = "SILODEST";
                    break;
                case "40":
                    columna = "EDAD_LECHE";
                    break;
                case "41":
                    columna = "T_CISTERNA";
                    break;
                case "42":
                    columna = "T_MUESTRA";
                    break;
                case "43":
                    columna = "INH_R";
                    break;
                case "44":
                    columna = "INH_L";
                    break;
                case "46":
                    columna = "ALCOHOL";
                    break;
                case "47":
                    columna = "C_FILTRO";
                    break;
                case "48":
                    columna = "INH_BOB";
                    break;
                case "52":
                    columna = "CURVA_PH";
                    break;
                case "53":
                    columna = "TEXTURA";
                    break;
                case "60":
                    columna = "SILO_DESTINO";
                    break;
                case "61":
                    columna = "CROMATOGRAMAS_FILM";
                    break;
                case "62":
                    columna = "PROTEINA_DILUCION";
                    break;
                case "63":
                    columna = "GRASA_DILUCION";
                    break;
                case "64":
                    columna = "TTP";
                    break;
                case "65":
                    columna = "TPP";
                    break;
                case "70":
                    columna = "SABOR_MERENGUE";
                    break;
                case "71":
                    columna = "OLOR_TORTILLA";
                    break;
                case "72" :
                    columna = "SABOR_TORTILLA";
                    break;
                case "73":
                    columna = "Espectofotometro";
                    break;
                case "74":
                    columna = "BOLETIN_GRASA";
                    break;
                case "75":
                    columna = "BOLETIN_PROTEINA";
                    break;
                case "76":
                    columna = "BRIX_PASTE_2500";
                    break;
                case "77":
                    columna = "BRIX_PASTE_6000";
                    break;
                case "78":
                    columna = "GLUTEN_BOLETIN";
                    break;
                case "79":
                    columna = "BOLETIN_LISTERIA";
                    break;
                case "80":
                    columna = "BOLETIN_SALMONELLA";
                    break;
                case "81":
                    columna = "PRE_BOLETIN_FQ";
                    break;
                case "82":
                    columna = "BOLETIN_SOJA";
                    break;
                case "83":
                    columna = "BOLETIN_CARAMELO";
                    break;
                case "84":
                    columna = "ETIQUETA_CORRECTA";
                    break;
                case "85":
                    columna = "CURVA_FERMENTACION";
                    break;
                case "86":
                    columna = "ATP_APERTURA";
                    break;
                case "87":
                    columna = "CATA_COMITE";
                    break;
                case "88":
                    columna = "TEXTUROMETRO";
                    break;
                case "89":
                    columna = "MUESTROTECA";
                    break;
                case "90":
                    columna = "CURVA_ENFRIAMIENTO";
                    break;
                case "91":
                    columna = "CURVA_FERMENTACION_";
                    break;
                case "92":
                    columna = "tp_tq_mix_crudo";
                    break;
                case "93":
                    columna = "tp_tq_mix_paste";
                    break;
                case "94":
                    columna = "tp_tq_enfr_dosif";
                    break;
                case "95":
                    columna = "ph_box";
                    break;
                case "96":
                    columna = "col_box";
                    break;
                case "97":
                    columna = "olor_box";
                    break;
                case "98":
                    columna = "sab_box";
                    break;
                case "99":
                    columna = "ºB_box";
                    break;
                case "100":
                    columna = "Purez";
                    break;
                case "101":
                    columna = "Org_antes_envio";
                    break;
                case "102":
                    columna = "ph_(50%)";
                    break;
                case "103":
                    columna = "Consistencia_20º";
                    break;
                case "104":
                    columna = "Sacarasa_bol";
                    break;
                case "105":
                    columna = "(%)sal_bol";
                    break;
                case "106":
                    columna = "ATP_Tq_formulacion";
                    break;
                case "107":
                    columna = "ATP_tq_destino";
                    break;
                case "108":
                    columna = "ATP_linea";
                    break;
                case "109":
                    columna = "ph_(6.67%)";
                    break;
                case "110":
                    columna = "olor_tort_box";
                    break;
                case "111":
                    columna = "Sabor_tort_box";
                    break;
                case "112":
                    columna = "ATP_manguera";
                    break;
                case "113":
                    columna = "Mohos_levad";
                    break;
                case "114":
                    columna = "Presencia_ojos";
                    break;
                case "115":
                    columna = "Recirculacion";
                    break;
                case "116":
                    columna = "b_tras_paste";
                    break;
                case "117":
                    columna = "bloom";
                    break;
                case "118":
                    columna = "CC_obli";
                    break;
                case "119":
                    columna = "textu_tras_tunel";
                    break;
                case "120":
                    columna = "textu_revision";
                    break;
                case "121":
                    columna = "HR_bol";
                    break;
                case "122":
                    columna = "t_frabricacion";
                    break;
                case "123":
                    columna = "Enterobac";
                    break;
                case "124":
                    columna = "Den";
                    break;
                case "125":
                    columna = "Contrast";
                    break;
                case "126":
                    columna = "Rev_Filtros";
                    break;
                case "127":
                    columna = "Gras_Box";
                    break;
                case "128":
                    columna = "Prot_box";
                    break;
                case "129":
                    columna = "V1";
                    break;
                case "130":
                    columna = "V2";
                    break;
                case "131":
                    columna = "V3";
                    break;
              

            }
            return columna;
        }
        private int columna_valor(int caracteristicas)
        {
            int valor = 0;
            
            switch (caracteristicas)
            {
                case 1:
                    valor = 12;
                    break;
                case 2:
                    valor = 13;
                    break;
                case 3:
                    valor = 14;
                    break;
                case 4:
                    valor = 15;
                    break;
                case 5:
                    valor = 16;
                    break;
                case 6:
                    valor = 17;
                    break;
                case 7:
                    valor = 18;
                    break;
                case 8:
                    valor = 19;
                    break;
                case 9:
                    valor = 20;
                    break;
                case 10:
                    valor = 21;
                    break;
                case 11:
                    valor = 22;
                    break;
                case 12:
                    valor = 23;
                    break;
                case 13:
                    valor = 24;
                    break;
                case 14:
                    valor = 25;
                    break;
                case 15:
                    valor = 26;
                    break;
                case 16:
                    valor = 27;
                    break;
                case 17:
                    valor = 28;
                    break;
                case 19:
                    valor = 30;
                    break;
                case 20:
                    valor = 31;
                    break;
                case 21:
                    valor = 32;
                    break;
                case 22:
                    valor = 33;
                    break;
                case 23:
                    valor = 34;
                    break;
                case 26:
                    valor = 37;
                    break;
                case 27:
                    valor = 38;
                    break;
                case 28:
                    valor = 39;
                    break;
                case 29:
                    valor = 40;
                    break;
                case 30:
                    valor = 41;
                    break;
                case 31:
                    valor = 42;
                    break;
                case 32:
                    valor = 43;
                    break;
                case 33:
                    valor = 44;
                    break;
                case 34:
                    valor = 45;
                    break;
                case 35:
                    valor = 46;
                    break;
                case 24:
                    valor = 47;
                    break;
                case 25:
                    valor = 48;
                    break;
                case 36:
                    valor = 49;
                    break;
                case 37:
                    valor = 50;
                    break;
                case 38:
                    valor = 51;
                    break;
                case 39:
                    valor = 52;
                    break;
                case 40:
                    valor = 53;
                    break;
                case 41:
                    valor = 54;
                    break;
                case 42:
                    valor = 55;
                    break;
                case 43:
                    valor = 56;
                    break;
                case 44:
                    valor = 57;
                    break;
                case 46:
                    valor = 59;
                    break;
                case 47:
                    valor = 60;
                    break;
                case 48:
                    valor = 61;
                    break;
                case 49:
                    valor = 64;
                    break;
                case 50:
                    valor = 65;
                    break;
                case 51:
                    valor = 66;
                    break;
                case 52:
                    valor = 67;
                    break;
                case 18:
                    valor = 76;
                    break;
                case 53:
                    valor = 76;
                    break;
                case 54:
                    valor = 77;
                    break;
                case 58:
                    valor = 78;
                    break;
                case 59:
                    valor = 79;
                    break;
                case 60:
                    valor = 85;
                    break;
                case 61:
                    valor = 86;
                    break;
                case 62:
                    valor = 83;
                    break;
                case 63:
                    valor = 84;
                    break;
                case 64:
                    valor = 87;
                    break;
                case 65:
                    valor = 88;
                    break;
                case 66:
                    valor = 89;
                    break;
                case 67:
                    valor = 90;
                    break;
                case 68:
                    valor = 91;
                    break;

                case 69:
                    valor = 92;
                    break;
                case 70:
                    valor = 93;
                    break;
                case 71:
                    valor = 94;
                    break;
                case 72:
                    valor = 95;
                    break;
                case 73:
                    valor = 96;
                    break;
                case 74:
                    valor = 97;
                    break;
                case 75:
                    valor = 98;
                    break;
                case 76:
                    valor = 99;
                    break;
                case 77:
                    valor = 100;
                    break;
                case 78:
                    valor = 101;
                    break;
                case 79:
                    valor = 102;
                    break;
                case 80:
                    valor = 103;
                    break;
                case 81:
                    valor = 104;
                    break;
                case 82:
                    valor = 105;
                    break;
                case 83:
                    valor = 106;
                    break;
                case 84:
                    valor = 107;
                    break;
                case 85:
                    valor = 108;
                    break;
                case 86:
                    valor = 109;
                    break;
                case 87:
                    valor = 110;
                    break;

            }
            return valor;
        }
        public string Get_caracteristicas_nombre( string caracteristica)
        {
            string cod_carac = nombre_caracteristica_columna(caracteristica);
            if (cod_carac==caracteristica)
            {
                return caracteristica;
            }
            string sql = "SELECT [Caracteristica] FROM [QC600].[dbo].[Organolectico_Carac] where Cod_Caracteristica =" + cod_carac;
            Quality con = new Quality();
            
            return con.sql_string(sql);
        }
    private DataTable datos { get; set; }
        [WebMethod(EnableSession = true)]
        public string GetCaracteristicas_articulo(Organoleptico datos)
        {
            string sql = @"SELECT  [CARACTERISTICAS_ARTICULO].[Caracteristica],
[Organolectico_Carac].Caracteristica + case when  [CARACTERISTICAS_ARTICULO].[Caracteristica] = 64 then [Organolectico_Carac].Observaciones else (case when [Organolectico_Carac].Tipo_Dato =1 then 
' ( ' + Coalesce (cast([V_Min] as varchar),'') +' | ' +  Coalesce (cast( [V_Max] as varchar),'') +' ):' 
else (case when [V_Max]=0 then ' ( Incorrecto ) ' else ' ( Correcto ) ' end) end)
+case when [CARACTERISTICAS_ARTICULO].Obligatorio = 1 then '*' else '' end end as lbl ,
 [Organolectico_Carac].Tipo_Dato  ,
 [CARACTERISTICAS_ARTICULO].A_Lote
FROM [QC600].[dbo].[CARACTERISTICAS_ARTICULO] inner join[QC600].[dbo].[Organolectico_Carac] on [Cod_Caracteristica]= [CARACTERISTICAS_ARTICULO].[Caracteristica]  where Articulo = " + datos.ARTICULO;
            Quality con = new Quality();
            DataTable datos_estados = con.Sql_Datatable(sql);
            return Newtonsoft.Json.JsonConvert.SerializeObject(datos_estados, Newtonsoft.Json.Formatting.Indented);
        }
        [WebMethod(EnableSession = true)]
        public string GetCaracteristicas_articulo2(Organoleptico datos)
        {
            string sql = @"SELECT  [CARACTERISTICAS_ARTICULO].[Caracteristica],
[Organolectico_Carac].Caracteristica + (case when [Organolectico_Carac].Tipo_Dato =1 then 
' ( ' + cast([V_Min] as varchar) +' | ' + cast( [V_Max] as varchar) +' ):' 
else (case when [V_Max]=0 then ' ( Incorrecto ) ' else ' ( Correcto ) ' end) end)
+case when [CARACTERISTICAS_ARTICULO].Obligatorio = 1 then '*' else '' end as lbl ,
 [Organolectico_Carac].Tipo_Dato  ,
 [CARACTERISTICAS_ARTICULO].A_Lote 
FROM [QC600].[dbo].[CARACTERISTICAS_ARTICULO] inner join[QC600].[dbo].[Organolectico_Carac] on [Cod_Caracteristica]= [CARACTERISTICAS_ARTICULO].[Caracteristica] where Articulo = " + datos.ARTICULO;
            Quality con = new Quality();
            DataTable datos_estados = con.Sql_Datatable(sql);

            sql = @" SELECT * FROM [QC600].[dbo].[DATOS_ORGANOLEPTICO] WHERE [ID_LOTE]=" + datos.ID_LOTE;
            DataTable datos_Organoleptico = con.Sql_Datatable(sql);
            var query = datos_estados.AsEnumerable().
    Select (articulo => new 
    {
        Caracteristica = articulo.Field<int>("Caracteristica"),
        lbl = articulo.Field<string>("lbl"),
        Tipo_Dato = articulo.Field<int>("Tipo_Dato")
            });
            DataTable resultados = new DataTable("Lista");
            resultados.Columns.Add("Caracteristica", typeof(int));
            resultados.Columns.Add("lbl", typeof(string));
            resultados.Columns.Add("Tipo_Dato", typeof(int));
            resultados.Columns.Add("Valor", typeof(double));
            

            foreach (var ArticuloInfo in query)
            {
                double val = 0;
                string vale = datos_Organoleptico.AsEnumerable().Select(r => r[columna_valor(ArticuloInfo.Caracteristica)].ToString()).First();
                double.TryParse(vale, out val);
                DataRow rowResul = resultados.NewRow();
                rowResul["Caracteristica"] = ArticuloInfo.Caracteristica;
                rowResul["lbl"] = ArticuloInfo.lbl;
                rowResul["Tipo_Dato"] = ArticuloInfo.Tipo_Dato;
                rowResul["Valor"] = val;
                resultados.Rows.Add(rowResul);
            }
                
            return Newtonsoft.Json.JsonConvert.SerializeObject(resultados, Newtonsoft.Json.Formatting.Indented);
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

            string sql = "Insert INTO [CARACTERISTICAS_ARTICULO]  ([Articulo],[Caracteristica], [V_Min],[V_Max],[Obligatorio],[A_lote] ) values (" + datos.articulo + " , " + datos.Cod_Caracteristica
                + " , " + datos.V_Min + " , " + datos.V_Max + " , " + datos.Obligatorio + " , " + datos.A_lote + " )";
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
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",1," + datos.PH_CRUDO_AP.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.PH_CRUDO_DP, out Datos_actuales);
            double.TryParse(datos_antiguo.PH_CRUDO_DP, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",2," + datos.PH_CRUDO_DP.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.BRIX_CRUDO_AP, out Datos_actuales);
            double.TryParse(datos_antiguo.BRIX_CRUDO_AP, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",3," + datos.BRIX_CRUDO_AP.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.BRIX_CRUDO_DP, out Datos_actuales);
            double.TryParse(datos_antiguo.BRIX_CRUDO_DP, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",4," + datos.BRIX_CRUDO_DP.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos._HUMEDAD, out Datos_actuales);
            double.TryParse(datos_antiguo._HUMEDAD, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",5," + datos._HUMEDAD.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos._ES, out Datos_actuales);
            double.TryParse(datos_antiguo._ES, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",6," + datos._ES.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.HC, out Datos_actuales);
            double.TryParse(datos_antiguo.HC, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",7," + datos.HC.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.SACAROSA, out Datos_actuales);
            double.TryParse(datos_antiguo.SACAROSA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",8," + datos.SACAROSA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.GRASA, out Datos_actuales);
            double.TryParse(datos_antiguo.GRASA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",9," + datos.GRASA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.PROTEINA, out Datos_actuales);
            double.TryParse(datos_antiguo.PROTEINA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",10," + datos.PROTEINA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.LACTOSA, out Datos_actuales);
            double.TryParse(datos_antiguo.LACTOSA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",11," + datos.LACTOSA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.TEMPERATURA, out Datos_actuales);
            double.TryParse(datos_antiguo.TEMPERATURA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",12," + datos.TEMPERATURA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.PH, out Datos_actuales);
            double.TryParse(datos_antiguo.PH, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",13," + datos.PH.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.COLOR, out Datos_actuales);
            double.TryParse(datos_antiguo.COLOR, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",14," + datos.COLOR.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.SABOR, out Datos_actuales);
            double.TryParse(datos_antiguo.SABOR, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",15," + datos.SABOR.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CORTE, out Datos_actuales);
            double.TryParse(datos_antiguo.CORTE, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",16," + datos.CORTE.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.FILM, out Datos_actuales);
            double.TryParse(datos_antiguo.FILM, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",17," + datos.FILM.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CATA, out Datos_actuales);
            double.TryParse(datos_antiguo.CATA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",18," + datos.CATA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.GLUTEN, out Datos_actuales);
            double.TryParse(datos_antiguo.GLUTEN, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",19," + datos.GLUTEN.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CASEINA, out Datos_actuales);
            double.TryParse(datos_antiguo.CASEINA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",20," + datos.CASEINA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.LISTERIA, out Datos_actuales);
            double.TryParse(datos_antiguo.LISTERIA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",21," + datos.LISTERIA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.SALMONELLA, out Datos_actuales);
            double.TryParse(datos_antiguo.SALMONELLA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",22," + datos.SALMONELLA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.PPC, out Datos_actuales);
            double.TryParse(datos_antiguo.PPC, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",23," + datos.PPC.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            //Estados
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.ID_ESTADO, out Datos_actuales);
            double.TryParse(datos_antiguo.ID_ESTADO, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA,OBSERVACIONES)  
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",0," + datos.ID_ESTADO.Replace(',', '.') + ",'" + usuario + "',GETDATE(),'" + datos.OBS_ESTADO + "')";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.IMPUREZAS, out Datos_actuales);
            double.TryParse(datos_antiguo.IMPUREZAS, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",26," + datos.IMPUREZAS.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }


            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.NC, out Datos_actuales);
            double.TryParse(datos_antiguo.NC, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",0," + datos.NC.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CONSISTENCIA, out Datos_actuales);
            double.TryParse(datos_antiguo.CONSISTENCIA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",27," + datos.CONSISTENCIA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.OLOR, out Datos_actuales);
            double.TryParse(datos_antiguo.OLOR, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",28," + datos.OLOR.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.EMULSION, out Datos_actuales);
            double.TryParse(datos_antiguo.EMULSION, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA)  
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",29," + datos.EMULSION.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.HR, out Datos_actuales);
            double.TryParse(datos_antiguo.HR, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {

                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",30," + datos.HR.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }


            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.TS, out Datos_actuales);
            double.TryParse(datos_antiguo.TS, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",31," + datos.TS.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }


            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.SNF, out Datos_actuales);
            double.TryParse(datos_antiguo.SNF, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",32," + datos.SNF.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }


            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.ASPECTO, out Datos_actuales);
            double.TryParse(datos_antiguo.ASPECTO, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",33," + datos.ASPECTO.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.DORNIC, out Datos_actuales);
            double.TryParse(datos_antiguo.DORNIC, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",34," + datos.DORNIC.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.INHIBIDORES, out Datos_actuales);
            double.TryParse(datos_antiguo.INHIBIDORES, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",35," + datos.INHIBIDORES.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.BRIX, out Datos_actuales);
            double.TryParse(datos_antiguo.BRIX, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",25," + datos.BRIX.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.BOLETIN, out Datos_actuales);
            double.TryParse(datos_antiguo.BOLETIN, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",24," + datos.BOLETIN.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.SILO, out Datos_actuales);
            double.TryParse(datos_antiguo.SILO, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",35," + datos.SILO.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.TTRANSP, out Datos_actuales);
            double.TryParse(datos_antiguo.TTRANSP, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",36," + datos.TTRANSP.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.LAVADO, out Datos_actuales);
            double.TryParse(datos_antiguo.LAVADO, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",38," + datos.LAVADO.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.SILODEST, out Datos_actuales);
            double.TryParse(datos_antiguo.SILODEST, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",39," + datos.SILODEST.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.EDAD_LECHE, out Datos_actuales);
            double.TryParse(datos_antiguo.EDAD_LECHE, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",40," + datos.EDAD_LECHE.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.T_CISTERNA, out Datos_actuales);
            double.TryParse(datos_antiguo.T_CISTERNA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",41," + datos.T_CISTERNA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.T_MUESTRA, out Datos_actuales);
            double.TryParse(datos_antiguo.T_MUESTRA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",42," + datos.T_MUESTRA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.INH_R, out Datos_actuales);
            double.TryParse(datos_antiguo.INH_R, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",43," + datos.INH_R.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.INH_L, out Datos_actuales);
            double.TryParse(datos_antiguo.INH_L, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",44," + datos.INH_L.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.D, out Datos_actuales);
            double.TryParse(datos_antiguo.D, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",45," + datos.D.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.ALCOHOL, out Datos_actuales);
            double.TryParse(datos_antiguo.ALCOHOL, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",46," + datos.ALCOHOL.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.C_FILTRO, out Datos_actuales);
            double.TryParse(datos_antiguo.C_FILTRO, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",47," + datos.C_FILTRO.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.INH_BOB, out Datos_actuales);
            double.TryParse(datos_antiguo.INH_BOB, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",48," + datos.INH_BOB.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.MERMA, out Datos_actuales);
            double.TryParse(datos_antiguo.MERMA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",1," + datos.MERMA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.FLATOXINA, out Datos_actuales);
            double.TryParse(datos_antiguo.FLATOXINA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",49," + datos.FLATOXINA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CONTROL_PESO_CALIDAD, out Datos_actuales);
            double.TryParse(datos_antiguo.CONTROL_PESO_CALIDAD, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",50," + datos.CONTROL_PESO_CALIDAD.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CONTROL_PESO_LINEA, out Datos_actuales);
            double.TryParse(datos_antiguo.CONTROL_PESO_LINEA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",51," + datos.CONTROL_PESO_LINEA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CURVA_PH, out Datos_actuales);
            double.TryParse(datos_antiguo.CURVA_PH, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",52," + datos.CURVA_PH.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.TEXTURA, out Datos_actuales);
            double.TryParse(datos_antiguo.TEXTURA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",53," + datos.TEXTURA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.ABSORVANCIA, out Datos_actuales);
            double.TryParse(datos_antiguo.ABSORVANCIA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",54," + datos.ABSORVANCIA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.WEB, out Datos_actuales);
            double.TryParse(datos_antiguo.WEB, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",58," + datos.WEB.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CAG, out Datos_actuales);
            double.TryParse(datos_antiguo.CAG, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",59," + datos.CAG.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.SILO_DESTINO, out Datos_actuales);
            double.TryParse(datos_antiguo.SILO_DESTINO, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",60," + datos.SILO_DESTINO.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CROMATOGRAMAS_FILM, out Datos_actuales);
            double.TryParse(datos_antiguo.CROMATOGRAMAS_FILM, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",61," + datos.CROMATOGRAMAS_FILM.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.PROTEINA_DILUCION, out Datos_actuales);
            double.TryParse(datos_antiguo.PROTEINA_DILUCION, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",62," + datos.PROTEINA_DILUCION.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.GRASA_DILUCION, out Datos_actuales);
            double.TryParse(datos_antiguo.GRASA_DILUCION, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",63," + datos.GRASA_DILUCION.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.SABOR_MERENGUE, out Datos_actuales);
            double.TryParse(datos_antiguo.SABOR_MERENGUE, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",70," + datos.SABOR_MERENGUE.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.OLOR_TORTILLA, out Datos_actuales);
            double.TryParse(datos_antiguo.OLOR_TORTILLA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",71," + datos.OLOR_TORTILLA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.SABOR_TORTILLA, out Datos_actuales);
            double.TryParse(datos_antiguo.SABOR_TORTILLA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",72," + datos.SABOR_TORTILLA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.Espectofotometro, out Datos_actuales);
            double.TryParse(datos_antiguo.Espectofotometro, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",73," + datos.Espectofotometro.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.BOLETIN_GRASA, out Datos_actuales);
            double.TryParse(datos_antiguo.BOLETIN_GRASA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",74," + datos.BOLETIN_GRASA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.BOLETIN_PROTEINA, out Datos_actuales);
            double.TryParse(datos_antiguo.BOLETIN_PROTEINA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",75," + datos.BOLETIN_PROTEINA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.BRIX_PASTE_2500, out Datos_actuales);
            double.TryParse(datos_antiguo.BRIX_PASTE_2500, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",76," + datos.BRIX_PASTE_2500.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.BRIX_PASTE_6000, out Datos_actuales);
            double.TryParse(datos_antiguo.BRIX_PASTE_6000, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != 0 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",77," + datos.BRIX_PASTE_6000.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.GLUTEN_BOLETIN, out Datos_actuales);
            double.TryParse(datos_antiguo.GLUTEN_BOLETIN, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",78," + datos.GLUTEN_BOLETIN.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.BOLETIN_LISTERIA, out Datos_actuales);
            double.TryParse(datos_antiguo.BOLETIN_LISTERIA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",79," + datos.BOLETIN_LISTERIA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.BOLETIN_SALMONELLA, out Datos_actuales);
            double.TryParse(datos_antiguo.BOLETIN_SALMONELLA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",80," + datos.BOLETIN_SALMONELLA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.PRE_BOLETIN_FQ, out Datos_actuales);
            double.TryParse(datos_antiguo.PRE_BOLETIN_FQ, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",81," + datos.PRE_BOLETIN_FQ.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.BOLETIN_SOJA, out Datos_actuales);
            double.TryParse(datos_antiguo.BOLETIN_SOJA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",82," + datos.BOLETIN_SOJA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.BOLETIN_CARAMELO, out Datos_actuales);
            double.TryParse(datos_antiguo.BOLETIN_CARAMELO, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",83," + datos.BOLETIN_CARAMELO.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.ETIQUETA_CORRECTA, out Datos_actuales);
            double.TryParse(datos_antiguo.ETIQUETA_CORRECTA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",84," + datos.ETIQUETA_CORRECTA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CURVA_FERMENTACION, out Datos_actuales);
            double.TryParse(datos_antiguo.CURVA_FERMENTACION, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",85," + datos.CURVA_FERMENTACION.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.ATP_APERTURA, out Datos_actuales);
            double.TryParse(datos_antiguo.ATP_APERTURA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",86," + datos.ATP_APERTURA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CATA_COMITE, out Datos_actuales);
            double.TryParse(datos_antiguo.CATA_COMITE, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",87," + datos.CATA_COMITE.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.TEXTUROMETRO, out Datos_actuales);
            double.TryParse(datos_antiguo.TEXTUROMETRO, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",88," + datos.TEXTUROMETRO.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.MUESTROTECA, out Datos_actuales);
            double.TryParse(datos_antiguo.MUESTROTECA, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",89," + datos.MUESTROTECA.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CURVA_ENFRIAMIENTO, out Datos_actuales);
            double.TryParse(datos_antiguo.CURVA_ENFRIAMIENTO, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",90," + datos.CURVA_ENFRIAMIENTO.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CURVA_FERMENTACION_, out Datos_actuales);
            double.TryParse(datos_antiguo.CURVA_FERMENTACION_, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",91," + datos.CURVA_FERMENTACION_.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.tp_tq_mix_crudo, out Datos_actuales);
            double.TryParse(datos_antiguo.tp_tq_mix_crudo, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",92," + datos.tp_tq_mix_crudo.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.tp_tq_mix_paste, out Datos_actuales);
            double.TryParse(datos_antiguo.tp_tq_mix_paste, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",93," + datos.tp_tq_mix_paste.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.tp_tq_enfr_dosif, out Datos_actuales);
            double.TryParse(datos_antiguo.tp_tq_enfr_dosif, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",94," + datos.tp_tq_enfr_dosif.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            /***
             * ***
             * */
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.ph_box, out Datos_actuales);
            double.TryParse(datos_antiguo.ph_box, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",95," + datos.ph_box.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.col_box, out Datos_actuales);
            double.TryParse(datos_antiguo.col_box, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",96," + datos.col_box.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.olor_box, out Datos_actuales);
            double.TryParse(datos_antiguo.olor_box, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",97," + datos.olor_box.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.sab_box, out Datos_actuales);
            double.TryParse(datos_antiguo.sab_box, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",98," + datos.sab_box.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.B_box, out Datos_actuales);
            double.TryParse(datos_antiguo.B_box, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",99," + datos.B_box.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.Purez, out Datos_actuales);
            double.TryParse(datos_antiguo.Purez, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",100," + datos.Purez.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.Org_antes_envio, out Datos_actuales);
            double.TryParse(datos_antiguo.Org_antes_envio, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",101," + datos.Org_antes_envio.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }


            /*****
             * ***
             * */

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.ph_50, out Datos_actuales);
            double.TryParse(datos_antiguo.ph_50, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",102," + datos.ph_50.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.Consistencia_20, out Datos_actuales);
            double.TryParse(datos_antiguo.Consistencia_20, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",103," + datos.Consistencia_20.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.Sacarasa_bol, out Datos_actuales);
            double.TryParse(datos_antiguo.Sacarasa_bol, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",104," + datos.Sacarasa_bol.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.sal_bol, out Datos_actuales);
            double.TryParse(datos_antiguo.sal_bol, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",105," + datos.sal_bol.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.ATP_Tq_formulacion, out Datos_actuales);
            double.TryParse(datos_antiguo.ATP_Tq_formulacion, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",106," + datos.ATP_Tq_formulacion.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            /****
             * ***
             * */
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.ATP_tq_destino, out Datos_actuales);
            double.TryParse(datos_antiguo.ATP_tq_destino, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",107," + datos.ATP_tq_destino.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.ATP_linea, out Datos_actuales);
            double.TryParse(datos_antiguo.ATP_linea, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",108," + datos.ATP_linea.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.ph_6_67, out Datos_actuales);
            double.TryParse(datos_antiguo.ph_6_67, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",109," + datos.ph_6_67.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.olor_tort_box, out Datos_actuales);
            double.TryParse(datos_antiguo.olor_tort_box, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",110," + datos.olor_tort_box.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.Sabor_tort_box, out Datos_actuales);
            double.TryParse(datos_antiguo.Sabor_tort_box, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",111," + datos.Sabor_tort_box.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.ATP_manguera, out Datos_actuales);
            double.TryParse(datos_antiguo.ATP_manguera, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",112," + datos.ATP_manguera.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.Mohos_levad, out Datos_actuales);
            double.TryParse(datos_antiguo.Mohos_levad, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",113," + datos.Mohos_levad.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.Presencia_ojos, out Datos_actuales);
            double.TryParse(datos_antiguo.Presencia_ojos, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",114," + datos.Presencia_ojos.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.Recirculacion, out Datos_actuales);
            double.TryParse(datos_antiguo.Recirculacion, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",115," + datos.Recirculacion.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.b_tras_paste, out Datos_actuales);
            double.TryParse(datos_antiguo.b_tras_paste, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",116," + datos.b_tras_paste.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.bloom, out Datos_actuales);
            double.TryParse(datos_antiguo.bloom, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",117," + datos.bloom.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.CC_obli, out Datos_actuales);
            double.TryParse(datos_antiguo.CC_obli, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",118," + datos.CC_obli.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.textu_tras_tunel, out Datos_actuales);
            double.TryParse(datos_antiguo.textu_tras_tunel, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",119," + datos.textu_tras_tunel.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.textu_revision, out Datos_actuales);
            double.TryParse(datos_antiguo.textu_revision, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",120," + datos.textu_revision.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.HR_bol, out Datos_actuales);
            double.TryParse(datos_antiguo.HR_bol, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",121," + datos.HR_bol.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.t_frabricacion, out Datos_actuales);
            double.TryParse(datos_antiguo.t_frabricacion, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",122," + datos.t_frabricacion.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.Enterobac, out Datos_actuales);
            double.TryParse(datos_antiguo.Enterobac, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",123," + datos.Enterobac.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.Den, out Datos_actuales);
            double.TryParse(datos_antiguo.Den, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",124," + datos.Den.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.Contrast, out Datos_actuales);
            double.TryParse(datos_antiguo.Contrast, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",125," + datos.Contrast.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }

            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.Rev_Filtros, out Datos_actuales);
            double.TryParse(datos_antiguo.Rev_Filtros, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",126," + datos.Rev_Filtros.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.Gras_Box, out Datos_actuales);
            double.TryParse(datos_antiguo.Gras_Box, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",127," + datos.Gras_Box.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.Prot_box, out Datos_actuales);
            double.TryParse(datos_antiguo.Prot_box, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",128," + datos.Prot_box.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.V1, out Datos_actuales);
            double.TryParse(datos_antiguo.V1, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",129," + datos.V1.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.V2, out Datos_actuales);
            double.TryParse(datos_antiguo.V2, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",130," + datos.V2.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
                con.sql_update(sql);
            }
            Datos_actuales = -999999;
            Datos_anteriores = -999999;
            double.TryParse(datos.V3, out Datos_actuales);
            double.TryParse(datos_antiguo.V3, out Datos_anteriores);
            if (Datos_actuales != Datos_anteriores && Datos_actuales != -1 && Datos_actuales != -999999)
            {
                sql = @"INSERT INTO [CARACTERISTICAS_DATOS] ( ARTICULO, SSCC, ID_LOTE, CARACTERISTICA, VALOR, USUARIO, FECHA) 
                    VALUES (" + datos.ARTICULO + ",'" + datos.SSCC + "'," + datos.ID_LOTE + ",131," + datos.V3.Replace(',', '.') + ",'" + usuario + "',GETDATE())";
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
                sql = "update [DATOS_ORGANOLEPTICO] set [PH_CRUDO_AP]= " + datos.PH_CRUDO_AP.Replace(',', '.') + " where [ID_LOTE]=" + datos.ID_LOTE ;
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
                sql = "update [DATOS_ORGANOLEPTICO] set [IMPUREZAS]='" + datos.IMPUREZAS.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.NC.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [NC]='" + datos.NC.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.CONSISTENCIA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CONSISTENCIA]='" + datos.CONSISTENCIA.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.OLOR.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [OLOR]='" + datos.OLOR.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.EMULSION.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [EMULSION]='" + datos.EMULSION.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.HR.Length > 0)
            {

                sql = "update [DATOS_ORGANOLEPTICO] set [HR]='" + datos.HR.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);

            }

            if (datos.TS.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [TS]='" + datos.TS.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.SNF.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [SNF]='" + datos.SNF.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.ASPECTO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ASPECTO]='" + datos.ASPECTO.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.DORNIC.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [DORNIC]='" + datos.DORNIC.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.INHIBIDORES.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [INHIBIDORES]='" + datos.INHIBIDORES.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.BRIX.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [BRIX]='" + datos.BRIX.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.BOLETIN.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [BOLETIN]='" + datos.BOLETIN.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.SILO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [SILO]='" + datos.SILO.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.TTRANSP.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [TTRANSP]='" + datos.TTRANSP.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.LAVADO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [LAVADO]='" + datos.LAVADO.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.SILODEST.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [SILODEST]='" + datos.SILODEST.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.EDAD_LECHE.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [EDAD_LECHE]='" + datos.EDAD_LECHE.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.T_CISTERNA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [T_CISTERNA]='" + datos.T_CISTERNA.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.T_MUESTRA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [T_MUESTRA]='" + datos.T_MUESTRA.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.INH_R.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [INH_R]='" + datos.INH_R.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.INH_L.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [INH_L]='" + datos.INH_L.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.D.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [D]='" + datos.D.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ALCOHOL.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ALCOHOL]='" + datos.ALCOHOL.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.C_FILTRO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [C_FILTRO]='" + datos.C_FILTRO.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.INH_BOB.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [INH_BOB]='" + datos.INH_BOB.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.MERMA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [MERMA]='" + datos.MERMA.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.FLATOXINA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [FLATOXINA]='" + datos.FLATOXINA.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.CONTROL_PESO_CALIDAD.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CONTROL_PESO_CALIDAD]='" + datos.CONTROL_PESO_CALIDAD.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.CONTROL_PESO_LINEA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CONTROL_PESO_LINEA]='" + datos.CONTROL_PESO_LINEA.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.CURVA_PH.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CURVA_PH]='" + datos.CURVA_PH.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ESTADO_COMITE.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ESTADO_COMITE]='" + datos.ESTADO_COMITE.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.HORA_ARRIBA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [HORA_ARRIBA]='" + datos.HORA_ARRIBA.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ESTADO_ARRIBA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ESTADO_ARRIBA]='" + datos.ESTADO_ARRIBA.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.HORA_MEDIO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [HORA_MEDIO]='" + datos.HORA_MEDIO.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ESTADO_MEDIO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ESTADO_MEDIO]='" + datos.ESTADO_MEDIO.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ESTADO_ABAJO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ESTADO_ABAJO]='" + datos.ESTADO_ABAJO.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.HORA_ABAJO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [HORA_ABAJO]='" + datos.HORA_ABAJO.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.OBSERVA_ARRIBA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [OBSERVA_ARRIBA]='" + datos.OBSERVA_ARRIBA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.OBSERVA_MEDIO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [OBSERVA_MEDIO]='" + datos.OBSERVA_MEDIO + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.OBSERVA_ABAJO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [OBSERVA_ABAJO]='" + datos.OBSERVA_ABAJO + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.OBSEVACIONES_COMITE.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [OBSEVACIONES_COMITE]='" + datos.OBSEVACIONES_COMITE + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.TEXTURA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [TEXTURA]='" + datos.TEXTURA.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ABSORVANCIA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ABSORVANCIA]='" + datos.ABSORVANCIA.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.WEB.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [WEB]='" + datos.WEB.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
       
            if (datos.CAG.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CAG]='" + datos.CAG.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.PROTEINA_DILUCION.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [PROTEINA_DILUCION]='" + datos.PROTEINA_DILUCION.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.GRASA_DILUCION.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [GRASA_DILUCION]='" + datos.GRASA_DILUCION.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.SILO_DESTINO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [SILO_DESTINO]='" + datos.SILO_DESTINO.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.CROMATOGRAMAS_FILM.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CROMATOGRAMAS_FILM]='" + datos.CROMATOGRAMAS_FILM.Replace(',', '.') + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.TTP.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [TTP]='" + datos.TTP + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.TPP.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [TPP]='" + datos.TPP + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.SABOR_MERENGUE.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [SABOR_MERENGUE]='" + datos.SABOR_MERENGUE + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.OLOR_TORTILLA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [OLOR_TORTILLA]='" + datos.OLOR_TORTILLA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.SABOR_TORTILLA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [SABOR_TORTILLA]='" + datos.SABOR_TORTILLA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.Espectofotometro.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [Espectofotometro]='" + datos.Espectofotometro + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.BOLETIN_PROTEINA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [BOLETIN_PROTEINA]='" + datos.BOLETIN_PROTEINA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.BOLETIN_GRASA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [BOLETIN_GRASA]='" + datos.BOLETIN_GRASA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.BRIX_PASTE_2500.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [BRIX_PASTE_2500]='" + datos.BRIX_PASTE_2500 + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.BRIX_PASTE_6000.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [BRIX_PASTE_6000]='" + datos.BRIX_PASTE_6000 + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.GLUTEN_BOLETIN.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [GLUTEN_BOLETIN]='" + datos.GLUTEN_BOLETIN + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.BOLETIN_LISTERIA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [BOLETIN_LISTERIA]='" + datos.BOLETIN_LISTERIA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.BOLETIN_SALMONELLA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [BOLETIN_SALMONELLA]='" + datos.BOLETIN_SALMONELLA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.PRE_BOLETIN_FQ.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [PRE_BOLETIN_FQ]='" + datos.PRE_BOLETIN_FQ + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.BOLETIN_SOJA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [BOLETIN_SOJA]='" + datos.BOLETIN_SOJA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.BOLETIN_CARAMELO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [BOLETIN_CARAMELO]='" + datos.BOLETIN_CARAMELO + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ETIQUETA_CORRECTA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ETIQUETA_CORRECTA]='" + datos.ETIQUETA_CORRECTA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.CURVA_FERMENTACION.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CURVA_FERMENTACION]='" + datos.CURVA_FERMENTACION + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ATP_APERTURA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ATP_APERTURA]='" + datos.ATP_APERTURA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.CATA_COMITE.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CATA_COMITE]='" + datos.CATA_COMITE + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.TEXTUROMETRO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [TEXTUROMETRO]='" + datos.TEXTUROMETRO + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.MUESTROTECA.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [MUESTROTECA]='" + datos.MUESTROTECA + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.CURVA_ENFRIAMIENTO.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CURVA_ENFRIAMIENTO]='" + datos.CURVA_ENFRIAMIENTO + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.CURVA_FERMENTACION_.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CURVA_FERMENTACION_]='" + datos.CURVA_FERMENTACION_ + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.tp_tq_mix_crudo.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [tp_tq_mix_crudo]='" + datos.tp_tq_mix_crudo + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.tp_tq_mix_paste.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [tp_tq_mix_paste]='" + datos.tp_tq_mix_paste + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.tp_tq_enfr_dosif.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [tp_tq_enfr_dosif]='" + datos.tp_tq_enfr_dosif + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }


            if (datos.ph_box.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ph_box]='" + datos.ph_box + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.col_box.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [col_box]='" + datos.col_box + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.olor_box.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [olor_box]='" + datos.olor_box + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.sab_box.Length > 0)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [sab_box]='" + datos.sab_box + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.B_box.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ºB_box]='" + datos.B_box + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.Purez.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [Purez]='" + datos.Purez + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.Org_antes_envio.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [Org_antes_envio]='" + datos.Org_antes_envio + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ph_50.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ph_(50%)]='" + datos.ph_50 + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.Consistencia_20.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [Consistencia_20º]='" + datos.Consistencia_20 + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.Sacarasa_bol.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [Sacarasa_bol]='" + datos.Sacarasa_bol + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.sal_bol.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [(%)sal_bol]='" + datos.sal_bol + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ATP_Tq_formulacion.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ATP_Tq_formulacion]='" + datos.ATP_Tq_formulacion + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ATP_tq_destino.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ATP_tq_destino]='" + datos.ATP_tq_destino + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ATP_linea.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ATP_linea]='" + datos.ATP_linea + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ph_6_67.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ph_(6.67%)]='" + datos.ph_6_67 + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.olor_tort_box.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [olor_tort_box]='" + datos.olor_tort_box + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.Sabor_tort_box.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [Sabor_tort_box]='" + datos.Sabor_tort_box + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.ATP_manguera.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [ATP_manguera]='" + datos.ATP_manguera + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            //
            if (datos.Mohos_levad.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [Mohos_levad]='" + datos.Mohos_levad + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }

            if (datos.Presencia_ojos.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [Presencia_ojos]='" + datos.Presencia_ojos + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.Recirculacion.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [Recirculacion]='" + datos.Recirculacion + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.b_tras_paste.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [b_tras_paste]='" + datos.b_tras_paste + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.bloom.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [bloom]='" + datos.bloom + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.CC_obli.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [CC_obli]='" + datos.CC_obli + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.textu_tras_tunel.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [textu_tras_tunel]='" + datos.textu_tras_tunel + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.textu_revision.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [textu_revision]='" + datos.textu_revision + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.HR_bol.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [HR_bol]='" + datos.HR_bol + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.t_frabricacion.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [t_frabricacion]='" + datos.t_frabricacion + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.Enterobac.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [Enterobac]='" + datos.Enterobac + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.Den.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [Den]='" + datos.Den + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.Contrast.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [Contrast]='" + datos.Contrast + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }


            if (datos.Rev_Filtros.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [Rev_Filtros]='" + datos.Rev_Filtros + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.Gras_Box.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [Gras_Box]='" + datos.Gras_Box + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.Prot_box.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [Prot_box]='" + datos.Prot_box + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.V1.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [V1]='" + datos.V1 + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.V2.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [V2]='" + datos.V2 + "' where [ID_LOTE]=" + datos.ID_LOTE;
                con.sql_update(sql);
            }
            if (datos.V3.Length > 000)
            {
                sql = "update [DATOS_ORGANOLEPTICO] set [V3]='" + datos.V3 + "' where [ID_LOTE]=" + datos.ID_LOTE;
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
            if (datos.CARAC_LOTE.Length > 0)
            {
                
                string[] Caracteristica_A_lote = datos.CARAC_LOTE.Split('$');
                foreach (string caracteristica in Caracteristica_A_lote)
                {
                    string id = datos.ID_LOTE;
                    if (caracteristica.Length > 0){
                        sql = @"EXEC[dbo].[PROPAGAR_CARACTERISTICAS_LOTE] "+ caracteristica+", "+ datos.ID_LOTE;
                        con.sql_update(sql);
                        sql = @"SELECT [Caracteristica] FROM [QC600].[dbo].[Organolectico_Carac] where [Cod_Caracteristica]=" + caracteristica;
                        string carac=con.sql_string(sql);

                        sql = @"insert into [QC600].[dbo].[CONTROL_PROPAGAR_LOTE] ([FECHA] ,[SSCC],[ID_LOTE],[ARTICULO],[LOTE_INTERNO],[ACCION],[OBSERVACIONES]) values ( getdate(),'"+ datos.SSCC +@"'," + datos.ID_LOTE + @"," + datos.ARTICULO + @",'" + datos.LOTE_INTERNO +@"','PROPAGAR CARACTERISTICA LOTE', 'Propago el valor de la caracteristica "+ carac+ " a todo el lote')";
                        con.sql_update(sql);
                    }
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
        private static void FormatWorksheetData(List<string> dateColumns, List<string> hideColumns, DataTable table, ExcelWorksheet ws)
        {
            int columnCount = table.Columns.Count;
            int rowCount = table.Rows.Count;

            ExcelRange r;

            // which columns have dates in
            for (int i = 1; i <= columnCount; i++)
            {
                // if cell header value matches a date column
                if (dateColumns.Contains(ws.Cells[1, i].Value.ToString()))
                {
                    r = ws.Cells[2, i, rowCount + 1, i];
                    r.AutoFitColumns();
                    r.Style.Numberformat.Format = @"dd MMM yyyy hh:mm";
                }
            }
            // get all data and autofit
            r = ws.Cells[1, 1, rowCount + 1, columnCount];
            r.AutoFitColumns();

            // which columns have columns that should be hidden
            for (int i = 1; i <= columnCount; i++)
            {
                // if cell header value matches a hidden column
                if (hideColumns.Contains(ws.Cells[1, i].Value.ToString()))
                {
                    ws.Column(i).Hidden = true;
                }
            }
        }
        public IEnumerable<Organoleptico> Obtener_datos2(string fecha1, string fecha2)
        {
            string sql = @"SELECT  [ID]
      ,[DATOS_ORGANOLEPTICO].[ARTICULO]
	  ,[ARTICULO].[Descr Abreviada] as [DESCRIPCION]
      ,[LOTE_INTERNO]
      ,[NUMPALET]
      ,[FECHA_SSCC_CON]
      ,Estado.Estado AS ESTADO
      ,[SSCC]
      ,OBS_ESTADO
      ,CONVERT (VARCHAR,[FECHA_CREACION],103) AS FECHA_CREACION      
      ,[ESTADO_COMITE]
      ,[OBSEVACIONES_COMITE]
 ,[ID_LOTE]

,[DATOS_ORGANOLEPTICO].[ESTADO] AS ID_ESTADO
,[MERMA]
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
                        LOTE_INTERNO = lin[3].ToString(),
                        NUMPALET = lin[4].ToString(),
                        FECHA_SSCC_CON = lin[5].ToString(),
                        ESTADO = lin[6].ToString(),
                        SSCC = lin[7].ToString(),
                        OBS_ESTADO = lin[8].ToString(),
                        FECHA_CREACION = lin[9].ToString(),
                        ESTADO_COMITE = lin[10].ToString(),
                        OBSEVACIONES_COMITE = lin[11].ToString(),
                        ID_LOTE = lin[12].ToString(),
                        ID_ESTADO = lin[13].ToString(),
                        MERMA = lin[14].ToString()

                    };
                    lista_datos.Add(linea);
                }
            }
            return lista_datos;
        }
        [WebMethod(EnableSession = true)]
       
        public string GetTableData4(object myData)
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
            var search12 = get_parametters(myData, "sSearch_12").ToString(CultureInfo.CurrentCulture);
            var search13 = get_parametters(myData, "sSearch_13").ToString(CultureInfo.CurrentCulture);
            var search14 = get_parametters(myData, "sSearch_14").ToString(CultureInfo.CurrentCulture);
            // var records = ObtenerCaracteristicas(roleId).ToList();
            var records = Obtener_datos2(fecha1, fecha2).ToList();
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
                filterRecords = filterRecords.Where(l => l.LOTE_INTERNO.ToUpperInvariant().Contains(search3.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search4.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.NUMPALET.ToUpperInvariant().Contains(search4.ToUpperInvariant())).ToList();  // prue.ToList();
            }

            if (search5.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.FECHA_SSCC_CON.ToUpperInvariant().Contains(search5.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search8.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.ESTADO.ToUpperInvariant().Contains(search8.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search9.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.SSCC.ToUpperInvariant().Contains(search9.ToUpperInvariant())).ToList();  // prue.ToList();
            }
          
            if (search12.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.FECHA_CREACION.ToUpperInvariant().Contains(search12.ToUpperInvariant())).ToList();  // prue.ToList();
            }
           
            if (search14.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.FECHACADUCIDAD.ToUpperInvariant().Contains(search14.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            var orderedResults = sortOrder == "asc"
                                ? filterRecords.OrderBy(o => o.ID)
                                : filterRecords.OrderByDescending(o => o.ID);
            var itemsToSkip = displayStart == 0
                              ? 0
                              : displayStart;
            //  var filterRecords = (orderedResults);
            datos = orderedResults.ToDataTable();
            Session["datos"] = datos;
            


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
                sb.Append("\"" + result.LOTE_INTERNO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.NUMPALET.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.FECHA_SSCC_CON.Replace('\r', ' ').Replace('\n', ' ') + "\",");            
                sb.Append("\"" + result.ESTADO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.SSCC.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.OBS_ESTADO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.FECHA_CREACION.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ESTADO_COMITE.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.OBSEVACIONES_COMITE.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ID_LOTE.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ID_ESTADO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.MERMA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"<img class='image-details' src='content/details_open.png' runat='server' height='16' width='16' alt='View Details'/>\"");
                sb.Append("]");
                hasMoreRecords = true;
            }
            sb.Append("]}");


            return sb.ToString();

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
            var tipo = get_parametters(myData, "tipo").ToString(CultureInfo.CurrentCulture);
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
            var search12 = get_parametters(myData, "sSearch_12").ToString(CultureInfo.CurrentCulture);
            var search13 = get_parametters(myData, "sSearch_13").ToString(CultureInfo.CurrentCulture);
            var search14 = get_parametters(myData, "sSearch_14").ToString(CultureInfo.CurrentCulture);
            // var records = ObtenerCaracteristicas(roleId).ToList();
            var records =Obtener_datos(fecha1, fecha2,tipo).ToList(); ;
          
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
                filterRecords = filterRecords.Where(l => l.LOTE_INTERNO.ToUpperInvariant().Contains(search3.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search4.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.NUMPALET.ToUpperInvariant().Contains(search4.ToUpperInvariant())).ToList();  // prue.ToList();
            }

            if (search5.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.FECHA_SSCC_CON.ToUpperInvariant().Contains(search5.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search8.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.ESTADO.ToUpperInvariant().Contains(search8.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search9.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.SSCC.ToUpperInvariant().Contains(search9.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            /* if (search7.Length > 0)
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
            }*/
            if (search12.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.FECHA_CREACION.ToUpperInvariant().Contains(search12.ToUpperInvariant())).ToList();  // prue.ToList();
            }
          /*  if (search11.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.HORA.ToUpperInvariant().Contains(search11.ToUpperInvariant())).ToList();  // prue.ToList();
            }*/
            if (search14.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.FECHACADUCIDAD.ToUpperInvariant().Contains(search14.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            var orderedResults = sortOrder == "asc"
                                ? filterRecords.OrderBy(o => o.ID)
                                : filterRecords.OrderByDescending(o => o.ID);
            var itemsToSkip = displayStart == 0
                              ? 0
                              : displayStart ;
            //  var filterRecords = (orderedResults);
            datos=orderedResults.ToDataTable();
            Session["datos"] = datos;



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
                // System.Text.RegularExpressions.Regex.Replace(, @"[^a-zA-Z0-9\+\-\ \.\,\:]", "")
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ID.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ARTICULO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.DESCRIPCION.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.LOTE_INTERNO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.NUMPALET.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.FECHA_SSCC_CON.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.UD_ACTUAL.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.KG_ACTUAL.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ESTADO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.SSCC.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.OBS_ESTADO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ID_LOTE.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.FECHA_CREACION.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.HORA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.FECHACADUCIDAD.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.PH_CRUDO_AP.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.PH_CRUDO_DP.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.BRIX_CRUDO_AP.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.BRIX_CRUDO_DP.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result._HUMEDAD.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result._ES.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.HC.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.SACAROSA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.GRASA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.PROTEINA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.LACTOSA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.TEMPERATURA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.PH.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.COLOR.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.SABOR.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.CORTE.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.FILM.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.CATA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.GLUTEN.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.CASEINA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.LISTERIA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.SALMONELLA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.PPC.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");              
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.BOLETIN.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.BRIX.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.IMPUREZAS.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.CONSISTENCIA, @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.OLOR.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.EMULSION.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.HR.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.TS.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.SNF.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ASPECTO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.DORNIC.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.INHIBIDORES.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.NC.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.SILO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.TTRANSP.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.LAVADO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.SILODEST.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.EDAD_LECHE.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.T_CISTERNA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.T_MUESTRA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.INH_R.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.INH_L.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.D.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" +  System.Text.RegularExpressions.Regex.Replace(result.ALCOHOL.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.C_FILTRO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.INH_BOB.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");               
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.FLATOXINA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.MERMA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.CONTROL_PESO_CALIDAD.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.CONTROL_PESO_LINEA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.CURVA_PH.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.TEXTURA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ABSORVANCIA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.WEB.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.CAG.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ESTADO_COMITE.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.HORA_ARRIBA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ESTADO_ARRIBA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.OBSERVA_ARRIBA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.HORA_MEDIO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ESTADO_MEDIO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.OBSERVA_MEDIO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.HORA_ABAJO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ESTADO_ABAJO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.OBSERVA_ABAJO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.OBSEVACIONES_COMITE.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");              
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ID_ESTADO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.SILO_DESTINO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.CROMATOGRAMAS_FILM.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.PROTEINA_DILUCION.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.GRASA_DILUCION.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.TTP.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.TPP.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.SABOR_MERENGUE.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.OLOR_TORTILLA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.SABOR_TORTILLA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.Espectofotometro.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.BOLETIN_GRASA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.BOLETIN_PROTEINA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.BRIX_PASTE_2500.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.BRIX_PASTE_6000.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.GLUTEN_BOLETIN.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.BOLETIN_LISTERIA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.BOLETIN_SALMONELLA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.PRE_BOLETIN_FQ.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.BOLETIN_SOJA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "")  + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.BOLETIN_CARAMELO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ETIQUETA_CORRECTA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.CURVA_FERMENTACION.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ATP_APERTURA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.CATA_COMITE.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.TEXTUROMETRO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.MUESTROTECA.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.CURVA_ENFRIAMIENTO.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.CURVA_FERMENTACION_.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.tp_tq_mix_crudo.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.tp_tq_mix_paste.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.tp_tq_enfr_dosif.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ph_box.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.col_box.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.olor_box.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.sab_box.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.B_box.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.Purez.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.Org_antes_envio.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ph_50.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.Consistencia_20.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.Sacarasa_bol.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.sal_bol.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ATP_Tq_formulacion.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ATP_tq_destino.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ATP_linea.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ph_6_67.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.olor_tort_box.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.Sabor_tort_box.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.ATP_manguera.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.Mohos_levad.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.Presencia_ojos.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.Recirculacion.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.b_tras_paste.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.bloom.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.CC_obli.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.textu_tras_tunel.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.textu_revision.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.HR_bol.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.t_frabricacion.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.Enterobac.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.Den.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.Contrast.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.Rev_Filtros.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.Gras_Box.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.Prot_box.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.V1.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.V2.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");
                sb.Append("\"" + System.Text.RegularExpressions.Regex.Replace(result.V3.Replace('\r', ' ').Replace('\n', ' '), @"[^a-zA-Z0-9\+\-\ \.\,\:]", "") + "\",");

                sb.Append("\"<img class='image-details' src='content/details_open.png' runat='server' height='16' width='16' alt='View Details'/>\"");
                sb.Append("]");
                hasMoreRecords = true;
            }
            sb.Append("]}");
           

            return sb.ToString(); 

        }
        [WebMethod(EnableSession = true)]
        public string GetTableData2(object myData)
        {

            var echo = int.Parse(get_parametters(myData, "sEcho"));
            var displayLength = int.Parse(get_parametters(myData, "iDisplayLength"));
            var displayStart = int.Parse(get_parametters(myData, "iDisplayStart"));
            var sortOrder = get_parametters(myData, "sSortDir_0").ToString(CultureInfo.CurrentCulture);
            //var roleId = HttpContext.Current.Request.Params["roleId"].ToString(CultureInfo.CurrentCulture
            Uri myRef =HttpContext.Current.Request.UrlReferrer;
            string matricula = HttpUtility.ParseQueryString(myRef.Query).Get("matricula").TrimStart('0').Replace("91x", "");
              //  var matricula = HttpContext.Current.Request.QueryString["matricula"];
         // var matricula = get_parametters(myData, "Matricula").ToString(CultureInfo.CurrentCulture);
           if (matricula ==null) { return string.Empty; }
            if (matricula.Length <= 0 )
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
            var search12 = get_parametters(myData, "sSearch_12").ToString(CultureInfo.CurrentCulture);
            var search13 = get_parametters(myData, "sSearch_13").ToString(CultureInfo.CurrentCulture);
            var search14 = get_parametters(myData, "sSearch_14").ToString(CultureInfo.CurrentCulture);
            // var records = ObtenerCaracteristicas(roleId).ToList();
            var records = Obtener_datos_Matricula(matricula).ToList();
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
                filterRecords = filterRecords.Where(l => l.LOTE_INTERNO.ToUpperInvariant().Contains(search3.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search4.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.NUMPALET.ToUpperInvariant().Contains(search4.ToUpperInvariant())).ToList();  // prue.ToList();
            }

            if (search5.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.FECHA_SSCC_CON.ToUpperInvariant().Contains(search5.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search8.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.ESTADO.ToUpperInvariant().Contains(search8.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search9.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.SSCC.ToUpperInvariant().Contains(search9.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            /* if (search7.Length > 0)
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
            }*/
            if (search12.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.FECHA_CREACION.ToUpperInvariant().Contains(search12.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            /*  if (search11.Length > 0)
              {
                  filterRecords = filterRecords.Where(l => l.HORA.ToUpperInvariant().Contains(search11.ToUpperInvariant())).ToList();  // prue.ToList();
              }*/
            if (search14.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.FECHACADUCIDAD.ToUpperInvariant().Contains(search14.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            var orderedResults = sortOrder == "asc"
                                ? filterRecords.OrderBy(o => o.ID)
                                : filterRecords.OrderByDescending(o => o.ID);
            var itemsToSkip = displayStart == 0
                              ? 0
                              : displayStart + 1;
            //  var filterRecords = (orderedResults);
            datos = orderedResults.ToDataTable();
            Session["datos"] = datos;



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
                sb.Append("\"" + result.LOTE_INTERNO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.NUMPALET.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.FECHA_SSCC_CON.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.UD_ACTUAL.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.KG_ACTUAL.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ESTADO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.SSCC.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.OBS_ESTADO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
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
                sb.Append("\"" + result.BOLETIN.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.BRIX.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.IMPUREZAS.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.CONSISTENCIA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
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
                sb.Append("\"" + result.FLATOXINA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.MERMA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.CONTROL_PESO_CALIDAD.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.CONTROL_PESO_LINEA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.CURVA_PH.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.TEXTURA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ABSORVANCIA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.WEB.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.CAG.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ESTADO_COMITE.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.HORA_ARRIBA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ESTADO_ARRIBA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.OBSERVA_ARRIBA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.HORA_MEDIO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ESTADO_MEDIO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.OBSERVA_MEDIO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.HORA_ABAJO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ESTADO_ABAJO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.OBSERVA_ABAJO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.OBSEVACIONES_COMITE.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ID_ESTADO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"<img class='image-details' src='content/details_open.png' runat='server' height='16' width='16' alt='View Details'/>\"");
                sb.Append("]");
                hasMoreRecords = true;
            }
            sb.Append("]}");


            return sb.ToString();

        }
     
        [WebMethod(EnableSession = true)]
        public string GetTableData3(object myData)
        {

            var echo = int.Parse(get_parametters(myData, "sEcho"));
            var displayLength = int.Parse(get_parametters(myData, "iDisplayLength"));
            var displayStart = int.Parse(get_parametters(myData, "iDisplayStart"));
            var sortOrder = get_parametters(myData, "sSortDir_0").ToString(CultureInfo.CurrentCulture);
            //var roleId = HttpContext.Current.Request.Params["roleId"].ToString(CultureInfo.CurrentCulture
            Uri myRef = HttpContext.Current.Request.UrlReferrer;
            string lote = HttpUtility.ParseQueryString(myRef.Query).Get("lote").TrimStart('0');
            string fecha1 = HttpUtility.ParseQueryString(myRef.Query).Get("fecha1").TrimStart('0');
            string fecha2 = HttpUtility.ParseQueryString(myRef.Query).Get("fecha2").TrimStart('0');
            //  var matricula = HttpContext.Current.Request.QueryString["matricula"];
            // var matricula = get_parametters(myData, "Matricula").ToString(CultureInfo.CurrentCulture);
            if (lote == null || fecha1==null ||fecha2==null) { return string.Empty; }
            if (lote.Length <= 0 || fecha1.Length<=0 || fecha2.Length<=0)
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
            var search12 = get_parametters(myData, "sSearch_12").ToString(CultureInfo.CurrentCulture);
            var search13 = get_parametters(myData, "sSearch_13").ToString(CultureInfo.CurrentCulture);
            var search14 = get_parametters(myData, "sSearch_14").ToString(CultureInfo.CurrentCulture);
            // var records = ObtenerCaracteristicas(roleId).ToList();
            var records = Obtener_datos_Matricula(lote,fecha1,fecha2).ToList();
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
                filterRecords = filterRecords.Where(l => l.LOTE_INTERNO.ToUpperInvariant().Contains(search3.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search4.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.NUMPALET.ToUpperInvariant().Contains(search4.ToUpperInvariant())).ToList();  // prue.ToList();
            }

            if (search5.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.FECHA_SSCC_CON.ToUpperInvariant().Contains(search5.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search8.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.ESTADO.ToUpperInvariant().Contains(search8.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            if (search9.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.SSCC.ToUpperInvariant().Contains(search9.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            /* if (search7.Length > 0)
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
            }*/
            if (search12.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.FECHA_CREACION.ToUpperInvariant().Contains(search12.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            /*  if (search11.Length > 0)
              {
                  filterRecords = filterRecords.Where(l => l.HORA.ToUpperInvariant().Contains(search11.ToUpperInvariant())).ToList();  // prue.ToList();
              }*/
            if (search14.Length > 0)
            {
                filterRecords = filterRecords.Where(l => l.FECHACADUCIDAD.ToUpperInvariant().Contains(search14.ToUpperInvariant())).ToList();  // prue.ToList();
            }
            var orderedResults = sortOrder == "asc"
                                ? filterRecords.OrderBy(o => o.ID)
                                : filterRecords.OrderByDescending(o => o.ID);
            var itemsToSkip = displayStart == 0
                              ? 0
                              : displayStart + 1;
            //  var filterRecords = (orderedResults);
            datos = orderedResults.ToDataTable();
            Session["datos"] = datos;



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
                sb.Append("\"" + result.LOTE_INTERNO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.NUMPALET.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.FECHA_SSCC_CON.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.UD_ACTUAL.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.KG_ACTUAL.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ESTADO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.SSCC.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.OBS_ESTADO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
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
                sb.Append("\"" + result.BOLETIN.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.BRIX.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.IMPUREZAS.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.CONSISTENCIA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
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
                sb.Append("\"" + result.FLATOXINA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.MERMA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.CONTROL_PESO_CALIDAD.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.CONTROL_PESO_LINEA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.CURVA_PH.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.TEXTURA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ABSORVANCIA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.WEB.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.CAG.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ESTADO_COMITE.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.HORA_ARRIBA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ESTADO_ARRIBA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.OBSERVA_ARRIBA.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.HORA_MEDIO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ESTADO_MEDIO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.OBSERVA_MEDIO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.HORA_ABAJO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.ESTADO_ABAJO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
                sb.Append("\"" + result.OBSERVA_ABAJO.Replace('\r', ' ').Replace('\n', ' ') + "\",");
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
            string sql = @"select [CARACTERISTICAS_ARTICULO].id, Articulo,ARTICULO.Descripción, [CARACTERISTICAS_ARTICULO].Caracteristica,[Organolectico_Carac].Caracteristica, V_Min, V_Max, Obligatorio, 
 [CARACTERISTICAS_ARTICULO].A_Lote
  FROM [QC600].[dbo].[CARACTERISTICAS_ARTICULO] inner join ARTICULO on ARTICULO.Artículo=[CARACTERISTICAS_ARTICULO].[Articulo]
  inner join [Organolectico_Carac] on [CARACTERISTICAS_ARTICULO].Caracteristica= [Organolectico_Carac].Cod_Caracteristica   where [CARACTERISTICAS_ARTICULO].Articulo=" + articulo;
            Quality con = new Quality();
            DataTable datos = con.Sql_Datatable(sql);
            List<Caracteristicas> lista_datos = new List<Caracteristicas>();
            foreach (DataRow lin in datos.AsEnumerable())
            {
                Caracteristicas linea = new Caracteristicas { Id = lin[0].ToString(), articulo = lin[1].ToString(), art_descripcion = lin[2].ToString(), Cod_Caracteristica = lin[3].ToString(), Caracteristica = lin[4].ToString(), V_Min = lin[5].ToString(), V_Max = lin[6].ToString(), Obligatorio = lin[7].ToString(), A_lote= lin[8].ToString() };
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
            public string A_lote { get; set; }

        }


        public IEnumerable<Organoleptico> Obtener_datos(string fecha1, string fecha2, string tipo)
        {
            DateTime f1 = Convert.ToDateTime(fecha1);
            DateTime f2 = Convert.ToDateTime(fecha2);
            string sql = "";
            if (f1 < Convert.ToDateTime("01/01/2019") && f2 < Convert.ToDateTime("01/01/2019"))
            {
                sql = @"SELECT  [DATOS_ORGANOLEPTICO_BK].[ID]
      ,[DATOS_ORGANOLEPTICO_BK].[ARTICULO]
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
      ,[DATOS_ORGANOLEPTICO_BK].[TEMPERATURA]
      ,[DATOS_ORGANOLEPTICO_BK].[PH]
      ,[DATOS_ORGANOLEPTICO_BK].[COLOR]
      ,[DATOS_ORGANOLEPTICO_BK].[SABOR]
      ,[DATOS_ORGANOLEPTICO_BK].[CORTE]
      ,[DATOS_ORGANOLEPTICO_BK].[FILM]
      ,[DATOS_ORGANOLEPTICO_BK].[CATA]
      ,[DATOS_ORGANOLEPTICO_BK].[GLUTEN]
      ,[DATOS_ORGANOLEPTICO_BK].[CASEINA]
      ,[DATOS_ORGANOLEPTICO_BK].[LISTERIA]
      ,[DATOS_ORGANOLEPTICO_BK].[SALMONELLA]
      ,[DATOS_ORGANOLEPTICO_BK].[PPC]
,replace( OBS_ESTADO,'/','') as [OBS_ESTADO]
,[BOLETIN]
,[DATOS_ORGANOLEPTICO_BK].[BRIX]  

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
,[DATOS_ORGANOLEPTICO_BK].[NC]

,[DATOS_ORGANOLEPTICO_BK].[SILO]
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
,[DATOS_ORGANOLEPTICO_BK].[TEXTURA]
,[ABSORVANCIA]
,[WEB]
,[CAG]
,[ESTADO_COMITE]
,[HORA_ARRIBA]
      ,[ESTADO_ARRIBA]
      ,[HORA_MEDIO]
      ,[ESTADO_MEDIO]
      ,[HORA_ABAJO]
      ,[ESTADO_ABAJO]
      ,[OBSEVACIONES_COMITE]
,[OBSERVA_ARRIBA]
      ,[OBSERVA_MEDIO]
      ,[OBSERVA_ABAJO]
,[DATOS_ORGANOLEPTICO_BK].[ESTADO] AS ID_ESTADO
,[SILO_DESTINO]
      ,[CROMATOGRAMAS_FILM]
,[PROTEINA_DILUCION]
      ,[GRASA_DILUCION],TTP ,TPP,SABOR_MERENGUE,OLOR_TORTILLA,SABOR_TORTILLA,Espectofotometro,[BOLETIN_GRASA]
      ,[BOLETIN_PROTEINA]
,[BRIX_PASTE_2500]
      ,[BRIX_PASTE_6000]
,[GLUTEN_BOLETIN]
 ,[BOLETIN_LISTERIA]
      ,[BOLETIN_SALMONELLA]
,[PRE_BOLETIN_FQ],[BOLETIN_SOJA],[BOLETIN_CARAMELO],[ETIQUETA_CORRECTA],[CURVA_FERMENTACION]
      ,[ATP_APERTURA]
,CATA_COMITE
      ,TEXTUROMETRO
,[MUESTROTECA]
      ,[CURVA_ENFRIAMIENTO]
      ,[CURVA_FERMENTACION_]
,[tp_tq_mix_crudo]
      ,[tp_tq_mix_paste]
      ,[tp_tq_enfr_dosif]
 ,[ph_box]
      ,[col_box]
      ,[olor_box]
      ,[sab_box]
      ,[ºB_box]
      ,[Purez]
      ,[Org_antes_envio]
      ,[ph_(50%)]
      ,[Consistencia_20º]
      ,[Sacarasa_bol]
      ,[(%)sal_bol]
      ,[ATP_Tq_formulacion]
      ,[ATP_tq_destino]
      ,[ATP_linea]
      ,[ph_(6.67%)]
      ,[olor_tort_box]
      ,[Sabor_tort_box]
      ,[ATP_manguera]
,[Mohos_levad]
,[Presencia_ojos]
,[Recirculacion]
,null as [b_tras_paste]
,null as bloom, null as CC_obli
,null as textu_tras_tunel
, null as textu_revision
,null as HR_bol
,null as t_frabricacion
,null as Enterobac
,null as Den
,null as Contrast
, null as Rev_Filtros
, null as Gras_Box
, null as Prot_box
, null as V1
, null as V2
, null as V3
  FROM [QC600].[dbo].[DATOS_ORGANOLEPTICO_BK]

  INNER JOIN ARTICULO ON ARTICULO.Artículo=[DATOS_ORGANOLEPTICO_BK].ARTICULO 
  INNER JOIN Estado ON Estado.ID_Estado=[DATOS_ORGANOLEPTICO_BK].ESTADO
  inner join FAMILIA on ARTICULO.Familia=Familia.Familia ";
                switch (tipo)
                {
                    case "1":
                        sql = sql + @" WHERE  (([DATOS_ORGANOLEPTICO_BK].SSCC in (select ssccsilo from SILO) ) or ([DATOS_ORGANOLEPTICO_BK].ID_LOTE in (select idlote from SSCC_CON inner join sscc on sscc.Id=sscc_con.IdPadre where sscc.sscc=DATOS_ORGANOLEPTICO_BK.SSCC))) and FECHA_CREACION Between convert(datetime,'" + fecha1 + @"',103) and convert(datetime,'" + fecha2 + @"',103)";
                        break;
                    case "2":
                        sql = sql + @" WHERE  (([DATOS_ORGANOLEPTICO_BK].SSCC in (select ssccsilo from SILO) ) or ([DATOS_ORGANOLEPTICO_BK].ID_LOTE in (select idlote from SSCC_CON inner join sscc on sscc.Id=sscc_con.IdPadre where sscc.sscc=DATOS_ORGANOLEPTICO_BK.SSCC))) and FECHA_CREACION Between convert(datetime,'" + fecha1 + @"',103) and convert(datetime,'" + fecha2 + @"',103) and FAMILIA.Agrupación=9";
                        break;
                    case "3":
                        sql = sql + @" WHERE  (([DATOS_ORGANOLEPTICO_BK].SSCC in (select ssccsilo from SILO) ) or ([DATOS_ORGANOLEPTICO_BK].ID_LOTE in (select idlote from SSCC_CON inner join sscc on sscc.Id=sscc_con.IdPadre where sscc.sscc=DATOS_ORGANOLEPTICO_BK.SSCC))) and FECHA_CREACION Between convert(datetime,'" + fecha1 + @"',103) and convert(datetime,'" + fecha2 + @"',103) and ((ARTICULO.Discrim_4='VE' and FAMILIA.Agrupación=2) OR (ARTICULO.Discrim_4 like '%P%') )";
                        break;
                    case "4":
                        sql = sql + @" WHERE  (([DATOS_ORGANOLEPTICO_BK].SSCC in (select ssccsilo from SILO) ) or ([DATOS_ORGANOLEPTICO_BK].ID_LOTE in (select idlote from SSCC_CON inner join sscc on sscc.Id=sscc_con.IdPadre where sscc.sscc=DATOS_ORGANOLEPTICO_BK.SSCC))) and FECHA_CREACION Between convert(datetime,'" + fecha1 + @"',103) and convert(datetime,'" + fecha2 + @"',103) and ARTICULO.Familia  Between 400 and 500";
                        break;
                }
            }
            else { 
             sql = @"SELECT  [DATOS_ORGANOLEPTICO].[ID]
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
      ,[DATOS_ORGANOLEPTICO].[TEMPERATURA]
      ,[DATOS_ORGANOLEPTICO].[PH]
      ,[DATOS_ORGANOLEPTICO].[COLOR]
      ,[DATOS_ORGANOLEPTICO].[SABOR]
      ,[DATOS_ORGANOLEPTICO].[CORTE]
      ,[DATOS_ORGANOLEPTICO].[FILM]
      ,[DATOS_ORGANOLEPTICO].[CATA]
      ,[DATOS_ORGANOLEPTICO].[GLUTEN]
      ,[DATOS_ORGANOLEPTICO].[CASEINA]
      ,[DATOS_ORGANOLEPTICO].[LISTERIA]
      ,[DATOS_ORGANOLEPTICO].[SALMONELLA]
      ,[DATOS_ORGANOLEPTICO].[PPC]
,replace( OBS_ESTADO,'/','') as [OBS_ESTADO]
,[BOLETIN]
,[DATOS_ORGANOLEPTICO].[BRIX]  

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
,[DATOS_ORGANOLEPTICO].[NC]

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
,[DATOS_ORGANOLEPTICO].[TEXTURA]
,[ABSORVANCIA]
,[WEB]
,[CAG]
,[ESTADO_COMITE]
,[HORA_ARRIBA]
      ,[ESTADO_ARRIBA]
      ,[HORA_MEDIO]
      ,[ESTADO_MEDIO]
      ,[HORA_ABAJO]
      ,[ESTADO_ABAJO]
      ,[OBSEVACIONES_COMITE]
,[OBSERVA_ARRIBA]
      ,[OBSERVA_MEDIO]
      ,[OBSERVA_ABAJO]
,[DATOS_ORGANOLEPTICO].[ESTADO] AS ID_ESTADO
,[SILO_DESTINO]
      ,[CROMATOGRAMAS_FILM]
,[PROTEINA_DILUCION]
      ,[GRASA_DILUCION],TTP ,TPP,SABOR_MERENGUE,OLOR_TORTILLA,SABOR_TORTILLA,Espectofotometro,[BOLETIN_GRASA]
      ,[BOLETIN_PROTEINA]
,[BRIX_PASTE_2500]
      ,[BRIX_PASTE_6000]
,[GLUTEN_BOLETIN]
 ,[BOLETIN_LISTERIA]
      ,[BOLETIN_SALMONELLA]
,[PRE_BOLETIN_FQ],[BOLETIN_SOJA],[BOLETIN_CARAMELO],[ETIQUETA_CORRECTA],[CURVA_FERMENTACION]
      ,[ATP_APERTURA]
,CATA_COMITE
      ,TEXTUROMETRO
,[MUESTROTECA]
      ,[CURVA_ENFRIAMIENTO]
      ,[CURVA_FERMENTACION_]
,[tp_tq_mix_crudo]
      ,[tp_tq_mix_paste]
      ,[tp_tq_enfr_dosif]
 ,[ph_box]
      ,[col_box]
      ,[olor_box]
      ,[sab_box]
      ,[ºB_box]
      ,[Purez]
      ,[Org_antes_envio]
      ,[ph_(50%)]
      ,[Consistencia_20º]
      ,[Sacarasa_bol]
      ,[(%)sal_bol]
      ,[ATP_Tq_formulacion]
      ,[ATP_tq_destino]
      ,[ATP_linea]
      ,[ph_(6.67%)]
      ,[olor_tort_box]
      ,[Sabor_tort_box]
      ,[ATP_manguera]
,[Mohos_levad]
,[Presencia_ojos]
,[Recirculacion]
,[b_tras_paste]
,bloom,CC_obli,textu_tras_tunel
,textu_revision
,HR_bol
,t_frabricacion
,Enterobac
,Den
,Contrast
,Rev_Filtros
,Gras_Box
,Prot_box
,V1
,V2
,V3
  FROM [QC600].[dbo].[DATOS_ORGANOLEPTICO]

  INNER JOIN ARTICULO ON ARTICULO.Artículo=[DATOS_ORGANOLEPTICO].ARTICULO 
  INNER JOIN Estado ON Estado.ID_Estado=[DATOS_ORGANOLEPTICO].ESTADO
  inner join FAMILIA on ARTICULO.Familia=Familia.Familia ";
            switch (tipo) { 
                case "1":
                sql =sql + @" WHERE  (([DATOS_ORGANOLEPTICO].SSCC in (select ssccsilo from SILO) ) or ([DATOS_ORGANOLEPTICO].ID_LOTE in (select idlote from SSCC_CON inner join sscc on sscc.Id=sscc_con.IdPadre where sscc.sscc=DATOS_ORGANOLEPTICO.SSCC))) and FECHA_CREACION Between convert(datetime,'" + fecha1 + @"',103) and convert(datetime,'" + fecha2 + @"',103)";
                    break;
                case "2":
                    sql = sql+ @" WHERE  (([DATOS_ORGANOLEPTICO].SSCC in (select ssccsilo from SILO) ) or ([DATOS_ORGANOLEPTICO].ID_LOTE in (select idlote from SSCC_CON inner join sscc on sscc.Id=sscc_con.IdPadre where sscc.sscc=DATOS_ORGANOLEPTICO.SSCC))) and FECHA_CREACION Between convert(datetime,'" + fecha1 + @"',103) and convert(datetime,'" + fecha2 + @"',103) and FAMILIA.Agrupación=9";
                    break;
                case "3":
                    sql = sql + @" WHERE  (([DATOS_ORGANOLEPTICO].SSCC in (select ssccsilo from SILO) ) or ([DATOS_ORGANOLEPTICO].ID_LOTE in (select idlote from SSCC_CON inner join sscc on sscc.Id=sscc_con.IdPadre where sscc.sscc=DATOS_ORGANOLEPTICO.SSCC))) and FECHA_CREACION Between convert(datetime,'" + fecha1 + @"',103) and convert(datetime,'" + fecha2 + @"',103) and ((ARTICULO.Discrim_4='VE' and FAMILIA.Agrupación=2) OR (ARTICULO.Discrim_4 like '%P%') )";
                    break;
                case "4":
                    sql = sql + @" WHERE  (([DATOS_ORGANOLEPTICO].SSCC in (select ssccsilo from SILO) ) or ([DATOS_ORGANOLEPTICO].ID_LOTE in (select idlote from SSCC_CON inner join sscc on sscc.Id=sscc_con.IdPadre where sscc.sscc=DATOS_ORGANOLEPTICO.SSCC))) and FECHA_CREACION Between convert(datetime,'" + fecha1 + @"',103) and convert(datetime,'" + fecha2 + @"',103) and ARTICULO.Familia  Between 400 and 500";
                    break;
            }
            }
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
                        TEXTURA = lin[69].ToString(),
                        ABSORVANCIA = lin[70].ToString(),
                        WEB = lin[71].ToString(),
                        CAG = lin[72].ToString(),

                        ESTADO_COMITE = lin[73].ToString(),
                        HORA_ARRIBA = lin[74].ToString(),
                        ESTADO_ARRIBA = lin[75].ToString(),
                        HORA_MEDIO = lin[76].ToString(),
                        ESTADO_MEDIO = lin[77].ToString(),
                        HORA_ABAJO = lin[78].ToString(),
                        ESTADO_ABAJO = lin[79].ToString(),
                        OBSEVACIONES_COMITE = lin[80].ToString(),
                        OBSERVA_ARRIBA = lin[81].ToString(),
                        OBSERVA_MEDIO = lin[82].ToString(),
                        OBSERVA_ABAJO = lin[83].ToString(),
                        ID_ESTADO = lin[84].ToString(),
                        SILO_DESTINO = lin[85].ToString(),
                        CROMATOGRAMAS_FILM = lin[86].ToString(),
                        PROTEINA_DILUCION =lin[87].ToString(),
                        GRASA_DILUCION=lin[88].ToString(),
                        TTP = lin[89].ToString(),

                        TPP = lin[90].ToString()
                        ,
                        SABOR_MERENGUE = lin[91].ToString()
                        , OLOR_TORTILLA= lin[92].ToString(),
                        SABOR_TORTILLA= lin[93].ToString()
                        ,Espectofotometro= lin[94].ToString(),
                        BOLETIN_GRASA = lin[95].ToString(),
                        BOLETIN_PROTEINA = lin[96].ToString(),
                        BRIX_PASTE_2500 = lin[97].ToString(),
                        BRIX_PASTE_6000 = lin[98].ToString(),
                        GLUTEN_BOLETIN =lin[99].ToString(),
                        BOLETIN_LISTERIA = lin[100].ToString(),
                        BOLETIN_SALMONELLA = lin[101].ToString(),
                        PRE_BOLETIN_FQ = lin[102].ToString(),
                        BOLETIN_SOJA = lin[103].ToString(),
                        BOLETIN_CARAMELO = lin[104].ToString(),
                        ETIQUETA_CORRECTA = lin[105].ToString(),
                       
                        CURVA_FERMENTACION = lin[106].ToString(),
    
                        ATP_APERTURA = lin[107].ToString(),
                        CATA_COMITE = lin[108].ToString(),
                        TEXTUROMETRO = lin[109].ToString(),
                        MUESTROTECA = lin[110].ToString(),
                        CURVA_ENFRIAMIENTO = lin[111].ToString(),
                        CURVA_FERMENTACION_ = lin[112].ToString(),
                        tp_tq_mix_crudo = lin[113].ToString(),
                        tp_tq_mix_paste = lin[114].ToString(),
                        tp_tq_enfr_dosif = lin[115].ToString(),
                        ph_box = lin[116].ToString(),
                        col_box = lin[117].ToString(),
                        olor_box = lin[118].ToString(),
                        sab_box = lin[119].ToString(),
                        B_box = lin[120].ToString(),
                        Purez = lin[121].ToString(),
                        Org_antes_envio = lin[122].ToString(),
                        ph_50 = lin[123].ToString(),
                        Consistencia_20 = lin[124].ToString(),
                        Sacarasa_bol = lin[125].ToString(),
                        sal_bol = lin[126].ToString(),
                        ATP_Tq_formulacion = lin[127].ToString(),
                        ATP_tq_destino = lin[128].ToString(),
                        ATP_linea = lin[129].ToString(),
                        ph_6_67 = lin[130].ToString(),
                        olor_tort_box = lin[131].ToString(),
                        Sabor_tort_box = lin[132].ToString(),
                        ATP_manguera = lin[133].ToString(),
                        Mohos_levad=lin[134].ToString(),
                        Presencia_ojos=lin[135].ToString(),
                        Recirculacion = lin[136].ToString(),
                        b_tras_paste = lin[137].ToString(),
                        bloom = lin[138].ToString(),
                        CC_obli = lin[139].ToString(),
                        textu_tras_tunel = lin[140].ToString(),
                        textu_revision = lin[141].ToString(),
                        HR_bol= lin[142].ToString(),
                        t_frabricacion= lin[143].ToString(),
                        Enterobac= lin[144].ToString(),
                        Den = lin[145].ToString(),
                        Contrast = lin[146].ToString(),
                        Rev_Filtros = lin[147].ToString(),
                        Gras_Box = lin[148].ToString(),
                        Prot_box = lin[149].ToString(),
                        V1 = lin[150].ToString(),
                        V2 = lin[151].ToString(),
                        V3 = lin[152].ToString()
                    };
                    lista_datos.Add(linea);
                }
            }
            return lista_datos;
        }
        public IEnumerable<Organoleptico> Obtener_datos_Matricula(string matricula)
        {
            string sql = @"SELECT  [DATOS_ORGANOLEPTICO].[ID]
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
      ,[DATOS_ORGANOLEPTICO].[TEMPERATURA]
      ,[DATOS_ORGANOLEPTICO].[PH]
      ,[DATOS_ORGANOLEPTICO].[COLOR]
      ,[DATOS_ORGANOLEPTICO].[SABOR]
      ,[DATOS_ORGANOLEPTICO].[CORTE]
      ,[DATOS_ORGANOLEPTICO].[FILM]
      ,[DATOS_ORGANOLEPTICO].[CATA]
      ,[DATOS_ORGANOLEPTICO].[GLUTEN]
      ,[DATOS_ORGANOLEPTICO].[CASEINA]
      ,[DATOS_ORGANOLEPTICO].[LISTERIA]
      ,[DATOS_ORGANOLEPTICO].[SALMONELLA]
      ,[DATOS_ORGANOLEPTICO].[PPC]
,[OBS_ESTADO]
,[BOLETIN]
,[DATOS_ORGANOLEPTICO].[BRIX]  

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
,[DATOS_ORGANOLEPTICO].[NC]

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
,[DATOS_ORGANOLEPTICO].[TEXTURA]
,[ABSORVANCIA]
,[WEB]
,[CAG]
,[ESTADO_COMITE]
,[HORA_ARRIBA]
      ,[ESTADO_ARRIBA]
      ,[HORA_MEDIO]
      ,[ESTADO_MEDIO]
      ,[HORA_ABAJO]
      ,[ESTADO_ABAJO]
      ,[OBSEVACIONES_COMITE]
,[OBSERVA_ARRIBA]
      ,[OBSERVA_MEDIO]
      ,[OBSERVA_ABAJO]
,[DATOS_ORGANOLEPTICO].[ESTADO] AS ID_ESTADO
,[SILO_DESTINO]
      ,[CROMATOGRAMAS_FILM]
,[PROTEINA_DILUCION]
      ,[GRASA_DILUCION],TTP ,TPP,SABOR_MERENGUE,OLOR_TORTILLA,SABOR_TORTILLA,Espectofotometro,[BOLETIN_GRASA]
      ,[BOLETIN_PROTEINA]
,[BRIX_PASTE_2500]
      ,[BRIX_PASTE_6000]
,[GLUTEN_BOLETIN]
 ,[BOLETIN_LISTERIA]
      ,[BOLETIN_SALMONELLA]
,[PRE_BOLETIN_FQ],[BOLETIN_SOJA],[BOLETIN_CARAMELO],[ETIQUETA_CORRECTA],[CURVA_FERMENTACION]
      ,[ATP_APERTURA]
,CATA_COMITE
      ,TEXTUROMETRO
,[MUESTROTECA]
      ,[CURVA_ENFRIAMIENTO]
      ,[CURVA_FERMENTACION_]
,[tp_tq_mix_crudo]
      ,[tp_tq_mix_paste]
      ,[tp_tq_enfr_dosif]
 ,[ph_box]
      ,[col_box]
      ,[olor_box]
      ,[sab_box]
      ,[ºB_box]
      ,[Purez]
      ,[Org_antes_envio]
      ,[ph_(50%)]
      ,[Consistencia_20º]
      ,[Sacarasa_bol]
      ,[(%)sal_bol]
      ,[ATP_Tq_formulacion]
      ,[ATP_tq_destino]
      ,[ATP_linea]
      ,[ph_(6.67%)]
      ,[olor_tort_box]
      ,[Sabor_tort_box]
      ,[ATP_manguera]
,[Mohos_levad],
[Presencia_ojos]
,[Recirculacion]
,b_tras_paste,
bloom,CC_obli,textu_tras_tunel
,textu_revision
,HR_bol
,t_frabricacion
,Enterobac
,Den
,Contrast
,Rev_Filtros
,Gras_Box
,Prot_box
,V1 
,V2
,V3
  FROM [QC600].[dbo].[DATOS_ORGANOLEPTICO]
inner join sscc_con on idlote=ID_LOTE
  INNER JOIN ARTICULO ON ARTICULO.Artículo=[DATOS_ORGANOLEPTICO].ARTICULO 
  INNER JOIN Estado ON Estado.ID_Estado=[DATOS_ORGANOLEPTICO].ESTADO  WHERE  SSCC = '" + matricula + @"'";
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
                        TEXTURA = lin[69].ToString(),
                        ABSORVANCIA = lin[70].ToString(),
                        WEB = lin[71].ToString(),
                        CAG = lin[72].ToString(),
                        ESTADO_COMITE = lin[73].ToString(),
                        HORA_ARRIBA = lin[74].ToString(),
                        ESTADO_ARRIBA = lin[75].ToString(),
                        HORA_MEDIO = lin[76].ToString(),
                        ESTADO_MEDIO = lin[77].ToString(),
                        HORA_ABAJO = lin[78].ToString(),
                        ESTADO_ABAJO = lin[79].ToString(),
                        OBSEVACIONES_COMITE = lin[80].ToString(),
                        OBSERVA_ARRIBA = lin[81].ToString(),
                        OBSERVA_MEDIO = lin[82].ToString(),
                        OBSERVA_ABAJO = lin[83].ToString(),
                        ID_ESTADO = lin[84].ToString(),
                        SILO_DESTINO = lin[85].ToString(),
                        CROMATOGRAMAS_FILM = lin[86].ToString(),
                        PROTEINA_DILUCION = lin[87].ToString(),
                        GRASA_DILUCION = lin[88].ToString(),
                        TTP = lin[89].ToString(),

                        TPP = lin[90].ToString()
                        ,
                        SABOR_MERENGUE = lin[91].ToString(),
                        OLOR_TORTILLA = lin[92].ToString(),
                        SABOR_TORTILLA = lin[93].ToString()
                        ,Espectofotometro = lin[94].ToString(),
                        BOLETIN_GRASA = lin[95].ToString(),
                        BOLETIN_PROTEINA = lin[96].ToString(),
                        BRIX_PASTE_2500 = lin[97].ToString(),
                        BRIX_PASTE_6000 = lin[98].ToString(),
                        GLUTEN_BOLETIN = lin[99].ToString(),
                        BOLETIN_LISTERIA = lin[100].ToString(),
                        BOLETIN_SALMONELLA = lin[101].ToString(),
                        PRE_BOLETIN_FQ = lin[102].ToString()
                        ,
                        BOLETIN_SOJA = lin[103].ToString(),
                        BOLETIN_CARAMELO = lin[104].ToString(),
                        ETIQUETA_CORRECTA = lin[105].ToString(),

                               CURVA_FERMENTACION = lin[106].ToString(),

                        ATP_APERTURA = lin[107].ToString(),
                        CATA_COMITE = lin[108].ToString(),
                        TEXTUROMETRO = lin[109].ToString(),
                        MUESTROTECA = lin[110].ToString(),
                        CURVA_ENFRIAMIENTO = lin[111].ToString(),
                        CURVA_FERMENTACION_ = lin[112].ToString(),
                        tp_tq_mix_crudo = lin[113].ToString(),
                        tp_tq_mix_paste = lin[114].ToString(),
                        tp_tq_enfr_dosif = lin[115].ToString(),
                        ph_box = lin[116].ToString(),
                        col_box = lin[117].ToString(),
                        olor_box = lin[118].ToString(),
                        sab_box = lin[119].ToString(),
                        B_box = lin[120].ToString(),
                        Purez = lin[121].ToString(),
                        Org_antes_envio = lin[122].ToString(),
                        ph_50 = lin[123].ToString(),
                        Consistencia_20 = lin[124].ToString(),
                        Sacarasa_bol = lin[125].ToString(),
                        sal_bol = lin[126].ToString(),
                        ATP_Tq_formulacion = lin[127].ToString(),
                        ATP_tq_destino = lin[128].ToString(),
                        ATP_linea = lin[129].ToString(),
                        ph_6_67 = lin[130].ToString(),
                        olor_tort_box = lin[131].ToString(),
                        Sabor_tort_box = lin[132].ToString(),
                        ATP_manguera = lin[133].ToString(),
                        Mohos_levad=lin[134].ToString(),
                        Presencia_ojos = lin[135].ToString(),
                        Recirculacion = lin[136].ToString(),
                        b_tras_paste = lin[137].ToString(),
                        bloom = lin[138].ToString(),
                        CC_obli = lin[139].ToString(),
                        textu_tras_tunel = lin[140].ToString(),
                        textu_revision = lin[141].ToString(),
                        HR_bol=lin[142].ToString(),
                        t_frabricacion = lin[143].ToString(),
                        Enterobac = lin[144].ToString(),
                        Den = lin[145].ToString(),
                        Contrast = lin[146].ToString(),
                        Rev_Filtros = lin[147].ToString(),
                        Gras_Box = lin[148].ToString(),
                        Prot_box = lin[149].ToString(),
                        V1 = lin[150].ToString(),
                        V2 = lin[151].ToString(),
                        V3 = lin[152].ToString()
                    };
                    lista_datos.Add(linea);
                }
            }
            return lista_datos;
        }
        public IEnumerable<Organoleptico> Obtener_datos_Matricula(string lote,string fecha1,string fecha2)
        {
            string sql = @"SELECT  [DATOS_ORGANOLEPTICO].[ID]
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
      ,[DATOS_ORGANOLEPTICO].[TEMPERATURA]
      ,[DATOS_ORGANOLEPTICO].[PH]
      ,[DATOS_ORGANOLEPTICO].[COLOR]
      ,[DATOS_ORGANOLEPTICO].[SABOR]
      ,[DATOS_ORGANOLEPTICO].[CORTE]
      ,[DATOS_ORGANOLEPTICO].[FILM]
      ,[DATOS_ORGANOLEPTICO].[CATA]
      ,[DATOS_ORGANOLEPTICO].[GLUTEN]
      ,[DATOS_ORGANOLEPTICO].[CASEINA]
      ,[DATOS_ORGANOLEPTICO].[LISTERIA]
      ,[DATOS_ORGANOLEPTICO].[SALMONELLA]
      ,[DATOS_ORGANOLEPTICO].[PPC]
,[OBS_ESTADO]
,[BOLETIN]
,[DATOS_ORGANOLEPTICO].[BRIX]  

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
,[DATOS_ORGANOLEPTICO].[NC]

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
,[DATOS_ORGANOLEPTICO].[TEXTURA]
,[ABSORVANCIA]
,[WEB]
,[CAG]
,[ESTADO_COMITE]
,[HORA_ARRIBA]
      ,[ESTADO_ARRIBA]
      ,[HORA_MEDIO]
      ,[ESTADO_MEDIO]
      ,[HORA_ABAJO]
      ,[ESTADO_ABAJO]
      ,[OBSEVACIONES_COMITE]
,[OBSERVA_ARRIBA]
      ,[OBSERVA_MEDIO]
      ,[OBSERVA_ABAJO]
,[DATOS_ORGANOLEPTICO].[ESTADO] AS ID_ESTADO
,[SILO_DESTINO]
      ,[CROMATOGRAMAS_FILM]
,[PROTEINA_DILUCION]
      ,[GRASA_DILUCION],TTP ,TPP,SABOR_MERENGUE,OLOR_TORTILLA,SABOR_TORTILLA,Espectofotometro,[BOLETIN_GRASA]
      ,[BOLETIN_PROTEINA]
,[BRIX_PASTE_2500]
      ,[BRIX_PASTE_6000]
,[GLUTEN_BOLETIN]
 ,[BOLETIN_LISTERIA]
      ,[BOLETIN_SALMONELLA]
,[PRE_BOLETIN_FQ],[BOLETIN_SOJA],[BOLETIN_CARAMELO],[ETIQUETA_CORRECTA],[CURVA_FERMENTACION]
      ,[ATP_APERTURA],
CATA_COMITE
      ,TEXTUROMETRO
,[MUESTROTECA]
      ,[CURVA_ENFRIAMIENTO]
      ,[CURVA_FERMENTACION_
,[tp_tq_mix_crudo]
      ,[tp_tq_mix_paste]
      ,[tp_tq_enfr_dosif]
 ,[ph_box]
      ,[col_box]
      ,[olor_box]
      ,[sab_box]
      ,[ºB_box]
      ,[Purez]
      ,[Org_antes_envio]
      ,[ph_(50%)]
      ,[Consistencia_20º]
      ,[Sacarasa_bol]
      ,[(%)sal_bol]
      ,[ATP_Tq_formulacion]
      ,[ATP_tq_destino]
      ,[ATP_linea]
      ,[ph_(6.67%)]
      ,[olor_tort_box]
      ,[Sabor_tort_box]
      ,[ATP_manguera]
,[Mohos_levad]
,[Presencia_ojos]
,[Recirculacion]
,b_tras_paste
,bloom,CC_obli
,textu_tras_tunel
,textu_revision
,HR_bol
,t_frabricacion
,Enterobac
,Den
,Contrast
,Rev_Filtros
,Gras_Box
,Prot_box
,V1
,V2
,V3
  FROM [QC600].[dbo].[DATOS_ORGANOLEPTICO]
inner join sscc_con on idlote=ID_LOTE
  INNER JOIN ARTICULO ON ARTICULO.Artículo=[DATOS_ORGANOLEPTICO].ARTICULO 
  INNER JOIN Estado ON Estado.ID_Estado=[DATOS_ORGANOLEPTICO].ESTADO  WHERE  [LOTE_INTERNO] like '%" + lote + @"%' AND FECHA_SSCC_CON Between convert(datetime,'" + fecha1 + @"',103) and convert(datetime,'" + fecha2 + @"',103)";
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
                        TEXTURA = lin[69].ToString(),
                        ABSORVANCIA = lin[70].ToString(),
                        WEB = lin[71].ToString(),
                        CAG = lin[72].ToString(),
                        ESTADO_COMITE = lin[73].ToString(),
                        HORA_ARRIBA = lin[74].ToString(),
                        ESTADO_ARRIBA = lin[75].ToString(),
                        HORA_MEDIO = lin[76].ToString(),
                        ESTADO_MEDIO = lin[77].ToString(),
                        HORA_ABAJO = lin[78].ToString(),
                        ESTADO_ABAJO = lin[79].ToString(),
                        OBSEVACIONES_COMITE = lin[80].ToString(),
                        OBSERVA_ARRIBA = lin[81].ToString(),
                        OBSERVA_MEDIO = lin[82].ToString(),
                        OBSERVA_ABAJO = lin[83].ToString(),
                        ID_ESTADO = lin[84].ToString(),
                        SILO_DESTINO = lin[85].ToString(),
                        CROMATOGRAMAS_FILM = lin[86].ToString(),
                        PROTEINA_DILUCION = lin[87].ToString(),
                        GRASA_DILUCION = lin[88].ToString(),
 TTP = lin[89].ToString(),

                        TPP = lin[90].ToString()
                        ,
                        SABOR_MERENGUE = lin[91].ToString(),
                        OLOR_TORTILLA = lin[92].ToString(),
                        SABOR_TORTILLA = lin[93].ToString(),
                        Espectofotometro = lin[94].ToString(),
                        BOLETIN_GRASA = lin[95].ToString(),
                        BOLETIN_PROTEINA = lin[96].ToString(),
                        BRIX_PASTE_2500 = lin[97].ToString(),
                        BRIX_PASTE_6000 = lin[98].ToString(),
                        GLUTEN_BOLETIN= lin[99].ToString(),
                        BOLETIN_LISTERIA = lin[100].ToString(),
                        BOLETIN_SALMONELLA = lin[101].ToString(),
                        PRE_BOLETIN_FQ = lin[102].ToString(),
                        BOLETIN_SOJA = lin[103].ToString(),
                        BOLETIN_CARAMELO = lin[104].ToString(),
                        ETIQUETA_CORRECTA = lin[105].ToString(),

                               CURVA_FERMENTACION = lin[106].ToString(),

                        ATP_APERTURA = lin[107].ToString(),
                        CATA_COMITE = lin[108].ToString(),
                        TEXTUROMETRO = lin[109].ToString(),
                        MUESTROTECA = lin[110].ToString(),
                        CURVA_ENFRIAMIENTO = lin[111].ToString(),
                        CURVA_FERMENTACION_ = lin[112].ToString(),
                        tp_tq_mix_crudo = lin[113].ToString(),
                        tp_tq_mix_paste = lin[114].ToString(),
                        tp_tq_enfr_dosif = lin[115].ToString(),
                        ph_box = lin[116].ToString(),
                        col_box = lin[117].ToString(),
                        olor_box = lin[118].ToString(),
                        sab_box = lin[119].ToString(),
                        B_box = lin[120].ToString(),
                        Purez = lin[121].ToString(),
                        Org_antes_envio = lin[122].ToString(),
                        ph_50 = lin[123].ToString(),
                        Consistencia_20 = lin[124].ToString(),
                        Sacarasa_bol = lin[125].ToString(),
                        sal_bol = lin[126].ToString(),
                        ATP_Tq_formulacion = lin[127].ToString(),
                        ATP_tq_destino = lin[128].ToString(),
                        ATP_linea = lin[129].ToString(),
                        ph_6_67 = lin[130].ToString(),
                        olor_tort_box = lin[131].ToString(),
                        Sabor_tort_box = lin[132].ToString(),
                        ATP_manguera = lin[133].ToString(),
                        Mohos_levad=lin[134].ToString(),
                        Presencia_ojos = lin[135].ToString(),
                        Recirculacion=lin[136].ToString(),
                        b_tras_paste = lin[137].ToString(),
                        bloom = lin[138].ToString(),
                        CC_obli = lin[139].ToString(),
                        textu_tras_tunel = lin[140].ToString(),
                        textu_revision = lin[141].ToString(),
                        HR_bol= lin[142].ToString(),
                        t_frabricacion = lin[143].ToString(),
                        Enterobac = lin[144].ToString(),
                        Den = lin[145].ToString(),
                        Contrast = lin[146].ToString(),
                        Rev_Filtros = lin[147].ToString(),
                        Gras_Box = lin[148].ToString(),
                        Prot_box = lin[149].ToString(),
                        V1 = lin[150].ToString(),
                        V2 = lin[151].ToString(),
                        V3 = lin[152].ToString()
                    };
                    lista_datos.Add(linea);
                }
            }
            return lista_datos;
        }
        public IEnumerable<Organoleptico> Obtener_fila_datos(string IdLote)
        {
            string sql = @"SELECT   [DATOS_ORGANOLEPTICO].[ID]
      ,[DATOS_ORGANOLEPTICO].[ARTICULO]
	  ,[ARTICULO].[Descr Abreviada] as [DESCRIPCION]
      ,[DATOS_ORGANOLEPTICO].[FECHA_SSCC_CON]
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
      ,[DATOS_ORGANOLEPTICO].[TEMPERATURA]
      ,[DATOS_ORGANOLEPTICO].[PH]
      ,[DATOS_ORGANOLEPTICO].[COLOR]
      ,[DATOS_ORGANOLEPTICO].[SABOR]
      ,[DATOS_ORGANOLEPTICO].[CORTE]
      ,[DATOS_ORGANOLEPTICO].[FILM]
      ,[DATOS_ORGANOLEPTICO].[CATA]
      ,[DATOS_ORGANOLEPTICO].[GLUTEN]
      ,[DATOS_ORGANOLEPTICO].[CASEINA]
      ,[DATOS_ORGANOLEPTICO].[LISTERIA]
      ,[DATOS_ORGANOLEPTICO].[SALMONELLA]
      ,[DATOS_ORGANOLEPTICO].[PPC]
,[OBS_ESTADO]
,[BOLETIN]
,[DATOS_ORGANOLEPTICO].[BRIX]  

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
,[DATOS_ORGANOLEPTICO].[NC]

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
,[DATOS_ORGANOLEPTICO].[TEXTURA]
,[ABSORVANCIA]
,[WEB]
,[CAG]
,[ESTADO_COMITE]
,[HORA_ARRIBA]
      ,[ESTADO_ARRIBA]
      ,[HORA_MEDIO]
      ,[ESTADO_MEDIO]
      ,[HORA_ABAJO]
      ,[ESTADO_ABAJO]
      ,[OBSEVACIONES_COMITE]
,[OBSERVA_ARRIBA]
      ,[OBSERVA_MEDIO]
      ,[OBSERVA_ABAJO]
,[DATOS_ORGANOLEPTICO].[ESTADO] AS ID_ESTADO

,[SILO_DESTINO]
      ,[CROMATOGRAMAS_FILM]
,[PROTEINA_DILUCION]
      ,[GRASA_DILUCION]
,TTP ,TPP,SABOR_MERENGUE,OLOR_TORTILLA,SABOR_TORTILLA,Espectofotometro,[BOLETIN_GRASA]
      ,[BOLETIN_PROTEINA]
,[BRIX_PASTE_2500]
      ,[BRIX_PASTE_6000]
,[GLUTEN_BOLETIN]
,[BOLETIN_LISTERIA]
      ,[BOLETIN_SALMONELLA]
,[PRE_BOLETIN_FQ],[BOLETIN_SOJA],[BOLETIN_CARAMELO],[ETIQUETA_CORRECTA],[CURVA_FERMENTACION]
      ,[ATP_APERTURA]
,CATA_COMITE
      ,TEXTUROMETRO
,[MUESTROTECA]
      ,[CURVA_ENFRIAMIENTO]
      ,[CURVA_FERMENTACION_]
,[tp_tq_mix_crudo]
      ,[tp_tq_mix_paste]
      ,[tp_tq_enfr_dosif]
 ,[ph_box]
      ,[col_box]
      ,[olor_box]
      ,[sab_box]
      ,[ºB_box]
      ,[Purez]
      ,[Org_antes_envio]
      ,[ph_(50%)]
      ,[Consistencia_20º]
      ,[Sacarasa_bol]
      ,[(%)sal_bol]
      ,[ATP_Tq_formulacion]
      ,[ATP_tq_destino]
      ,[ATP_linea]
      ,[ph_(6.67%)]
      ,[olor_tort_box]
      ,[Sabor_tort_box]
      ,[ATP_manguera]
,[Mohos_levad]
,[Presencia_ojos]
,[Recirculacion]
,[b_tras_paste],
bloom,CC_obli
,textu_tras_tunel
,textu_revision
,HR_bol
,t_frabricacion,
Enterobac,
Den
,Contrast
,Rev_Filtros
,Gras_Box
,Prot_box
,V1
,V2
,V3
  FROM [QC600].[dbo].[DATOS_ORGANOLEPTICO]
inner join sscc_con on idlote=ID_LOTE
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
                        TEXTURA = lin[69].ToString(),
                        ABSORVANCIA = lin[70].ToString(),
                        WEB = lin[71].ToString(),
                        CAG = lin[72].ToString(),
                        ESTADO_COMITE = lin[73].ToString(),
                        HORA_ARRIBA = lin[74].ToString(),
                        ESTADO_ARRIBA = lin[75].ToString(),
                        HORA_MEDIO = lin[76].ToString(),
                        ESTADO_MEDIO = lin[77].ToString(),
                        HORA_ABAJO = lin[78].ToString(),
                        ESTADO_ABAJO = lin[79].ToString(),
                        OBSEVACIONES_COMITE = lin[80].ToString(),
                        OBSERVA_ARRIBA = lin[81].ToString(),
                        OBSERVA_MEDIO = lin[82].ToString(),
                        OBSERVA_ABAJO = lin[83].ToString(),
                        ID_ESTADO = lin[84].ToString(),
                      SILO_DESTINO = lin[85].ToString(),
                        CROMATOGRAMAS_FILM = lin[86].ToString(),
                        PROTEINA_DILUCION = lin[87].ToString(),
                        GRASA_DILUCION = lin[88].ToString(),
                        TTP = lin[89].ToString(),

         TPP = lin[90].ToString(),
         SABOR_MERENGUE= lin[91].ToString(),
                        OLOR_TORTILLA = lin[92].ToString(),
                        SABOR_TORTILLA = lin[93].ToString(),
                        Espectofotometro= lin[94].ToString(),
                        BOLETIN_GRASA = lin[95].ToString(),
                        BOLETIN_PROTEINA = lin[96].ToString(),
                        BRIX_PASTE_2500 = lin[97].ToString() ,
                        BRIX_PASTE_6000 = lin[98].ToString(),
                        GLUTEN_BOLETIN = lin[99].ToString()
                        ,
                        BOLETIN_LISTERIA = lin[100].ToString(),
                        BOLETIN_SALMONELLA = lin[101].ToString()
                        ,
                        PRE_BOLETIN_FQ = lin[102].ToString(),
                        BOLETIN_SOJA = lin[103].ToString(),
                        BOLETIN_CARAMELO = lin[104].ToString(),
                        ETIQUETA_CORRECTA = lin[105].ToString(),
                        CURVA_FERMENTACION = lin[106].ToString(),

                        ATP_APERTURA = lin[107].ToString(),
                        CATA_COMITE = lin[108].ToString(),
                        TEXTUROMETRO = lin[109].ToString(),
                        MUESTROTECA = lin[110].ToString(),
                        CURVA_ENFRIAMIENTO = lin[111].ToString(),
                        CURVA_FERMENTACION_ = lin[112].ToString(),
                        tp_tq_mix_crudo = lin[113].ToString(),
                        tp_tq_mix_paste = lin[114].ToString(),
                        tp_tq_enfr_dosif = lin[115].ToString(),
                        ph_box = lin[116].ToString(),
                        col_box = lin[117].ToString(),
                        olor_box = lin[118].ToString(),
                        sab_box = lin[119].ToString(),
                        B_box = lin[120].ToString(),
                        Purez = lin[121].ToString(),
                        Org_antes_envio = lin[122].ToString(),
                        ph_50 = lin[123].ToString(),
                        Consistencia_20 = lin[124].ToString(),
                        Sacarasa_bol = lin[125].ToString(),
                        sal_bol = lin[126].ToString(),
                        ATP_Tq_formulacion = lin[127].ToString(),
                        ATP_tq_destino = lin[128].ToString(),
                        ATP_linea = lin[129].ToString(),
                        ph_6_67 = lin[130].ToString(),
                        olor_tort_box = lin[131].ToString(),
                        Sabor_tort_box = lin[132].ToString(),
                        ATP_manguera = lin[133].ToString(),
                        Mohos_levad=lin[134].ToString(),
                        Presencia_ojos = lin[135].ToString(),
                        Recirculacion=lin[136].ToString(),
                        b_tras_paste = lin[137].ToString(),
                        bloom = lin[138].ToString(),
                        CC_obli = lin[139].ToString(),
                        textu_tras_tunel = lin[140].ToString(),
                        textu_revision = lin[141].ToString(),
                        HR_bol=lin[142].ToString(),
                        t_frabricacion = lin[143].ToString(),
                        Enterobac = lin[144].ToString(),
                        Den = lin[145].ToString(),
                        Contrast = lin[146].ToString(),
                        Rev_Filtros = lin[147].ToString(),
                        Gras_Box = lin[148].ToString(),
                        Prot_box = lin[149].ToString(),
                        V1 = lin[150].ToString(),
                        V2 = lin[151].ToString(),
                        V3 = lin[152].ToString()

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
            public string TEXTURA { get; set; }
            public string ABSORVANCIA { get; set; }
            public string WEB { get; set; }
            public string CAG { get; set; }
            public string ESTADO_COMITE { get; set; }
            public string HORA_ARRIBA { get; set; }
            public string ESTADO_ARRIBA { get; set; }
            public string HORA_MEDIO { get; set; }
            public string ESTADO_MEDIO { get; set; }

            public string HORA_ABAJO { get; set; }
            public string ESTADO_ABAJO { get; set; }
            public string OBSEVACIONES_COMITE { get; set; }
            public string OBSERVA_ARRIBA { get; set; }
            public string OBSERVA_MEDIO { get; set; }
            public string OBSERVA_ABAJO { get; set; }
            public string PROTEINA_DILUCION { get; set; }
            public string GRASA_DILUCION { get; set; }

            public string SILO_DESTINO { get; set; }

            public string CROMATOGRAMAS_FILM { get; set; }
            public string TTP { get; set; }

            public string TPP { get; set; }
            public string OLOR_TORTILLA { get; set; }
            public string SABOR_TORTILLA { get; set; }
            public string SABOR_MERENGUE { get; set; }
            public string Espectofotometro { get; set; }
            
            public string BOLETIN_GRASA { get; set; }
            public string  BOLETIN_PROTEINA { get; set; }
            public string BRIX_PASTE_2500 { get; set; }
            public string BRIX_PASTE_6000 { get; set; }
            public string GLUTEN_BOLETIN { get; set; }
            public string BOLETIN_LISTERIA { get; set; }
            public string BOLETIN_SALMONELLA { get; set; }
            public string PRE_BOLETIN_FQ { get; set; }
            public string BOLETIN_SOJA { get; set; }
            public string BOLETIN_CARAMELO { get; set; }
            public string ETIQUETA_CORRECTA { get; set; }

            public string CURVA_FERMENTACION { get; set; }
            public string ATP_APERTURA { get; set; }
            public string CATA_COMITE { get; set; }
            public string TEXTUROMETRO { get; set; }
            public string MUESTROTECA { get; set; }
            public string CURVA_ENFRIAMIENTO { get; set; }
            public string CURVA_FERMENTACION_ { get; set; }
            public string tp_tq_mix_crudo { get; set; }
            public string tp_tq_mix_paste { get; set; }
            public string tp_tq_enfr_dosif { get; set; }

            public string ph_box { get; set; }
            public string col_box { get; set; }
            public string olor_box { get; set; }
            public string sab_box { get; set; }
            public string B_box { get; set; }
            public string Purez { get; set; }
            public string Org_antes_envio { get; set; }
            public string ph_50 { get; set; }
            public string Consistencia_20 { get; set; }
            public string Sacarasa_bol { get; set; }
            public string sal_bol { get; set; }
            public string ATP_Tq_formulacion { get; set; }
            public string ATP_tq_destino { get; set; }
            public string ATP_linea { get; set; }
            public string ph_6_67 { get; set; }
            public string olor_tort_box { get; set; }
            public string Sabor_tort_box { get; set; }
            public string ATP_manguera { get; set; }
            public string Mohos_levad { get; set; }
            public string Presencia_ojos { get; set; }

            public string Recirculacion { get; set; }
            public string b_tras_paste { get; set; }
            public string bloom { get; set; }
            public string CC_obli { get; set; }
            public string textu_tras_tunel  { get; set; }
            public string textu_revision  { get; set; }
            public string HR_bol { get; set; }
            public string t_frabricacion { get; set; }
            public string Enterobac { get; set; }
            public string Den { get; set; }
            public string Contrast { get; set; }
            public string Rev_Filtros { get; set; }
            public string CARAC_LOTE { get; set; }
            public string Gras_Box { get; set; }
            public string Prot_box { get; set; }
            public string V1 { get; set; }
            public string V2 { get; set; }
            public string V3 { get; set; }
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hola a todos";
        }
    }
    }

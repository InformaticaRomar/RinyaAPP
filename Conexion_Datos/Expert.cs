using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Data;
namespace Conexion_Datos
{
    public class Expert
    {
        public bool sql_update(string sql)
        {
            string Con_E = Properties.Settings.Default.Conex_Expert;

            using (OracleConnection con = new OracleConnection(Con_E))
            {
                try
                {
                    con.Open();
                    OracleCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
        public DataTable Sql_Datatable(string sql)
        {
            string Con_E = Properties.Settings.Default.Conex_Expert;
            DataTable dt = new DataTable("Lineas");

            using (OracleConnection cnn2 = new OracleConnection(Con_E))
            {
                try
                {
                    cnn2.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cnn2;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    OracleDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                    dt.Load(dr);

                    cnn2.Dispose();
                    return dt;
                }
                catch (Exception)
                {
                    throw;

                }

            }
        }
    }
}

using System.Data;
using System.Data.SqlClient;
using System;
using WpfFront.Model;

namespace WpfFront.Utilidades
{

    public partial class SQL
    {

        public SqlConnection Connection;
        public SqlCommand Command;

        public SQL()
        {
            Connection = new SqlConnection(); //GetCnnString()
            Command = new SqlCommand();
        }

        public void DirectSQLNonQuery(string query, Conexion connection)
        {
            //SQLBase.ExecuteQuery(query, new SqlConnection(connection.Cadena));

            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection = new SqlConnection(connection.Cadena);
                    Connection.Open();
                }

                SqlCommand xCommand = new SqlCommand();
                xCommand.Connection = Connection;
                xCommand.CommandText = query;

                if (xCommand.Connection.State != ConnectionState.Open)
                    xCommand.Connection.Open();

                try { xCommand.ExecuteNonQuery(); }
                catch { }

            }
            catch (Exception )
            {
            }
            finally
            {
                Connection.Close();
            }


        }

        public DataTable DirectSQLQuery(string query, string swhere, string tableName, Conexion connection)
        {
            //return SQLBase.ReturnDataTable(query, swhere, tableName, new SqlConnection(connection.Cadena));

            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection = new SqlConnection(connection.Cadena);
                    Connection.Open();
                }

                DataTable ds = new DataTable(tableName);

                swhere = string.IsNullOrEmpty(swhere) ? swhere : " AND " + swhere;

                SqlDataAdapter objAdapter = new SqlDataAdapter(query + swhere, Connection);

                //Console.WriteLine(Query + sWhere);

                objAdapter.Fill(ds);

                return ds;
            }
            catch (Exception)
            {
                return null;
            }
            finally {
                Connection.Close();
            }

        }
    }
}

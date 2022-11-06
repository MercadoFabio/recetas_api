using RecetasSLN.dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RecetasBack.datos
{
    public class HelperDao
    {
        
        private static HelperDao instance;
        private string conectionString;


        private HelperDao()
        {
            conectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=db_recetas10;Integrated Security=True";
        }
        public static HelperDao GetInstance()
        {
            if (instance == null)
            {
                instance = new HelperDao();
            }
            return instance;
        }
        public DataTable ConsultaSql(string StoreName)
        {
            SqlConnection cnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                cnn.ConnectionString = conectionString;
                cnn.Open();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = StoreName;
                dt.Load(cmd.ExecuteReader());

            }
            catch (SqlException ex)
            {

                dt = null;
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }

            }
            return dt;
           
        }


        public bool save(Receta receta, string spMaestro, string spDetalle)
        {
            bool ok = true;
            SqlTransaction transaction = null;
            SqlConnection cnn = new SqlConnection(conectionString);
            try
            {
                cnn.Open();
                transaction = cnn.BeginTransaction();
                SqlCommand cmdMaestro = new SqlCommand(spMaestro, cnn, transaction);
                cmdMaestro.CommandType = CommandType.StoredProcedure;

                cmdMaestro.Parameters.AddWithValue("@id_receta", receta.RecetaNro);
                cmdMaestro.Parameters.AddWithValue("@tipo_receta", receta.TipoReceta);
                cmdMaestro.Parameters.AddWithValue("@nombre", receta.Nombre);
                if (receta.Chef != null)
                    cmdMaestro.Parameters.AddWithValue("@cheff", receta.Chef);
                else
                    cmdMaestro.Parameters.AddWithValue("@cheff", DBNull.Value);

                cmdMaestro.ExecuteNonQuery();
                int count = 1;
                //Se inserta Detalle Receta 
                foreach (DetalleReceta detalle in receta.DetalleRecetas)
                {
                    SqlCommand cmdDetalle = new SqlCommand(spDetalle, cnn, transaction);
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmdDetalle.Parameters.AddWithValue("@id_receta", receta.RecetaNro);
                    cmdDetalle.Parameters.AddWithValue("@id_ingrediente", detalle.Ingrediente.IngredienteID);
                    cmdDetalle.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                    count++;
                    cmdDetalle.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                ok = false;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
            return ok;
        }
   

        public int proximaReceta(string sp, string parametro)
        {

            SqlConnection cnn = new SqlConnection(conectionString);
            cnn.Open();
            SqlCommand cmd = new SqlCommand(sp, cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter param = new SqlParameter();
            param.ParameterName = parametro;
            param.SqlDbType = SqlDbType.Int;
            param.Direction = ParameterDirection.Output;

            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();
            cnn.Close();

            return (int)param.Value;
        }

    }
}

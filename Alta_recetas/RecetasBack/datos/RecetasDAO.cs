using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using RecetasSLN.dominio;
using RecetasBack.datos;

namespace RecetasSLN.datos
{
    class RecetasDAO : IRecetasDAO
    {

        public List<Receta> ListarRecetas()
        {

            DataTable t = HelperDao.GetInstance().ConsultaSql("SP_CONSULTAR_RECETAS");
            List<Receta> lst = new List<Receta>();
            foreach (DataRow row in t.Rows)
            {
                Receta rec = new Receta();
                rec.RecetaNro = Convert.ToInt32(row[0].ToString());
                rec.Nombre = row[1].ToString();
                rec.Chef = row[2].ToString();
                rec.TipoReceta = Convert.ToInt32(row[3].ToString());
                lst.Add(rec);
            }
            return lst;
        }

        public List<Ingrediente> ListarIngredientes()
        {

            DataTable t = HelperDao.GetInstance().ConsultaSql("SP_CONSULTAR_INGREDIENTES");
            List<Ingrediente> lst = new List<Ingrediente>();
            foreach (DataRow row in t.Rows)
            {
                Ingrediente ingrediente = new Ingrediente();
                ingrediente.IngredienteID = Convert.ToInt32(row[0].ToString());
                ingrediente.Nombre = row[1].ToString();
                ingrediente.UnidadMedida = row[2].ToString();

                lst.Add(ingrediente);
            }
            return lst;
        }


        public int ProximaReceta()
        {

            return HelperDao.GetInstance().proximaReceta("SP_PROXIMA_RECETA", "@proximo");

        }

        public bool EjecutarInsert(Receta oReceta)
        {
            return HelperDao.GetInstance().save(oReceta, "SP_INSERTAR_RECETA", "SP_INSERTAR_DETALLES");
        }

        public bool EditarReceta(Receta oReceta)
        {
            return HelperDao.GetInstance().save(oReceta, "SP_EDITAR_RECETA", "SP_INSERTAR_DETALLES");
        }
        public bool EliminarReceta(int id)
        {
            SqlConnection cnn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=db_recetas10;Integrated Security=True");

            int affected = 0;
            try
            {
                cnn.Open();

                SqlCommand cmd = new SqlCommand("SP_ELIMINAR_RECETAS", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idReceta", id);
                affected = cmd.ExecuteNonQuery();


            }
            catch (SqlException ex)
            {

            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                    cnn.Close();
            }

            return affected == 1;

        }

        public Receta getReceta(int Id)
        {

            Receta oReceta = new Receta();
            DetalleReceta dr = new DetalleReceta();
            SqlConnection cnn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=db_recetas10;Integrated Security=True");
            cnn.Open();
            SqlCommand cmd = new SqlCommand("GET_RECETA", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idReceta", Id);
            DataTable t = new DataTable();
            t.Load(cmd.ExecuteReader());
            cnn.Close();

            foreach (DataRow row in t.Rows)
            {
                oReceta.RecetaNro = Convert.ToInt32(row["id_receta"].ToString());
                oReceta.Nombre = row["nombre"].ToString();
                oReceta.Chef = row["cheff"].ToString();
                oReceta.RecetaNro = Convert.ToInt32(row["id_receta"].ToString());
            }
            return oReceta;
        }

    }
}

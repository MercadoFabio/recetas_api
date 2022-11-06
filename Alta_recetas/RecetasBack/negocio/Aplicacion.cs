using RecetasSLN.datos;
using RecetasSLN.dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasBack.negocio
{
    public class Aplicacion : IAplicacion
    {
        private IRecetasDAO dao;


        public Aplicacion()
        {
            dao = new RecetasDAO();
        }

        public List<Receta> ConsultaReceta()
        {
            return dao.ListarRecetas();
        }

        public List<Ingrediente> ConsultarIngredientes()
        {
            return dao.ListarIngredientes();
        }

        public bool CrearReceta(Receta oReceta)
        {
            return dao.EjecutarInsert(oReceta);
        }

        public bool EditarReceta(Receta oReceta)
        {
            return dao.EditarReceta(oReceta);
        }

        public bool EliminarReceta(int id)
        {
            return dao.EliminarReceta(id);
        }

        public Receta GetReceta(int id)
        {
            return dao.getReceta(id);
        }

        public int ProximaReceta()
        {
            return dao.ProximaReceta();
        }
    }
}

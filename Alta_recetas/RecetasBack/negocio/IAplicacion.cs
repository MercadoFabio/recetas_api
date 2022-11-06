using RecetasSLN.dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasBack.negocio
{
    public interface IAplicacion
    {
        public List<Ingrediente> ConsultarIngredientes();

        public bool CrearReceta(Receta oReceta);
        public List<Receta> ConsultaReceta();
        public bool EditarReceta(Receta oReceta);
        public bool EliminarReceta(int id);
        int ProximaReceta();
        Receta GetReceta(int id);
    }
}

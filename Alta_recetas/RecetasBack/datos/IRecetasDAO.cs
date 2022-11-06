using RecetasSLN.dominio;
using System.Data;

namespace RecetasSLN.datos
{
    interface IRecetasDAO
    {
        List<Ingrediente> ListarIngredientes();
        int ProximaReceta();
        bool EjecutarInsert(Receta oReceta);
        List<Receta> ListarRecetas();
        bool EliminarReceta(int id);
        bool EditarReceta(Receta oReceta);
        Receta getReceta(int Id);
    }
}
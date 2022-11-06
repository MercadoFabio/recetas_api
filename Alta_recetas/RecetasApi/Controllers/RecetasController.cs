using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecetasApi.Resultados;
using RecetasBack.negocio;
using RecetasSLN.dominio;

namespace RecetasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecetasController : ControllerBase

    {
        private Aplicacion app;

        private Resultado resultado;

        public RecetasController()
        {
            app = new Aplicacion();
            resultado = new Resultado();
        }
        [HttpGet("proximoID")]
        public IActionResult GetProximoID()
        {
            try
            {

                var nroReceta = (app.ProximaReceta());
                if (nroReceta != 0)
                {
                    resultado.StatusCode = 200;
                    resultado.Ok = true;
                    return Ok(nroReceta);
                }
                else
                {
                    resultado.StatusCode = 400;
                    resultado.SetError("Error al obtener el ID");
                    return Ok(resultado);
                }

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
        [HttpGet("ingrediente")]
        public IActionResult GetIngredientes()
        {
            try
            {
                var listaIngredientes = (app.ConsultarIngredientes());
                if (listaIngredientes != null)
                {

                    return Ok(listaIngredientes);
                }
                else
                {
                    resultado.SetError("Error al intentar consultar los ingredientes");
                    resultado.StatusCode = 400;
                    return BadRequest(resultado);
                }
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }
        }
        [HttpGet("receta")]
        public IActionResult GetRecetas()
        {
            try
            {
                var ListaRecetas = (app.ConsultaReceta());
                if (ListaRecetas != null)
                {
                    return Ok(ListaRecetas);
                }
                else
                {
                    resultado.StatusCode = 400;
                    resultado.Ok = false;
                    resultado.SetError("Error al obtener las recetas");
                    return BadRequest(resultado);
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
        [HttpGet("recetaPorID")]
        public IActionResult GetRecetasPorID(int id)
        {
            try
            {
                var LstReceta = app.GetReceta(id);

                if (LstReceta != null)
                {
                    return Ok(LstReceta);
                }
                else
                {
                    resultado.StatusCode = 400;
                    resultado.Ok = false;
                    resultado.SetError("Error al obtener las recetas");
                    return BadRequest(resultado);
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
        [HttpPost("GuardarReceta")]
        public IActionResult PostReceta(Receta oReceta)
        {

            try
            {
                var receta = app.CrearReceta(oReceta);

                if (receta)
                {
                    
                    return Ok("Se registro correctamente su receta");
                }
                else
                {
                    resultado.StatusCode = 400;
                    resultado.Ok = false;
                    resultado.SetError("Error al crear la receta");
                    return BadRequest(resultado);
                }
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }

        }
        [HttpDelete("eliminarReceta")]
        public IActionResult DeleteReceta(int id)
        {
            try
            {
                if (id == 0)
                {
                    resultado.StatusCode = 400;
                    resultado.SetError("ID receta incorrecta");
                    return BadRequest(resultado);
                }
                else
                {
                    return Ok(app.EliminarReceta(id));
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPut("UpdateReceta")]
        public IActionResult EditarReceta(Receta oReceta)
        {
            try
            {
                if (oReceta != null)
                {
                    return Ok(app.EditarReceta(oReceta));
                }
                else
                {
                    resultado.StatusCode = 400;
                    resultado.Ok = false;
                    resultado.SetError("Error al editar la receta");
                    return BadRequest(resultado);
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace SL_WebApi.Controllers
{
    //[Route("api/Usuario/")]
    //[ApiController]
    public class UsuarioController : Controller
    {

        [HttpGet]
        [Route("api/Usuario/GetAll")]

        public IActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();
            usuario.Direccion = new ML.Direccion();

            ML.Result result = BL.Usuario.GetAll(usuario);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpPost]
        [Route("api/Usuario/Add")]
        public IActionResult Post([FromBody] ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.Add(usuario);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpPut]
        [Route("api/usuario/update/{IdUsuario}")]
        public IActionResult Put(int IdUsuario, [FromBody] ML.Usuario usuario)
        {
            usuario.IdUsuario = IdUsuario;
            ML.Result result = BL.Usuario.Update(usuario);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpDelete]
        [Route("api/usuario/delete/{IdUsuario}")]

        public IActionResult Delete(int IdUsuario)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.IdUsuario = IdUsuario;
            var result = BL.Usuario.Delete(usuario);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        [Route("api/usuario/getById/{IdUsuario}")]
        public IActionResult GetById(int IdUsuario)
        {
            var result = BL.Usuario.GetId(IdUsuario);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }
    }
} 


        


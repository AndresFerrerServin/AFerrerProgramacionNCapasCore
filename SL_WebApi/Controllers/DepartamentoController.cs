using Microsoft.AspNetCore.Mvc;

namespace SL_WebApi.Controllers
{
    [Route("api/Departamento")]
    [ApiController]
    public class DepartamentoController : Controller
    {

        [HttpPost]
        [Route("Add")]
        public IActionResult Post([FromBody] ML.Departamento departamento)
        {
            ML.Result result = BL.Departamento.Add(departamento);

            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }
    }
}

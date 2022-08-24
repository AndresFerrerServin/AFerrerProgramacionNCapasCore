using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class DepartamentoController : Controller
    {
        public IActionResult GetAll()
        {
            ML.Departamento departamento = new ML.Departamento();
            ML.Result result = BL.Departamento.GetAll();
            if (result.Correct)
            {
                departamento.Departamentos = result.Objects.ToList();
                return View(departamento);
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al obtener la información" + result.ErrorMessage;
                return View();
            }
            
        }


        [HttpGet]
        public ActionResult Form(int? IdDepartamento)
        {
            ML.Departamento departamento = new ML.Departamento();
            ML.Result resultDepartamento = BL.Area.GetAll();
            

            if (IdDepartamento == null)
            {
                ViewBag.Titulo = "Agregar un Departamento";
                ViewBag.Accion = "Agregar";

                departamento.Area = new ML.Area();
                departamento.Area.Areas = resultDepartamento.Objects.ToList();
                


                return View(departamento);
            }
            else
            {
                ViewBag.Titulo = "Actualizar un departamento";
                ViewBag.Accion = "Actualizar";

                ML.Result result = BL.Departamento.GetId(IdDepartamento.Value);


                if (result.Correct)
                {
                    
                    departamento = (ML.Departamento)result.Object;

                    departamento.Area.Areas = resultDepartamento.Objects.ToList();

                    return View(departamento);
                }
                else
                {
                    ViewBag.Message = result.ErrorMessage;
                    return View();
                }

            }
        }

        [HttpPost]
        public ActionResult Form(ML.Departamento departamento)
        {
            if (departamento.IdDepartamento == 0)
            {
                ML.Result result = BL.Departamento.Add(departamento);

                if (result.Correct)
                {

                    ViewBag.Message = "Se ha agregado correctamente";
                    return PartialView("Modal");

                }

                else
                {

                    ViewBag.Message = result.ErrorMessage;
                    return PartialView("Modal");

                }

            }
            else
            {
                ML.Result result = BL.Departamento.Update(departamento);

                if (result.Correct)
                {
                    ViewBag.Message = " se actualizo Correctamente";
                    return PartialView("Modal");
                }

                else
                {
                    ViewBag.Message = " No se ha podido actualizar";
                    return PartialView("Modal");


                }

            }

        }


        [HttpGet]
        public ActionResult Delete (int IdDepartamento)
        {

            ML.Departamento departamento = new ML.Departamento();
            departamento.IdDepartamento = IdDepartamento;


            ML.Result result = BL.Departamento.Delete(departamento);
            if (result.Correct)
            {
                ViewBag.Message = "se ha eliminado correctamente";
            }
            else
            {
                ViewBag.Message = "Error al eliminar ";

            }
            return PartialView("Modal");
        }


    }
}

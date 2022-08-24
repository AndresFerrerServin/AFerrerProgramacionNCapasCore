using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class ProveedorController : Controller
    {
        public IActionResult GetAll()
        {
            ML.Proveedor proveedor = new ML.Proveedor();
            ML.Result result = BL.Proveedor.GetAll();
            if (result.Correct)
            {
                proveedor.Proveedores = result.Objects.ToList();
                return View(proveedor);
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al obtener la información" + result.ErrorMessage;
                return View();
            }

        }

        [HttpGet]
        public ActionResult Form(int? IdProveedor)
        {
            ML.Proveedor provedor = new ML.Proveedor();

            if (IdProveedor == null)
            {
               // ViewBag.Titulo = "Agregar un Proveedor";
               // ViewBag.Accion = "Agregar";

                return View();
            }
            else
            {
               // ViewBag.Titulo = "Actualizar un departamento";
               // ViewBag.Accion = "Actualizar";

                ML.Result result = BL.Proveedor.GetId(IdProveedor.Value);
                if (result.Correct)
                {

                    provedor = (ML.Proveedor)result.Object;

                    return View(provedor);
                }
                else
                {
                    ViewBag.Message = result.ErrorMessage;
                    return View();
                }
                
            }


        }

        [HttpPost]
        public ActionResult Form(ML.Proveedor proveedor)
        {
            if (proveedor.IdProveedor == 0)
            {
                ML.Result result = BL.Proveedor.Add(proveedor);

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
                ML.Result result = BL.Proveedor.Update(proveedor);

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
        public ActionResult Delete(int IdProveedor)
        {

            ML.Proveedor proveedor = new ML.Proveedor();
            proveedor.IdProveedor = IdProveedor;


            ML.Result result = BL.Proveedor.Delete(proveedor);
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


using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class ProductoController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Producto producto = new ML.Producto();

            ML.Result resultArea = BL.Area.GetAll();
            ML.Result resultDepartamento = BL.Departamento.GetAll();

            producto.Departamento = new ML.Departamento();
            producto.Departamento.Area = new ML.Area();

            producto.Nombre = (producto.Nombre == null) ? "" : producto.Nombre;
            producto.Departamento.IdDepartamento = (producto.Departamento.IdDepartamento == null) ? 0 : producto.Departamento.IdDepartamento;
            producto.Departamento.Area.IdArea = (producto.Departamento.Area.IdArea == null) ? 0 : producto.Departamento.Area.IdArea;

            ML.Result result = BL.Producto.GetAll(producto);


            producto.Departamento.Area.Areas = resultArea.Objects.ToList();
            producto.Departamento.Departamentos = resultDepartamento.Objects.ToList();


            if (result.Correct)
            {
                producto.Productos = result.Objects.ToList();
                return View(producto);
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al obtener la información" + result.ErrorMessage;
                return View();
            }

        }

        [HttpPost]
        public IActionResult GetAll(ML.Producto producto)
        {
            ML.Result result = BL.Producto.GetAll(producto);
            ML.Result resultArea = BL.Area.GetAll();
            ML.Result resultDepartamento = BL.Departamento.GetAll();

            producto.Departamento = new ML.Departamento();
            producto.Departamento.Area = new ML.Area();

            producto.Departamento.Area.Areas = resultArea.Objects.ToList();
            producto.Departamento.Departamentos = resultDepartamento.Objects.ToList();

            

            if (result.Correct)
            {
                producto.Productos = result.Objects.ToList();
                return View(producto);
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al obtener la información" + result.ErrorMessage;
                return View();
            }

        }
        [HttpGet]
        public ActionResult Form(int? IdProducto)
        {
            ML.Producto producto = new ML.Producto();
            ML.Result resultProveedor = BL.Proveedor.GetAll();
            ML.Result resultArea = BL.Area.GetAll();
            

            if (IdProducto == null)
            {
                //ViewBag.Titulo = "Agregar un producto";
               // ViewBag.Accion = "Agregar";

                producto.Proveedor = new ML.Proveedor();
                
                producto.Departamento = new ML.Departamento();
                producto.Departamento.Area = new ML.Area();


                producto.Proveedor.Proveedores = resultProveedor.Objects.ToList();
                producto.Departamento.Area.Areas = resultArea.Objects.ToList();
               


                return View(producto);
            }
            else
            {
                //ViewBag.Titulo = "Actualizar un producto";
                //ViewBag.Accion = "Actualizar";

                ML.Result result = BL.Producto.GetId(IdProducto.Value);


                if (result.Correct)
                {
                    producto = (ML.Producto)result.Object;
                    producto.Proveedor.Proveedores = resultProveedor.Objects.ToList();

                    ML.Result resultDepartamento = BL.Departamento.GetByIdArea(producto.Departamento.Area.IdArea);
                    producto.Departamento.Area.Areas = resultArea.Objects;
                    producto.Departamento.Departamentos = resultDepartamento.Objects;
                    
                    return View(producto);
                }
                else
                {
                    ViewBag.Message = result.ErrorMessage;
                    return View();
                }

            }
        }

        [HttpPost]
        public ActionResult Form(ML.Producto producto)
        {
            //Obtener la imagen
            IFormFile file = Request.Form.Files["IFImagen"];

            //Valido si trae la imagen
            if (file != null)
            {
                //LLama al metodo que tranforma a byte la imagen
                byte[] ImagenBytes = ConvertToBytes(file);
                //Convierte a base 64 y guardalo
                producto.Imagen = Convert.ToBase64String(ImagenBytes);

            }


            if (producto.IdProducto == 0)
            {
                ML.Result result = BL.Producto.Add(producto);

                if (result.Correct)
                {

                    ViewBag.Message = "Se ha agregado correctamente";
                    return PartialView("Modal");

                }

                else
                {
                    
                    return RedirectToAction("Form");

                }

            }
            else
            {
                ML.Result result = BL.Producto.Update(producto);

                if (result.Correct)
                {
                    ViewBag.Message = "Se ha agregado correctamente";
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
        public ActionResult Delete(int IdProducto)
        {

            ML.Producto producto = new ML.Producto();
            producto.IdProducto = IdProducto;


            ML.Result result = BL.Producto.Delete(producto);
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


        public static byte[] ConvertToBytes(IFormFile imagen)
        {

            using var fileStream = imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }

        public JsonResult DepartamentoGetByIdArea(int IdArea)
        {
           ML.Result result = BL.Departamento.GetByIdArea(IdArea);
            return Json(result.Objects);
        }

        [HttpPost]
        public ActionResult CargaMasiva()
        {
            ML.Result resultErrores = new ML.Result();
            resultErrores.Objects = new List<object>();
            try
            {
                IFormFile archivo = Request.Form.Files["Archivo"];
                using (StreamReader sr = new StreamReader(archivo.OpenReadStream()))
                {
                    string line;
                    line =sr.ReadLine();
                    while ((line = sr.ReadLine()) != null) 
                    {
                        string[] datos = line.Split('|');

                        ML.Producto producto = new ML.Producto();
                        producto.Nombre = datos[0];
                        producto.PrecioUnitario = decimal.Parse(datos[1]);
                        producto.Stock = int.Parse(datos[2]);
                        producto.Descripcion = datos[3];
                        producto.Proveedor = new ML.Proveedor();
                        producto.Proveedor.IdProveedor = int.Parse(datos[4]);
                        producto.Departamento = new ML.Departamento();
                        producto.Departamento.IdDepartamento = int.Parse(datos[5]);
                        producto.Imagen = null;

                        ML.Result result = BL.Producto.Add(producto);

                        if (!result.Correct)
                        {
                            resultErrores.Objects.Add("No se inserto el Nombre"+producto.Nombre +
                                                      "No se inserto el PrecioUnitario" + producto.PrecioUnitario +
                                                      "No se inserto el Stock" + producto.Stock +
                                                      "No se inserto la Descripcion" + producto.Descripcion +
                                                      "No se inserto el Proveedor" + producto.Proveedor.IdProveedor +
                                                      "No se inserto el Departamento" + producto.Departamento.IdDepartamento);

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine ("No se pudo leer el archivo");
                Console.WriteLine (e.Message);
            }
            return PartialView("Modal");
        }
    }
}

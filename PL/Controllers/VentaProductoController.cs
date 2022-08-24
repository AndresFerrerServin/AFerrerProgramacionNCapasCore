using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class VentaProductoController : Controller
    {
        
        [HttpPost]
        public ActionResult GetAll(ML.Producto producto)
        {
            ML.Result result = BL.Producto.ProductoGetByIdDepartamento(producto.Departamento.IdDepartamento);
            ML.Result resultDepartamento = BL.Departamento.GetByIdArea(producto.Departamento.Area.IdArea);
            ML.Result resultArea = BL.Area.GetAll();

            producto.Departamento.Departamentos = resultDepartamento.Objects;
            producto.Departamento.Area.Areas = resultArea.Objects;
            producto.Productos = result.Objects;

            return View("ProductoGetAll", producto);
        }


        public ActionResult GetAll()
        {
            ML.Producto producto = new ML.Producto();
            ML.Result result = BL.Producto.GetAll(producto);
            
            ML.Result resultDepartamento = BL.Departamento.GetAll();
            ML.Result resultAreas = BL.Area.GetAll();

            producto.Departamento = new ML.Departamento();
            producto.Departamento.Area = new ML.Area();

            if (result.Correct)
            {
                producto.Departamento.Departamentos = resultDepartamento.Objects;
                producto.Departamento.Area.Areas = resultAreas.Objects;
                return View(producto);
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al obtener la información" + result.ErrorMessage;
                return PartialView("ValidationModal");
            }
        }
        public JsonResult GetDepartamento (int IdArea)
        {
            ML.Departamento departamento = new ML.Departamento();

            departamento.IdDepartamento = 0;
            departamento.Nombre = "Selecciona una opcion";

            var result = BL.Departamento.GetByIdArea(IdArea);
            result.Objects.Insert(0, departamento);

            return Json(result.Objects);
        
        }


        public ActionResult Cart()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CartPost(ML.Producto producto)
        {
            ML.VentaProducto ventaProducto = new ML.VentaProducto();
            ventaProducto.VentaProductos = new List<object>();

            if (HttpContext.Session.GetString("Producto") ==null)
            {
                producto.Cantidad = producto.Cantidad = 1;
                ventaProducto.VentaProductos.Add(producto);
                HttpContext.Session.SetString("Producto", Newtonsoft.Json.JsonConvert.SerializeObject(ventaProducto.VentaProductos));
                var session = HttpContext.Session.GetString("Producto");
            }
            else
            {
                var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Producto"));

                foreach (var obj in ventaSession)
                {
                    ML.Producto objProducto = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(obj.ToString());
                    ventaProducto.VentaProductos.Add(objProducto);
                }

                foreach (ML.Producto venta in ventaProducto.VentaProductos.ToList())
                {
                    if (producto.IdProducto == venta.IdProducto)
                    {
                        venta.Cantidad = venta.Cantidad + 1 ; 
                    }
                    else
                    {
                        producto.Cantidad = venta.Cantidad = 1;
                        ventaProducto.VentaProductos.Add(producto);
                    }

                }
                HttpContext.Session.SetString("Producto", Newtonsoft.Json.JsonConvert.SerializeObject(ventaProducto.VentaProductos));

            }
            if (HttpContext.Session.GetString("Producto")!= null)
            {
                ViewBag.Message = "Se ha agregado el producto a tu carrito";
                return PartialView("Modal");

            }
            else
            {
                ViewBag.Message = " No se pudo agregar";
                return PartialView("Modal");
            }
        }
        [HttpGet]
        public ActionResult ResumenCompra(ML.VentaProducto ventaProducto)
        {
            if (HttpContext.Session.GetString("Producto") == null)
            {
                return View();
            }
            else
            {
                var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Producto"));
                ventaProducto.VentaProductos = new List<object>();

                foreach (var obj in ventaSession)
                {
                    ML.Producto objProducto = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(obj.ToString());
                    ventaProducto.VentaProductos.Add(objProducto);
                }
            }
            return View(ventaProducto);
        }
    }
}

using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace PL.Controllers
{
    public class CargaMasivaController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly IHostingEnvironment _hostingEnvironment;

        public CargaMasivaController(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult ProductoCargaMasiva()
        {
            ML.Result result = new ML.Result();
            return View(result);
        }
        [HttpPost]
        public ActionResult ProductoCargaMasiva(ML.Producto producto)
        {

            IFormFile archivo = Request.Form.Files["FileExcel"];

            if (HttpContext.Session.GetString("PathArchivo") == null) //obtiene informacion de la sesion
            {
                if (archivo.Length > 0)
                {
                    string fileName = Path.GetFileName(archivo.FileName);// nombre del archivo
                    string folderPath = _configuration["PathFolder:value"];
                    string extensionArchivo = Path.GetExtension(archivo.FileName).ToLower();
                    string extensionModulo = _configuration["TipoExcel"];//tipo de archivo

                    if (extensionArchivo == extensionModulo)
                    {
                        //Crea una copia del archivo insertado
                        string filePath = Path.Combine(_hostingEnvironment.ContentRootPath, folderPath, Path.GetFileNameWithoutExtension(fileName)) + '-' + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                        if (!System.IO.File.Exists(filePath))
                        {
                            using (FileStream stream = new FileStream(filePath, FileMode.Create))
                            {
                                archivo.CopyTo(stream);
                            }

                            string connectionString = _configuration["ConnectionStringExcel:value"] + filePath;
                            ML.Result resultProducto = BL.Producto.ConvertirExceltoDataTable(connectionString);

                            if (resultProducto.Correct)
                            {
                                ML.Result resultValidacion = BL.Producto.ValidarExcel(resultProducto.Objects);
                                if (resultValidacion.Objects.Count == 0)
                                {
                                    resultValidacion.Correct = true;
                                    HttpContext.Session.SetString("PathArchivo", filePath);
                                }

                                return View(resultValidacion);
                            }
                            else
                            {
                                ViewBag.Message = "El excel no contiene registros";
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Favor de seleccionar un archivo .xlsx, Verifique su archivo";
                    }
                }
                else
                {
                    ViewBag.Message = "No selecciono ningun archivo, Seleccione uno correctamente";
                }
            }
            else
            {
                string rutaArchivoExcel = HttpContext.Session.GetString("PathArchivo");
                string connectionString = _configuration["ConnectionStringExcel:value"] + rutaArchivoExcel;

                ML.Result resultData = BL.Producto.ConvertirExceltoDataTable(connectionString);
                if (resultData.Correct)
                {
                    ML.Result resultErrores = new ML.Result();
                    resultErrores.Objects = new List<object>();

                    foreach (ML.Producto productoItem in resultData.Objects)
                    {

                        ML.Result resultAdd = BL.Producto.Add(productoItem);
                        if (!resultAdd.Correct)
                        {
                            resultErrores.Objects.Add("No se insertó los productos con nombre: " + productoItem.Nombre + " PrecioUnitario:" + productoItem.PrecioUnitario + "Stock:" + productoItem.Stock + "Proveedor" + productoItem.Proveedor.IdProveedor + "Departamento" + productoItem.Departamento.IdDepartamento + "Descripcion" + productoItem.Descripcion + " Error: " + resultAdd.ErrorMessage);
                        }
                    }
                    if (resultErrores.Objects.Count > 0)
                    {
                        string folderError = _configuration["PathFolderError:value"];
                        string fileError = Path.Combine(_hostingEnvironment.WebRootPath, folderError + @"\logErrores.txt");
                        using (StreamWriter writer = new StreamWriter(fileError))
                        {
                            foreach (string ln in resultErrores.Objects)
                            {
                                writer.WriteLine(ln);
                            }
                        }
                        ViewBag.Message = "Los Productos No han sido registrados correctamente";
                    }
                    else
                    {
                        ViewBag.Message = "Los Productos han sido registrados correctamente";
                    }

                }

            }
            return PartialView("Modal");
        }

    }
}
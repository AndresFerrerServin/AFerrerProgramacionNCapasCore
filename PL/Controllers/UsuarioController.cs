using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly IConfiguration _configuration;

        private readonly IHostingEnvironment _hostingEnvironment;

        public UsuarioController(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
            //    usuario.Nombre =(usuario.Nombre == null)? "":usuario.Nombre;
            //    usuario.ApellidoPaterno = (usuario.ApellidoPaterno == null) ? "" : usuario.ApellidoPaterno;
            //    usuario.ApellidoMaterno = (usuario.ApellidoMaterno == null) ? "" : usuario.ApellidoMaterno; 

            //    ML.Result result = BL.Usuario.GetAll(usuario);
            //    if (result.Correct)
            //    {
            //        usuario.Usuarios = result.Objects.ToList();
            //        return View(usuario);
            //    }
            //    else
            //    {
            //        ViewBag.Message = "Ocurrió un error al obtener la información" + result.ErrorMessage;
            //        return PartialView("ValidationModal");

            //    }


            ML.Result resultUsuario = new ML.Result();
            resultUsuario.Objects = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["WebAPI"]);

                var responseTask = client.GetAsync("api/Usuario/GetAll");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.Objects)
                    {
                        ML.Usuario resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(resultItem.ToString());
                        resultUsuario.Objects.Add(resultItemList);
                    }
                }
                usuario.Usuarios = resultUsuario.Objects;
            }

            return View(usuario);

        }

            

        [HttpPost]
        public IActionResult GetAll(ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.GetAll(usuario);
            if (result.Correct)
            {
                usuario.Usuarios = result.Objects.ToList();
                return View(usuario);
            }
            else
            {
                ViewBag.Message = "Ocurrió un error al obtener la información" + result.ErrorMessage;
                return PartialView("ValidationModal");

            }

        }

        [HttpGet]
        public ActionResult Form(int? IdUsuario)

        {
            ML.Usuario usuario = new ML.Usuario();
            ML.Result resultRol = BL.Rol.GetAll();
            ML.Result resultPais = BL.Pais.GetAll();
            ML.Result resultDireccion = BL.Direccion.GetAll();
            ML.Result resultUsuario = new ML.Result();
            


            if (IdUsuario == null)
            {
                ViewBag.Titulo = "Agregar un Usuario";
                ViewBag.Accion = "Agregar";

                usuario.Rol = new ML.Rol();
                usuario.Rol.Roles = resultRol.Objects.ToList();
                

                usuario.Direccion = new ML.Direccion();
                usuario.Direccion.Colonia = new ML.Colonia();
                usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                
                usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;
                usuario.Direccion.Direcciones = resultDireccion.Objects;
                

                return View(usuario);
            }
            else
            {
                ViewBag.Titulo = "Actualizar un Usuario";
                ViewBag.Accion = "Actualizar";

                //ML.Result result = BL.Usuario.GetId(IdUsuario.Value);
                ML.Result result = new ML.Result();

                    usuario = (ML.Usuario)result.Object;
                    

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_configuration["WebAPI"]);
                        var responseTask = client.GetAsync("api/usuario/getById/" + IdUsuario);
                        responseTask.Wait();
                        var resultAPI = responseTask.Result;
                        if (resultAPI.IsSuccessStatusCode)
                        {
                            var readTask = resultAPI.Content.ReadAsAsync<ML.Result>();
                            readTask.Wait();

                            ML.Usuario resultItemList = new ML.Usuario();
                            resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(readTask.Result.Object.ToString());
                            result.Object = resultItemList;



                        resultItemList.Rol.Roles = resultRol.Objects.ToList();

                        ML.Result resultEstado = BL.Estado.GetByIdPais(resultItemList.Direccion.Colonia.Municipio.Estado.Pais.IdPais);

                        ML.Result resultMunicipio = BL.Municipio.GetByIdEstado(resultItemList.Direccion.Colonia.Municipio.Estado.IdEstado);

                        ML.Result resultColonia = BL.Colonia.GetByIdMunicipio(resultItemList.Direccion.Colonia.Municipio.IdMunicipio);


                        resultItemList.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;
                        resultItemList.Direccion.Colonia.Municipio.Estado.Estados = resultEstado.Objects;
                        resultItemList.Direccion.Colonia.Municipio.Municipios = resultMunicipio.Objects;
                        resultItemList.Direccion.Colonia.Colonias = resultColonia.Objects;

                        return View(resultItemList);
                        }
                        else
                        {
                            ViewBag.Message = result.ErrorMessage;
                            return View();
                        }
                    
                    }
                

            }
           
        }

        [HttpPost]
        public ActionResult Form(ML.Usuario usuario)
        {
            //Obtener la imagen
            IFormFile file = Request.Form.Files["IFImagen"];

            //Valido si trae la imagen
            if (file != null)
            {
                //LLama al metodo que tranforma a byte la imagen
                byte[] ImagenBytes = ConvertToBytes(file);
                //Convierte a base 64 y guardalo
                usuario.Imagen = Convert.ToBase64String(ImagenBytes);

            }

            if (usuario.IdUsuario == 0)
            {
                //ML.Result result = BL.Usuario.Add(usuario);



                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["WebAPI"]);

                    var postTask = client.PostAsJsonAsync<ML.Usuario>("api/Usuario/Add", usuario);
                    postTask.Wait();

                    var result = postTask.Result;


                    if (result.IsSuccessStatusCode)
                    {

                        ViewBag.Message = "Se ha agregado correctamente";
                        return PartialView("Modal");

                    }

                    else
                    {

                        ViewBag.Message = "No se pudo agregar";
                        return PartialView("Modal");

                    }

                }
            }
            else
            {
                //ML.Result result = BL.Usuario.Update(usuario);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["WebAPI"]);

                    //HTTP POST
                    var postTask = client.PutAsJsonAsync<ML.Usuario>("api/usuario/update/" + usuario.IdUsuario, usuario);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
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
        }



        [HttpGet]
        public ActionResult Delete(int IdUsuario)
        {

            ML.Usuario usuario = new ML.Usuario();
            usuario.IdUsuario = IdUsuario;


            // ML.Result result = BL.Usuario.Delete(usuario);


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["WebAPI"]);

                //HTTP POST
                var postTask = client.DeleteAsync("api/usuario/delete/" + IdUsuario);
                postTask.Wait();

                var result = postTask.Result;
                
                if (result.IsSuccessStatusCode)
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


        public JsonResult EstadoGetByIdPais(int IdPais) 
        {
            ML.Result result = BL.Estado.GetByIdPais(IdPais);
            return Json(result.Objects);
        }
        public JsonResult MunicipioGetByIdEstado(int IdEstado)
        {
            ML.Result result = BL.Municipio.GetByIdEstado(IdEstado);
            return Json(result.Objects);
        }
        public JsonResult ColoniaGetByIdMunicipio(int IdMunicipio)
        {
            ML.Result result = BL.Colonia.GetByIdMunicipio(IdMunicipio);
            return Json(result.Objects);
        }
        public static byte[] ConvertToBytes(IFormFile imagen)
        {

            using var fileStream = imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }

        [HttpGet]
        public ActionResult UpdateEstatus (int IdUsuario)
        {
            ML.Result result = BL.Usuario.GetId(IdUsuario);
            if (result.Correct)
            {
                ML.Usuario usuario = new ML.Usuario();
                usuario = (ML.Usuario)result.Object;

                usuario.Estatus = (usuario.Estatus) ? false : true;

                ML.Result resultUpdate = BL.Usuario.Update(usuario);
                if (result.Correct)
                {
                    ViewBag.Message = "El estatus se actulizo";
                }
                else
                {
                    ViewBag.Message = "Error al eliminar ";

                }
                
            }
            return PartialView("Modal");
        }
    }
}


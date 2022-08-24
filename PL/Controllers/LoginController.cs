using Microsoft.AspNetCore.Mvc;


namespace PL.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login (string password , string email)
        {
            ML.Result result = BL.Usuario.GetByEmeil(email);
            if (result.Correct)
            {
                ML.Usuario usuario = ((ML.Usuario)result.Object);
                if (usuario.Password == password)
                {
                    //return View("~/views/Home/Index.cshtml");
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ViewBag.Message = "La contraseña no exixte";
                    return PartialView("Modal");
                }
            }
            else
            {
                ViewBag.Message = "El email no exixte";
                return PartialView("Modal");
            }
        }
    }
}

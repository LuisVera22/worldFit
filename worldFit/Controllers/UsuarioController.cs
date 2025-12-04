using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Dominio.Entidad.Entidad;
using Infraestructura.SQL; 

namespace worldFit.Controllers
{
    public class UsuarioController : Controller
    {
       
        usuarioDAO _usuario = new usuarioDAO();
        rutinaDAO _rutina = new rutinaDAO();


        public ActionResult Index()
        {
            IEnumerable<Rutina> lista = _rutina.GetAll();
            return View(lista);
        }

        
        public ActionResult Registro()
        {
            return View(new Usuario());
        }

        [HttpPost] public ActionResult Registro(Usuario reg) {

            if (ModelState.IsValid)
            {
                string mensaje = _usuario.Add(reg);
                TempData["Mensaje"] = mensaje;
            }
            return View(new Usuario());
        }


        public ActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Login(string correo, string clave)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(clave))
            {
                ViewBag.Mensaje = "Debe ingresar correo y clave.";
                return View();
            }

            Usuario user = _usuario.Login(correo, clave);

            if (user != null)
            {
              
                Session["UsuarioID"] = user.IdUsuario;
                Session["UsuarioNombre"] = user.nombre;
                Session["UsuarioCorreo"] = user.correo;

                return RedirectToAction("Perfil");
            }
            else
            {
                ViewBag.Mensaje = "Correo o clave incorrectos.";
                return View();
            }
        }


        public ActionResult Perfil()
        {
            if (Session["UsuarioID"] == null)
                return RedirectToAction("Login");

            int idUsuario = Convert.ToInt32(Session["UsuarioID"]);

            ViewBag.Nombre = Session["UsuarioNombre"];
            ViewBag.Correo = Session["UsuarioCorreo"];

            var historial = new imcDAO().HistorialPorUsuario(idUsuario);
            ViewBag.Historial = historial;

         
            var habitos = new habitoDAO().ListarPorUsuario(idUsuario);
            ViewBag.Habitos = habitos;

            // Rutinas
            var usuarioRutinaDAO = new usuarioRutinaDAO();
            var rutinas = usuarioRutinaDAO.ListarRutinasPorUsuario(idUsuario);
            ViewBag.Rutinas = rutinas;


            return View();

        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }


        

    }


}

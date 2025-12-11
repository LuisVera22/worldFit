using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost]
        public ActionResult Registro(Usuario reg)
        {
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

        // PERFIL PRINCIPAL
        public ActionResult Perfil()
        {
            if (Session["UsuarioID"] == null)
                return RedirectToAction("Login");

            int idUsuario = Convert.ToInt32(Session["UsuarioID"]);

            ViewBag.Nombre = Session["UsuarioNombre"];
            ViewBag.Correo = Session["UsuarioCorreo"];

            // Rutinas
            var usuarioRutinaDAO = new usuarioRutinaDAO();
            var rutinas = usuarioRutinaDAO.ListarRutinasPorUsuario(idUsuario);
            ViewBag.Rutinas = rutinas;

<<<<<<< HEAD
            var ejercicios = new ejercicioDAO().ListarEjerciciosDelUsuario(idUsuario);
            ViewBag.Ejercicios = ejercicios;

=======
>>>>>>> develop
            return View();
        }

        // ---------- PARTIAL: Historial IMC ----------
        public PartialViewResult HistorialIMC(int pageIMC = 1)
        {
            if (Session["UsuarioID"] == null)
                return PartialView("_HistorialIMC", new List<Imc>());

            int idUsuario = Convert.ToInt32(Session["UsuarioID"]);

            var historial = new imcDAO().HistorialPorUsuario(idUsuario) ?? new List<Imc>();

            int pageSize = 5;
            int totalPagesIMC = historial.Count() > 0
                ? (int)Math.Ceiling(historial.Count() / (double)pageSize)
                : 0;

            if (pageIMC < 1) pageIMC = 1;
            if (totalPagesIMC > 0 && pageIMC > totalPagesIMC) pageIMC = totalPagesIMC;

            ViewBag.PageIMC = pageIMC;
            ViewBag.TotalPagesIMC = totalPagesIMC;

            var paginatedIMC = historial
                .OrderByDescending(x => x.fechaRegistro)
                .Skip((pageIMC - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return PartialView("_HistorialIMC", paginatedIMC);
        }

        // ---------- PARTIAL: Hábitos ----------
        public PartialViewResult Habitos(int pageHabitos = 1)
        {
            if (Session["UsuarioID"] == null)
                return PartialView("_Habitos", new List<Habito>());

            int idUsuario = Convert.ToInt32(Session["UsuarioID"]);

            var habitos = new habitoDAO().ListarPorUsuario(idUsuario) ?? new List<Habito>();

            int pageSize = 5;
            int totalPagesHabitos = habitos.Count() > 0
                ? (int)Math.Ceiling(habitos.Count() / (double)pageSize)
                : 0;

            if (pageHabitos < 1) pageHabitos = 1;
            if (totalPagesHabitos > 0 && pageHabitos > totalPagesHabitos) pageHabitos = totalPagesHabitos;

            ViewBag.PageHabitos = pageHabitos;
            ViewBag.TotalPagesHabitos = totalPagesHabitos;

            var paginatedHabitos = habitos
                .OrderByDescending(x => x.fechaRegistro)
                .Skip((pageHabitos - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return PartialView("_Habitos", paginatedHabitos);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
<<<<<<< HEAD


        public ActionResult AgregarEjercicioUsuario(int idEjercicio)
        {
            int idUsuario = Convert.ToInt32(Session["UsuarioID"]);
            new ejercicioDAO().AgregarEjercicioUsuario(idUsuario, idEjercicio);
            return RedirectToAction("Perfil", "Usuario");
        }

        public ActionResult MisEjercicios()
        {
            if (Session["UsuarioID"] == null)
                return RedirectToAction("Login", "Usuario");

            int idUsuario = (int)Session["UsuarioID"];

            var lista = new ejercicioDAO().ListarEjerciciosDelUsuario(idUsuario);

            return View(lista);
        }



=======
>>>>>>> develop
    }
}

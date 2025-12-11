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


        public ActionResult Perfil(int? historialPage, int? habitosPage)
        {
            if (Session["UsuarioID"] == null)
                return RedirectToAction("Login");

            int idUsuario = Convert.ToInt32(Session["UsuarioID"]);

            ViewBag.Nombre = Session["UsuarioNombre"];
            ViewBag.Correo = Session["UsuarioCorreo"];

            // Tamaños de página
            const int pageSizeHistorial = 5;
            const int pageSizeHabitos = 5;

            // HISTORIAL IMC
            var historialCompleto = new imcDAO()
                .HistorialPorUsuario(idUsuario)
                .OrderByDescending(h => h.fechaRegistro)  // ordenado por fecha
                .ToList();

            int paginaHistorialActual = historialPage ?? 1;
            int totalHistorial = historialCompleto.Count();

            var historialPaginado = historialCompleto
                .Skip((paginaHistorialActual - 1) * pageSizeHistorial)
                .Take(pageSizeHistorial)
                .ToList();

            ViewBag.Historial = historialPaginado;
            ViewBag.HistorialPaginaActual = paginaHistorialActual;
            ViewBag.HistorialTotalPaginas =
                (int)Math.Ceiling((double)totalHistorial / pageSizeHistorial);

            // HÁBITOS
            var habitosCompletos = new habitoDAO()
                .ListarPorUsuario(idUsuario)
                .OrderByDescending(h => h.fechaRegistro)
                .ToList();

            int paginaHabitosActual = habitosPage ?? 1;
            int totalHabitos = habitosCompletos.Count();

            var habitosPaginados = habitosCompletos
                .Skip((paginaHabitosActual - 1) * pageSizeHabitos)
                .Take(pageSizeHabitos)
                .ToList();

            ViewBag.Habitos = habitosPaginados;
            ViewBag.HabitosPaginaActual = paginaHabitosActual;
            ViewBag.HabitosTotalPaginas =
                (int)Math.Ceiling((double)totalHabitos / pageSizeHabitos);

            // RUTINAS Y EJERCICIOS
            var usuarioRutinaDAO = new usuarioRutinaDAO();
            var rutinas = usuarioRutinaDAO.ListarRutinasPorUsuario(idUsuario);
            ViewBag.Rutinas = rutinas;

            var ejercicios = new ejercicioDAO().ListarEjerciciosDelUsuario(idUsuario);
            ViewBag.Ejercicios = ejercicios;

            return View();
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }


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

    }


}

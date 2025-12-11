using Dominio.Entidad.Entidad;
using Infraestructura.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace worldFit.Controllers
{
    public class RutinaController : Controller
    {
        rutinaDAO _rutinaDAO = new rutinaDAO();
        ejercicioDAO _ejercicioDAO = new ejercicioDAO();
        usuarioRutinaDAO _usuarioRutinaDAO = new usuarioRutinaDAO();

        // Lista TODAS las rutinas globales (no por usuario)
        public ActionResult ListaRutinas()
        {
            var rutinas = _rutinaDAO.GetAll();
            return View(rutinas);
        }

        // Vista de ejercicios de la rutina (globales + agregados por usuario)
        public ActionResult VerEjercicios(int idRutina)
        {
            if (Session["UsuarioID"] == null)
                return RedirectToAction("Login", "Usuario");

            int idUsuario = (int)Session["UsuarioID"];

            // Ejercicios GLOBALes de la rutina
            var ejercicios = new ejercicioDAO().ListarPorRutina(idRutina);

            // Rutina asignada?
            var usuarioRutinaDAO = new usuarioRutinaDAO();
            var rutinasUsuario = usuarioRutinaDAO.ListarRutinasPorUsuario(idUsuario);

            bool rutinaAsignada = rutinasUsuario.Any(r => r.idRutina == idRutina);

            ViewBag.RutinaAsignada = rutinaAsignada;
            ViewBag.IdRutina = idRutina;

            return View(ejercicios);
        }


        // Llamado desde AJAX para cargar ejercicios en el modal
        public ActionResult ObtenerEjercicios(int idRutina)
        {
            if (Session["UsuarioID"] == null)
                return RedirectToAction("Login", "Usuario");

            int idUsuario = Convert.ToInt32(Session["UsuarioID"]);

            var lista = _ejercicioDAO.ListarEjerciciosUsuarioPorRutina(idUsuario, idRutina);

            return PartialView("_ListaEjerciciosRutina", lista);
        }

        // Agregar una rutina al usuario
        public ActionResult AgregarRutinaUsuario(int idRutina)
        {
            if (Session["UsuarioID"] == null)
                return RedirectToAction("Login", "Usuario");

            int idUsuario = (int)Session["UsuarioID"];

            _usuarioRutinaDAO.AgregarUsuarioRutina(idUsuario, idRutina);

            return RedirectToAction("VerEjercicios", new { idRutina = idRutina });
        }

        public JsonResult EliminarEjercicioUsuario(int idEjercicioUsuario)
        {
            bool ok = new ejercicioDAO().EliminarEjercicioUsuario(idEjercicioUsuario);
            return Json(new { success = ok }, JsonRequestBehavior.AllowGet);
        }


    }
}

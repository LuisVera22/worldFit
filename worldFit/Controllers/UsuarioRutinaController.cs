using Dominio.Entidad.Entidad;
using Infraestructura.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace worldFit.Controllers
{
    public class UsuarioRutinaController : Controller
    {
        usuarioRutinaDAO _dao = new usuarioRutinaDAO();

        // 🔹 Muestra todas las rutinas del usuario logueado
        public ActionResult MisRutinas()
        {
            if (Session["UsuarioID"] == null)
                return RedirectToAction("Login", "Usuario");

            int idUsuario = Convert.ToInt32(Session["UsuarioID"]);

            List<UsuarioRutina> lista = _dao.ListarRutinasPorUsuario(idUsuario);
            return View(lista);
        }

        // 🔹 Agregar una rutina al usuario
        public ActionResult Agregar(int idRutina)
        {
            if (Session["UsuarioID"] == null)
                return RedirectToAction("Login", "Usuario");

            int idUsuario = Convert.ToInt32(Session["UsuarioID"]);

            // Meta semanal por defecto (puedes cambiarla)
            int metaSemanal = 4;

            string mensaje = _dao.AgregarRutinaUsuario(idUsuario, idRutina, metaSemanal);

            TempData["Mensaje"] = mensaje;
            return RedirectToAction("MisRutinas");
        }

        // 🔹 Actualizar el progreso (sumar un día cumplido)
        public ActionResult ActualizarProgreso(int idUsuarioRutina)
        {
            _dao.ActualizarProgreso(idUsuarioRutina);
            return RedirectToAction("MisRutinas");
        }

        // 🔹 Eliminar rutina del usuario
        public ActionResult Eliminar(int idUsuarioRutina)
        {
            string mensaje = _dao.EliminarRutinaUsuario(idUsuarioRutina);
            TempData["Mensaje"] = mensaje;
            return RedirectToAction("MisRutinas");
        }
    }
}

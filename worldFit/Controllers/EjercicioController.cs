using Dominio.Entidad.Entidad;
using Infraestructura.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace worldFit.Controllers
{
    public class EjercicioController : Controller
    {
        ejercicioDAO _ejercicio = new ejercicioDAO();
        public ActionResult Index()
        {
            IEnumerable<Ejercicio> lista = _ejercicio.ListarTodos();

            if (lista == null)
                lista = new List<Ejercicio>();

            return View(lista);

        }

        public ActionResult AgregarEjercicioUsuario(int idEjercicio)
        {
            if (Session["UsuarioID"] == null)
                return RedirectToAction("Login", "Usuario");

            int idUsuario = Convert.ToInt32(Session["UsuarioID"]);

            _ejercicio.AgregarEjercicioUsuario(idUsuario, idEjercicio);

            TempData["Mensaje"] = "Ejercicio agregado correctamente ✔";

            return RedirectToAction("Perfil", "Usuario");
        }
    }
}
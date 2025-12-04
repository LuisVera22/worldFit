using Dominio.Entidad.Abstraccion;
using Dominio.Entidad.Entidad;
using Infraestructura.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace worldFit.Controllers
{
    public class HabitoController : Controller
    {

        habitoDAO _habito = new habitoDAO();

        
        public ActionResult Index()
        {
            var lista = _habito.GetAll();
            return View(lista);
        }

        public ActionResult MisHabitos()
        {
            if (Session["UsuarioID"] == null)
                return RedirectToAction("Login", "Usuario");

            int idUsuario = Convert.ToInt32(Session["UsuarioID"]);
            var lista = _habito.ListarPorUsuario(idUsuario);
            return View(lista);
        }

        public ActionResult Registrar()
        {
            if (Session["UsuarioID"] == null)
                return RedirectToAction("Login", "Usuario");

            return View(new Habito { fechaRegistro = DateTime.Now });
        }

        [HttpPost]
        public ActionResult Registrar(Habito reg)
        {
            if (Session["UsuarioID"] == null)
                return RedirectToAction("Login", "Usuario");

            reg.idUsuario = Convert.ToInt32(Session["UsuarioID"]);

            if (ModelState.IsValid)
            {
                string mensaje = _habito.Add(reg);
                ViewBag.Mensaje = mensaje;
                return RedirectToAction("MisHabitos");
            }

            ViewBag.Mensaje = "Ocurrió un error al registrar el hábito.";
            return View(reg);
        }

        public ActionResult Editar(int id)
        {
            var habito = _habito.BuscarPorId(id);
            if (habito == null)
                return HttpNotFound();

            return View(habito);
        }

        [HttpPost]
        public ActionResult Editar(Habito reg)
        {
            if (ModelState.IsValid)
            {
                string mensaje = _habito.Update(reg);
                ViewBag.Mensaje = mensaje;
                return RedirectToAction("MisHabitos");
            }

            return View(reg);
        }
    }
}

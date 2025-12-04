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
            IEnumerable<Ejercicio> lista = _ejercicio.GetAll();

            if (lista == null)
                lista = new List<Ejercicio>();

            return View(lista);

        }
    }
}
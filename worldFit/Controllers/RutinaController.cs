using Dominio.Entidad.Entidad;
using Infraestructura.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace worldFit.Controllers
{
    public class RutinaController : Controller
    {
        rutinaDAO _rutina = new rutinaDAO();

        public ActionResult ListaRutinas()
        {
            IEnumerable<Rutina> lista = _rutina.GetAll();

            if (lista == null)
                lista = new List<Rutina>();

            return View(lista);
        }

        public ActionResult VerEjercicios(int idRutina)
        {
            var lista = _rutina.ListarPorRutina(idRutina);
            return View(lista);
        }

       
        

    }
}
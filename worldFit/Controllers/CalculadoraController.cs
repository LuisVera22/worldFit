using Dominio.Entidad.Entidad;
using Infraestructura.SQL;
using System;
using System.Web.Mvc;

namespace worldFit.Controllers
{
    public class CalculadoraController : Controller
    {
        imcDAO _imc = new imcDAO();

        // ============================
        //   CALCULADORA IMC
        // ============================
        public ActionResult Calculadora()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(decimal peso, decimal altura)
        {
            if (Session["UsuarioID"] == null)
                return RedirectToAction("Login", "Usuario");

            if (peso <= 0 || altura <= 0)
            {
                ViewBag.Error = "Debe ingresar valores válidos para peso y altura.";
                return View("Calculadora");
            }

            int idUsuario = Convert.ToInt32(Session["UsuarioID"]);

            decimal alturaMetros = altura / 100;
            decimal imc = Math.Round(peso / (alturaMetros * alturaMetros), 2);

            string estado;
            if (imc < 18.5m)
                estado = "Bajo peso";
            else if (imc < 25m)
                estado = "Peso normal";
            else if (imc < 30m)
                estado = "Sobrepeso";
            else
                estado = "Obesidad";

            var registro = new Imc
            {
                idUsuario = idUsuario,
                peso = peso,
                altura = altura,
                imcCalculado = imc,
                fechaRegistro = DateTime.Now
            };

            _imc.Add(registro);

            ViewBag.IMC = imc;
            ViewBag.Mensaje = $"Tu IMC es {imc} → {estado}";
            ViewBag.Estado = estado;

            return View("Calculadora");
        }

        public ActionResult Historial()
        {
            if (Session["UsuarioID"] == null)
                return RedirectToAction("Login", "Usuario");

            int idUsuario = Convert.ToInt32(Session["UsuarioID"]);
            var historial = _imc.HistorialPorUsuario(idUsuario);

            return View(historial);
        }


        // ============================
        //   CÁLCULO DE CALORÍAS
        // ============================
        [HttpGet]
        public ActionResult Calorias()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Calorias(string genero, int edad, decimal altura, decimal peso, string actividad, string objetivo)
        {
            if (peso <= 0 || altura <= 0 || edad <= 0)
            {
                ViewBag.Error = "Por favor, ingrese valores válidos.";
                return View();
            }

            // 1) Calcular TMB (Harris-Benedict)
            double tmb = (genero == "masculino")
                ? (10 * (double)peso) + (6.25 * (double)altura) - (5 * edad) + 5
                : (10 * (double)peso) + (6.25 * (double)altura) - (5 * edad) - 161;

            // 2) Factor de actividad
            double factor = 1.2;
            switch (actividad)
            {
                case "ligero": factor = 1.375; break;
                case "moderado": factor = 1.55; break;
                case "intenso": factor = 1.725; break;
                case "muyintenso": factor = 1.9; break;
            }

            double calorias = tmb * factor;

            // 3) Ajustar según objetivo
            if (objetivo == "bajar") calorias -= 500;
            if (objetivo == "subir") calorias += 500;

            ViewBag.Calorias = Math.Round(calorias, 0);
            ViewBag.Mensaje = $"Tu requerimiento calórico estimado es {Math.Round(calorias)} kcal.";

            return View();
        }


        // ============================
        //   CÁLCULO DE MACROS
        // ============================
        [HttpGet]
        public ActionResult Macros()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Macros(double calorias, int comidas, string tipo)
        {
            if (calorias <= 0 || comidas <= 0)
            {
                ViewBag.Error = "Por favor, ingresa valores válidos.";
                return View();
            }

            double pctCarb = 0, pctProt = 0, pctGrasa = 0;

            switch (tipo)
            {
                case "equilibrado": pctCarb = 0.40; pctProt = 0.30; pctGrasa = 0.30; break;
                case "bajoCarb": pctCarb = 0.20; pctProt = 0.40; pctGrasa = 0.40; break;
                case "altoProt": pctCarb = 0.30; pctProt = 0.40; pctGrasa = 0.30; break;
                case "cetogenico": pctCarb = 0.05; pctProt = 0.25; pctGrasa = 0.70; break;
                default: pctCarb = 0.40; pctProt = 0.30; pctGrasa = 0.30; break;
            }

            double kcalCarb = calorias * pctCarb;
            double kcalProt = calorias * pctProt;
            double kcalGrasa = calorias * pctGrasa;

            double gCarb = kcalCarb / 4;
            double gProt = kcalProt / 4;
            double gGrasa = kcalGrasa / 9;

            double carbComida = gCarb / comidas;
            double protComida = gProt / comidas;
            double grasaComida = gGrasa / comidas;

            ViewBag.Calorias = calorias;
            ViewBag.Comidas = comidas;
            ViewBag.Tipo = tipo;

            ViewBag.Carb = Math.Round(gCarb);
            ViewBag.Prot = Math.Round(gProt);
            ViewBag.Grasa = Math.Round(gGrasa);

            ViewBag.CarbComida = Math.Round(carbComida);
            ViewBag.ProtComida = Math.Round(protComida);
            ViewBag.GrasaComida = Math.Round(grasaComida);

            return View();
        }
    }
}

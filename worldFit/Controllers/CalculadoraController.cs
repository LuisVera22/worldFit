using Dominio.Entidad.Entidad;
using Infraestructura.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace worldFit.Controllers
{
    public class CalculadoraController : Controller
    {
        imcDAO _imc = new imcDAO();

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
                valorIMC = imc,
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

            // 1️⃣ Calcular TMB (Tasa Metabólica Basal)
            double tmb;
            if (genero == "masculino")
                tmb = (10 * (double)peso) + (6.25 * (double)altura) - (5 * edad) + 5;
            else
                tmb = (10 * (double)peso) + (6.25 * (double)altura) - (5 * edad) - 161;

            // 2️⃣ Factor de actividad
            double factor = 1.2;

            switch (actividad)
            {
                case "sedentario":
                    factor = 1.2;
                    break;
                case "ligero":
                    factor = 1.375;
                    break;
                case "moderado":
                    factor = 1.55;
                    break;
                case "intenso":
                    factor = 1.725;
                    break;
                case "muyintenso":
                    factor = 1.9;
                    break;
            }
            double calorias = tmb * factor;

            // 3️⃣ Ajuste según objetivo
            if (objetivo == "bajar") calorias -= 500;
            if (objetivo == "subir") calorias += 500;

            ViewBag.Calorias = Math.Round(calorias, 0);
            ViewBag.Mensaje = $"Tu requerimiento calórico diario estimado es {Math.Round(calorias)} kcal.";

            return View();
        }



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

            // Porcentajes según tipo de dieta
            double pctCarb = 0, pctProt = 0, pctGrasa = 0;

            switch (tipo)
            {
                case "equilibrado": // 40/30/30
                    pctCarb = 0.40; pctProt = 0.30; pctGrasa = 0.30;
                    break;
                case "bajoCarb": // 20/40/40
                    pctCarb = 0.20; pctProt = 0.40; pctGrasa = 0.40;
                    break;
                case "altoProt": // 30/40/30
                    pctCarb = 0.30; pctProt = 0.40; pctGrasa = 0.30;
                    break;
                case "cetogenico": // 5/25/70
                    pctCarb = 0.05; pctProt = 0.25; pctGrasa = 0.70;
                    break;
                default:
                    pctCarb = 0.40; pctProt = 0.30; pctGrasa = 0.30;
                    break;
            }

            // Calorías por macronutriente
            double kcalCarb = calorias * pctCarb;
            double kcalProt = calorias * pctProt;
            double kcalGrasa = calorias * pctGrasa;

            // Gramos totales
            double gCarb = kcalCarb / 4;
            double gProt = kcalProt / 4;
            double gGrasa = kcalGrasa / 9;

            // Por comida
            double carbComida = gCarb / comidas;
            double protComida = gProt / comidas;
            double grasaComida = gGrasa / comidas;

            // Enviar resultados a la vista
            ViewBag.Calorias = calorias;
            ViewBag.Comidas = comidas;
            ViewBag.Tipo = tipo;
            ViewBag.Carb = Math.Round(gCarb, 0);
            ViewBag.Prot = Math.Round(gProt, 0);
            ViewBag.Grasa = Math.Round(gGrasa, 0);
            ViewBag.CarbComida = Math.Round(carbComida, 0);
            ViewBag.ProtComida = Math.Round(protComida, 0);
            ViewBag.GrasaComida = Math.Round(grasaComida, 0);

            return View();
        }

    }
}
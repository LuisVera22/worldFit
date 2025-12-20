using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WorldFit.Tests
{
    [TestClass]
    public class ImcTests
    {
        [TestMethod]
        public void CalcularIMC_DatosValidos_RetornaValorEsperado()
        {
           
            decimal peso = 70;
            decimal alturaCm = 170;

            decimal alturaMetros = alturaCm / 100;

            decimal imc = Math.Round(
                peso / (alturaMetros * alturaMetros),
                2
            );

        
            Assert.IsTrue(imc > 24 && imc < 25);
        }

        [TestMethod]
        public void IMC_DebeSerMayorACero()
        {
            decimal peso = 60;
            decimal altura = 165;

            decimal alturaMetros = altura / 100;
            decimal imc = peso / (alturaMetros * alturaMetros);

            Assert.IsTrue(imc > 0);
        }

        [TestMethod]
        public void IMC_PesoNormal()
        {
            decimal peso = 65;
            decimal altura = 170;

            decimal alturaMetros = altura / 100;
            decimal imc = peso / (alturaMetros * alturaMetros);

            string estado =
                imc < 18.5m ? "Bajo peso" :
                imc < 25m ? "Peso normal" :
                imc < 30m ? "Sobrepeso" : "Obesidad";

            Assert.AreEqual("Peso normal", estado);
        }

    }
}

using System;

namespace Dominio.Entidad.Entidad
{
    public class Imc
    {
        public int idIMC { get; set; }
        public int idUsuario { get; set; }
        public decimal peso { get; set; }
        public decimal altura { get; set; }
        public decimal imcCalculado { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}
